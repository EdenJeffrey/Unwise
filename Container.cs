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
        public float Volume { get; set; }
        [DataMember]
        public float Pitch { get; set; }
        public Container()
        {
            Name = GetType().Name;
            Volume = 0.0f;
            Pitch = 0.0f;
        }

        public Container(string name)
        {
            Name = name;
            Volume = 0.0f;
            Pitch = 0.0f;
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
