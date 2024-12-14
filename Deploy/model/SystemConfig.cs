namespace Deploy.model
{
    public class SystemConfig
    {
        public string? ApplicationName { get; set; }
        public string? ApplicationLocation { get; set; }
        public string? ServerName { get; set; }
        public required string ServerStartFileRelativeLocation { get; set; }
        public required string ServerStopFileRelativeLocation { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
