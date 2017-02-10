using System.Collections.Generic;

namespace SDK.Lib
{
    public class MsgRouteNotify
    {
        protected MList<MsgRouteDispHandle> mDispList;

        public MsgRouteNotify()
        {
            this.mDispList = new MList<MsgRouteDispHandle>();
        }

        public void addOneDisp(MsgRouteDispHandle disp)
        {
            if(!this.mDispList.Contains(disp))
            {
                this.mDispList.Add(disp);
            }
        }

        public void removeOneDisp(MsgRouteDispHandle disp)
        {
            if(this.mDispList.Contains(disp))
            {
                this.mDispList.Remove(disp);
            }
        }

        public void handleMsg(MsgRouteBase msg)
        {
            foreach(var item in this.mDispList.list())
            {
                item.handleMsg(msg);
            }

            Ctx.mInstance.mPoolSys.deleteObj(msg);
        }
    }
}