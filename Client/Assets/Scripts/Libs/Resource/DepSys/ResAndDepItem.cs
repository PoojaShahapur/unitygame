namespace SDK.Lib
{
    public class ResAndDepItem
    {
        public LoadParam m_loadParam;           // 保存资源加载的参数
        public string[] m_depNameArr;           // 依赖的名字数组

        public ResAndDepItem()
        {

        }

        public void loadDep()
        {
            for(int i = 0; i < m_depNameArr.Length; ++i)
            {
                LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.m_path = m_depNameArr[i];
                param.m_loadEventHandle = onLoadEventHandle;
                param.m_loadNeedCoroutine = m_loadParam.m_loadNeedCoroutine;
                param.m_resNeedCoroutine = m_loadParam.m_resNeedCoroutine;
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
                Ctx.m_instance.m_poolSys.deleteObj(m_loadParam);
            }
        }
    }
}