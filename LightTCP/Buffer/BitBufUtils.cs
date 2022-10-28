using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public class BitBufUtils
{
    public static int GetByteCount(BitsEnum bitDepth) => (byte)bitDepth / 8 + ((byte)bitDepth % 8 == 0 ? 0 : 1);
    public static int GetByteCount(int bitsCount) => (byte)bitsCount / 8 + ((byte)bitsCount % 8 == 0 ? 0 : 1);
    public static int SumBitDepthToInt(params BitsEnum[] bitDepths) => bitDepths.ToList().Sum(z => (int)z);
}