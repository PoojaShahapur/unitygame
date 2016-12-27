﻿namespace SDK.Lib
{
    public class PlayerMainMovement : PlayerMovement
    {
        public PlayerMainMovement(SceneEntityBase entity)
            : base(entity)
        {
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.UpArrow, EventId.KEYPRESS_EVENT, onUpArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.UpArrow, EventId.KEYUP_EVENT, onUpArrowUp);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.W, EventId.KEYPRESS_EVENT, onUpArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.W, EventId.KEYUP_EVENT, onUpArrowUp);

            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.DownArrow, EventId.KEYPRESS_EVENT, onDownArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.DownArrow, EventId.KEYUP_EVENT, onDownArrowUp);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.S, EventId.KEYPRESS_EVENT, onDownArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.S, EventId.KEYUP_EVENT, onDownArrowUp);

            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.L, EventId.KEYUP_EVENT, onStartUp);
            Ctx.mInstance.mInputMgr.addAccelerationListener(EventId.ACCELERATIONMOVED_EVENT, onAccelerationMovedHandle);

            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.LeftArrow, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.LeftArrow, EventId.KEYUP_EVENT, onLeftArrowUp);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.A, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.A, EventId.KEYUP_EVENT, onLeftArrowUp);

            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.RightArrow, EventId.KEYPRESS_EVENT, onRightArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.RightArrow, EventId.KEYUP_EVENT, onRightArrowUp);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.D, EventId.KEYPRESS_EVENT, onRightArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.D, EventId.KEYUP_EVENT, onRightArrowUp);
        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            base.dispose();
        }

        override public void onTick(float delta)
        {
            // 绘制调试信息
            UtilApi.DrawLine(this.mEntity.getPos(), (this.mEntity as Player).mPlayerSplitMerge.getTargetPoint(), UnityEngine.Color.red);

            base.onTick(delta);
        }

        protected void onUpArrowPress(IDispatchObject dispObj)
        {
            UnityEngine.Quaternion quat = UtilMath.getRotateByOrient(UnityEngine.Vector3.forward);
            (this.mEntity as BeingEntity).setDestRotate(quat.eulerAngles, true);
            this.moveForward();
        }

        protected void onUpArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onDownArrowPress(IDispatchObject dispObj)
        {
            UnityEngine.Quaternion quat = UtilMath.getRotateByOrient(UnityEngine.Vector3.back);
            (this.mEntity as BeingEntity).setDestRotate(quat.eulerAngles, true);
            this.moveForward();
        }

        protected void onDownArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onLeftArrowPress(IDispatchObject dispObj)
        {
            //this.rotateLeft();
            UnityEngine.Quaternion quat = UtilMath.getRotateByOrient(UnityEngine.Vector3.left);
            (this.mEntity as BeingEntity).setDestRotate(quat.eulerAngles, true);
            this.moveForward();
        }

        protected void onLeftArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onRightArrowPress(IDispatchObject dispObj)
        {
            //this.rotateRight();
            UnityEngine.Quaternion quat = UtilMath.getRotateByOrient(UnityEngine.Vector3.right);
            (this.mEntity as BeingEntity).setDestRotate(quat.eulerAngles, true);
            this.moveForward();
        }

        protected void onRightArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        //protected void onStartUp(IDispatchObject dispObj)
        //{
        //    UnityEngine.Vector3 pos = this.mEntity.getPos();
        //    Player player = Ctx.mInstance.mPlayerMgr.getEntityByIndex(1) as Player;
        //    player.setDestPos(pos);
        //}

        override public void stopMove()
        {
            base.stopMove();

            Ctx.mInstance.mGlobalDelegate.mMainOrientStopChangedDispatch.dispatchEvent(this);
        }

        override public void stopRotate()
        {
            base.stopRotate();
        }

        override public void addActorLocalOffset(UnityEngine.Vector3 DeltaLocation)
        {
            base.addActorLocalOffset(DeltaLocation);
            this.onPosChanged();
        }

        override public void addActorLocalDestOffset(UnityEngine.Vector3 DeltaLocation)
        {
            base.addActorLocalDestOffset(DeltaLocation);
            this.onPosChanged();
        }

        override public void addLocalRotation(UnityEngine.Vector3 DeltaRotation)
        {
            base.addLocalRotation(DeltaRotation);
            (this.mEntity as Player).mPlayerSplitMerge.calcTargetPoint();
            Ctx.mInstance.mGlobalDelegate.mMainOrientChangedDispatch.dispatchEvent(this);
        }

        override public void setDestRotate(UnityEngine.Vector3 destRotate)
        {
            base.setDestRotate(destRotate);
            (this.mEntity as Player).mPlayerSplitMerge.calcTargetPoint();
            Ctx.mInstance.mGlobalDelegate.mMainOrientChangedDispatch.dispatchEvent(this);
        }

        protected void onAccelerationMovedHandle(IDispatchObject disoObj)
        {
            MAcceleration acceleration = disoObj as MAcceleration;
            (this.mEntity as Player).setDestRotate(acceleration.getOrient().eulerAngles, true);

            this.moveForward();

            Ctx.mInstance.mLogSys.log(string.Format("Acceleration orient is x = {0}, y = {1}, z = {2}", acceleration.getOrient().eulerAngles.x, acceleration.getOrient().eulerAngles.y, acceleration.getOrient().eulerAngles.z), LogTypeId.eLogAcceleration);
        }

        // 主角不移动，通过中心点移动
        override public void moveForward()
        {
            base.moveForward();
            //this.onPosChanged();
        }

        override public void sendMoveMsg()
        {
            // 移动后，更新 KBE 中的 Avatar 数据
            KBEngine.Event.fireIn(
                "updatePlayer",
                mEntity.getPos().x,
                mEntity.getPos().y,
                mEntity.getPos().z,
                mEntity.getRotateEulerAngle().y
                );
        }

        protected void onPosChanged()
        {
            Ctx.mInstance.mGlobalDelegate.mMainPosChangedDispatch.dispatchEvent(this);

            (this.mEntity as PlayerMain).onChildChanged();
        }
    }
}