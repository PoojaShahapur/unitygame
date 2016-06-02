namespace SDK.Lib
{
    /**
     * @brief 支持从本地和 Web 服务器加载场景和场景 Bundle 资源 ，采用 WWW 下载
     */
    public class LevelLoadItem : LoadItem
    {
        protected string m_levelName;

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        override public void load()
        {
            base.load();
            if(ResLoadType.eLoadResource == m_resLoadType)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            }
            else if (ResLoadType.eLoadStreamingAssets == m_resLoadType ||
                ResLoadType.eLoadLocalPersistentData == m_resLoadType)
            {
                // 暂时没有实现，需要加载 AssetBundles 加载
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            }
            else if (ResLoadType.eLoadWeb == m_resLoadType)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
            }
        }
    }
}