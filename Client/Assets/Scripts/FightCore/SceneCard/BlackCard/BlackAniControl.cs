using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class BlackAniControl : TrackAniControl
    {
        protected EventDispatch m_onEnterHandleEntryDisp;

        public BlackAniControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_onEnterHandleEntryDisp = new AddOnceAndCallOnceEventDispatch();
        }

        override public void dispose()
        {
            base.dispose();
        }

        protected void createAni()
        {
            if (m_startAni == null)
            {
                m_startAni = new DopeSheetAni();
                string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "EnemyCardAni.asset");

                m_startAni.setAniEndDisp(onEnemyFaPaiAniEnd);
                m_startAni.setControlInfo(path);
                m_startAni.setGO(this.m_card.gameObject());
                m_startAni.syncUpdateControl();
            }
        }

        // 开始动画，发牌区域到场景中间
        override public void startEnemyFaPaiAni()
        {
            createAni();

            m_startAni.stateId = 1;
            m_startAni.play();
        }

        // 开始卡牌动画播放结束
        protected void onEnemyFaPaiAniEnd(NumAniBase ani)
        {
            m_onEnterHandleEntryDisp.dispatchEvent(this.m_card);
            Ctx.m_instance.m_logSys.log("Enemy 卡牌从发牌区域到手牌区域动画结束");
        }

        override public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {
            m_onEnterHandleEntryDisp.addEventHandle(eventHandle);
        }

        // Enemy 卡牌初始位置，这个和自己卡牌手里的初始信息是不一样的
        override public void initPosInfo()
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);
            m_curPt.rot = new Vector3(-90, -180, -180);
            m_curPt.scale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}