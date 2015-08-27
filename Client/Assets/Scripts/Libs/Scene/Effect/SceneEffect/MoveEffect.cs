using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 可以移动的特效
     */
    public class MoveEffect : EffectBase
    {
        protected EffectMoveControl m_moveControl;
        protected Vector3 m_destPos;                // 终点
        protected EventDispatch m_moveDestEventDispatch;         // 移动到目标事件分发，注意不是
        protected float m_effectMoveTime;

        public MoveEffect(EffectRenderType renderType) :
            base(renderType)
        {
            m_moveControl = new EffectMoveControl(this);
            m_moveDestEventDispatch = new AddOnceAndCallOnceEventDispatch();
        }

        public EventDispatch moveDestEventDispatch
        {
            get
            {
                return m_moveDestEventDispatch;
            }
        }

        public Vector3 srcPos
        {
            get
            {
                return this.gameObject().transform.localPosition;
            }
            set
            {
                UtilApi.setPos(this.gameObject().transform, value);
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

        public float effectMoveTime
        {
            get
            {
                return m_effectMoveTime;
            }
            set
            {
                m_effectMoveTime = value;
            }
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
        }

        override public void setTableID(int tableId)
        {
            base.setTableID(tableId);
            //UtilApi.adjustEffectRST((m_render as EffectSpriteRender).spriteRender.selfGo.transform);
        }

        override public void play()
        {
            m_moveControl.moveToDest(this.gameObject().transform.localPosition, m_destPos, m_effectMoveTime, onMoveToDest);
            base.play();
        }

        public void syncUpdate()        // 加载资源
        {
            (m_render as SpriteEffectRender).spriteRender.syncUpdateCom();
        }

        public override void dispose()
        {
            m_moveDestEventDispatch.clearEventHandle();
            m_moveDestEventDispatch = null;

            base.dispose();
        }

        // 特效移动到终点
        protected void onMoveToDest(NumAniBase ani)
        {
            m_moveDestEventDispatch.dispatchEvent(this);
        }

        override public void addMoveDestEventHandle(Action<IDispatchObject> dispObj)
        {
            m_moveDestEventDispatch.addEventHandle(dispObj);
        }
    }
}