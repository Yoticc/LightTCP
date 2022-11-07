using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Client;
public class Connection : IDisposable
{
    private readonly TcpClient client;
    private readonly NetworkStream stream;
    private readonly EndPoint remoteEndPoint;
    private readonly Task readingTask;
    private readonly Task writingTask;
    private readonly Channel<string> channel;
    bool disposed;

    public Connection(TcpClient client)
    {
        this.client = client;
        stream = client.GetStream();
        remoteEndPoint = client.Client.RemoteEndPoint;
        channel = Channel.CreateUnbounded<string>();
        readingTask = RunReadingLoop();
        writingTask = RunWritingLoop();
    }

    private async Task RunReadingLoop()
    {
        try
        {
            byte[] headerBuffer = new byte[4];
            while (true)
            {
                int bytesReceived = await stream.ReadAsync(headerBuffer, 0, headerBuffer.Length);
                if (bytesReceived != 4)
                    break;
                int length = BinaryPrimitives.ReadInt32LittleEndian(headerBuffer);
                byte[] buffer = new byte[length];
                int count = 0;
                while (count < length)
                {
                    bytesReceived = await stream.ReadAsync(buffer, count, buffer.Length - count);
                    count += bytesReceived;
                }
            }
            stream.Close();
        }
        catch (IOException)
        {
            Console.WriteLine($"Connection has been closed");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType().Name + ": " + ex.Message);
        }
    }

    public async Task SendString(string message)
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            throw new ObjectDisposedException(GetType().FullName);
        disposed = true;
        if (client.Connected)
        {
            channel.Writer.Complete();
            stream.Close();
            Task.WaitAll(readingTask, writingTask);
        }
        if (disposing)
            client.Dispose();
    }

    ~Connection() => Dispose(false);
}