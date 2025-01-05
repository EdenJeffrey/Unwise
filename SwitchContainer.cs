using System.Runtime.Serialization;

namespace Unwise
{
    // Randomly selects a descendant container for playback, hashes previously played containers for performant comparison
    [DataContract]
    public class SwitchContainer : MultiContainer
    {
        public struct SwitchGroupingInfo
        {
            public string SwitchName;
            public List<int> ItemHashes;

            public SwitchGroupingInfo(string switchName, List<int> itemHashes)
            {
                SwitchName = switchName;
                ItemHashes = itemHashes;
            }
        }

        private List<string> SwitchesTemp = new List<string> 
        { 
            "one",
            "two",
            "three",
            "four",
            "five"
        };

        [DataMember]
        public override List<Container> Containers { get; set; }

        [DataMember]
        public List<SwitchGroupingInfo> SwitchGroupings { get; set; }

        public SwitchContainer()
        {
            Containers = new List<Container>();
            SwitchGroupings = new List<SwitchGroupingInfo>();
        }
        public SwitchContainer(string name) : base(name)
        {
            Containers = new List<Container>();
            SwitchGroupings = new List<SwitchGroupingInfo>();
        }

        public SwitchContainer(string name, IEnumerable<Container> containers = null) : base(name)
        {
            Containers = containers?.ToList() ?? new List<Container>();
            SwitchGroupings = new List<SwitchGroupingInfo>();
        }

        public override Container GetContainerToPlay()
        {
            return Containers[0];
        }
    }
}