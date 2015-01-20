using Game.Msg;
using SDK.Common;
using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景界面中可交互的，处理事件的
     */
    public class ItemSceneIOBase : ItemSceneBase
    {
        public Action<ItemSceneIOBase> m_clkCB;

        public override void onloaded(IDispatchObject resEvt)            // 资源加载成功
        {
            base.onloaded(resEvt);
            addEventHandle();
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_go, onBtnClkOpen);
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
    }
}