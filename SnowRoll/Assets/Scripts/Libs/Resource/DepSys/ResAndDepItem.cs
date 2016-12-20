using System;

namespace SDK.Lib
{
    public class ResAndDepItem : IDispatchObject
    {
        public string mLoadPath;                // 加载参数
        public bool mLoadNeedCoroutine;
        public bool mResNeedCoroutine;
        public string[] m_depNameArr;           // 依赖的名字数组
        public MList<string> mLoadedDepList;    // 加载成功的依赖列表
        public MList<string> mFailedDepList;    // 加载失败的依赖列表
        public bool mIsLoaded;                  // 是否已经加载
        public ResEventDispatch mResEventDispatch;  // 资源加载完成事件分发

        public ResAndDepItem()
        {
            mIsLoaded = false;
            mLoadedDepList = new MList<string>();
            mFailedDepList = new MList<string>();
            mResEventDispatch = new ResEventDispatch();
        }

        public void addEventHandle(MAction<IDispatchObject> handle)
        {
            mResEventDispatch.addEventHandle(null, handle);
        }

        public bool hasLoaded()
        {
            return mLoadedDepList.Count() + mFailedDepList.Count() == m_depNameArr.Length;
        }

        public bool hasSuccessLoaded()
        {
            return mLoadedDepList.Count() == m_depNameArr.Length;
        }

        public bool hasFailed()
        {
            return mFailedDepList.Count() > 0;
        }

        public void loadDep()
        {
            m_depNameArr = Ctx.mInstance.mDepResMgr.getDep(mLoadPath);
            mLoadedDepList.Clear();

            for (int i = 0; i < m_depNameArr.Length; ++i)
            {
                // 不管资源是否加载完成，都要再加载一次，增加引用计数，因此必须使用 loadResources 添加一次引用
                //if (Ctx.mInstance.mResLoadMgr.isResLoaded(m_depNameArr[i]))
                //{
                //    mLoadedDepList.Add(m_depNameArr[i]);
                //}
                //else
                //{
                    LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
                    param.setPath(m_depNameArr[i]);
                    param.mLoadEventHandle = onLoadEventHandle;
                    param.mLoadNeedCoroutine = mLoadNeedCoroutine;
                    param.mResNeedCoroutine = mResNeedCoroutine;
                    Ctx.mInstance.mResLoadMgr.loadAsset(param);       // 依赖加载也需要检查依赖
                    Ctx.mInstance.mPoolSys.deleteObj(param);
                //}
            }

            onDepLoaded();
        }

        public void unloadDep()
        {
            string resUniqueId = "";
            foreach (string path in m_depNameArr)
            {
                resUniqueId = LoadParam.convLoadPathToUniqueId(path);
                Ctx.mInstance.mResLoadMgr.unload(resUniqueId, onLoadEventHandle);
            }
        }

        public void onLoadEventHandle(IDispatchObject dispObj)
        {
            ResItem res = dispObj as ResItem;
            if (res.hasSuccessLoaded())
            {
                mLoadedDepList.Add(res.getResUniqueId());
            }
            else
            {
                mFailedDepList.Add(res.getResUniqueId());
            }

            onDepLoaded();
        }

        protected void onDepLoaded()
        {
            if(!mIsLoaded)
            {
                if (hasLoaded())      // 如果依赖都加载完成
                {
                    mIsLoaded = true;
                    mResEventDispatch.dispatchEvent(this);
                }
            }
        }
    }
}