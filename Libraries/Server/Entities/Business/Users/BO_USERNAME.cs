﻿using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Users;

[DynamicSize(32, 1)]
[Mnemonic("usrN")]
public class BO_USERNAME : IEventHandler<MSG_USERNAME>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_USERNAME inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_USERNAME) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_USERNAME));

        return null;
    }
}