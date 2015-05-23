using SDK.Common;
using System.Collections;
using UnityEngine;

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
            if (ResLoadType.eLoadDisc == m_resLoadType)
            {
                m_resLoadState.setSuccessLoaded();
                m_loadEventDispatch.dispatchEvent(this);
            }
            else if (ResLoadType.eLoadDicWeb == m_resLoadType || ResLoadType.eLoadWeb == m_resLoadType)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(downloadAsset());
            }
        }
    }
}