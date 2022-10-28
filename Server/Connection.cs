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
    public delegate void NewPacketHandle(Connection client, Packet buf);
    public event NewPacketHandle? NewPacket;
    private readonly NetworkStream stream;
    private readonly Task readingTask;
    private readonly Task writingTask;
    private readonly Action<Connection> disposeCallback;
    private readonly Channel<string> channel;
    public delegate void DisposedHandle();
    public event DisposedHandle? Disposed;
    public Server Server;
    public Connection(TcpClient client, Action<Connection> disposeCallback)
    {
        Client = client;
        stream = client.GetStream();
        RemoteEndPoint = client.Client.RemoteEndPoint;
        this.disposeCallback = disposeCallback;
        channel = Channel.CreateUnbounded<string>();
        readingTask = RunReadingLoop();
        writingTask = RunWritingLoop();
    }
    private async Task RunReadingLoop()
    {
        await Task.Yield();
        try
        {
            byte[] packetIDBuffer = new byte[4];
            int firstBytesReadCount = BitBufUtils.GetByteCount(Server.PacketIDBitDepth);
            //int fullBytesReadCount = BitBufUtils.GetByteCount(Server.PacketSizeBitDepth, firstBytesReadCount);
            while (true)
            {
                int receivedPacketID = await stream.ReadAsync(packetIDBuffer, 0, firstBytesReadCount);
                if (receivedPacketID != firstBytesReadCount)
                    break;

                ulong id = BinaryPrimitives.ReadUInt16LittleEndian(packetIDBuffer);
                int receivedHeader = await stream.ReadAsync(headerBuffer, 0, 4);
                if (receivedHeader != 4)
                    break;
                int length = BinaryPrimitives.ReadInt32LittleEndian(headerBuffer);
                if (length > short.MaxValue)
                {
                    Console.WriteLine("StackOverflow: packet buffer");
                    break;
                }
                byte[] buffer = new byte[length];
                int count = 0;
                int bytesReceived;
                while (count < length)
                {
                    bytesReceived = await stream.ReadAsync(buffer, count, buffer.Length - count);
                    count += bytesReceived;
                }
                Packet packet = (Packet)Activator.CreateInstance(Parent.Packets[id].GetType().AssemblyQualifiedName, new ByteBuf(buffer));
                packet.ID = id;
                NewPacket?.Invoke(this, packet);
            }
            #if DEBUG
            Console.WriteLine($"Client {RemoteEndPoint} has been disconnected.");
            #endif
            #if RELEASE
            Console.WriteLine("Client {IP Censored} has been disconnected");
            #endif
            stream.Close();
        }
        catch (IOException)
        {
            #if DEBUG
            Console.WriteLine($"Lost connect to {RemoteEndPoint}.");
            #endif
            #if RELEASE
            Console.WriteLine("Lost connect to {IP Censored}");
            #endif
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType().Name + ": " + ex.Message);
        }
        if (!disposed)
            disposeCallback(this);
    }
    public async Task SendMessage(string message)
    {
        await channel.Writer.WriteAsync(message);
    }
    private async Task RunWritingLoop()
    {
        byte[] header = new byte[4];
        await foreach (string message in channel.Reader.ReadAllAsync())
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            BinaryPrimitives.WriteInt32LittleEndian(header, buffer.Length);
            await stream.WriteAsync(header, 0, header.Length);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
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