namespace Deploy.model
{
    public class SystemConfig
    {
        public required string ApplicationProcessName { get; set; }
        public required string ApplicationLocation { get; set; }
        public required string? ApplicationArguments { get; set; }
        public string? ServerName { get; set; }
        public required string ServerWindowName { get; set; }
        public required string ServerStartFileRelativeLocation { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
