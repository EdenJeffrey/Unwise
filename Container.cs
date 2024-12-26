using System.Xml.Serialization;

namespace Unwise
{
    // Parent container class, all containers type will derive from this class
    [XmlInclude(typeof(AudioContainer))]
    [XmlInclude(typeof(RandomContainer))]
    [XmlInclude(typeof(MultiContainer))]
    public class Container
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public float Volume { get; set; }
        public float Pitch { get; set; }
        public Container() { }

        public Container(string name)
        {
            Name = name;
            Type = GetType().Name;
            Volume = 1;
            Pitch = 1;
        }
        public virtual Container GetContainerToPlay()
        {
            return this;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
