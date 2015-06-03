using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 白色卡牌渲染器，占位卡牌
     */
    public class WhiteCardRender : CardRenderBase
    {
        protected GameObject m_model;          // 模型资源

        public WhiteCardRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_model = UtilApi.createGameObject("WhiteCard");
        }

        override public void dispose()
        {
            if (m_model != null)
            {
                UtilApi.Destroy(m_model);
                m_model = null;
            }
        }

        override public GameObject gameObject()
        {
            return m_model;
        }

        override public Transform transform()
        {
            return m_model.transform;
        }

        override public void setGameObject(GameObject rhv)
        {
            m_model = rhv;
        }

        virtual public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            UtilApi.SetParent(m_model, pntGo_, false);
        }
    }
}