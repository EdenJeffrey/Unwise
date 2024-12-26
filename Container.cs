using System.Runtime.Serialization;

namespace Unwise
{
    // Parent container class, all containers type will derive from this class
    [DataContract]
    [KnownType(typeof(AudioContainer))]
    [KnownType(typeof(MultiContainer))]
    [KnownType(typeof(RandomContainer))]
    public class Container
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public float Volume { get; set; }
        [DataMember]
        public float Pitch { get; set; }
        public Container()
        {
            Name = string.Empty;
            Type = GetType().Name;
            Volume = 1;
            Pitch = 1;
        }

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
