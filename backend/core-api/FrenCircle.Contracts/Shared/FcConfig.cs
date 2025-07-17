namespace FrenCircle.Contracts.Shared
{
    public class FcConfig
    {
        public Toggles? Toggles { get; set; }
    }
    public class Toggles
    {
        public bool IncludeResponseTime { get; set; }
    }
}
