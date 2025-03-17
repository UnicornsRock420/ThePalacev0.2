using System.Runtime.Serialization;
using Lib.Common.Attributes;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Auth;

[Mnemonic("autr")]
public class MSG_AUTHRESPONSE : EventParams, IProtocolS2C
{
    [Str255] public string? NameAndPassword;

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