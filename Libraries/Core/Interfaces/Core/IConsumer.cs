namespace ThePalace.Core.Interfaces.Core
{
    public interface IConsumer : IFeature
    {
        void Consume(params object[] args);
    }
}
