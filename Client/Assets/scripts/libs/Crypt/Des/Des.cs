using System;
using UnityEngine;
namespace SDK.Lib
{
    // typedef unsigned char DES_cblock[8];
    // typedef /* const */ unsigned char const_DES_cblock[8];
    // #define DES_LONG unsigned long

    public class UnionCls
    {
        public ulong[] deslong;         //  2

        public UnionCls()
        {
            deslong = new ulong[2];
        }
    }

    public class DES_key_schedule : CryptKeyBase
    {
        public UnionCls[] ks;        // 16

        public DES_key_schedule()
        {
            ks = new UnionCls[16];

            for(int idx = 0; idx < 16; ++idx)
            {
                ks[idx] = new UnionCls();
            }
        }
    };

    public class Dec
    {
        public const int ITERATIONS = 16;
        public static int[] shifts2 = { 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 };

        // [8][64]
        public static ulong[,] DES_SPtrans = {
            {
                /* nibble 0 */
                0x02080800L, 0x00080000L, 0x02000002L, 0x02080802L,
                0x02000000L, 0x00080802L, 0x00080002L, 0x02000002L,
                0x00080802L, 0x02080800L, 0x02080000L, 0x00000802L,
                0x02000802L, 0x02000000L, 0x00000000L, 0x00080002L,
                0x00080000L, 0x00000002L, 0x02000800L, 0x00080800L,
                0x02080802L, 0x02080000L, 0x00000802L, 0x02000800L,
                0x00000002L, 0x00000800L, 0x00080800L, 0x02080002L,
                0x00000800L, 0x02000802L, 0x02080002L, 0x00000000L,
                0x00000000L, 0x02080802L, 0x02000800L, 0x00080002L,
                0x02080800L, 0x00080000L, 0x00000802L, 0x02000800L,
                0x02080002L, 0x00000800L, 0x00080800L, 0x02000002L,
                0x00080802L, 0x00000002L, 0x02000002L, 0x02080000L,
                0x02080802L, 0x00080800L, 0x02080000L, 0x02000802L,
                0x02000000L, 0x00000802L, 0x00080002L, 0x00000000L,
                0x00080000L, 0x02000000L, 0x02000802L, 0x02080800L,
                0x00000002L, 0x02080002L, 0x00000800L, 0x00080802L,
            },
            {
                /* nibble 1 */
                0x40108010L, 0x00000000L, 0x00108000L, 0x40100000L,
                0x40000010L, 0x00008010L, 0x40008000L, 0x00108000L,
                0x00008000L, 0x40100010L, 0x00000010L, 0x40008000L,
                0x00100010L, 0x40108000L, 0x40100000L, 0x00000010L,
                0x00100000L, 0x40008010L, 0x40100010L, 0x00008000L,
                0x00108010L, 0x40000000L, 0x00000000L, 0x00100010L,
                0x40008010L, 0x00108010L, 0x40108000L, 0x40000010L,
                0x40000000L, 0x00100000L, 0x00008010L, 0x40108010L,
                0x00100010L, 0x40108000L, 0x40008000L, 0x00108010L,
                0x40108010L, 0x00100010L, 0x40000010L, 0x00000000L,
                0x40000000L, 0x00008010L, 0x00100000L, 0x40100010L,
                0x00008000L, 0x40000000L, 0x00108010L, 0x40008010L,
                0x40108000L, 0x00008000L, 0x00000000L, 0x40000010L,
                0x00000010L, 0x40108010L, 0x00108000L, 0x40100000L,
                0x40100010L, 0x00100000L, 0x00008010L, 0x40008000L,
                0x40008010L, 0x00000010L, 0x40100000L, 0x00108000L,
            },
            {
                /* nibble 2 */
                0x04000001L, 0x04040100L, 0x00000100L, 0x04000101L,
                0x00040001L, 0x04000000L, 0x04000101L, 0x00040100L,
                0x04000100L, 0x00040000L, 0x04040000L, 0x00000001L,
                0x04040101L, 0x00000101L, 0x00000001L, 0x04040001L,
                0x00000000L, 0x00040001L, 0x04040100L, 0x00000100L,
                0x00000101L, 0x04040101L, 0x00040000L, 0x04000001L,
                0x04040001L, 0x04000100L, 0x00040101L, 0x04040000L,
                0x00040100L, 0x00000000L, 0x04000000L, 0x00040101L,
                0x04040100L, 0x00000100L, 0x00000001L, 0x00040000L,
                0x00000101L, 0x00040001L, 0x04040000L, 0x04000101L,
                0x00000000L, 0x04040100L, 0x00040100L, 0x04040001L,
                0x00040001L, 0x04000000L, 0x04040101L, 0x00000001L,
                0x00040101L, 0x04000001L, 0x04000000L, 0x04040101L,
                0x00040000L, 0x04000100L, 0x04000101L, 0x00040100L,
                0x04000100L, 0x00000000L, 0x04040001L, 0x00000101L,
                0x04000001L, 0x00040101L, 0x00000100L, 0x04040000L,
            },
            {
                /* nibble 3 */
                0x00401008L, 0x10001000L, 0x00000008L, 0x10401008L,
                0x00000000L, 0x10400000L, 0x10001008L, 0x00400008L,
                0x10401000L, 0x10000008L, 0x10000000L, 0x00001008L,
                0x10000008L, 0x00401008L, 0x00400000L, 0x10000000L,
                0x10400008L, 0x00401000L, 0x00001000L, 0x00000008L,
                0x00401000L, 0x10001008L, 0x10400000L, 0x00001000L,
                0x00001008L, 0x00000000L, 0x00400008L, 0x10401000L,
                0x10001000L, 0x10400008L, 0x10401008L, 0x00400000L,
                0x10400008L, 0x00001008L, 0x00400000L, 0x10000008L,
                0x00401000L, 0x10001000L, 0x00000008L, 0x10400000L,
                0x10001008L, 0x00000000L, 0x00001000L, 0x00400008L,
                0x00000000L, 0x10400008L, 0x10401000L, 0x00001000L,
                0x10000000L, 0x10401008L, 0x00401008L, 0x00400000L,
                0x10401008L, 0x00000008L, 0x10001000L, 0x00401008L,
                0x00400008L, 0x00401000L, 0x10400000L, 0x10001008L,
                0x00001008L, 0x10000000L, 0x10000008L, 0x10401000L,
            },
            {
                /* nibble 4 */
                0x08000000L, 0x00010000L, 0x00000400L, 0x08010420L,
                0x08010020L, 0x08000400L, 0x00010420L, 0x08010000L,
                0x00010000L, 0x00000020L, 0x08000020L, 0x00010400L,
                0x08000420L, 0x08010020L, 0x08010400L, 0x00000000L,
                0x00010400L, 0x08000000L, 0x00010020L, 0x00000420L,
                0x08000400L, 0x00010420L, 0x00000000L, 0x08000020L,
                0x00000020L, 0x08000420L, 0x08010420L, 0x00010020L,
                0x08010000L, 0x00000400L, 0x00000420L, 0x08010400L,
                0x08010400L, 0x08000420L, 0x00010020L, 0x08010000L,
                0x00010000L, 0x00000020L, 0x08000020L, 0x08000400L,
                0x08000000L, 0x00010400L, 0x08010420L, 0x00000000L,
                0x00010420L, 0x08000000L, 0x00000400L, 0x00010020L,
                0x08000420L, 0x00000400L, 0x00000000L, 0x08010420L,
                0x08010020L, 0x08010400L, 0x00000420L, 0x00010000L,
                0x00010400L, 0x08010020L, 0x08000400L, 0x00000420L,
                0x00000020L, 0x00010420L, 0x08010000L, 0x08000020L,
            },
            {
                /* nibble 5 */
                0x80000040L, 0x00200040L, 0x00000000L, 0x80202000L,
                0x00200040L, 0x00002000L, 0x80002040L, 0x00200000L,
                0x00002040L, 0x80202040L, 0x00202000L, 0x80000000L,
                0x80002000L, 0x80000040L, 0x80200000L, 0x00202040L,
                0x00200000L, 0x80002040L, 0x80200040L, 0x00000000L,
                0x00002000L, 0x00000040L, 0x80202000L, 0x80200040L,
                0x80202040L, 0x80200000L, 0x80000000L, 0x00002040L,
                0x00000040L, 0x00202000L, 0x00202040L, 0x80002000L,
                0x00002040L, 0x80000000L, 0x80002000L, 0x00202040L,
                0x80202000L, 0x00200040L, 0x00000000L, 0x80002000L,
                0x80000000L, 0x00002000L, 0x80200040L, 0x00200000L,
                0x00200040L, 0x80202040L, 0x00202000L, 0x00000040L,
                0x80202040L, 0x00202000L, 0x00200000L, 0x80002040L,
                0x80000040L, 0x80200000L, 0x00202040L, 0x00000000L,
                0x00002000L, 0x80000040L, 0x80002040L, 0x80202000L,
                0x80200000L, 0x00002040L, 0x00000040L, 0x80200040L,
            },
            {
                /* nibble 6 */
                0x00004000L, 0x00000200L, 0x01000200L, 0x01000004L,
                0x01004204L, 0x00004004L, 0x00004200L, 0x00000000L,
                0x01000000L, 0x01000204L, 0x00000204L, 0x01004000L,
                0x00000004L, 0x01004200L, 0x01004000L, 0x00000204L,
                0x01000204L, 0x00004000L, 0x00004004L, 0x01004204L,
                0x00000000L, 0x01000200L, 0x01000004L, 0x00004200L,
                0x01004004L, 0x00004204L, 0x01004200L, 0x00000004L,
                0x00004204L, 0x01004004L, 0x00000200L, 0x01000000L,
                0x00004204L, 0x01004000L, 0x01004004L, 0x00000204L,
                0x00004000L, 0x00000200L, 0x01000000L, 0x01004004L,
                0x01000204L, 0x00004204L, 0x00004200L, 0x00000000L,
                0x00000200L, 0x01000004L, 0x00000004L, 0x01000200L,
                0x00000000L, 0x01000204L, 0x01000200L, 0x00004200L,
                0x00000204L, 0x00004000L, 0x01004204L, 0x01000000L,
                0x01004200L, 0x00000004L, 0x00004004L, 0x01004204L,
                0x01000004L, 0x01004200L, 0x01004000L, 0x00004004L,
            },
            {
                /* nibble 7 */
                0x20800080L, 0x20820000L, 0x00020080L, 0x00000000L,
                0x20020000L, 0x00800080L, 0x20800000L, 0x20820080L,
                0x00000080L, 0x20000000L, 0x00820000L, 0x00020080L,
                0x00820080L, 0x20020080L, 0x20000080L, 0x20800000L,
                0x00020000L, 0x00820080L, 0x00800080L, 0x20020000L,
                0x20820080L, 0x20000080L, 0x00000000L, 0x00820000L,
                0x20000000L, 0x00800000L, 0x20020080L, 0x20800080L,
                0x00800000L, 0x00020000L, 0x20820000L, 0x00000080L,
                0x00800000L, 0x00020000L, 0x20000080L, 0x20820080L,
                0x00020080L, 0x20000000L, 0x00000000L, 0x00820000L,
                0x20800080L, 0x20020080L, 0x20020000L, 0x00800080L,
                0x20820000L, 0x00000080L, 0x00800080L, 0x20020000L,
                0x20820080L, 0x00800000L, 0x20800000L, 0x20000080L,
                0x00820000L, 0x00020080L, 0x20020080L, 0x20800000L,
                0x00000080L, 0x20820000L, 0x00820080L, 0x00000000L,
                0x20000000L, 0x20800080L, 0x00020000L, 0x00820080L,
            }
        };

