using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public enum StreamFlags : byte
{
    Read,
    Write,
    ReadAndWrite
}