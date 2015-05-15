using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 左边卡牌列表中的一个卡牌
     */
    public class TuJianCardListItem
    {
        protected GameObject m_sceneGo;
        protected CardItemBase m_cardItem; // 卡牌基本数据

        public TuJianCardListItem(GameObject go_)
        {
            m_sceneGo = go_;
        }

        public CardItemBase cardItem
        {
            set
            {
                m_cardItem = value;
            }
        }

        public GameObject getGameObject()
        {
            return m_sceneGo;
        }
    }
}