        // [8][64]
        public static ulong[,] des_skb = {
            {
             /* for C bits (numbered as per FIPS 46) 1 2 3 4 5 6 */
             0x00000000L, 0x00000010L, 0x20000000L, 0x20000010L,
             0x00010000L, 0x00010010L, 0x20010000L, 0x20010010L,
             0x00000800L, 0x00000810L, 0x20000800L, 0x20000810L,
             0x00010800L, 0x00010810L, 0x20010800L, 0x20010810L,
             0x00000020L, 0x00000030L, 0x20000020L, 0x20000030L,
             0x00010020L, 0x00010030L, 0x20010020L, 0x20010030L,
             0x00000820L, 0x00000830L, 0x20000820L, 0x20000830L,
             0x00010820L, 0x00010830L, 0x20010820L, 0x20010830L,
             0x00080000L, 0x00080010L, 0x20080000L, 0x20080010L,
             0x00090000L, 0x00090010L, 0x20090000L, 0x20090010L,
             0x00080800L, 0x00080810L, 0x20080800L, 0x20080810L,
             0x00090800L, 0x00090810L, 0x20090800L, 0x20090810L,
             0x00080020L, 0x00080030L, 0x20080020L, 0x20080030L,
             0x00090020L, 0x00090030L, 0x20090020L, 0x20090030L,
             0x00080820L, 0x00080830L, 0x20080820L, 0x20080830L,
             0x00090820L, 0x00090830L, 0x20090820L, 0x20090830L,
             },
            {
             /* for C bits (numbered as per FIPS 46) 7 8 10 11 12 13 */
             0x00000000L, 0x02000000L, 0x00002000L, 0x02002000L,
             0x00200000L, 0x02200000L, 0x00202000L, 0x02202000L,
             0x00000004L, 0x02000004L, 0x00002004L, 0x02002004L,
             0x00200004L, 0x02200004L, 0x00202004L, 0x02202004L,
             0x00000400L, 0x02000400L, 0x00002400L, 0x02002400L,
             0x00200400L, 0x02200400L, 0x00202400L, 0x02202400L,
             0x00000404L, 0x02000404L, 0x00002404L, 0x02002404L,
             0x00200404L, 0x02200404L, 0x00202404L, 0x02202404L,
             0x10000000L, 0x12000000L, 0x10002000L, 0x12002000L,
             0x10200000L, 0x12200000L, 0x10202000L, 0x12202000L,
             0x10000004L, 0x12000004L, 0x10002004L, 0x12002004L,
             0x10200004L, 0x12200004L, 0x10202004L, 0x12202004L,
             0x10000400L, 0x12000400L, 0x10002400L, 0x12002400L,
             0x10200400L, 0x12200400L, 0x10202400L, 0x12202400L,
             0x10000404L, 0x12000404L, 0x10002404L, 0x12002404L,
             0x10200404L, 0x12200404L, 0x10202404L, 0x12202404L,
             },
            {
             /* for C bits (numbered as per FIPS 46) 14 15 16 17 19 20 */
             0x00000000L, 0x00000001L, 0x00040000L, 0x00040001L,
             0x01000000L, 0x01000001L, 0x01040000L, 0x01040001L,
             0x00000002L, 0x00000003L, 0x00040002L, 0x00040003L,
             0x01000002L, 0x01000003L, 0x01040002L, 0x01040003L,
             0x00000200L, 0x00000201L, 0x00040200L, 0x00040201L,
             0x01000200L, 0x01000201L, 0x01040200L, 0x01040201L,
             0x00000202L, 0x00000203L, 0x00040202L, 0x00040203L,
             0x01000202L, 0x01000203L, 0x01040202L, 0x01040203L,
             0x08000000L, 0x08000001L, 0x08040000L, 0x08040001L,
             0x09000000L, 0x09000001L, 0x09040000L, 0x09040001L,
             0x08000002L, 0x08000003L, 0x08040002L, 0x08040003L,
             0x09000002L, 0x09000003L, 0x09040002L, 0x09040003L,
             0x08000200L, 0x08000201L, 0x08040200L, 0x08040201L,
             0x09000200L, 0x09000201L, 0x09040200L, 0x09040201L,
             0x08000202L, 0x08000203L, 0x08040202L, 0x08040203L,
             0x09000202L, 0x09000203L, 0x09040202L, 0x09040203L,
             },
            {
             /* for C bits (numbered as per FIPS 46) 21 23 24 26 27 28 */
             0x00000000L, 0x00100000L, 0x00000100L, 0x00100100L,
             0x00000008L, 0x00100008L, 0x00000108L, 0x00100108L,
             0x00001000L, 0x00101000L, 0x00001100L, 0x00101100L,
             0x00001008L, 0x00101008L, 0x00001108L, 0x00101108L,
             0x04000000L, 0x04100000L, 0x04000100L, 0x04100100L,
             0x04000008L, 0x04100008L, 0x04000108L, 0x04100108L,
             0x04001000L, 0x04101000L, 0x04001100L, 0x04101100L,
             0x04001008L, 0x04101008L, 0x04001108L, 0x04101108L,
             0x00020000L, 0x00120000L, 0x00020100L, 0x00120100L,
             0x00020008L, 0x00120008L, 0x00020108L, 0x00120108L,
             0x00021000L, 0x00121000L, 0x00021100L, 0x00121100L,
             0x00021008L, 0x00121008L, 0x00021108L, 0x00121108L,
             0x04020000L, 0x04120000L, 0x04020100L, 0x04120100L,
             0x04020008L, 0x04120008L, 0x04020108L, 0x04120108L,
             0x04021000L, 0x04121000L, 0x04021100L, 0x04121100L,
             0x04021008L, 0x04121008L, 0x04021108L, 0x04121108L,
             },
            {
             /* for D bits (numbered as per FIPS 46) 1 2 3 4 5 6 */
             0x00000000L, 0x10000000L, 0x00010000L, 0x10010000L,
             0x00000004L, 0x10000004L, 0x00010004L, 0x10010004L,
             0x20000000L, 0x30000000L, 0x20010000L, 0x30010000L,
             0x20000004L, 0x30000004L, 0x20010004L, 0x30010004L,
             0x00100000L, 0x10100000L, 0x00110000L, 0x10110000L,
             0x00100004L, 0x10100004L, 0x00110004L, 0x10110004L,
             0x20100000L, 0x30100000L, 0x20110000L, 0x30110000L,
             0x20100004L, 0x30100004L, 0x20110004L, 0x30110004L,
             0x00001000L, 0x10001000L, 0x00011000L, 0x10011000L,
             0x00001004L, 0x10001004L, 0x00011004L, 0x10011004L,
             0x20001000L, 0x30001000L, 0x20011000L, 0x30011000L,
             0x20001004L, 0x30001004L, 0x20011004L, 0x30011004L,
             0x00101000L, 0x10101000L, 0x00111000L, 0x10111000L,
             0x00101004L, 0x10101004L, 0x00111004L, 0x10111004L,
             0x20101000L, 0x30101000L, 0x20111000L, 0x30111000L,
             0x20101004L, 0x30101004L, 0x20111004L, 0x30111004L,
             },
            {
             /* for D bits (numbered as per FIPS 46) 8 9 11 12 13 14 */
             0x00000000L, 0x08000000L, 0x00000008L, 0x08000008L,
             0x00000400L, 0x08000400L, 0x00000408L, 0x08000408L,
             0x00020000L, 0x08020000L, 0x00020008L, 0x08020008L,
             0x00020400L, 0x08020400L, 0x00020408L, 0x08020408L,
             0x00000001L, 0x08000001L, 0x00000009L, 0x08000009L,
             0x00000401L, 0x08000401L, 0x00000409L, 0x08000409L,
             0x00020001L, 0x08020001L, 0x00020009L, 0x08020009L,
             0x00020401L, 0x08020401L, 0x00020409L, 0x08020409L,
             0x02000000L, 0x0A000000L, 0x02000008L, 0x0A000008L,
             0x02000400L, 0x0A000400L, 0x02000408L, 0x0A000408L,
             0x02020000L, 0x0A020000L, 0x02020008L, 0x0A020008L,
             0x02020400L, 0x0A020400L, 0x02020408L, 0x0A020408L,
             0x02000001L, 0x0A000001L, 0x02000009L, 0x0A000009L,
             0x02000401L, 0x0A000401L, 0x02000409L, 0x0A000409L,
             0x02020001L, 0x0A020001L, 0x02020009L, 0x0A020009L,
             0x02020401L, 0x0A020401L, 0x02020409L, 0x0A020409L,
             },
            {
             /* for D bits (numbered as per FIPS 46) 16 17 18 19 20 21 */
             0x00000000L, 0x00000100L, 0x00080000L, 0x00080100L,
             0x01000000L, 0x01000100L, 0x01080000L, 0x01080100L,
             0x00000010L, 0x00000110L, 0x00080010L, 0x00080110L,
             0x01000010L, 0x01000110L, 0x01080010L, 0x01080110L,
             0x00200000L, 0x00200100L, 0x00280000L, 0x00280100L,
             0x01200000L, 0x01200100L, 0x01280000L, 0x01280100L,
             0x00200010L, 0x00200110L, 0x00280010L, 0x00280110L,
             0x01200010L, 0x01200110L, 0x01280010L, 0x01280110L,
             0x00000200L, 0x00000300L, 0x00080200L, 0x00080300L,
             0x01000200L, 0x01000300L, 0x01080200L, 0x01080300L,
             0x00000210L, 0x00000310L, 0x00080210L, 0x00080310L,
             0x01000210L, 0x01000310L, 0x01080210L, 0x01080310L,
             0x00200200L, 0x00200300L, 0x00280200L, 0x00280300L,
             0x01200200L, 0x01200300L, 0x01280200L, 0x01280300L,
             0x00200210L, 0x00200310L, 0x00280210L, 0x00280310L,
             0x01200210L, 0x01200310L, 0x01280210L, 0x01280310L,
             },
            {
             /* for D bits (numbered as per FIPS 46) 22 23 24 25 27 28 */
             0x00000000L, 0x04000000L, 0x00040000L, 0x04040000L,
             0x00000002L, 0x04000002L, 0x00040002L, 0x04040002L,
             0x00002000L, 0x04002000L, 0x00042000L, 0x04042000L,
             0x00002002L, 0x04002002L, 0x00042002L, 0x04042002L,
             0x00000020L, 0x04000020L, 0x00040020L, 0x04040020L,
             0x00000022L, 0x04000022L, 0x00040022L, 0x04040022L,
             0x00002020L, 0x04002020L, 0x00042020L, 0x04042020L,
             0x00002022L, 0x04002022L, 0x00042022L, 0x04042022L,
             0x00000800L, 0x04000800L, 0x00040800L, 0x04040800L,
             0x00000802L, 0x04000802L, 0x00040802L, 0x04040802L,
             0x00002800L, 0x04002800L, 0x00042800L, 0x04042800L,
             0x00002802L, 0x04002802L, 0x00042802L, 0x04042802L,
             0x00000820L, 0x04000820L, 0x00040820L, 0x04040820L,
             0x00000822L, 0x04000822L, 0x00040822L, 0x04040822L,
             0x00002820L, 0x04002820L, 0x00042820L, 0x04042820L,
             0x00002822L, 0x04002822L, 0x00042822L, 0x04042822L,
             }
        };

