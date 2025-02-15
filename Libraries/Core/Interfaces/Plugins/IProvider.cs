namespace ThePalace.Core.Interfaces.Plugins
{
    public interface IProvider : IFeature
    {
        object Provide(params object[] args);
    }
}