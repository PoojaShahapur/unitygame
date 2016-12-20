using System;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 可以移动的特效
     */
    public class MoveEffect : EffectBase
    {
        protected EffectMoveControl mMoveControl;
        protected Vector3 mDestPos;                // 终点
        protected EventDispatch mMoveDestEventDispatch;         // 移动到目标事件分发，注意不是
        protected float mEffectMoveTime;

        public MoveEffect(EffectRenderType renderType) :
            base(renderType)
        {
            mMoveControl = new EffectMoveControl(this);
            mMoveDestEventDispatch = new AddOnceAndCallOnceEventDispatch();
        }

        public EventDispatch moveDestEventDispatch
        {
            get
            {
                return mMoveDestEventDispatch;
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
                return mDestPos;
            }
            set
            {
                mDestPos = value;
            }
        }

        public float effectMoveTime
        {
            get
            {
                return mEffectMoveTime;
            }
            set
            {
                mEffectMoveTime = value;
            }
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
        }

        override public void setTableID(int tableId)
        {
            base.setTableID(tableId);
            //UtilApi.adjustEffectRST((mRender as EffectSpriteRender).spriteRender.selfGo.transform);
        }

        override public void play()
        {
            mMoveControl.moveToDest(this.gameObject().transform.localPosition, mDestPos, mEffectMoveTime, onMoveToDest);
            base.play();
        }

        public void syncUpdate()        // 加载资源
        {
            (mRender as SpriteEffectRender).spriteRender.syncUpdateCom();
        }

        public override void dispose()
        {
            mMoveDestEventDispatch.clearEventHandle();
            mMoveDestEventDispatch = null;

            base.dispose();
        }

        // 特效移动到终点
        protected void onMoveToDest(NumAniBase ani)
        {
            mMoveDestEventDispatch.dispatchEvent(this);
        }

        override public void addMoveDestEventHandle(MAction<IDispatchObject> dispObj)
        {
            mMoveDestEventDispatch.addEventHandle(null, dispObj);
        }
    }
}