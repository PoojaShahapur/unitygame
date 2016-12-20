using SDK.Lib;

namespace Game.Msg
{
    public class stMergeVersionCheckUserCmd : stDataUserCmd 
    {
        public uint dwMergeVersion;

        public stMergeVersionCheckUserCmd ()
        {
            byParam = MERGE_VERSION_CHECK_USERCMD_PARA;
        }

        public override void derialize(ByteBuffer bu)
        {
            base.derialize(bu);
            bu.readUnsignedInt32(ref dwMergeVersion);
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