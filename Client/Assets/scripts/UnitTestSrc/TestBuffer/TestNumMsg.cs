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

        public override void serialize(ByteArray ba)
        {
            base.serialize(ba);
            ba.writeUnsignedInt(num);
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);
            num = ba.readUnsignedInt();
        }
    }
}