using Client;
using LightTCP;
using LightTCP.Server;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Connection = Client.Connection;

int port = 8006;
Server server = new Server(port, Bits.X8);

TcpClient client = new TcpClient();
client.Connect(new IPAddress(new byte[] { 127, 0, 0, 1 }, port), port);
Connection con = new Connection(client);

Ping ping = new Ping();

_ = 3;

class Ping : Packet
{
    public Ping() : base(0, 24) { }
    private ulong ping = 3;
    public override void Write(WriteBitBuf buf)
    {
        buf.WriteUInt(ping, Bits.X24);
    }
    public override void Read(ReadBitBuf buf)
    {
        ping = buf.ReadUInt(Bits.X24);
    }
}