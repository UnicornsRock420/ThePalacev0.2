using ThePalace.Common.Enums.App;

namespace ThePalace.Common.Interfaces.Plugins
{
    public interface IFeature : IDisposable
    {
        string Name { get; }
        string Description { get; }

        DeviceTypes[] Devices { get; }
        FeatureTypes[] Features { get; }
        SubFeatureTypes[] SubFeatures { get; }
        PurposeTypes Purpose { get; }

        void Initialize(params object[] args);
    }
}