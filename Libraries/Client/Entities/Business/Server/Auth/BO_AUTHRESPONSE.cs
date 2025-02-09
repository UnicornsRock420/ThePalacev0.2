using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Server.Auth
{
    [Mnemonic("autr")]
    public partial class BO_AUTHRESPONSE : IEventHandler<MSG_AUTHRESPONSE>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}