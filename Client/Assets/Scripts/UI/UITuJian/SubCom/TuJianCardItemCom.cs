using SDK.Lib;
using SDK.Lib;
using System;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 收藏界面我的一个卡牌，和卡牌组不同
     */
    public class TuJianCardItemCom : TuJianCardRender
    {
        public CardItemBase m_cardItemBase; // 卡牌基本数据
        public Action<TuJianCardItemCom> m_clkCB;

        public TuJianCardItemCom(SceneEntityBase entity_) :
            base(entity_)
        {

        }

        override public void createCard(CardItemBase cardItem, GameObject pntGo_)
        {
            base.createCard(cardItem, pntGo_);

            UtilApi.setLayer(m_model.selfGo, Config.UIModelLayer);
            //UtilApi.setScale(gameObject.transform, new Vector3(0.24f, 1, 0.24f));
            this.cardItemBase = cardItem;
            addEventHandle();
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            //UtilApi.addEventHandle(m_model.selfGo, onBtnClkOpen);
            this.clickEntityDisp.addEventHandle(onBtnClkOpen);
        }

        // 点击开包按钮
        protected virtual void onBtnClkOpen(IDispatchObject dispObj)
        {
            if (!UtilApi.IsPointerOverGameObject())
            {
                // 点击放入中间的格子
                if (m_clkCB != null)
                {
                    m_clkCB(this);
                }
            }
        }

        public CardItemBase cardItemBase
        {
            set
            {
                m_cardItemBase = value;
            }
        }
    }
}