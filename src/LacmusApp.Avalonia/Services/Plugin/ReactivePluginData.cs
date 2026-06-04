namespace LacmusApp.Avalonia.Services.Plugin
{
    public class ReactivePluginData
    {
        public string Name { get; }
        public uint Version { get; }
        public uint ApiVersion { get; }

        public ReactivePluginData(string name, uint version, uint apiVersion)
        {
            Name = name;
            Version = version;
            ApiVersion = apiVersion;
        }
    }
}
