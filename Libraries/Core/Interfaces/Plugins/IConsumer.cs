namespace ThePalace.Core.Interfaces.Plugins
{
    public interface IConsumer : IFeature
    {
        void Consume(params object[] args);
    }
}
