using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ThePalace.Network.Exts.System.Net.Sockets;

public static class NetworkExts
{
    [DllImport("libc")]
    public static extern int setsockopt(int sockfd, int level, int optname, byte[] optval, int optlen);

    public static void DropConnection(this Socket handler)
    {
        if (handler == null) return;

        var actions = new List<Action>
        {
            () => handler.Disconnect(false),
            () => handler.Shutdown(SocketShutdown.Both),
            () => handler.Close(),
            () => handler.Dispose()
        };

        foreach (var action in actions)
            try
            {
                action();
            }
            catch
            {
            }
    }

    public static void DropConnection(this NetworkStream handler)
    {
        if (handler == null) return;

        var actions = new List<Action>
        {
            handler.Close,
            handler.Dispose
        };

        foreach (var action in actions)
            try
            {
                action();
            }
            catch
            {
            }
    }

    public static void SetKeepAlive(this Socket handler, bool on = true, int keepAliveInterval_InMilliseconds = 15000,
        int keepAliveTime_InMilliseconds = 15000)
    {
        var size = Marshal.SizeOf(new uint());

        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            setsockopt((int)handler.Handle, /* SOL_SOCKET */ 0x01, /* SO_KEEPALIVE */ 0x09,
                BitConverter.GetBytes(on ? 1 : 0), size);
            setsockopt((int)handler.Handle, /* IPPROTO_TCP */ 0x06, /* TCP_KEEPIDLE */ 0x04,
                BitConverter.GetBytes(keepAliveInterval_InMilliseconds), size);
            setsockopt((int)handler.Handle, /* IPPROTO_TCP */ 0x06, /* TCP_KEEPINTVL */ 0x05,
                BitConverter.GetBytes(keepAliveInterval_InMilliseconds), size);
        }
        else
        {
            var inOptionValues = new byte[size * 3];

            BitConverter.GetBytes((uint)(on ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)keepAliveTime_InMilliseconds).CopyTo(inOptionValues, size);
            BitConverter.GetBytes((uint)keepAliveInterval_InMilliseconds).CopyTo(inOptionValues, size * 2);

            handler.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }
    }

    public static IPAddress? GetIPAddress(this Socket handler)
    {
        return ((IPEndPoint)handler?.RemoteEndPoint)?.Address;
    }

    public static int? GetPort(this Socket handler)
    {
        return ((IPEndPoint)handler?.RemoteEndPoint)?.Port;
    }

    public static IPAddress Resolve(this string hostname, AddressFamily addressFamily = AddressFamily.InterNetwork)
    {
        return Dns.GetHostAddresses(hostname)
            .FirstOrDefault(addr =>
                addr.AddressFamily == addressFamily);
    }
}