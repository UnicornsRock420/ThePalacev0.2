using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Server.Auth
{
    [Mnemonic("autr")]
    public partial class MSG_AUTHRESPONSE : IProtocolS2C
    {
        public PString NameAndPassword;

        [IgnoreDataMember]
        public string? Username => NameAndPassword.Value
            ?.ToString()
            ?.Split(':')
            //?.Skip(0)
            ?.Take(1)
            ?.FirstOrDefault();

        [IgnoreDataMember]
        public string? Password => NameAndPassword.Value
            ?.ToString()
            ?.Split(':')
            ?.Skip(1)
            ?.Take(1)
            ?.FirstOrDefault();
    }
}