namespace SurfSwift.Entities
{
    public class DynamicAction
    {
        public required string Action { get; set; }
        public string? Element { get; set; }
        public string? Selector { get; set; }
        public int Repeat { get; set; }
        public List<DynamicAction>? Steps { get; set; }
        public List<DynamicAction>? OnSuccess { get; set; }
        public List<DynamicAction>? OnFailure { get; set; }
        public bool IsBypassed { get; set; } = false;
        public string? DownloadPath { get; set; }
    }
}
