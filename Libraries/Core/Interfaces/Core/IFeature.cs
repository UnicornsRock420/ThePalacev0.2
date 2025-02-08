using ThePalace.Core.Enums.App;

namespace ThePalace.Core.Interfaces.Core
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