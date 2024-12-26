using System.Runtime.Serialization;

namespace Unwise
{
    [DataContract]
    public class TreeNodeSerializable
    {
        [DataMember]
        public Container Data { get; set; }

        [DataMember]
        public List<TreeNodeSerializable> Children { get; set; } = new List<TreeNodeSerializable>();
    }

    public static class Serialization
    {
        private static readonly DataContractSerializerSettings Settings = new DataContractSerializerSettings
        {
            PreserveObjectReferences = true
        };
        // Serialization
        public static void SerializeToXml<T>(T obj, string filePath)
        {
            var serializer = new DataContractSerializer(typeof(T), Settings);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.WriteObject(fileStream, obj);
            }
        }

        public static T DeserializeFromXml<T>(string filePath)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.ReadObject(fileStream);
            }
        }

        // TreeView-Specific Serialization
        public static void SerializeTreeViewToXml(TreeView treeView, string filePath)
        {
            var rootNodes = new List<TreeNodeSerializable>();
            foreach (TreeNode rootNode in treeView.Nodes)
            {
                rootNodes.Add(ConvertToSerializable(rootNode));
            }
            SerializeToXml(rootNodes, filePath);
        }

        public static void DeserializeTreeViewFromXml(TreeView treeView, string filePath)
        {
            var rootNodes = DeserializeFromXml<List<TreeNodeSerializable>>(filePath);
            treeView.Nodes.Clear();
            foreach (var rootNode in rootNodes)
            {
                treeView.Nodes.Add(ConvertToTreeNode(rootNode));
            }
        }

        // Helper Methods for TreeView
        private static TreeNodeSerializable ConvertToSerializable(TreeNode node)
        {
            var serializableNode = new TreeNodeSerializable
            {
                Data = node.Tag as Container, // Assuming Container is stored in the Tag property
                Children = new List<TreeNodeSerializable>()
            };

            foreach (TreeNode childNode in node.Nodes)
            {
                serializableNode.Children.Add(ConvertToSerializable(childNode));
            }

            return serializableNode;
        }

        private static TreeNode ConvertToTreeNode(TreeNodeSerializable serializableNode)
        {
            var treeNode = new TreeNode
            {
                Text = serializableNode.Data.Name, // Display text, e.g., Name property
                Tag = serializableNode.Data       // Store the Container object in the Tag
            };

            foreach (var childSerializableNode in serializableNode.Children)
            {
                treeNode.Nodes.Add(ConvertToTreeNode(childSerializableNode));
            }

            return treeNode;
        }
    }
}
