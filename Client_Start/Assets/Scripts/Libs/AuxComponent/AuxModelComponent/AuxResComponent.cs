using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 自己拥有资源 
     */
    public class AuxResComponent : AuxComponent
    {
        protected GameObject mSelfLocalGo;
        protected string mPath;                          // 目录
        protected ModelRes mRes;

        public AuxResComponent()
        {
            this.mSelfLocalGo = UtilApi.createGameObject("ResLocalGO");
            this.mSelfLocalGo.name = "m_selfLocalGo";
        }

        public GameObject selfLocalGo
        {
            get
            {
                return this.mSelfLocalGo;
            }
            set
            {
                this.mSelfLocalGo = value;
            }
        }

        public string path
        {
            get
            {
                return this.mPath;
            }
            set
            {
                this.mPath = value;
            }
        }

        public override void setPntGo(GameObject go)
        {
            base.setPntGo(go);
            this.mSelfLocalGo.transform.SetParent(pntGo.transform, false);
        }

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            this.mRes = dispObj as ModelRes;
            if (this.mRes.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
            {
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, this.mRes.getLoadPath());

                this.mSelfGo = this.mRes.InstantiateObject(this.mPath);
                this.mSelfGo.transform.SetParent(this.mSelfLocalGo.transform, false);

                // 不是使用 m_resLoadMgr.load 接口加载的资源，不要使用 m_resLoadMgr.unload 去卸载资源
                // 卸载资源
                //Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
            }
            else if (this.mRes.refCountResLoadResultNotify.resLoadState.hasFailed())
            {
                this.mRes = dispObj as ModelRes;
                Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, this.mRes.getLoadPath());

                // 卸载资源
                //Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath());
            }
        }

        public virtual void unload()
        {
            if (this.mSelfGo != null)
            {
                UtilApi.Destroy(this.mSelfGo);
                this.mSelfGo = null;
                Ctx.m_instance.m_modelMgr.unload(this.mPath, null);
                this.mRes = null;
            }
        }

        public virtual void load()
        {
            bool needLoad = true;

            if (this.mRes != null)
            {
                if (this.mRes.getLogicPath() != this.mPath)
                {
                    unload();
                }
                else
                {
                    needLoad = false;
                }
            }
            if (needLoad)
            {
                if (!string.IsNullOrEmpty(this.mPath))
                {
                    LoadParam param;
                    param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                    param.setPath(this.mPath);
                    param.m_loadEventHandle = onLoadEventHandle;
                    Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                    Ctx.m_instance.m_poolSys.deleteObj(param);
                }
            }
        }
    }
}