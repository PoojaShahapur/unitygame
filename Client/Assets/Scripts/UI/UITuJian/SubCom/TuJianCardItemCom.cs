using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 收藏界面我的一个卡牌，和卡牌组不同
     */
    public class TuJianCardItemCom : SceneComponent
    {
        public CardItemBase m_cardItemBase; // 卡牌基本数据
        public Action<TuJianCardItemCom> m_clkCB;

        public override void Start()
        {
            addEventHandle();
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(gameObject, onBtnClkOpen);
        }

        // 点击开包按钮
        protected virtual void onBtnClkOpen(GameObject go)
        {
            // 点击放入中间的格子
            if (m_clkCB != null)
            {
                m_clkCB(this);
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