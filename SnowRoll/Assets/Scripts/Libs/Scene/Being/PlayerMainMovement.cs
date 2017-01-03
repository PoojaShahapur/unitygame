namespace SDK.Lib
{
    public class PlayerMainMovement : PlayerMovement
    {
        protected UnityEngine.Quaternion mForwardRotate;     // 记录当前的前向
        protected UnityEngine.Quaternion mRotate0;
        protected UnityEngine.Quaternion mRotate1;
        protected UnityEngine.Quaternion mRotate2;
        protected UnityEngine.Quaternion mRotate3;
        protected UnityEngine.Quaternion mRotate4;
        protected UnityEngine.Quaternion mRotate5;
        protected UnityEngine.Quaternion mRotate6;
        protected UnityEngine.Quaternion mRotate7;

        public PlayerMainMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mForwardRotate = UnityEngine.Quaternion.identity;
            this.mRotate0 = UnityEngine.Quaternion.identity;
            this.mRotate1 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 45, 0));
            this.mRotate2 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 90, 0));
            this.mRotate3 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 135, 0));

            this.mRotate4 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 180, 0));
            this.mRotate5 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 225, 0));
            this.mRotate6 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 270, 0));
            this.mRotate7 = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 315, 0));

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
            this.updateOrient();
            this.moveForward();
        }

        protected void onUpArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onDownArrowPress(IDispatchObject dispObj)
        {
            this.updateOrient();
            this.moveForward();
        }

        protected void onDownArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onLeftArrowPress(IDispatchObject dispObj)
        {
            this.updateOrient();
            this.moveForward();
        }

        protected void onLeftArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onRightArrowPress(IDispatchObject dispObj)
        {
            this.updateOrient();
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

        // 更新方向
        protected void updateOrient()
        {
            UnityEngine.Quaternion quat = mForwardRotate;

            if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.UpArrow))
            {
                if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.LeftArrow))
                {
                    quat = mForwardRotate * mRotate7;
                }
                else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.RightArrow))
                {
                    quat = mForwardRotate * mRotate1;
                }
                else
                {
                    quat = mForwardRotate;
                }
            }
            else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.DownArrow))
            {
                if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.LeftArrow))
                {
                    quat = mForwardRotate * mRotate5;
                }
                else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.RightArrow))
                {
                    quat = mForwardRotate * mRotate3;
                }
                else
                {
                    quat = mForwardRotate * mRotate4;
                }
            }
            else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.LeftArrow))
            {
                if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.UpArrow))
                {
                    quat = mForwardRotate * mRotate7;
                }
                else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.DownArrow))
                {
                    quat = mForwardRotate * mRotate5;
                }
                else
                {
                    quat = mForwardRotate * mRotate6;
                }
            }
            else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.RightArrow))
            {
                if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.UpArrow))
                {
                    quat = mForwardRotate * mRotate1;
                }
                else if (Ctx.mInstance.mInputMgr.keyJustPressed(InputKey.DownArrow))
                {
                    quat = mForwardRotate * mRotate3;
                }
                else
                {
                    quat = mForwardRotate * mRotate2;
                }
            }

            (this.mEntity as BeingEntity).setDestRotate(quat.eulerAngles, true);
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

        override public void setForwardRotate(UnityEngine.Vector3 rotate)
        {
            mForwardRotate = UnityEngine.Quaternion.Euler(rotate);
        }
    }
}