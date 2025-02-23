using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Media
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