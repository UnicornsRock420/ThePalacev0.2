﻿using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.EventsBus;
using Lib.Logging.Entities;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("navR")]
public class BO_ROOMGOTO : IEventHandler<MSG_ROOMGOTO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        if (sender is not IUserSessionState sessionState ||
            @event is not ProtocolEventParams { Request: MSG_ROOMGOTO inboundPacket } @params) return null;

        LoggerHub.Current.Debug(nameof(BO_ROOMGOTO) + $"[{@params.SourceID}]: {@params.RefNum}");

        throw new NotImplementedException(nameof(BO_ROOMGOTO));

        return null;
    }
}