        public static void LOAD_DATA_tmp(ulong a, int b, ulong[] s, ref ulong c, ref ulong d)
        {
            LOAD_DATA(a, b, s, ref c, ref d);
        }
        public static void LOAD_DATA(ulong R, int S, ulong[] s, ref ulong u, ref ulong t)
        {
            u = R ^ s[S % 2];
            t = R ^ s[S % 2 + 1];
            Debug.Log(string.Format("R = {0}    ", R));
            Debug.Log(string.Format("s[S  ] = {0}     ", s[S]));
            Debug.Log(string.Format("s[S+1] = {0}    ", s[S + 1]));
            Debug.Log(string.Format("t = {0}    ", t));
        }

        public static void D_ENCRYPT(ref ulong LL, ulong R, int S, ulong[] s, ref ulong u, ref ulong t) 
        {
            LOAD_DATA_tmp(R, S, s, ref u, ref t);
            t = CryptUtil.ROTATE(t, 4);
            Debug.Log(string.Format("t = {0}    ", t));
            LL^=
                DES_SPtrans[0, (u >> (int)2L)&0x3f]^
                DES_SPtrans[2, (u >> (int)10L) & 0x3f] ^
                DES_SPtrans[4, (u >> (int)18L) & 0x3f] ^
                DES_SPtrans[6, (u >> (int)26L) & 0x3f] ^
                DES_SPtrans[1, (t >> (int)2L) & 0x3f] ^
                DES_SPtrans[3, (t >> (int)10L) & 0x3f] ^
                DES_SPtrans[5, (t >> (int)18L) & 0x3f] ^
                DES_SPtrans[7, (t >> (int)26L) & 0x3f]; 
        }

