namespace CustomServerCore
{
    public interface ICustomPlugin
    {
        string Name { get; }
        string Version { get; }

        void Load();
        void Unload();
    }
}