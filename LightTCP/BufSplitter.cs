using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class BufSplitter
{
    public static List<byte[]> ConstructPacket(Packet packet, Bits serverPacketBitDepth)
    {
        BitSet buffer = new BitSet(BitBufUtils.GetBits(packet.ID, serverPacketBitDepth));

        int idEnd = BitBufUtils.GetByteCount(buffer.BitsCount);

        WriteBitBuf packetBuf = new WriteBitBuf();
        packet.Write(packetBuf);
        BitSet body = packetBuf.Set;

        int lengthBit = buffer.BitsCount + body.BitsCount + (packet.StaticSize == null ? packet.BitDepth : 0);
        int length = BitBufUtils.GetByteCount(lengthBit);

        int lengthEnd = 0;

        if (packet.StaticSize == null)
        {
            buffer.AddBits(BitBufUtils.GetBits(BitBufUtils.GetByteCount(lengthBit - (packet.BitDepth + buffer.BitsCount)), packet.BitDepth));
            lengthEnd = BitBufUtils.GetByteCount(buffer.BitsCount);
        }

        buffer.AddBits(body);

        byte[] bytes = buffer.AsBytes;
        List<byte[]> parts = new List<byte[]>();        

        if(idEnd != length)
        {
            parts.Add(bytes[0..idEnd]);
            if (packet.StaticSize == null)
            {
                if (lengthEnd == idEnd)
                    parts.Add(bytes[lengthEnd..length]);
                else
                {
                    parts.Add(bytes[idEnd..lengthEnd]);
                    parts.Add(bytes[lengthEnd..length]);
                }
            }
            else parts.Add(bytes[idEnd..length]);
        } else parts.Add(bytes);

        return parts;
    }
    /*
    public async BitSet AcceptFromPacket()
    {
        int packetIDBufferLength = BitBufUtils.GetByteCount(Server.PacketIDBitDepth);
        byte[] packetIDBuffer = new byte[packetIDBufferLength];
        BitSet packetSet = new BitSet();
        if (await stream.ReadAsync(packetIDBuffer, 0, packetIDBufferLength) != packetIDBuffer.Length)
            return null;

        packetSet.AddBits(packetIDBuffer);

        ulong packetID = BitBufUtils.GetUInt(new BitSet(packetIDBuffer).GetBits(0, (int)Server.PacketIDBitDepth));

        if (!Server.Packets.ContainsKey(packetID))
            return null;

        OutPacket instantPacket = (OutPacket)Activator.CreateInstance(Server.Packets[packetID].GetType());
        OutPacket packet;

        int length = 0;
        int packetBitDepth = (int)(Bits)instantPacket.BitDepth;
        if (instantPacket.StaticSize == null)
        {
            if (packetSet.BitsCount + packetBitDepth <= 8)
                length = (int)BitBufUtils.GetUInt(packetSet.GetBits((int)Server.PacketIDBitDepth, packetBitDepth));
            else
            {
                int alreadyHasBits = (packetSet.BitsCount - (int)Server.PacketIDBitDepth);
                int needBits = packetBitDepth - alreadyHasBits;
                int needBytes = BitBufUtils.GetByteCount(needBits);
                byte[] lengthBuffer = new byte[needBytes];
                if (await stream.ReadAsync(lengthBuffer, 0, needBytes) != lengthBuffer.Length)
                    break;
                packetSet.AddBits(lengthBuffer);
            }
        }
        else
        {
            length = (int)instantPacket.StaticSize;
            if (packetSet.BitsCount - ((int)Server.PacketIDBitDepth + packetBitDepth) >= length)
                packet = (OutPacket)Activator.CreateInstance(Server.Packets[packetID].GetType(), new ReadBitBuf(new BitSet(packetSet.GetBits(((int)Server.PacketIDBitDepth + packetBitDepth), length))));
        }

        byte[] buffer = new byte[length];
        int count = 0;
        int bytesReceived;
        while (count < length)
        {
            bytesReceived = await stream.ReadAsync(buffer, count, buffer.Length - count);
            count += bytesReceived;
        }

        packet = (OutPacket)Activator.CreateInstance(Server.Packets[packetID].GetType(), new ReadBitBuf(buffer));
        NewPacket?.Invoke(this, packet);
    }
    */
}