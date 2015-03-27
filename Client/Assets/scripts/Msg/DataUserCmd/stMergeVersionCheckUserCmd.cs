using SDK.Common;

namespace Game.Msg
{
    public class stMergeVersionCheckUserCmd : stDataUserCmd 
    {
        public uint dwMergeVersion;

        public stMergeVersionCheckUserCmd ()
        {
            byParam = MERGE_VERSION_CHECK_USERCMD_PARA;
        }

        public override void derialize(ByteArray ba)
        {
            base.derialize(ba);
            dwMergeVersion = ba.readUnsignedInt();
        }
    }
}


/// 发送当前合并版本号
//const BYTE MERGE_VERSION_CHECK_USERCMD_PARA = 53;
//struct stMergeVersionCheckUserCmd : public stDataUserCmd
//{
//  stMergeVersionCheckUserCmd()
//  {
//    byParam = MERGE_VERSION_CHECK_USERCMD_PARA;
//    dwMergeVersion = 0;
//  }

//  DWORD dwMergeVersion;
//};