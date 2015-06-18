using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 敌人的背面牌
     */
    public class BlackCard : SceneCardBase
    {
        protected DopeSheetAni m_startAni;
        protected EventDispatch m_onEnterHandleEntryDisp;

        public BlackCard(SceneDZData data) : 
            base(data)
        {
            m_sceneCardBaseData = new SceneCardBaseData();
            m_sceneCardBaseData.m_trackAniControl = new TrackAniControl(this);
            m_sceneCardBaseData.m_trackAniControl.initBlackPos();
            m_render = new BlackCardRender(this);
            m_onEnterHandleEntryDisp = new AddOnceAndCallOnceEventDispatch();
        }

        override public void dispose()
        {
            Ctx.m_instance.m_logSys.log("客户端彻底删除 Enemy Hand 卡牌");
            m_startAni.dispose();
            m_startAni = null;
            base.dispose();
        }

        override protected void removeRef()
        {
            m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].removeOneCard(this);
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as BlackCardRender).setIdAndPnt(objId, pntGo_);
        }

        protected void createAni()
        {
            if (m_startAni == null)
            {
                m_startAni = new DopeSheetAni();
                string path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSceneAnimatorController], "EnemyCardAni.asset");

                m_startAni.setAniEndDisp(onEnemuFaPaiAniEnd);
                m_startAni.setControlInfo(path);
                m_startAni.setGO(this.gameObject());
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

        // 开始卡牌动画播放结束，注意开始有 3 张或者 4 张卡牌做动画，只有一个有回调函数
        protected void onEnemuFaPaiAniEnd(NumAniBase ani)
        {
            m_onEnterHandleEntryDisp.dispatchEvent(this);
            Ctx.m_instance.m_logSys.log("Enemy 卡牌从发牌区域到手牌区域动画结束");
        }

        override public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {
            m_onEnterHandleEntryDisp.addEventHandle(eventHandle);
        }

        // 更新初始卡牌场景位置信息
        override public void updateInitCardSceneInfo(Transform trans)
        {
            UtilApi.setPos(this.transform(), trans.localPosition);
            UtilApi.setScale(this.transform(), trans.localScale);
            UtilApi.setRot(this.transform(), trans.localRotation);
        }
    }
}