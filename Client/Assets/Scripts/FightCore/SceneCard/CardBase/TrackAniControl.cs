﻿using SDK.Common;
using System.Collections;
using UnityEngine;

namespace FightCore
{
    public class TrackAniControl : CardControlBase
    {
        // 仅仅是在做移动动画的时候才会使用，人为拖动不会改变这两个值
        // 开始信息
        protected Vector3 m_startPos;       // 最终位置之前的一个位置
        protected Vector3 m_startRot;       // 最终旋转之前的一个旋转
        protected Vector3 m_startScale;     // 最终缩放之前的一个缩放

        // 目标信息
        protected Vector3 m_destPos;       // 最终位置
        protected Vector3 m_destRot;       // 最终旋转
        protected Vector3 m_destScale;     // 最终缩放

        // 路径堆栈
        protected Stack m_pathStack = new Stack();              // 路径堆栈，记录路径信息
        protected const float m_time = 0.5f;      // 动画时间
        protected NumAniParallel m_numAniParal = new NumAniParallel();       // 回退的时候，这个单独的动画序列
        protected const float m_height = 1.0f;

        public TrackAniControl(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        public override void init()
        {
            base.init();

            m_startPos = m_card.transform().localPosition;
            m_startRot = m_card.transform().localRotation.eulerAngles;
            m_startScale = m_card.transform().localScale;

            m_destPos = m_card.transform().localPosition;
            m_destRot = m_card.transform().localRotation.eulerAngles;
            m_destScale = m_card.transform().localScale;
        }

        public Vector3 startPos
        {
            set
            {
                m_startPos = value;
            }
        }

        public Vector3 startRot
        {
            set
            {
                m_startRot = value;
            }
        }

        public Vector3 startScale
        {
            set
            {
                m_startScale = value;
            }
        }

        public Vector3 destPos
        {
            get
            {
                return m_destPos;
            }
            set
            {
                m_destPos = value;
            }
        }

        public Vector3 destRot
        {
            set
            {
                m_destRot = value;
            }
        }

        public Vector3 destScale
        {
            set
            {
                m_destScale = value;
            }
        }

        public void moveToStart()
        {
            m_card.transform().localPosition = m_startPos;
            m_card.transform().localRotation = Quaternion.Euler(m_startRot);
            m_card.transform().localScale = m_startScale;
        }

        // 到目标位置，移动、旋转、缩放
        public void moveToDestRST()
        {
            saveCurRSTToStart();

            RSTAni rstAni;
            rstAni = new RSTAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            rstAni.destPos = m_destPos;
            rstAni.destRot = m_destRot;
            rstAni.destScale = m_destScale;

            m_numAniParal.play();
        }

        // 到目标位置，移动、旋转
        public void moveToDestRT()
        {
            saveCurRSTToStart();

            RTAni rtAni;
            rtAni = new RTAni();
            m_numAniParal.addOneNumAni(rtAni);
            rtAni.setGO(m_card.gameObject());
            rtAni.destPos = m_destPos;
            rtAni.destRot = m_destRot;

            m_numAniParal.play();
        }

        // 移动动画
        public void moveToDestT()
        {
            saveCurRSTToStart();

            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_card.gameObject());
            posAni.destPos = m_destPos;

            m_numAniParal.play();
        }

        // 开始拖动自动动画
        public void startDragAni()
        {
            // 一定要先保存信息
            //saveCurRSTToStart();      // 移动后，位置信息都已经改变后，才调用拖动的动画
            saveDestToStart();

            // 缩放
            destScale = SceneCardBase.BIGFACT;
            // 缩放直接到达位置
            m_destPos.y = m_height;
            m_card.transform().localPosition = m_destPos;

            moveScaleToDest();

            pushStartAndEndToStack();
        }

        // 结束拖动动画
        public void endDragAni()
        {
            // 缩放
            destScale = SceneCardBase.SMALLFACT;
        }

        // 保存当前的信息
        protected void saveCurRSTToStart()
        {
            m_startPos = m_card.transform().localPosition;
            m_startRot = m_card.transform().localRotation.eulerAngles;
            m_startScale = m_card.transform().localScale;
        }

        // 保存
        protected void saveStartToDest()
        {
            m_destPos = m_startPos;
            m_destRot = m_startRot;
            m_destScale = m_startScale;
        }

        //保存
        protected void saveDestToStart()
        {
            m_startPos = m_destPos;
            m_startRot = m_destRot;
            m_startScale = m_destScale;
        }

        //// 缩放
        protected void moveScaleToDest()
        {
            m_startScale = m_card.transform().localScale;

            ScaleAni rstAni = new ScaleAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            rstAni.destScale = m_destScale;
            m_numAniParal.play();
        }

        // 移动到之前的位置
        public void moveBackToPre()
        {
            // 将堆栈清空
            m_pathStack.Clear();

            saveStartToDest();
            moveToDestRST();
        }

        // 保存开始点和结束点到堆栈
        protected void pushStartAndEndToStack()
        {
            // 顺序是 T R S
            m_pathStack.Push(m_startPos);
            m_pathStack.Push(m_startRot);
            m_pathStack.Push(m_startScale);

            m_pathStack.Push(m_destPos);
            m_pathStack.Push(m_destRot);
            m_pathStack.Push(m_destScale);
        }

        // 卡牌从已经出牌区域返回到手里的卡牌位置
        public void retFormOutAreaToHandleArea()
        {
            m_card.dragControl.enableDrag();

            RSTAni rstAni = new RSTAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            rstAni.destPos = new Vector3(m_card.transform().localPosition.x, 1.0f, m_card.transform().localPosition.z);
            rstAni.destScale = SceneCardBase.BIGFACT;
            rstAni.destRot = m_card.transform().localRotation.eulerAngles;

            rstAni = new RSTAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            rstAni.destScale = (Vector3)m_pathStack.Pop();
            m_pathStack.Pop();
            rstAni.destRot = Vector3.one;
            rstAni.destPos = (Vector3)m_pathStack.Pop();

            m_destScale = (Vector3)m_pathStack.Pop();
            m_destRot = (Vector3)m_pathStack.Pop();
            m_destPos = (Vector3)m_pathStack.Pop();

            rstAni = new RSTAni();
            m_numAniParal.addOneNumAni(rstAni);
            rstAni.setGO(m_card.gameObject());
            rstAni.destScale = m_destScale;
            rstAni.destRot = m_destRot;
            rstAni.destPos = m_destPos;

            m_numAniParal.play();
        }
    }
}