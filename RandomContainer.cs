using System.Runtime.Serialization;

namespace Unwise
{
    // Randomly selects a descendant container for playback, hashes previously played containers for performant comparison
    [DataContract]
    public class RandomContainer : MultiContainer
    {
        [DataMember]
        public override List<Container> Containers { get; set; }
        [DataMember]
        public int AvoidRepeatingLast { get; set; }
        protected List<int> LastPlayedHashes { get; set; }

        public RandomContainer()
        {
            AvoidRepeatingLast = 1;
            LastPlayedHashes = new List<int>();
            Containers = new List<Container>();
        }
        public RandomContainer(string name) : base(name)
        {
            AvoidRepeatingLast = 1;
            LastPlayedHashes = new List<int>();
            Containers = new List<Container>();
        }

        public RandomContainer(string name, IEnumerable<Container> containers = null) : base(name)
        {
            Containers = containers?.ToList() ?? new List<Container>();
            AvoidRepeatingLast = 1;
            LastPlayedHashes = new List<int>();
            Containers = new List<Container>();
        }

        public override Container GetContainerToPlay()
        {
            if (AvoidRepeatingLast == 0)
            {
                return GetRandomItem(Containers);
            }

            if (LastPlayedHashes == null) LastPlayedHashes = new List<int>();
            if (Containers == null) Containers = new List<Container>();
            // Filter containers to exclude those with hashes in LastPlayedHashes
            var availableContainers = Containers
                .Where(c => c != null && !LastPlayedHashes.Contains(GetContainerHash(c)))
                .ToList();

            // If no available containers, reset the LastPlayedHashes and allow repeating
            if (!availableContainers.Any())
            {
                LastPlayedHashes.Clear();
                availableContainers = Containers.Where(c => c != null).ToList();
            }

            if (availableContainers.Count < 1) return new Container();
            var randomContainer = GetRandomItem(availableContainers);

            // Update LastPlayedHashes
            var hash = GetContainerHash(randomContainer);
            LastPlayedHashes.Add(hash);

            // Trim LastPlayedHashes to the specified length
            if (LastPlayedHashes.Count > AvoidRepeatingLast)
            {
                LastPlayedHashes = LastPlayedHashes.Skip(LastPlayedHashes.Count - AvoidRepeatingLast).ToList();
            }

            return randomContainer;
        }

        // Generate hash for container
        public int GetContainerHash(Container container)
        {
            return container.GetHashCode();
        }

        // Random item from list helper function, with a static cached random
        private static Random random = new Random();
        public static T GetRandomItem<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("The list cannot be null or empty.");
            }

            int randomIndex = random.Next(list.Count);
            return list[randomIndex];
        }
    }
}