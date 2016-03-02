using System.IO;

namespace SDK.Lib
{
    public class DepRes : RefCount
    {
        protected ResItem m_res;

        public void load(string path)
        {
            if (m_res != null)
            {
                this.incRef();
            }
            else
            {
                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                MFileSys.modifyLoadParam(Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath], path), param);
                param.m_loadEventHandle = onLoadEventHandle;
                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }

        public void unload()
        {

        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;

            if (res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                m_res = res;
            }
            else if (res.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                // 卸载资源
                Ctx.m_instance.m_resLoadMgr.unload(res.GetPath(), onLoadEventHandle);
            }
        }
    }
}