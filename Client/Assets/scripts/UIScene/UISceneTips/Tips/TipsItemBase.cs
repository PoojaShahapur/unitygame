using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 基本的提示信息
     */
    public class TipsItemBase
    {
        protected SceneTipsData m_sceneTipsData;
        protected GameObject m_tipsItemRoot;            // 每一个显示的根节点

        public TipsItemBase(SceneTipsData data)
        {
            m_sceneTipsData = data;
        }

        public virtual void show()
        {
            m_tipsItemRoot.SetActive(true);
        }

        public virtual void hide()
        {
            m_tipsItemRoot.SetActive(false);
        }
    }
}