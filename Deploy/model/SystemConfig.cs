namespace Deploy.model
{
    public class SystemConfig
    {
        public required string ApplicationProcessName { get; set; }
        public required string ApplicationLocation { get; set; }
        public string? ApplicationArguments { get; set; }
        public required string ServerName { get; set; }
        public required string ServerWindowName { get; set; }
        public required string ServerStartFileRelativeLocation { get; set; }
        public required string ServerStartLog { get; set; }
        public string? ProjectCacheFolderToDelete { get; set; }
        public List<Project>? Projects { get; set; }
    }
}
