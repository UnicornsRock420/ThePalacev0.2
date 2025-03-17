namespace Lib.Common.Interfaces.Plugins;

public interface IProvider : IFeature
{
    object Provide(params object[] args);
}