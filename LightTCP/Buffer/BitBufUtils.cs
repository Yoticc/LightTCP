using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class BitBufUtils
{
    public static int GetByteCount(Bits bitDepth) => (byte)bitDepth / 8 + ((byte)bitDepth % 8 == 0 ? 0 : 1);
    public static int GetByteCount(uint bitsCount) => (byte)bitsCount / 8 + ((byte)bitsCount % 8 == 0 ? 0 : 1);
    public static int GetByteCount(int bitsCount) => (byte)bitsCount / 8 + ((byte)bitsCount % 8 == 0 ? 0 : 1);
    public static int SumBitDepthToInt(params Bits[] bitDepths) => bitDepths.ToList().Sum(z => (int)z);
    public static bool[] GetBits(long value, Bits bitDepth) => GetBits(value, (int)bitDepth);
    public static bool[] GetBits(long value, int bitDepth)
    {
        bool[] bits = new bool[bitDepth];

        if (value < 0)
            bits[^1] = true;

        byte[] buffer = BitConverter.GetBytes(Math.Abs(value));

        for (int i = 0; i < bits.Length - 1; i++)
            bits[i] = (buffer[i / 8] & (1 << (i % 8))) != 0;

        return bits;
    }

    public static bool[] GetBits(ulong value, Bits bitDepth) => GetBits(value, (int)bitDepth);
    public static bool[] GetBits(ulong value, int bitDepth)
    {
        bool[] bits = new bool[bitDepth];

        if (value >= long.MaxValue)
        {
            bits[^1] = true;
            value -= long.MaxValue;
        }

        byte[] buffer = BitConverter.GetBytes(value);

        for (int i = 0; i < bits.Length - 1; i++)
            bits[i] = ((long)buffer[i / 8] & (1 << (i % 8))) != 0;

        return bits;
    }
    
    public static bool[] GetBits(byte[] bytes, int pos, int count) => new BitSet(bytes).GetBits(pos, count);

    public static long GetInt(bool[] bits)
    {
        if (bits.Length == 1)
            return bits[0] ? 1 : 0;

        long value = 0;
        for(int i = 0; i < bits.Length - 1; i++)
            if (bits[i])
                value |= (long)1 << i;

        return bits[^1] ? -value : value;
    }

    public static ulong GetUInt(bool[] bits)
    {
        if (bits.Length == 1)
            return (ulong)(bits[0] ? 1 : 0);

        long value = 0;
        for (int i = 0; i < bits.Length - 1; i++)
            if (bits[i])
                value |= (long)1 << i;

        return bits[^1] ? ((ulong)value + long.MaxValue) : (ulong)value;
    }

    public static byte ToByte(bool[] bits)
    {
        byte value = 0;
        for (int i = 0; i < bits.Length - 1; i++)
            if (bits[i])
                value |= (byte)(1 << i);
        return value;
    }
}