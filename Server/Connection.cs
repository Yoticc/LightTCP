using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LightTCP.Server;
public class Connection : IDisposable
{
    public readonly TcpClient Client;
    public readonly EndPoint RemoteEndPoint;
    public event DisposedHandle? Disposed;
    public Server Server;
    public readonly StreamFlags Flags;

    public delegate void NewPacketHandle(Connection client, Packet packet);
    public event NewPacketHandle? NewPacket;

    private readonly NetworkStream stream;
    private readonly Task? readingTask;
    private readonly Task? writingTask;
    private readonly Action<Connection> disposeCallback;
    private readonly Channel<Packet> channel;
    public delegate void DisposedHandle();

    public Connection(TcpClient client, Action<Connection> disposeCallback, StreamFlags flags = StreamFlags.ReadAndWrite)
    {
        Flags = flags;
        Client = client;
        stream = client.GetStream();
        RemoteEndPoint = client.Client.RemoteEndPoint;
        this.disposeCallback = disposeCallback;
        channel = Channel.CreateUnbounded<Packet>();

        if(flags == StreamFlags.Read || flags == StreamFlags.ReadAndWrite)
            readingTask = RunReadingLoop();

        if(flags == StreamFlags.Write || flags == StreamFlags.ReadAndWrite)
            writingTask = RunWritingLoop();
    }

    private async Task RunReadingLoop()
    {
        await Task.Yield();
        try
        {
            while (true)
            {
                int idBitEnd = Server.PacketIDBitDepth;
                int idEnd = BitBufUtils.GetByteCount(idBitEnd);
                byte[] idBuffer = new byte[idEnd];
                await stream.ReadAsync(idBuffer, 0, idBuffer.Length);

                int cur = 0;

                ulong id = BitBufUtils.GetUInt(new BitSet(idBuffer).GetBits(0, Server.PacketIDBitDepth));

                cur += Server.PacketIDBitDepth;

                Packet instancePacket = (Packet)Activator.CreateInstance(Server.Packets[id].GetType());

                int length;

                if (instancePacket.StaticSize == null)
                {
                    int packetSizeLength = instancePacket.BitDepth;
                    byte[] lengthBuffer = new byte[BitBufUtils.GetByteCount(instancePacket.BitDepth - BitBufUtils.GetByteCount(cur) - Server.PacketIDBitDepth)];
                    await stream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                    length = (int)BitBufUtils.GetUInt(new BitSet(lengthBuffer).AsBits);
                }
                else length = (int)instancePacket.StaticSize;

                byte[] buffer = new byte[length];

                ReadBitBuf bitbuf = new ReadBitBuf(buffer);

                instancePacket.Read(bitbuf);

                NewPacket?.Invoke(this, instancePacket);
            }
            Console.WriteLine($"Client[{RemoteEndPoint}] has been disconnected.");
            stream.Close();
        }
        catch (IOException) { Console.WriteLine($"Lost connect to {RemoteEndPoint}."); }
        catch (Exception ex) { Console.WriteLine(ex.GetType().Name + ": " + ex.Message); }
        
        if (!disposed)
            disposeCallback(this);
    }

    private async Task RunWritingLoop()
    {
        await foreach (Packet packet in channel.Reader.ReadAllAsync())
            foreach (byte[] part in BufSplitter.ConstructPacket(packet, Server.PacketIDBitDepth))
                await stream.WriteAsync(part, 0, part.Length);
    }

    #region Dispose
    private bool disposed;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        Disposed?.Invoke();
        if (disposed)
            throw new ObjectDisposedException(GetType().FullName);
        disposed = true;
        if (Client.Connected)
        {
            channel.Writer.Complete();
            stream.Close();
            Task.WaitAll(readingTask, writingTask);
        }
        if (disposing)
        {
            Client.Dispose();
        }
    }
    ~Connection() => Dispose(false);
    #endregion
}