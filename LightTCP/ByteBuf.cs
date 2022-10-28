using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConquestServer.Libraries;
public class ByteBuf
{
    public ByteBuf(byte[] bytes)
    {
        Bytes = bytes;
        reader = new BinaryReader(new MemoryStream(bytes));
    }
    public byte[] Bytes { get; private set; }
    private BinaryReader reader;
    public string ReadUtf8()
    {
        int length = reader.ReadInt32();
        return Encoding.ASCII.GetString(reader.ReadBytes(length));
    }
    public short ReadShort() => reader.ReadInt16();
    public byte ReadByte() => reader.ReadByte();
    public byte[] ReadBytes(int count) => reader.ReadBytes(count);
    public bool[] ReadBits(int count) => BytesToBits(ReadBytes(count / 8 + (count % 8 == 0 ? 0 : 1)));
    private static bool[] ByteToBits(byte @byte)
    {
        var values = new bool[8];

        values[0] = (@byte & 1) != 0;
        values[1] = (@byte & 2) != 0;
        values[2] = (@byte & 4) != 0;
        values[3] = (@byte & 8) != 0;
        values[4] = (@byte & 16) != 0;
        values[5] = (@byte & 32) != 0;
        values[6] = (@byte & 64) != 0;
        values[7] = (@byte & 128) != 0;

        return values;
    }
    private static bool[] BytesToBits(byte[] bytes)
    {
        BitArray bitArr = new BitArray(bytes);
        bool[] result = new bool[bitArr.Length];
        bitArr.CopyTo(result, 0);
        return result;
    }
}