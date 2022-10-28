using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class Packet
{
    public ushort ID;
}
public enum PacketEnum : ushort
{
    Connection = 0,
    Position = 1,
    PlaceBlock = 2,
    LoadHighChunk = 3
}