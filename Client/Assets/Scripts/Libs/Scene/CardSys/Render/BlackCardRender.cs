using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 对方手里背面牌
     */
    public class BlackCardRender : CardRenderBase
    {
        protected AuxDynModel m_model;          // 模型资源

        public BlackCardRender(SceneEntityBase entity_) :
            base(entity_)
        {
            m_model = new AuxDynModel();
        }

        public AuxDynModel model
        {
            get
            {
                return m_model;
            }
        }

        override public void dispose()
        {
            if (m_model != null)
            {
                m_model.dispose();
                m_model = null;
            }
        }

        override public GameObject gameObject()
        {
            return m_model.selfGo;
        }

        override public Transform transform()
        {
            return m_model.selfGo.transform;
        }

        override public void setGameObject(GameObject rhv)
        {
            model.selfGo = rhv;
        }

        virtual public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            m_model.pntGo = pntGo_;
            m_model.modelResPath = "Model/Character/duishoucard.prefab";
            m_model.syncUpdateModel();
        }
    }
}