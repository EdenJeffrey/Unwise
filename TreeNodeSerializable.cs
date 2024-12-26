using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Unwise
{
    public class TreeNodeSerializable
    {
        [XmlElement]
        public Container Data { get; set; } // Stores the data of the node (polymorphic)

        [XmlArray("Children")]
        [XmlArrayItem("TreeNodeSerializable")]
        public List<TreeNodeSerializable> Children { get; set; } = new List<TreeNodeSerializable>();

        public TreeNodeSerializable() { } // Parameterless constructor required for serialization
    }
}