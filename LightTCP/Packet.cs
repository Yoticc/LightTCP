using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public abstract class Packet
{
    public Packet(ulong id, int size)
    {
        ID = id;
        StaticSize = size;
    }
    public Packet(ulong id, Bits depth)
    {
        ID = id;
        BitDepth = depth;
    }
    public readonly ulong ID;
    public readonly int? StaticSize;
    public readonly Bits BitDepth;
    //Write to buffer
    public abstract void Write(WriteBitBuf buf);
    //Read from buffer
    public abstract void Read(ReadBitBuf buf);
}