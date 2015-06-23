using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class ExceptBlackIOControl : IOControlBase
    {
        public ExceptBlackIOControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        public override void init()
        {
            base.init();
            this.m_card.clickEntityDisp.addEventHandle(onCardClick);
        }

        // 输入按下
        override public void onCardDown(IDispatchObject dispObj)
        {
            m_card.m_sceneDZData.m_watchCardInfo.startWatch(m_card);
        }

        // 输入释放
        override public void onCardUp(IDispatchObject dispObj)
        {
            m_card.m_sceneDZData.m_watchCardInfo.stopWatch();
        }

        // 结束转换模型
        override public void endConvModel(int type)
        {
            this.m_card.clickEntityDisp.addEventHandle(onCardClick);
        }
    }
}