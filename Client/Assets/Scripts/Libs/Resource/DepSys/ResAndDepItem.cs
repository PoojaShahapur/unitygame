namespace SDK.Lib
{
    public class ResAndDepItem
    {
        public LoadParam m_loadParam;           // 保存资源加载的参数
        public string[] m_depNameArr;           // 依赖的名字数组
        public MList<string> mLoadedDepList;    // 加载的依赖列表
        public bool mIsLoaded;                  // 是否已经加载

        public ResAndDepItem()
        {
            mIsLoaded = false;
            mLoadedDepList = new MList<string>();
        }

        public void loadDep()
        {
            mLoadedDepList.Clear();

            for (int i = 0; i < m_depNameArr.Length; ++i)
            {
                if (Ctx.m_instance.m_resLoadMgr.isResLoaded(m_depNameArr[i]))
                {
                    mLoadedDepList.Add(m_depNameArr[i]);
                }
                else
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

            loadMainRes();
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
            ResItem res = dispObj as ResItem;
            mLoadedDepList.Add(res.GetPath());

            loadMainRes();
        }

        protected void loadMainRes()
        {
            if(!mIsLoaded)
            {
                if (mLoadedDepList.Count() == m_depNameArr.Length)      // 如果依赖都加载完成
                {
                    mIsLoaded = true;
                    Ctx.m_instance.m_resLoadMgr.loadResources(m_loadParam, false);    // 直接加载，不检查依赖
                    Ctx.m_instance.m_poolSys.deleteObj(m_loadParam);
                    m_loadParam = null;
                }
            }
        }
    }
}