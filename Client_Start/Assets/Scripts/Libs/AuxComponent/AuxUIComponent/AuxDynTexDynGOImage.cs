using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxDynTexDynGOImage : AuxDynTexImage
    {
        protected string mPrefabPath;      // Prefab 目录
        protected PrefabRes mPrefabRes;  // Prefab 资源
        protected bool mIsNeedReload = false;

        public AuxDynTexDynGOImage(bool bNeedPlaceHolderGo = false)
        {
            this.mIsNeedPlaceHolderGo = bNeedPlaceHolderGo;
        }

        override public void dispose()
        {
            if (this.mSelfGo != null)
            {
                UtilApi.Destroy(this.mSelfGo);
            }

            base.dispose();
            
            if(this.mPrefabRes != null)
            {
                Ctx.mInstance.mPrefabMgr.unload(this.mPrefabRes.getResUniqueId(), null);
            }
        }

        public string prefabPath
        {
            set
            {
                if (this.mPrefabPath != value)
                {
                    this.mIsNeedReload = true;
                }
                this.mPrefabPath = value;
            }
        }

        // 查找 UI 组件
        override public void findWidget()
        {
            if (string.IsNullOrEmpty(this.mGoName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
            {
                this.mImage = UtilApi.getComByP<Image>(this.mSelfGo);
            }
            else
            {
                this.mImage = UtilApi.getComByP<Image>(this.mSelfGo, this.mGoName);
            }
        }

        // 加载 Prefab
        protected void loadPrefab()
        {
            if (this.mIsNeedReload)
            {
                if (this.mSelfGo != null)
                {
                    UtilApi.Destroy(this.mSelfGo);
                    this.mSelfGo = null;
                }
                if (this.mPrefabRes != null)
                {
                    Ctx.mInstance.mPrefabMgr.unload(this.mPrefabRes.getResUniqueId(), null);
                    this.mPrefabRes = null;
                }
                this.mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndSyncLoad<PrefabRes>(this.mPrefabPath);
                this.mSelfGo = this.mPrefabRes.InstantiateObject(this.mPrefabPath);

                if (this.mIsNeedPlaceHolderGo && this.mPlaceHolderGo != null)
                {
                    UtilApi.SetParent(this.mSelfGo, this.mPlaceHolderGo, false);
                }
                else if (this.mPntGo != null)
                {
                    UtilApi.SetParent(this.mSelfGo, this.mPntGo, false);
                }

                findWidget();
                this.mIsImageGoChange = true;      // 设置重新更新图像
            }
            mIsNeedReload = false;
        }

        override public void syncUpdateCom()
        {
            loadPrefab();
            base.syncUpdateCom();
        }
    }
}