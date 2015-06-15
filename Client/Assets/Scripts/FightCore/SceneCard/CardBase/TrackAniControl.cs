using SDK.Common;
using SDK.Lib;
using System;
using System.Collections;
using UnityEngine;

namespace FightCore
{
    public class TrackAniControl : CardControlBase
    {
        // 仅仅是在做移动动画的时候才会使用，人为拖动不会改变这两个值
        // 开始信息
        //protected Vector3 m_startPos;       // 最终位置之前的一个位置
        //protected Vector3 m_startRot;       // 最终旋转之前的一个旋转
        //protected Vector3 m_startScale;     // 最终缩放之前的一个缩放

        // 目标信息
        //protected Vector3 m_destPos;       // 最终位置
        //protected Vector3 m_destRot;       // 最终旋转
        //protected Vector3 m_destScale;     // 最终缩放

        // 路径堆栈
        //protected Stack m_pathStack = new Stack();              // 路径堆栈，记录路径信息
        //protected const float m_time = 0.5f;      // 动画时间
        protected NumAniParallel m_numAniParal = new NumAniParallel();       // 回退的时候，这个单独的动画序列
        protected WayPtList m_wayPtList;
        protected WayPtItem m_curPt;

        public TrackAniControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_wayPtList = new WayPtList();
        }

        public override void init()
        {
            base.init();

            //m_startPos = m_card.transform().localPosition;
            //m_startRot = m_card.transform().localRotation.eulerAngles;
            //m_startScale = m_card.transform().localScale;

            //m_destPos = m_card.transform().localPosition;
            //m_destRot = m_card.transform().localRotation.eulerAngles;
            //m_destScale = m_card.transform().localScale;
        }

        //public Vector3 startPos
        //{
        //    set
        //    {
        //        m_startPos = value;
        //    }
        //}

        //public Vector3 startRot
        //{
        //    set
        //    {
        //        m_startRot = value;
        //    }
        //}

        //public Vector3 startScale
        //{
        //    set
        //    {
        //        m_startScale = value;
        //    }
        //}

        //public Vector3 destPos
        //{
        //    get
        //    {
        //        return m_destPos;
        //    }
        //    set
        //    {
        //        m_destPos = value;
        //    }
        //}

        //public Vector3 destRot
        //{
        //    set
        //    {
        //        m_destRot = value;
        //    }
        //}

