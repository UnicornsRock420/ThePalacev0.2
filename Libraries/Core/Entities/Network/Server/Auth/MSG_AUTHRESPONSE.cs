using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Server.Auth
{
    [Mnemonic("autr")]
    public partial class MSG_AUTHRESPONSE : IProtocolS2C
    {
        [PString(1, 255)]
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