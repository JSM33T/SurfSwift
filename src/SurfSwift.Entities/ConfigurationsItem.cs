namespace SurfSwift.Entities
{
    public class ConfigurationItem
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public required string Value { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class ActionScript
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string JsonScript { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string JsonData { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