        //public Vector3 destScale
        //{
        //    set
        //    {
        //        m_destScale = value;
        //    }
        //}

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
            //UtilApi.setPos(m_card.transform(), m_startPos);
            //UtilApi.setRot(m_card.transform(), m_startRot);         // Quaternion.Euler(m_startRot)
            //UtilApi.setScale(m_card.transform(), m_startScale);

            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);
            UtilApi.setPos(m_card.transform(), m_curPt.pos);
            UtilApi.setRot(m_card.transform(), m_curPt.rot);
            UtilApi.setScale(m_card.transform(), m_curPt.scale);
        }

        // 到目标位置，移动、旋转、缩放
        public void moveToDestRST(PosType type = PosType.eHandDown)
        {
            //saveCurRSTToStart();

            m_curPt = m_wayPtList.getPosInfo(type);

            RSTAni rstAni;
            rstAni = new RSTAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            rstAni.destPos = m_curPt.pos;
            rstAni.destRot = m_curPt.rot;
            rstAni.destScale = m_curPt.scale;

            m_numAniParal.play();
        }

        // 到目标位置，移动、缩放
        public void moveToDestST(PosType type = PosType.eHandDown)
        {
            //saveCurRSTToStart();

            m_curPt = m_wayPtList.getPosInfo(type);

            STAni stAni;
            stAni = new STAni();
            m_numAniParal.addOneNumAni(stAni);
            stAni.setGO(m_card.gameObject());
            stAni.destPos = m_curPt.pos;
            stAni.destScale = m_curPt.scale;

            m_numAniParal.play();
        }

        // 到目标位置，移动、旋转
        public void moveToDestRT(PosType type = PosType.eHandDown)
        {
            //saveCurRSTToStart();

            m_curPt = m_wayPtList.getPosInfo(type);

            RTAni rtAni;
            rtAni = new RTAni();
            m_numAniParal.addOneNumAni(rtAni);
            rtAni.setGO(m_card.gameObject());
            rtAni.destPos = m_curPt.pos;
            rtAni.destRot = m_curPt.rot;

            m_numAniParal.play();
        }

        // 移动动画
        public void moveToDestT(PosType type = PosType.eHandDown)
        {
            //saveCurRSTToStart();

            m_curPt = m_wayPtList.getPosInfo(type);

            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_card.gameObject());
            posAni.destPos = m_curPt.pos;

            m_numAniParal.play();
        }

        // 开始拖动自动动画
        public void startDragAni()
        {
            // 一定要先保存信息
            //saveCurRSTToStart();      // 移动后，位置信息都已经改变后，才调用拖动的动画
            //saveDestToStart();

            m_curPt = m_wayPtList.getAndAddPosInfo(PosType.eHandUp);
            // 缩放
            //destScale = SceneDZCV.BIGFACT;
            m_curPt.scale = SceneDZCV.BIGFACT;
            // 缩放直接到达位置
            //m_destPos.y = m_card.m_sceneDZData.getDragCardHeight();
            m_curPt.pos = new Vector3(m_card.transform().localPosition.x, m_card.m_sceneDZData.getDragCardHeight(), m_card.transform().localPosition.z);
            //UtilApi.setPos(m_card.transform(), m_destPos);
            UtilApi.setPos(m_card.transform(), m_curPt.pos);

            moveScaleToDest(PosType.eHandUp);

            //pushStartAndEndToStack();
        }

        // 结束拖动动画
        public void endDragAni()
        {
            // 缩放
            //destScale = SceneDZCV.SMALLFACT;
        }

        // 保存当前的信息
        protected void saveCurRSTToStart()
        {
            //m_startPos = m_card.transform().localPosition;
            //m_startRot = m_card.transform().localRotation.eulerAngles;
            //m_startScale = m_card.transform().localScale;

            m_curPt = m_wayPtList.getAndAddPosInfo(PosType.eHandDown);

            m_curPt.pos = m_card.transform().localPosition;
            m_curPt.rot = m_card.transform().localRotation.eulerAngles;
            m_curPt.scale = m_card.transform().localScale;
        }

        // 保存
        protected void saveStartToDest()
        {
            //m_destPos = m_startPos;
            //m_destRot = m_startRot;
            //m_destScale = m_startScale;
        }

        // 保存
        protected void saveDestToStart()
        {
            //m_startPos = m_destPos;
            //m_startRot = m_destRot;
            //m_startScale = m_destScale;
        }

        // 缩放
        protected void moveScaleToDest(PosType type)
        {
            //m_startScale = m_card.transform().localScale;

            m_curPt = m_wayPtList.getPosInfo(type);

            ScaleAni rstAni = new ScaleAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            //rstAni.destScale = m_destScale;
            rstAni.destScale = m_curPt.scale;
            m_numAniParal.play();
        }

        // 移动到之前的位置，拖放停止移动回原来的位置
        public void moveBackToPre()
        {
            // 将堆栈清空
            //m_pathStack.Clear();

            //saveStartToDest();
            //  由于拖动可能旋转和缩放都发生了改变，因此， RST 都需要变化
            moveToDestRST(PosType.eHandDown);
        }

        // 保存开始点和结束点到堆栈
        protected void pushStartAndEndToStack()
        {
            // 顺序是 T R S
            //m_pathStack.Push(m_startPos);
            //m_pathStack.Push(m_startRot);
            //m_pathStack.Push(m_startScale);

            //m_pathStack.Push(m_destPos);
            //m_pathStack.Push(m_destRot);
            //m_pathStack.Push(m_destScale);
        }

        // 卡牌从已经出牌区域返回到手里的卡牌位置
        public void retFormOutAreaToHandleArea()
        {
            m_card.convHandleModel();
            m_card.dragControl.enableDrag();

            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_card.gameObject());
            posAni.destPos = new Vector3(m_card.transform().localPosition.x, m_card.m_sceneDZData.getDragCardHeight(), m_card.transform().localPosition.z);
            //rstAni.destScale = SceneDZCV.BIGFACT;
            //rstAni.destRot = m_card.transform().localRotation.eulerAngles;

            m_curPt = m_wayPtList.getPosInfo(PosType.eHandUp);

            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_card.gameObject());
            //rstAni.destScale = (Vector3)m_pathStack.Pop();
            //m_pathStack.Pop();
            //rstAni.destRot = Vector3.one;
            //rstAni.destPos = (Vector3)m_pathStack.Pop();
            posAni.destPos = m_curPt.pos;

            //m_destScale = (Vector3)m_pathStack.Pop();
            //m_destRot = (Vector3)m_pathStack.Pop();
            //m_destPos = (Vector3)m_pathStack.Pop();

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
    }
}