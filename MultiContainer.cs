namespace Unwise
{
    // Parent multi container class, all multi audio container class derive from this, e.e random, sequence, switch etc etc.
    public class MultiContainer : Container
    {
        public virtual List<Container> Containers { get; set; }

        public MultiContainer() { }
        public MultiContainer(string name) : base(name)
        {
            Containers = new List<Container>();
        }

        public MultiContainer(string name, IEnumerable<Container> containers = null) : base(name)
        {
            Containers = containers?.ToList() ?? new List<Container>();
        }

        public void AddContainer(Container container)
        {
            if (container != null && !Containers.Contains(container))
            {
                Containers.Add(container);
            }
        }

        public bool RemoveContainer(Container container)
        {
            return Containers.Remove(container);
        }

        public void ClearContainers()
        {
            Containers.Clear();
        }

        public override string ToString()
        {
            return $"{Name} (Audio Containers: {Containers.Count})";
        }
    }
}
