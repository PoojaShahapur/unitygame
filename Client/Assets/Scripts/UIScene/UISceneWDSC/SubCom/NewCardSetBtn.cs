using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /**
     * @brief 添加卡牌组按钮处理事件
     */
    public class NewCardSetBtn : InterActiveEntity
    {
        public SceneWDSCData m_sceneWDSCData;

        public override void Start()
        {

        }

        //选职业
        public void OnMouseUpAsButton()
        {

        }

        Vector3 lastpostion;
        public void hide()
        {
            lastpostion = transform.localPosition;
            transform.Translate(new Vector3(0, 0, -20f));
        }

        public void goBack()
        {
            transform.localPosition = lastpostion;
        }

        // 更新新建按钮位置
        public void updatePos()
        {
            transform.localPosition = new Vector3(0, 0, -0.525f * m_sceneWDSCData.m_wdscCardSetPnl.m_leftCardGroupList.getChildCount());
        }
    }
}