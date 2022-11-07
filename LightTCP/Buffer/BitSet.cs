using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class BitSet
{
    public BitSet(params byte[] bytes)
    {
        Bytes = bytes.ToList();
        BitsCount = bytes.Length << 3;
    }

    public BitSet(List<byte> bytes, int bitsCount)
    {
        Bytes = bytes;
        BitsCount = bitsCount;
    }

    public BitSet(int bitsCount)
    {
        Bytes = Enumerable.Repeat((byte)0, bitsCount / 8 + (bitsCount % 8 == 0 ? 0 : 1)).ToList();
        BitsCount = bitsCount;
    }

    public BitSet(bool[] bits)
    {
        Bytes = new List<byte>();
        BitsCount = 0;
        SetBits(bits, 0);
    }

    public List<byte> Bytes { get; private set; }
    public int BitsCount { get; private set; }

    public void SetBits(bool[] bits, int startIndex)
    {
        if(BitsCount < startIndex + bits.Length)
            AllocateNewBits(startIndex + bits.Length - BitsCount);
        for (int i = 0; i < bits.Length; i++)
            if (bits[i])
                SetBit(startIndex + i, bits[i]);
    }

    public void SetBits(BitSet set, int startIndex) => SetBits(set.AsBits, startIndex);

    public void AllocateNewBits(int bitsCount)
    {
        int newBitsCount = BitsCount + bitsCount;
        int newByteCount = newBitsCount / 8 + (newBitsCount % 8 == 0 ? 0 : 1);
        if(newByteCount != Bytes.Count)
            Bytes.AddRange(Enumerable.Repeat((byte)0, newByteCount - Bytes.Count));
        BitsCount = newBitsCount;
    }

    public void SetArray(List<byte> bytes, int bitsCount)
    {
        Bytes = bytes;
        BitsCount = bitsCount;
    }

    public void AddBit(bool bit) => AddBits(new bool[] { bit });

    public void AddBits(bool[] bits)
    {
        int pos = BitsCount;
        AllocateNewBits(bits.Length);
        SetBits(bits, pos);
    }

    public void AddBits(BitSet set)
    {
        bool[] bits = set.AsBits;
        int pos = BitsCount;
        AllocateNewBits(bits.Length);
        SetBits(bits, pos);
    }

    public void AddBits(byte[] bytes)
    {
        bool[] bits = new bool[bytes.Length << 3];
        for (int b = 0; b < bytes.Length; b++)
            for (int i = 0; i < 8; i++)
                bits[(b << 3) + i] = (bytes[b] & (1 << i)) != 0;

        int pos = BitsCount;
        AllocateNewBits(bits.Length);
        SetBits(bits, pos);
    }

    public bool[] GetBits(int pos, int count)
    {
        bool[] bits = new bool[count];
        for (int i = 0; i < count; i++)
            bits[i] = GetBit(pos + i);
        return bits;
    }

    public bool[] AsBits
    {
        get
        {
            bool[] bits = new bool[BitsCount];
            for (int i = 0; i < BitsCount; i++)
                bits[i] = GetBit(i);
            return bits;
        }
    }

    public byte[] AsBytes => Bytes.ToArray();

    public bool this[int bit]
    {
        get => GetBit(bit);
        set => SetBit(bit, value);
    }

    public bool GetBit(int pos) => (Bytes[pos / 8] & (1 << (pos % 8))) != 0;
    public void SetBit(int pos, bool bit)
    {
        if(bit)
            Bytes[pos / 8] ^= (byte)(1 << (pos % 8));
        else Bytes[pos / 8] = (byte)(Bytes[pos / 8] & ~(1 << (pos % 8)));
    }

    public void SetBit(int pos) => Bytes[pos / 8] ^= (byte) (1 << (pos % 8));

    public byte GetByte(int pos) => Bytes[pos];
    public void SetByte(int pos, byte value) => Bytes[pos] = value;
}