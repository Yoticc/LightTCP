using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class WriteBitBuf
{
    public WriteBitBuf() 
    {
        Set = new BitSet(0);
        Cur = 0;
    }

    public WriteBitBuf(BitSet set)
    {
        Set = set;
        Cur = set.BitsCount;
    }

    public WriteBitBuf(byte[] bytes) : this(new BitSet(bytes)) { }

    public WriteBitBuf(byte[] bytes, int bitsCount) : this(new BitSet(bytes.ToList(), bitsCount)) { }

    public BitSet Set { get; private set; }
    public int Cur { get; private set; }

    public byte[] AsBytes => Set.Bytes.ToArray();

    public void WriteInt(long value, Bits depth)
    {
        Set.SetBits(BitBufUtils.GetBits(value, depth), Cur);
        Cur += (int)depth;
    }

    public WriteBitBuf WriteUInt(ulong value, Bits depth)
    {
        Set.SetBits(BitBufUtils.GetBits(value, depth), Cur);
        Cur += (int)depth;
        return this;
    }

    public void WriteBytes(byte[] bytes)
    {
        Set.AddBits(bytes);
        Cur += bytes.Length << 3;
    }

    public void WriteBoolean(bool value)
    {
        Set.AddBit(value);
        Cur++;
    }

    public void WriteBits(bool[] bits)
    {
        Set.AddBits(bits);
        Cur += bits.Length;
    }
}