        public static void DES_set_key_unchecked(byte[] key, DES_key_schedule schedule)
        {
            ulong c = 0, d = 0, t = 0, s = 0, t2 = 0;
            byte[] in_Key;
            ulong[] k;
            int i;

            in_Key = key;

            int c2l_in_Key_startIdx = 0;
            CryptUtil.c2l(in_Key, ref c, ref c2l_in_Key_startIdx);
            CryptUtil.c2l(in_Key, ref d, ref c2l_in_Key_startIdx);

            /*
             * do PC1 in 47 simple operations :-) Thanks to John Fletcher
             * (john_fletcher@lccmail.ocf.llnl.gov) for the inspiration. :-)
             */
            CryptUtil.PERM_OP(ref d, ref c, ref t, 4, 0x0f0f0f0fL);
            CryptUtil.HPERM_OP(ref c, t, -2, 0xcccc0000L);
            CryptUtil.HPERM_OP(ref d, t, -2, 0xcccc0000L);
            CryptUtil.PERM_OP(ref d, ref c, ref t, 1, 0x55555555L);
            CryptUtil.PERM_OP(ref c, ref d, ref t, 8, 0x00ff00ffL);
            CryptUtil.PERM_OP(ref d, ref c, ref t, 1, 0x55555555L);
            d = (((d & 0x000000ffL) << (int)16L) | (d & 0x0000ff00L) |
                 ((d & 0x00ff0000L) >> (int)16L) | ((c & 0xf0000000L) >> (int)4L));
            c &= 0x0fffffffL;

            for (i = 0; i < ITERATIONS; i++) {
                k = schedule.ks[i].deslong;

                if (shifts2[i] != 0) {
                    c = ((c >> (int)2L) | (c << (int)26L));
                    d = ((d >> (int)2L) | (d << (int)26L));
                } else {
                    c = ((c >> (int)1L) | (c << (int)27L));
                    d = ((d >> (int)1L) | (d << (int)27L));
                }
                c &= 0x0fffffffL;
                d &= 0x0fffffffL;
                /*
                 * could be a few less shifts but I am to lazy at this point in time
                 * to investigate
                 */
                s = des_skb[0, (c) & 0x3f] |
                    des_skb[1, ((c >> (int)6L) & 0x03) | ((c >> (int)7L) & 0x3c)] |
                    des_skb[2, ((c >> (int)13L) & 0x0f) | ((c >> (int)14L) & 0x30)] |
                    des_skb[3, ((c >> (int)20L) & 0x01) | ((c >> (int)21L) & 0x06) |
                               ((c >> (int)22L) & 0x38)];
                t = des_skb[4, (d) & 0x3f] |
                    des_skb[5, ((d >> (int)7L) & 0x03) | ((d >> (int)8L) & 0x3c)] |
                    des_skb[6, (d >> (int)15L) & 0x3f] |
                    des_skb[7, ((d >> (int)21L) & 0x0f) | ((d >> (int)22L) & 0x30)];

                /* table contained 0213 4657 */
                t2 = ((t << (int)16L) | (s & 0x0000ffffL)) & 0xffffffffL;
                k[0] = CryptUtil.ROTATE(t2, 30) & 0xffffffffL;

                t2 = ((s >> (int)16L) | (t & 0xffff0000L));
                k[1] = CryptUtil.ROTATE(t2, 26) & 0xffffffffL;
            }
        }

