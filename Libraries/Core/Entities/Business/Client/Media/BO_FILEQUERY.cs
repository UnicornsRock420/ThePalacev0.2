using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Media;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Client.Media
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