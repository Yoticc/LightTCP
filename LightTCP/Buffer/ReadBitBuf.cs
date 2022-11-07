using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class ReadBitBuf
{
    public ReadBitBuf(BitSet set)
    {
        Set = set;
    }

    public ReadBitBuf(byte[] bytes) : this(new BitSet(bytes)) { }

    public ReadBitBuf(byte[] bytes, int bitsCount) : this(new BitSet(bytes.ToList(), bitsCount)) { }

    public BitSet Set { get; private set; }
    public int Cur { get; private set; } = 0;

    public long ReadInt(Bits depth)
    {
        long value = BitBufUtils.GetInt(Set.GetBits(Cur, (int)depth));
        Cur += (int)depth;
        return value;
    }

    public ulong ReadUInt(Bits depth)
    {
        ulong value = BitBufUtils.GetUInt(Set.GetBits(Cur, (int)depth));
        Cur += (int)depth;
        return value;
    }

    public void WriteInt(ulong value, Bits depth)
    {
        Set.SetBits(BitBufUtils.GetBits((long)value, depth - 1), Cur += (int)depth - 1);
        Set.SetBit(Cur++, value > long.MaxValue);
    }
}