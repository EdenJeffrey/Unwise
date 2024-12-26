using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Unwise
{
    public static class Serialization
    {
        // XML Serialization
        public static void SerializeToXml<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T DeserializeFromXml<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
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