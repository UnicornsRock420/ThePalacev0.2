using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Client.Auth
{
    [Mnemonic("auth")]
    public partial class BO_AUTHENTICATE : IEventHandler<MSG_AUTHENTICATE>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}