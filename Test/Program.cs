using LightTCP;
using System.Collections;

/*
WriteBitBuf w = new WriteBitBuf();

long value = 100;

Console.WriteLine(value);

w.WriteInt(value, BitsEnum.X12);

byte[] bytes = w.AsBytes;

ReadBitBuf r = new ReadBitBuf(bytes);

Console.WriteLine(r.ReadInt(BitsEnum.X12));
*/
Ping ping = new Ping();

List<byte[]> netPackets = BufSplitter.FromServerToClient(ping, Bits.X4);

_ = 3;

//[.... ,,,,] 
//[,,,, ....]
//[........] [........] [.... ||||]

//Static:
//[.... ,,,,]
//[,,,,,,,,] [,,,,,,,,] [,,,, ||||]

class Ping : OutPacket
{
    public Ping() : base(0, 24) { }
    public override void WritePacket(WriteBitBuf buf)
    {
        buf.WriteUInt(3, Bits.X24);
    }
}