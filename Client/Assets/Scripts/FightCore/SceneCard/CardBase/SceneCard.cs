using BehaviorLibrary;
using FSM;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 英雄卡、技能卡、装备卡、随从卡、法术卡基类，但是不是对方手里的背面卡的基类
     */
    public class SceneCard : SceneCardBase
    {
        protected bool m_svrDispose;        // 服务器已经删除了这个对象

        public SceneCard(SceneDZData data) : 
            base(data)
        {
            m_svrDispose = false;
            m_sceneCardBaseData = new SceneCardBaseData();
            m_sceneCardBaseData.m_fightData = new FightData();
            m_sceneCardBaseData.m_animFSM = new AnimFSM();
            m_sceneCardBaseData.m_animFSM.card = this;
            m_sceneCardBaseData.m_animFSM.Start();

            m_sceneCardBaseData.m_moveControl = new CardMoveControl(this);

            m_sceneCardBaseData.m_aiController = new AIController();
            m_sceneCardBaseData.m_aiController.btID = BTID.e1000;
            m_sceneCardBaseData.m_aiController.possess(this);
        }

        override public bool getSvrDispose()
        {
            return m_svrDispose;
        }

        override public void setSvrDispose(bool rhv = true)
        {
            m_svrDispose = rhv;
        }

        override public void init()
        {
            base.init();

            if (m_sceneCardBaseData.m_clickControl != null)
            {
                m_sceneCardBaseData.m_clickControl.init();
            }
            if (m_sceneCardBaseData.m_trackAniControl != null)
            {
                m_sceneCardBaseData.m_trackAniControl.init();
            }
            if (m_sceneCardBaseData.m_dragControl != null)
            {
                m_sceneCardBaseData.m_dragControl.init();
            }
            if (m_sceneCardBaseData.m_effectControl != null)
            {
                m_sceneCardBaseData.m_effectControl.init();
            }
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
            //m_sceneCardBaseData.m_animFSM.Update();                 // 更新状态机
            if (m_sceneCardBaseData.m_fightData != null)
            {
                m_sceneCardBaseData.m_fightData.onTime(delta);          // 更新战斗数据
            }
            if(m_sceneCardBaseData.m_aiController != null)
            {
                m_sceneCardBaseData.m_aiController.onTick(delta);
            }
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as CardPlayerRender).setIdAndPnt(objId, pntGo_);
        }

        // 是否在战斗中
        override public bool bInFight()
        {
            return m_sceneCardBaseData.m_fightData.attackData.attackList.Count() > 0 || m_sceneCardBaseData.m_fightData.hurtData.hurtList.Count() > 0;
        }

        // 客户端检测是否能删除
        override public bool canDelFormClient()
        {
            return m_svrDispose && this.m_sceneCardItem.svrCard.hp == 0;        // 如果服务器认为已经释放并且血是 0 了，就可以删除了
        }
    }
}