using SDK.Common;
using Game.Msg;

namespace UnitTestSrc
{
    public class UnitTestNumCmd : stLogonUserCmd
    {
        public uint num;

        public UnitTestNumCmd()
        {
            byParam = USER_VERIFY_VER_PARA;
            num = 2001;
        }

        public override void serialize(ByteBuffer ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt32(num);
        }

        public override void derialize(ByteBuffer ba)
        {
            base.derialize(ba);
            ba.readUnsignedInt32(ref num);
        }
    }
}