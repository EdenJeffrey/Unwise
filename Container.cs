namespace Unwise
{
    // Parent container class, all containers type will derive from this class
    public class Container
    {
        public string Name { get; set; }
        public float Volume { get; set; }
        public float Pitch { get; set; }

        public Container(string name)
        {
            Name = name;
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
