using System.Runtime.Serialization;

namespace Unwise_Serialization
{
    [DataContract]
    public class Container
    {
        [DataMember]
        public Unwise.Container Data { get; set; }

        [DataMember]
        public List<Container> Children { get; set; } = new List<Container>();
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
            var rootNodes = new List<Container>();
            foreach (TreeNode rootNode in treeView.Nodes)
            {
                rootNodes.Add(ConvertToSerializable(rootNode));
            }
            SerializeToXml(rootNodes, filePath);
        }

        public static void DeserializeTreeViewFromXml(TreeView treeView, string filePath)
        {
            var rootNodes = DeserializeFromXml<List<Container>>(filePath);
            treeView.Nodes.Clear();
            foreach (var rootNode in rootNodes)
            {
                treeView.Nodes.Add(ConvertToTreeNode(rootNode));
            }
        }

        // Helper Methods for TreeView
        private static Container ConvertToSerializable(TreeNode node)
        {
            var serializableNode = new Container
            {
                Data = node.Tag as Unwise.Container, // Assuming Container is stored in the Tag property
                Children = new List<Container>()
            };

            foreach (TreeNode childNode in node.Nodes)
            {
                serializableNode.Children.Add(ConvertToSerializable(childNode));
            }

            return serializableNode;
        }

        private static TreeNode ConvertToTreeNode(Container serializableNode)
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
