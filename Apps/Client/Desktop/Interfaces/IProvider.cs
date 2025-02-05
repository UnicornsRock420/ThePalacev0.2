namespace ThePalace.Client.Desktop.Interfaces
{
    public interface IProvider : IFeature
    {
        object Provide(params object[] args);
    }
}