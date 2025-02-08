using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Auth
{
    [Mnemonic("autr")]
    public partial class MSG_AUTHRESPONSE : IntegrationEvent, IProtocolS2C
    {
        [Str255]
        public string? NameAndPassword;

        [IgnoreDataMember]
        public string? Username => NameAndPassword
            ?.Split(':')
            //?.Skip(0)
            ?.Take(1)
            ?.FirstOrDefault();

        [IgnoreDataMember]
        public string? Password => NameAndPassword
            ?.Split(':')
            ?.Skip(1)
            ?.Take(1)
            ?.FirstOrDefault();
    }
}