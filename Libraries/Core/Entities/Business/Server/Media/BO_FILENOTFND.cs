using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Server.Media
{
    [Mnemonic("fnfe")]
    public partial class BO_FILENOTFND : IEventHandler<MSG_FILENOTFND>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}