        public static void DES_encrypt1(ulong[] data, DES_key_schedule ks, int enc)
        {
            ulong l = 0, r = 0, t = 0, u = 0;
            //int i = 0;
            ulong[] s = null;

            r = data[0];
            l = data[1];

            CryptUtil.IP(ref r, ref l);
            /*
             * Things have been modified so that the initial rotate is done outside
             * the loop.  This required the DES_SPtrans values in sp.h to be rotated
             * 1 bit to the right. One perl script later and things have a 5% speed
             * up on a sparc2. Thanks to Richard Outerbridge
             * <71755.204@CompuServe.COM> for pointing this out.
             */
            /* clear the top bits on machines with 8byte longs */
            /* shift left by 2 */
            r = CryptUtil.ROTATE(r, 29) & 0xffffffffL;
            l = CryptUtil.ROTATE(l, 29) & 0xffffffffL;

            //s = ks.ks[0].deslong;
            /*
             * I don't know if it is worth the effort of loop unrolling the inner
             * loop
             */
            if (enc != 0) {
                s = ks.ks[0].deslong;
                D_ENCRYPT(ref l, r, 0, s, ref u, ref t);     /* 1 */
                s = ks.ks[1].deslong;
                D_ENCRYPT(ref r, l, 2, s, ref u, ref t);     /* 2 */
                s = ks.ks[2].deslong;
                D_ENCRYPT(ref l, r, 4, s, ref u, ref t);     /* 3 */
                s = ks.ks[3].deslong;
                D_ENCRYPT(ref r, l, 6, s, ref u, ref t);     /* 4 */
                s = ks.ks[4].deslong;
                D_ENCRYPT(ref l, r, 8, s, ref u, ref t);     /* 5 */
                s = ks.ks[5].deslong;
                D_ENCRYPT(ref r, l, 10, s, ref u, ref t);    /* 6 */
                s = ks.ks[6].deslong;
                D_ENCRYPT(ref l, r, 12, s, ref u, ref t);    /* 7 */
                s = ks.ks[7].deslong;
                D_ENCRYPT(ref r, l, 14, s, ref u, ref t);    /* 8 */
                s = ks.ks[8].deslong;
                D_ENCRYPT(ref l, r, 16, s, ref u, ref t);    /* 9 */
                s = ks.ks[9].deslong;
                D_ENCRYPT(ref r, l, 18, s, ref u, ref t);    /* 10 */
                s = ks.ks[10].deslong;
                D_ENCRYPT(ref l, r, 20, s, ref u, ref t);    /* 11 */
                s = ks.ks[11].deslong;
                D_ENCRYPT(ref r, l, 22, s, ref u, ref t);    /* 12 */
                s = ks.ks[12].deslong;
                D_ENCRYPT(ref l, r, 24, s, ref u, ref t);    /* 13 */
                s = ks.ks[13].deslong;
                D_ENCRYPT(ref r, l, 26, s, ref u, ref t);    /* 14 */
                s = ks.ks[14].deslong;
                D_ENCRYPT(ref l, r, 28, s, ref u, ref t);    /* 15 */
                s = ks.ks[15].deslong;
                D_ENCRYPT(ref r, l, 30, s, ref u, ref t);    /* 16 */
            } 
            else 
            {
                s = ks.ks[15].deslong;
                D_ENCRYPT(ref l, r, 30, s, ref u, ref t);    /* 16 */
                s = ks.ks[14].deslong;
                D_ENCRYPT(ref r, l, 28, s, ref u, ref t);    /* 15 */
                s = ks.ks[13].deslong;
                D_ENCRYPT(ref l, r, 26, s, ref u, ref t);    /* 14 */
                s = ks.ks[12].deslong;
                D_ENCRYPT(ref r, l, 24, s, ref u, ref t);    /* 13 */
                s = ks.ks[11].deslong;
                D_ENCRYPT(ref l, r, 22, s, ref u, ref t);    /* 12 */
                s = ks.ks[10].deslong;
                D_ENCRYPT(ref r, l, 20, s, ref u, ref t);    /* 11 */
                s = ks.ks[9].deslong;
                D_ENCRYPT(ref l, r, 18, s, ref u, ref t);    /* 10 */
                s = ks.ks[8].deslong;
                D_ENCRYPT(ref r, l, 16, s, ref u, ref t);    /* 9 */
                s = ks.ks[7].deslong;
                D_ENCRYPT(ref l, r, 14, s, ref u, ref t);    /* 8 */
                s = ks.ks[6].deslong;
                D_ENCRYPT(ref r, l, 12, s, ref u, ref t);    /* 7 */
                s = ks.ks[5].deslong;
                D_ENCRYPT(ref l, r, 10, s, ref u, ref t);    /* 6 */
                s = ks.ks[4].deslong;
                D_ENCRYPT(ref r, l, 8, s, ref u, ref t);     /* 5 */
                s = ks.ks[3].deslong;
                D_ENCRYPT(ref l, r, 6, s, ref u, ref t);     /* 4 */
                s = ks.ks[2].deslong;
                D_ENCRYPT(ref r, l, 4, s, ref u, ref t);     /* 3 */
                s = ks.ks[1].deslong;
                D_ENCRYPT(ref l, r, 2, s, ref u, ref t);     /* 2 */
                s = ks.ks[0].deslong;
                D_ENCRYPT(ref r, l, 0, s, ref u, ref t);     /* 1 */
            }

            /* rotate and clear the top bits on machines with 8byte longs */
            l = CryptUtil.ROTATE(l, 3) & 0xffffffffL;
            r = CryptUtil.ROTATE(r, 3) & 0xffffffffL;

            CryptUtil.FP(ref r, ref l);
            data[0] = l;
            data[1] = r;
            l = r = t = u = 0;
        }

