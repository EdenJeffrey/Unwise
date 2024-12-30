using System.Runtime.Serialization;

namespace Unwise
{
    // Randomly selects a descendant container for playback, hashes previously played containers for performant comparison
    [DataContract]
    public class SwitchContainer : MultiContainer
    {
        [DataMember]
        public override List<Container> Containers { get; set; }

        public SwitchContainer()
        {
            Containers = new List<Container>();
        }
        public SwitchContainer(string name) : base(name)
        {
            Containers = new List<Container>();
        }

        public SwitchContainer(string name, IEnumerable<Container> containers = null) : base(name)
        {
            Containers = containers?.ToList() ?? new List<Container>();
            Containers = new List<Container>();
        }

        public override Container GetContainerToPlay()
        {
            return Containers[0];
        }
    }
}