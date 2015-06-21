using SDK.Common;
using SDK.Lib;
using System;
using System.Collections;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 所有轨迹动画基类
     */
    public class TrackAniControl : CardControlBase, IElemMove
    {
        protected NumAniParallel m_numAniParal;       // 回退的时候，这个单独的动画序列
        protected WayPtList m_wayPtList;
        protected WayPtItem m_curPt;
        protected DopeSheetAni m_startAni;

        public TrackAniControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_numAniParal = new NumAniParallel();
            m_numAniParal.setAniSeqEndDisp(onMoveEndHandle);
            m_wayPtList = new WayPtList();
            initPosInfo();
        }

        public override void init()
        {
            base.init();
        }


        override public void dispose()
        {
            m_startAni.dispose();
            m_startAni = null;
            base.dispose();
        }

        public WayPtList wayPtList
        {
            get
            {
                return m_wayPtList;
            }
            set
            {
                m_wayPtList = value;
            }
        }

        // 移动到开始位置
        public void moveToStart()
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);
            UtilApi.setPos(m_card.transform(), m_curPt.pos);
            UtilApi.setRot(m_card.transform(), m_curPt.rot);
            UtilApi.setScale(m_card.transform(), m_curPt.scale);
        }

        //// 到目标位置，移动、旋转、缩放
        //private void moveToDestRST(PosType type = PosType.eHandDown)
        //{
        //    m_curPt = m_wayPtList.getPosInfo(type);

        //    RSTAni rstAni;
        //    rstAni = new RSTAni();
        //    m_numAniParal.addOneNumAni(rstAni);
        //    rstAni.setGO(m_card.gameObject());
        //    rstAni.destPos = m_curPt.pos;
        //    rstAni.destRot = m_curPt.rot;
        //    rstAni.destScale = m_curPt.scale;

        //    m_numAniParal.play();
        //}

        //// 到目标位置，移动、缩放
        //private void moveToDestST(PosType type = PosType.eHandDown)
        //{
        //    m_curPt = m_wayPtList.getPosInfo(type);

        //    STAni stAni;
        //    stAni = new STAni();
        //    m_numAniParal.addOneNumAni(stAni);
        //    stAni.setGO(m_card.gameObject());
        //    stAni.destPos = m_curPt.pos;
        //    stAni.destScale = m_curPt.scale;

        //    m_numAniParal.play();
        //}

        //// 到目标位置，移动、旋转
        //private void moveToDestRT(PosType type = PosType.eHandDown)
        //{
        //    m_curPt = m_wayPtList.getPosInfo(type);

        //    RTAni rtAni;
        //    rtAni = new RTAni();
        //    m_numAniParal.addOneNumAni(rtAni);
        //    rtAni.setGO(m_card.gameObject());
        //    rtAni.destPos = m_curPt.pos;
        //    rtAni.destRot = m_curPt.rot;

        //    m_numAniParal.play();
        //}

        //// 移动动画
        //private void moveToDestT(PosType type = PosType.eHandDown)
        //{
        //    m_curPt = m_wayPtList.getPosInfo(type);

        //    // 旋转和缩放直接设置
        //    UtilApi.setRot(m_card.transform(), m_curPt.rot);
        //    UtilApi.setScale(m_card.transform(), m_curPt.scale);

        //    PosAni posAni;
        //    posAni = new PosAni();
        //    m_numAniParal.addOneNumAni(posAni);
        //    posAni.setGO(m_card.gameObject());
        //    posAni.destPos = m_curPt.pos;

        //    m_numAniParal.play();
        //}

        //// 缩放
        //private void moveToDestScale(PosType type)
        //{
        //    m_curPt = m_wayPtList.getPosInfo(type);

        //    ScaleAni scaleAni = new ScaleAni();
        //    m_numAniParal.addOneNumAni(scaleAni);
        //    scaleAni.setGO(m_card.gameObject());
        //    scaleAni.destScale = m_curPt.scale;
        //    m_numAniParal.play();
        //}

        // 移动动画
        public void moveToDestPos(PosType type = PosType.eHandDown)
        {
            // 开始做轨迹动画前需要关闭 DopeSheet 动画 Drag Title 动画
            // 停止 DopeSheet 动画
            if (m_startAni != null)
            {
                m_startAni.stop();
                m_startAni.disable();
            }
            m_card.ioControl.disableDragTitle();

            m_curPt = m_wayPtList.getPosInfo(type);

            if (!UtilApi.isVectorEqual(m_card.transform().localEulerAngles, m_curPt.rot))
            {
                RotAni rotAni;
                rotAni = new RotAni();
                m_numAniParal.addOneNumAni(rotAni);
                rotAni.setGO(m_card.gameObject());
                rotAni.destRot = m_curPt.rot;
            }
            if (!UtilApi.isVectorEqual(m_card.transform().localScale, m_curPt.scale))
            {
                ScaleAni scaleAni;
                scaleAni = new ScaleAni();
                m_numAniParal.addOneNumAni(scaleAni);
                scaleAni.setGO(m_card.gameObject());
                scaleAni.destScale = m_curPt.scale;
            }
            if (!UtilApi.isVectorEqual(m_card.transform().localPosition, m_curPt.pos))
            {
                PosAni posAni;
                posAni = new PosAni();
                m_numAniParal.addOneNumAni(posAni);
                posAni.setGO(m_card.gameObject());
                posAni.destPos = m_curPt.pos;
            }

            m_numAniParal.play();
        }

        // 开始拖动自动动画
        public void startDragAni()
        {
            m_curPt = m_wayPtList.getAndAddPosInfo(PosType.eHandUp);
            // 位置直接到达位置
            m_curPt.pos = new Vector3(m_card.transform().localPosition.x, m_card.m_sceneDZData.getDragCardHeight(), m_card.transform().localPosition.z);
            UtilApi.setPos(m_card.transform(), m_curPt.pos);
            moveToDestPos(PosType.eHandUp);
        }

        // 结束拖动动画
        public void endDragAni()
        {
            // 缩放
            //destScale = SceneDZCV.SMALLFACT;
        }

        // 移动到之前的位置，拖放停止移动回原来的位置
        public void moveBackToPre()
        {
            // 由于拖动可能旋转和缩放都发生了改变，因此，RST 都需要变化
            moveToDestPos(PosType.eHandDown);
        }

        // 卡牌从已经出牌区域返回到手里的卡牌位置
        public void retFormOutAreaToHandleArea()
        {
            m_card.convHandleModel();
            m_card.ioControl.enableDrag();

            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_card.gameObject());
            posAni.destPos = new Vector3(m_card.transform().localPosition.x, m_card.m_sceneDZData.getDragCardHeight(), m_card.transform().localPosition.z);

            m_curPt = m_wayPtList.getPosInfo(PosType.eHandUp);

            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_card.gameObject());
            posAni.destPos = m_curPt.pos;
            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);

            STAni stAni;
            stAni = new STAni();
            m_numAniParal.addOneNumAni(stAni);
            stAni.setGO(m_card.gameObject());
            stAni.destPos = m_curPt.pos;
            stAni.destScale = m_curPt.scale;

            m_numAniParal.play();
        }

        public void updateOutCardScaleInfo(Transform trans)
        {
            m_curPt = m_wayPtList.getAndAddPosInfo(PosType.eOutDown);
            m_curPt.scale = trans.localScale;
        }

        // Enemy 卡牌初始位置，这个和自己卡牌手里的初始信息是不一样的
        virtual public void initPosInfo()
        {

        }
        
        protected void onMoveEndHandle(NumAniSeqBase dispObj)
        {
            // 只要一动结束，就开启动画
            if (m_startAni != null)
            {
                m_startAni.enable();
            }

            m_card.ioControl.enableDragTitle();
        }

        // 开始卡牌动画
        virtual public void faPai2MinAni()
        {

        }

        virtual public void min2HandleAni()
        {

        }

        virtual public void start2HandleAni()
        {

        }

        virtual public void addEnterHandleEntryDisp(System.Action<IDispatchObject> eventHandle)
        {

        }

        virtual public void startEnemyFaPaiAni()
        {

        }

        virtual public void setPos(Vector3 pos)
        {

        }

        virtual public void updatePos()
        {

        }

        virtual public void normalState()
        {

        }

        virtual public void expandState()
        {

        }
    }
}