        public static void DES_ecb_encrypt(byte[] input, byte[] output,
                     DES_key_schedule ks, int enc)
        {
            ulong l = 0;
            ulong[] ll = new ulong[2];
            byte[] in_input = input;
            byte[] out_output = output;

            int c2l_in_input_startIdx = 0;
            int l2c_out_output_startIdx = 0;

            CryptUtil.c2l(in_input, ref l, ref c2l_in_input_startIdx);
            ll[0] = l;
            CryptUtil.c2l(in_input, ref l, ref c2l_in_input_startIdx);
            ll[1] = l;
            DES_encrypt1(ll, ks, enc);
            l = ll[0];
            CryptUtil.l2c(l, out_output, ref l2c_out_output_startIdx);
            l = ll[1];
            CryptUtil.l2c(l, out_output, ref l2c_out_output_startIdx);
            l = ll[0] = ll[1] = 0;
        }

        public static void DES_ecb_encrypt_one(byte[] inBytesTotal, uint startPos, uint inLen, ref byte[] outBytesTotal,
                     DES_key_schedule ks, int enc)
        {
            int len_ = (((int)inLen + 7) / 8) * 8;
            outBytesTotal = new byte[len_];

            byte[] inBytes = new byte[8];
            byte[] outBytes = new byte[8];

            int idx = 0;
            for (idx = 0; idx < inLen; idx += 8)
            {
                // 保证 8 个字节
                CryptUtil.get8Byte(inBytesTotal, (int)startPos + idx, inBytes);
                DES_ecb_encrypt(inBytes, outBytes, ks, enc);
                Array.Copy(outBytes, 0, outBytesTotal, idx, 8);
            }
        }
    }
}