namespace ThePalace.Client.Desktop.Interfaces
{
    public interface IConsumer : IFeature
    {
        void Consume(params object[] args);
    }
}
