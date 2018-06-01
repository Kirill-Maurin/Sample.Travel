namespace Sample.Travel
{
    public sealed class Waypoint
    {
        public Waypoint(string name) => Name = name;

        public string Name { get; }
        public override bool Equals(object obj) => obj is Waypoint w && Name.Equals(w.Name);
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => $"Waypoint: {Name}";
        public static bool operator == (Waypoint left, Waypoint right) => left?.Equals(right) ?? false;
        public static bool operator != (Waypoint left, Waypoint right) => !(left == right);
    }
}
