using SDK.Lib;
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

        public override void serialize(ByteBuffer bu)
        {
            base.serialize(bu);
            bu.writeUnsignedInt32(num);
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt32(ref num);
        }
    }
}