using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Media;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Client.Media
{
    [Mnemonic("qFil")]
    public partial class BO_FILEQUERY : IEventHandler<MSG_FILEQUERY>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}