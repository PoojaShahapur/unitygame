using SDK.Common;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class SceneAniCard : SceneCardEntityBase
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
        protected NumAniSeq m_numAniSeq = new NumAniSeq();       // 回退的时候，这个单独的动画序列
        protected const float m_height = 1.0f;

        public override void Start()
        {
            base.Start();

            m_startPos = transform.localPosition;
            m_startRot = transform.localRotation.eulerAngles;
            m_startScale = transform.localScale;

            m_destPos = transform.localPosition;
            m_destRot = transform.localRotation.eulerAngles;
            m_destScale = transform.localScale;
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
            transform.localPosition = m_startPos;
            transform.localRotation = Quaternion.Euler(m_startRot);
            transform.localScale = m_startScale;
        }

        // 到目标位置
        public void moveToDest()
        {
            saveCurRSTToStart();

            RSTAni rstAni = new RSTAni();
            m_numAniSeq.addOneNumAni(rstAni);
            rstAni.setGO(gameObject);
            rstAni.destPos = m_destPos;
            rstAni.destRot = m_destRot;
            rstAni.destScale = m_destScale;

            m_numAniSeq.play();
        }

        // 开始拖动自动动画
        public void startDragAni()
        {
            // 一定要先保存信息
            //saveCurRSTToStart();      // 移动后，位置信息都已经改变后，才调用拖动的动画
            saveDestToStart();

            // 缩放
            destScale = SceneCardEntityBase.BIGFACT;
            // 缩放直接到达位置
            m_destPos.y = m_height;
            transform.localPosition = m_destPos;

            moveScaleToDest();

            pushStartAndEndToStack();
        }

        // 结束拖动动画
        public void endDragAni()
        {
            // 缩放
            destScale = SceneCardEntityBase.SMALLFACT;
        }

        // 保存当前的信息
        protected void saveCurRSTToStart()
        {
            m_startPos = transform.localPosition;
            m_startRot = transform.localRotation.eulerAngles;
            m_startScale = transform.localScale;
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
            m_startScale = transform.localScale;

            ScaleAni rstAni = new ScaleAni();
            m_numAniSeq.addOneNumAni(rstAni);
            rstAni.setGO(gameObject);
            rstAni.destScale = m_destScale;
            m_numAniSeq.play();
        }

        // 移动到之前的位置
        public void moveBackToPre()
        {
            // 将堆栈清空
            m_pathStack.Clear();

            saveStartToDest();
            moveToDest();
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
            enableDrag();

            SimpleCurveAni rstAni = new SimpleCurveAni();
            m_numAniSeq.addOneNumAni(rstAni);
            rstAni.setGO(gameObject);

            m_numAniSeq.play();
        }
    }
}