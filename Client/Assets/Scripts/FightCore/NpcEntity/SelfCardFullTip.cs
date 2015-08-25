using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 自己卡牌已经满了，得不到新的卡牌了
     */
    public class SelfCardFullTip : NpcEntityBase
    {
        protected AuxStaticModel m_model;
        protected AuxLabel m_desc;

        public SelfCardFullTip()
        {
            m_model = new AuxStaticModel();
        }

        public AuxLabel desc
        {
            get
            {
                return m_desc;
            }
            set
            {
                m_desc = value;
            }
        }

        override public GameObject gameObject()
        {
            return m_model.selfGo;
        }

        override public void setGameObject(GameObject rhv)
        {
            m_model.selfGo = rhv;
        }

        override public void show()
        {
            if (m_model != null)
            {
                m_model.show();
            }
        }

        override public void hide()
        {
            if (m_model != null)
            {
                m_model.hide();
            }
        }

        override public bool IsVisible()
        {
            if (m_model != null)
            {
                return m_model.IsVisible();
            }

            return true;
        }
    }
}