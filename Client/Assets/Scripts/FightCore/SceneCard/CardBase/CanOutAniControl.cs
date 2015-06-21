using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 初始牌动画控制器，只包括自己初始牌，法术卡、随从卡，不包括英雄卡、技能卡、武器卡
     */
    public class CanOutAniControl : ExceptBlackAniControl
    {
        protected EventDispatch m_onEnterHandleEntryDisp;
        protected ScaleGridElement m_scaleGridElement;

        public CanOutAniControl(SceneCardBase rhv) :
            base(rhv)
        {
            m_onEnterHandleEntryDisp = new AddOnceAndCallOnceEventDispatch();
        }

        protected void createAni()
        {
            if (m_startAni == null)
            {
                m_startAni = new DopeSheetAni();
                string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "SelfCardAni.asset");
                m_startAni.setAniEndDisp(onFaPai2MinAniEnd);
                m_startAni.setControlInfo(path);
                m_startAni.setGO(this.m_card.gameObject());
                m_startAni.syncUpdateControl();
            }
        }

        // 开始动画，发牌区域到场景中间
        override public void faPai2MinAni()
        {
            createAni();

            m_startAni.stateId = convIdx2StateId(0);
            m_startAni.play();
        }

        // 开始卡牌动画播放结束，注意开始有 3 张或者 4 张卡牌做动画，只有一个有回调函数
        protected void onFaPai2MinAniEnd(NumAniBase ani)
        {
            //m_startAni.stateId = 0;
            //m_startAni.play();
            //m_startAni.stop();

            //m_sceneCardBaseData.m_behaviorControl.moveToDestDirect(trackAniControl.destPos);    // 移动到终点位置
            Ctx.m_instance.m_logSys.log("自己卡牌从发牌区到场景区域动画结束");
        }

        // 开始动画，场景中间到手牌区域动画
        override public void min2HandleAni()
        {
            m_startAni.setAniEndDisp(onMin2HandleEnd);
            m_startAni.stateId = convIdx2StateId(1);
            m_startAni.play();
        }

        protected void onMin2HandleEnd(NumAniBase ani)
        {
            //m_startAni.stateId = 0;
            //m_startAni.play();
            m_onEnterHandleEntryDisp.dispatchEvent(this.m_card);
            Ctx.m_instance.m_logSys.log("自己卡牌从场景区域到手牌区域动画结束");
        }

        // 发牌区域到手牌区域
        override public void start2HandleAni()
        {
            createAni();

            m_startAni.setAniEndDisp(onStart2HandleAni);
            m_startAni.stateId = convIdx2StateId(3);
            m_startAni.play();
        }

        protected void onStart2HandleAni(NumAniBase ani)
        {
            Ctx.m_instance.m_logSys.log("自己卡牌从发牌区域到手牌区域动画结束");
            m_onEnterHandleEntryDisp.dispatchEvent(this.m_card);
        }

        override public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {
            m_onEnterHandleEntryDisp.addEventHandle(eventHandle);
        }

        protected int convIdx2StateId(int type)
        {
            if (0 == type)       // 获取发牌到场牌中心动画 Id
            {
                return m_card.getStartIdx() + 1;
            }
            else if (1 == type)  // 场牌到手牌动画 Id
            {
                return (m_card.getStartIdx() + 1 + 10);
            }
            else if (2 == type) // 不要的牌，回退发牌区
            {
                return (m_card.getStartIdx() + 1 + 20);
            }
            else if (3 == type)  // 直接从发牌区域到手牌区域
            {
                return 31;
            }

            return 1;
        }

        override public void initPosInfo()
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);
            m_curPt.rot = new UnityEngine.Vector3(0, 0, -3);
            m_curPt.scale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);

            m_curPt = m_wayPtList.getPosInfo(PosType.eHandUp);
            m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
            m_curPt.scale = new UnityEngine.Vector3(1, 1, 1);

            m_curPt = m_wayPtList.getPosInfo(PosType.eOutDown);
            m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
            m_curPt.scale = new UnityEngine.Vector3(1, 1, 1);

            m_curPt = m_wayPtList.getPosInfo(PosType.eWatchUp);
            m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
            m_curPt.scale = new UnityEngine.Vector3(1, 1, 1);
        }


        override public void setNormalPos(Vector3 pos)
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);
            m_curPt.pos = pos;
            moveToDestPos(PosType.eHandDown);
        }

        override public void setExpandPos(Vector3 pos)
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eScaleUp);

            m_curPt.pos = pos;
            if (m_scaleGridElement.isNormalState())
            {
                m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
                m_curPt.scale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
            }
            else
            {
                m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
                m_curPt.scale = new UnityEngine.Vector3(1, 1, 1);
            }

            moveToDestPos(PosType.eScaleUp);
        }

        override public void normalState()
        {
            m_scaleGridElement.normal();
        }

        override public void expandState()
        {
            m_scaleGridElement.expand();
        }
    }
}