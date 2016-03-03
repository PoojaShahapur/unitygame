using System.IO;

namespace SDK.Lib
{
    public class ResAndDepItem
    {
        public LoadParam m_loadParam;
        public string[] m_depNameArr;

        public void loadDep()
        {
            for(int i = 0; i < m_depNameArr.Length; ++i)
            {
                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.m_path = m_depNameArr[i];
                param.m_loadEventHandle = onLoadEventHandle;
                param.m_loadNeedCoroutine = true;
                param.m_resNeedCoroutine = true;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);       // 依赖加载也需要检查依赖
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }

        public void unloadDep()
        {
            foreach(string path in m_depNameArr)
            {
                Ctx.m_instance.m_resLoadMgr.unload(path, onLoadEventHandle);
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            if(Ctx.m_instance.m_depResMgr.checkIfAllDepLoaded(m_depNameArr))
            {
                Ctx.m_instance.m_resLoadMgr.loadResources(m_loadParam, false);    // 直接加载，不检查依赖
            }
        }
    }
}