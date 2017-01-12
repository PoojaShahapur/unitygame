namespace SDK.Lib
{
    public class PlayerMainMovement : PlayerMovement
    {
        protected UnityEngine.Quaternion mForwardRotate;     // 记录当前的前向
        protected UnityEngine.Quaternion mRotate;

        protected bool isUpPress = false;
        protected bool isDownPress = false;
        protected bool isLeftPress = false;
        protected bool isRightPress = false;

        public PlayerMainMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mForwardRotate = UnityEngine.Quaternion.identity;
            this.mRotate = UnityEngine.Quaternion.identity;

            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.UpArrow, EventId.KEYPRESS_EVENT, onUpArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.UpArrow, EventId.KEYUP_EVENT, onUpArrowUp);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.W, EventId.KEYPRESS_EVENT, onUpArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.W, EventId.KEYUP_EVENT, onUpArrowUp);

            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.DownArrow, EventId.KEYPRESS_EVENT, onDownArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.DownArrow, EventId.KEYUP_EVENT, onDownArrowUp);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.S, EventId.KEYPRESS_EVENT, onDownArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.S, EventId.KEYUP_EVENT, onDownArrowUp);
            
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.LeftArrow, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.LeftArrow, EventId.KEYUP_EVENT, onLeftArrowUp);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.A, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.A, EventId.KEYUP_EVENT, onLeftArrowUp);

            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.RightArrow, EventId.KEYPRESS_EVENT, onRightArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.RightArrow, EventId.KEYUP_EVENT, onRightArrowUp);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.D, EventId.KEYPRESS_EVENT, onRightArrowPress);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.D, EventId.KEYUP_EVENT, onRightArrowUp);
        }

        override public void init()
        {
            base.init();
        }

        override public void dispose()
        {
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.UpArrow, EventId.KEYPRESS_EVENT, onUpArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.UpArrow, EventId.KEYUP_EVENT, onUpArrowUp);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.W, EventId.KEYPRESS_EVENT, onUpArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.W, EventId.KEYUP_EVENT, onUpArrowUp);

            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.DownArrow, EventId.KEYPRESS_EVENT, onDownArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.DownArrow, EventId.KEYUP_EVENT, onDownArrowUp);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.S, EventId.KEYPRESS_EVENT, onDownArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.S, EventId.KEYUP_EVENT, onDownArrowUp);

            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.LeftArrow, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.LeftArrow, EventId.KEYUP_EVENT, onLeftArrowUp);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.A, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.A, EventId.KEYUP_EVENT, onLeftArrowUp);

            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.RightArrow, EventId.KEYPRESS_EVENT, onRightArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.RightArrow, EventId.KEYUP_EVENT, onRightArrowUp);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.D, EventId.KEYPRESS_EVENT, onRightArrowPress);
            //Ctx.mInstance.mInputMgr.removeKeyListener(InputKey.D, EventId.KEYUP_EVENT, onRightArrowUp);

            base.dispose();
        }

        override public void onTick(float delta)
        {
            // 绘制调试信息
            UtilApi.DrawLine(this.mEntity.getPos(), (this.mEntity as Player).mPlayerSplitMerge.getTargetPoint(), UnityEngine.Color.red);

            base.onTick(delta);
            OnMove();
        }

        public UnityEngine.Quaternion getForwardRotate()
        {
            return mForwardRotate;
        }

        protected void onUpArrowPress(IDispatchObject dispObj)
        {
            isUpPress = true;
        }

        protected void onUpArrowUp(IDispatchObject dispObj)
        {
            isUpPress = false;
        }

        protected void onDownArrowPress(IDispatchObject dispObj)
        {
            isDownPress = true;
        }

        protected void onDownArrowUp(IDispatchObject dispObj)
        {
            isDownPress = false;
        }

        protected void onLeftArrowPress(IDispatchObject dispObj)
        {
            isLeftPress = true;
        }

        protected void onLeftArrowUp(IDispatchObject dispObj)
        {
            isLeftPress = false;
        }

        protected void onRightArrowPress(IDispatchObject dispObj)
        {
            isRightPress = true;
        }

        protected void onRightArrowUp(IDispatchObject dispObj)
        {
            isRightPress = false;
        }

        protected void OnMove()
        {
            if (!UtilPath.isWindowsRuntime()) return;
            if (!isUpPress && !isDownPress && !isLeftPress && !isRightPress)
            {
                return;
            }

            float x = UnityEngine.Input.GetAxis("Horizontal");
            float y = UnityEngine.Input.GetAxis("Vertical");

            float roate = 0;            
            roate = UtilMath.ATan2(x, y);

            this.mRotate = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, roate * UtilMath.fRad2Deg, 0));
            this.updateOrient();
            this.moveForward();
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
            
            quat = mForwardRotate * mRotate;
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
            Ctx.mInstance.mCommonData.setClickSplit(false);

            Ctx.mInstance.mGlobalDelegate.mMainPosChangedDispatch.dispatchEvent(this);

            (this.mEntity as PlayerMain).onChildChanged();
        }

        override public void setForwardRotate(UnityEngine.Vector3 rotate)
        {
            mForwardRotate = UnityEngine.Quaternion.Euler(rotate);
        }
    }
}