using System.Runtime.Serialization;

namespace Unwise
{
    // Basic audio container, stores path to file for playback
    [DataContract]
    public class AudioContainer : Container
    {
        [DataMember]
        public string AudioFilePath { get; set; }

        public AudioContainer()
        {
            AudioFilePath = "";
        }
        public AudioContainer(string name) : base(name)
        {
            AudioFilePath = "";
        }

        public AudioContainer(string name, string audioFilePath) : base(name)
        {
            AudioFilePath = audioFilePath;
        }

        public override Container GetContainerToPlay()
        {
            return this;
        }
        public override string ToString()
        {
            return $"{Name} (Audio: {AudioFilePath})";
        }
    }
}
