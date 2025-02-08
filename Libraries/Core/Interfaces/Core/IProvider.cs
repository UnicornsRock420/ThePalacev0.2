namespace ThePalace.Core.Interfaces.Core
{
    public interface IProvider : IFeature
    {
        object Provide(params object[] args);
    }
}