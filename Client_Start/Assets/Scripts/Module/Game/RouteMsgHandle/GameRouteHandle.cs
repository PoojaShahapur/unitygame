using SDK.Lib;

namespace Game.Game
{
    public class GameRouteHandle : MsgRouteHandleBase
    {
        public GameRouteHandle()
        {
            this.addMsgRouteHandle(MsgRouteID.eMRIDThreadLog, threadLog);
            this.addMsgRouteHandle(MsgRouteID.eMRIDSocketOpened, onSocketOpened);
        }

        protected void threadLog(IDispatchObject dispObj)
        {
            MsgRouteBase msg = dispObj as MsgRouteBase;
            Ctx.mInstance.mLogSys.log((msg as ThreadLogMR).mLogSys);
        }

        protected void onSocketOpened(IDispatchObject dispObj)
        {
            Ctx.mInstance.mLuaSystem.onSocketConnected();
        }
    }
}