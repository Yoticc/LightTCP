using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTCP;
public struct Bits
{
    private static int[] dimensions = Enumerable.Range(1, 64).ToArray();
    public Bits(int d)
    {
        value = dimensions[d - 1];
    }

    public static implicit operator int(Bits value) => value.value;

    public static implicit operator Bits(int value) => new Bits(value);

    private int value;

    ///<summary>
    ///<br>0 or 1</br>
    ///</summary>
    public const int X1 = 1;
    ///<summary>
    ///<br>0 to 3</br>
    ///<br>-2 to 1</br>
    ///</summary>
    public const int X2 = 2;
    ///<summary>
    ///<br>0 to 7</br>
    ///<br>-4 to 3</br>
    ///</summary>
    public const int X3 = 3;
    ///<summary>
    ///<br>0 to 15</br>
    ///<br>-8 to 7</br>
    ///</summary>
    public const int X4 = 4;
    ///<summary>
    ///<br>0 to 31</br>
    ///<br>-16 to 15</br>
    ///</summary>
    public const int X5 = 5;
    ///<summary>
    ///<br>0 to 63</br>
    ///<br>-32 to 31</br>
    ///</summary>
    public const int X6 = 6;
    ///<summary>
    ///<br>0 to 127</br>
    ///<br>-64 to 63</br>
    ///</summary>
    public const int X7 = 7;
    ///<summary>
    ///<br>0 to 255</br>
    ///<br>-128 to 127</br>
    ///</summary>
    public const int X8 = 8;
    ///<summary>
    ///<br>0 to 511</br>
    ///<br>-256 to 255</br>
    ///</summary>
    public const int X9 = 9;
    ///<summary>
    ///<br>0 to 1023</br>
    ///<br>-512 to 511</br>
    ///</summary>
    public const int X10 = 10;
    ///<summary>
    ///<br>0 to 2047</br>
    ///<br>-1024 to 1023</br>
    ///</summary>
    public const int X11 = 11;
    ///<summary>
    ///<br>0 to 4095</br>
    ///<br>-2048 to 2047</br>
    ///</summary>
    public const int X12 = 12;
    ///<summary>
    ///<br>0 to 8191</br>
    ///<br>-4096 to 4095</br>
    ///</summary>
    public const int X13 = 13;
    ///<summary>
    ///<br>0 to 16383</br>
    ///<br>-8192 to 8191</br>
    ///</summary>
    public const int X14 = 14;
    ///<summary>
    ///<br>0 to 32767</br>
    ///<br>-16384 to 16383</br>
    ///</summary>
    public const int X15 = 15;
    ///<summary>
    ///<br>0 to 65535</br>
    ///<br>-32768 to 32767</br>
    ///</summary>
    public const int X16 = 16;
    ///<summary>
    ///<br>0 to 131071</br>
    ///<br>-65536 to 65535</br>
    ///</summary>
    public const int X17 = 17;
    ///<summary>
    ///<br>0 to 262143</br>
    ///<br>-131072 to 131071</br>
    ///</summary>
    public const int X18 = 18;
    ///<summary>
    ///<br>0 to 524287</br>
    ///<br>-262144 to 262143</br>
    ///</summary>
    public const int X19 = 19;
    ///<summary>
    ///<br>0 to 1048575</br>
    ///<br>-524288 to 524287</br>
    ///</summary>
    public const int X20 = 20;
    ///<summary>
    ///<br>0 to 2097151</br>
    ///<br>-1048576 to 1048575</br>
    ///</summary>
    public const int X21 = 21;
    ///<summary>
    ///<br>0 to 4194303</br>
    ///<br>-2097152 to 2097151</br>
    ///</summary>
    public const int X22 = 22;
    ///<summary>
    ///<br>0 to 8388607</br>
    ///<br>-4194304 to 4194303</br>
    ///</summary>
    public const int X23 = 23;
    ///<summary>
    ///<br>0 to 16777215</br>
    ///<br>-8388608 to 8388607</br>
    ///</summary>
    public const int X24 = 24;
    ///<summary>
    ///<br>0 to 33554431</br>
    ///<br>-16777216 to 16777215</br>
    ///</summary>
    public const int X25 = 25;
    ///<summary>
    ///<br>0 to 67108863</br>
    ///<br>-33554432 to 33554431</br>
    ///</summary>
    public const int X26 = 26;
    ///<summary>
    ///<br>0 to 134217727</br>
    ///<br>-67108864 to 67108863</br>
    ///</summary>
    public const int X27 = 27;
    ///<summary>
    ///<br>0 to 268435455</br>
    ///<br>-134217728 to 134217727</br>
    ///</summary>
    public const int X28 = 28;
    ///<summary>
    ///<br>0 to 536870911</br>
    ///<br>-268435456 to 268435455</br>
    ///</summary>
    public const int X29 = 29;
    ///<summary>
    ///<br>0 to 1073741823</br>
    ///<br>-536870912 to 536870911</br>
    ///</summary>
    public const int X30 = 30;
    ///<summary>
    ///<br>0 to 2147483647</br>
    ///<br>-1073741824 to 1073741823</br>
    ///</summary>
    public const int X31 = 31;
    ///<summary>
    ///<br>0 to 4294967295</br>
    ///<br>-2147483648 to 2147483647</br>
    ///</summary>
    public const int X32 = 32;
    ///<summary>
    ///<br>0 to 8589934591</br>
    ///<br>-4294967296 to 4294967295</br>
    ///</summary>
    public const int X33 = 33;
    ///<summary>
    ///<br>0 to 17179869183</br>
    ///<br>-8589934592 to 8589934591</br>
    ///</summary>
    public const int X34 = 34;
    ///<summary>
    ///<br>0 to 34359738367</br>
    ///<br>-17179869184 to 17179869183</br>
    ///</summary>
    public const int X35 = 35;
    ///<summary>
    ///<br>0 to 68719476735</br>
    ///<br>-34359738368 to 34359738367</br>
    ///</summary>
    public const int X36 = 36;
    ///<summary>
    ///<br>0 to 137438953471</br>
    ///<br>-68719476736 to 68719476735</br>
    ///</summary>
    public const int X37 = 37;
    ///<summary>
    ///<br>0 to 274877906943</br>
    ///<br>-137438953472 to 137438953471</br>
    ///</summary>
    public const int X38 = 38;
    ///<summary>
    ///<br>0 to 549755813887</br>
    ///<br>-274877906944 to 274877906943</br>
    ///</summary>
    public const int X39 = 39;
    ///<summary>
    ///<br>0 to 1099511627775</br>
    ///<br>-549755813888 to 549755813887</br>
    ///</summary>
    public const int X40 = 40;
    ///<summary>
    ///<br>0 to 2199023255551</br>
    ///<br>-1099511627776 to 1099511627775</br>
    ///</summary>
    public const int X41 = 41;
    ///<summary>
    ///<br>0 to 4398046511103</br>
    ///<br>-2199023255552 to 2199023255551</br>
    ///</summary>
    public const int X42 = 42;
    ///<summary>
    ///<br>0 to 8796093022207</br>
    ///<br>-4398046511104 to 4398046511103</br>
    ///</summary>
    public const int X43 = 43;
    ///<summary>
    ///<br>0 to 17592186044415</br>
    ///<br>-8796093022208 to 8796093022207</br>
    ///</summary>
    public const int X44 = 44;
    ///<summary>
    ///<br>0 to 35184372088831</br>
    ///<br>-17592186044416 to 17592186044415</br>
    ///</summary>
    public const int X45 = 45;
    ///<summary>
    ///<br>0 to 70368744177663</br>
    ///<br>-35184372088832 to 35184372088831</br>
    ///</summary>
    public const int X46 = 46;
    ///<summary>
    ///<br>0 to 140737488355327</br>
    ///<br>-70368744177664 to 70368744177663</br>
    ///</summary>
    public const int X47 = 47;
    ///<summary>
    ///<br>0 to 281474976710655</br>
    ///<br>-140737488355328 to 140737488355327</br>
    ///</summary>
    public const int X48 = 48;
    ///<summary>
    ///<br>0 to 562949953421311</br>
    ///<br>-281474976710656 to 281474976710655</br>
    ///</summary>
    public const int X49 = 49;
    ///<summary>
    ///<br>0 to 1125899906842623</br>
    ///<br>-562949953421312 to 562949953421311</br>
    ///</summary>
    public const int X50 = 50;
    ///<summary>
    ///<br>0 to 2251799813685247</br>
    ///<br>-1125899906842624 to 1125899906842623</br>
    ///</summary>
    public const int X51 = 51;
    ///<summary>
    ///<br>0 to 4503599627370495</br>
    ///<br>-2251799813685248 to 2251799813685247</br>
    ///</summary>
    public const int X52 = 52;
    ///<summary>
    ///<br>0 to 9007199254740991</br>
    ///<br>-4503599627370496 to 4503599627370495</br>
    ///</summary>
    public const int X53 = 53;
    ///<summary>
    ///<br>0 to 18014398509481983</br>
    ///<br>-9007199254740992 to 9007199254740991</br>
    ///</summary>
    public const int X54 = 54;
    ///<summary>
    ///<br>0 to 36028797018963967</br>
    ///<br>-18014398509481984 to 18014398509481983</br>
    ///</summary>
    public const int X55 = 55;
    ///<summary>
    ///<br>0 to 72057594037927935</br>
    ///<br>-36028797018963968 to 36028797018963967</br>
    ///</summary>
    public const int X56 = 56;
    ///<summary>
    ///<br>0 to 144115188075855871</br>
    ///<br>-72057594037927936 to 72057594037927935</br>
    ///</summary>
    public const int X57 = 57;
    ///<summary>
    ///<br>0 to 288230376151711743</br>
    ///<br>-144115188075855872 to 144115188075855871</br>
    ///</summary>
    public const int X58 = 58;
    ///<summary>
    ///<br>0 to 576460752303423487</br>
    ///<br>-288230376151711744 to 288230376151711743</br>
    ///</summary>
    public const int X59 = 59;
    ///<summary>
    ///<br>0 to 1152921504606846975</br>
    ///<br>-576460752303423488 to 576460752303423487</br>
    ///</summary>
    public const int X60 = 60;
    ///<summary>
    ///<br>0 to 2305843009213693951</br>
    ///<br>-1152921504606846976 to 1152921504606846975</br>
    ///</summary>
    public const int X61 = 61;
    ///<summary>
    ///<br>0 to 4611686018427387903</br>
    ///<br>-2305843009213693952 to 2305843009213693951</br>
    ///</summary>
    public const int X62 = 62;
    ///<summary>
    ///<br>0 to 9223372036854775807</br>
    ///<br>-4611686018427387904 to 4611686018427387903</br>
    ///</summary>
    public const int X63 = 63;
    ///<summary>
    ///<br>0 to 18446744073709551615</br>
    ///<br>-9223372036854775808 to 9223372036854775807</br>
    ///</summary>
    public const int X64 = 64;
}