using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP.Server;
public class Server : IDisposable
{
    public Server(int port, BitsEnum packetIDBitDepth, BitsEnum packetSizeBitDepth)
    {
        listener = new TcpListener(IPAddress.Any, port);
        Clients = new List<Connection>();
        servertask = ListenAsync();
        PacketIDBitDepth = packetIDBitDepth;
        PacketSizeBitDepth = packetSizeBitDepth;
    }
    public readonly BitsEnum PacketIDBitDepth;
    public readonly BitsEnum PacketSizeBitDepth;

    private readonly TcpListener listener;
    public readonly List<Connection> Clients;
    public Dictionary<ulong, Packet> Packets;

    private readonly Task servertask;
    private bool disposed;

    public delegate void NewPacketHandle(Connection client, Packet packet);
    public event NewPacketHandle? NewPacket;
    public delegate void NewConnectionHandle(Connection client);
    public event NewConnectionHandle? NewConnection;
    public delegate void LogHandle(string line);
    public event LogHandle? NewLogMessage;
    public async Task ListenAsync()
    {
        try
        {
            listener.Start();
            Log($"Server[{listener.LocalEndpoint}] started");
            Console.WriteLine();
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Log($"New Connection[{client.Client.RemoteEndPoint}]");
                Console.WriteLine();
                lock (Clients)
                {
                    Connection connection = new Connection(client, c => { lock (Clients) { Clients.Remove(c); } c.Dispose(); }) { Server = this };
                    connection.NewPacket += (con, packet) => NewPacket?.Invoke(con, packet);
                    NewConnection?.Invoke(connection);
                    Clients.Add(connection);
                }
            }
        }
        catch (SocketException)
        {
            Log($"Server[{listener.LocalEndpoint}] stoped");
        }
    }
    private void Log(string line) => NewLogMessage?.Invoke(line);
    public void Stop() => listener.Stop();
    #region Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (disposed)
            throw new ObjectDisposedException(typeof(Server).FullName);
        disposed = true;
        listener.Stop();
        if (disposing)
            lock (Clients)
            {
                if (Clients.Count > 0)
                {
                    Log("Disposing all clients...");
                    foreach (Connection client in Clients)
                        client.Dispose();
                    Log("All clients were disposed.");
                }
            }
    }
    ~Server() => Dispose(false);
#endregion
}
