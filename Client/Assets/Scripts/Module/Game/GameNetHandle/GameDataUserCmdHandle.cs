using Game.Msg;
using SDK.Common;
using SDK.Lib;

namespace Game.Game
{
    public class GameDataUserCmdHandle : NetCmdHandleBase
    {
        public GameDataUserCmdHandle()
        {
            m_id2HandleDic[stDataUserCmd.MAIN_USER_DATA_USERCMD_PARA] = psstMainUserDataUserCmd;
        }

        // 如果要调试，可以重载，方便调试
        //public override void handleMsg(ByteArray ba, byte byCmd, byte byParam)
        //{
        //
        //}

        protected void psstMainUserDataUserCmd(ByteArray msg)
        {
            stMainUserDataUserCmd cmd = new stMainUserDataUserCmd();
            cmd.derialize(msg);
            Ctx.m_instance.m_dataPlayer.m_dataMain = cmd.data;
        }
    }
}