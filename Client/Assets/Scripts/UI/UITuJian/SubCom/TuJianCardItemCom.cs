﻿using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 收藏界面我的一个卡牌，和卡牌组不同
     */
    public class TuJianCardItemCom : SceneCardRes
    {
        public CardItemBase m_cardItemBase; // 卡牌基本数据
        public Action<TuJianCardItemCom> m_clkCB;

        override public void createCard(CardItemBase cardItem)
        {
            base.createCard(cardItem);

            UtilApi.setLayer(gameObject, Config.UIModelLayer);
            //UtilApi.setScale(gameObject.transform, new Vector3(0.24f, 1, 0.24f));

            setGameObject(gameObject);
            this.cardItemBase = cardItem;
        }

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