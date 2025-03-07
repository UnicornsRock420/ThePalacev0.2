using System.Collections;
using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Entities;
using ThePalace.Network.Interfaces;

namespace ThePalace.Common.Server.Entities.Core
{
    public class ServerSessionState : Disposable, IServerSessionState
    {
        public ServerSessionState() { }
        ~ServerSessionState() => this.Dispose();

        public void Dispose()
        {
            ConnectionState?.Dispose();
            ConnectionState = null;

            UserDesc = null;
            RegInfo = null;

            LastActivity = null;

            base.Dispose();
        }

        public Guid Id => Guid.NewGuid();
        public uint UserId { get; set; }

        public DateTime? LastActivity { get; set; }

        public IConnectionState? ConnectionState { get; set; } = new ConnectionState();

        public UserDesc? UserDesc { get; set; } = new();
        public RegistrationRec? RegInfo { get; set; } = new();

        public object? State { get; set; }
    }
}