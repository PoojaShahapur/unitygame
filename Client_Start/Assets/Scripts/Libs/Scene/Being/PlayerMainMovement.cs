namespace SDK.Lib
{
    public class PlayerMainMovement : PlayerMovement
    {
        protected AddOnceEventDispatch mOrientChangedDisp;     // 方向改变分发器
        protected AddOnceEventDispatch mPosChangedDisp;        // 位置改变分发器
        protected AddOnceEventDispatch mOrientStopChangedDisp; // 方向改变停止分发器
        protected AddOnceEventDispatch mPosStopChangedDisp;    // 位置改变停止分发器

        public PlayerMainMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mOrientChangedDisp = new AddOnceEventDispatch();
            this.mPosChangedDisp = new AddOnceEventDispatch();
            this.mOrientStopChangedDisp = new AddOnceEventDispatch();
            this.mPosStopChangedDisp = new AddOnceEventDispatch();

            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.UpArrow, EventId.KEYPRESS_EVENT, onUpArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.UpArrow, EventId.KEYUP_EVENT, onUpArrowUp);
            //Ctx.mInstance.mInputMgr.addKeyListener(InputKey.L, EventId.KEYUP_EVENT, onStartUp);
            Ctx.mInstance.mInputMgr.addAccelerationListener(EventId.ACCELERATIONMOVED_EVENT, onAccelerationMovedHandle);

            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.LeftArrow, EventId.KEYPRESS_EVENT, onLeftArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.LeftArrow, EventId.KEYUP_EVENT, onLeftArrowUp);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.RightArrow, EventId.KEYPRESS_EVENT, onRightArrowPress);
            Ctx.mInstance.mInputMgr.addKeyListener(InputKey.RightArrow, EventId.KEYUP_EVENT, onRightArrowUp);
        }

        override public void init()
        {
            base.init();

            //this.Start();
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

            //this.Update();
        }

        protected void onUpArrowPress(IDispatchObject dispObj)
        {
            this.moveForward();
        }

        protected void onUpArrowUp(IDispatchObject dispObj)
        {
            this.stopMove();
        }

        protected void onLeftArrowPress(IDispatchObject dispObj)
        {
            this.rotateLeft();
        }

        protected void onLeftArrowUp(IDispatchObject dispObj)
        {

        }

        protected void onRightArrowPress(IDispatchObject dispObj)
        {
            this.rotateRight();
        }

        protected void onRightArrowUp(IDispatchObject dispObj)
        {

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

            this.mOrientStopChangedDisp.dispatchEvent(this);
        }

        override public void stopRotate()
        {
            base.stopRotate();
        }

        override public void addActorLocalOffset(UnityEngine.Vector3 DeltaLocation)
        {
            base.addActorLocalOffset(DeltaLocation);
            //(this.mEntity as Player).mPlayerSplitMerge.reduceTargetLength(-DeltaLocation.z);
            (this.mEntity as Player).mPlayerSplitMerge.calcTargetPoint();
            this.mPosChangedDisp.dispatchEvent(this);
        }

        override public void addActorLocalDestOffset(UnityEngine.Vector3 DeltaLocation)
        {
            base.addActorLocalDestOffset(DeltaLocation);
            //(this.mEntity as Player).mPlayerSplitMerge.reduceTargetLength(-DeltaLocation.z);
            (this.mEntity as Player).mPlayerSplitMerge.calcTargetPoint();
            this.mPosChangedDisp.dispatchEvent(this);
        }

        override public void addLocalRotation(UnityEngine.Vector3 DeltaRotation)
        {
            base.addLocalRotation(DeltaRotation);
            (this.mEntity as Player).mPlayerSplitMerge.calcTargetPoint();
            this.mOrientChangedDisp.dispatchEvent(this);
        }

        override public void setDestRotate(UnityEngine.Vector3 destRotate)
        {
            base.setDestRotate(destRotate);
            (this.mEntity as Player).mPlayerSplitMerge.calcTargetPoint();
            this.mOrientChangedDisp.dispatchEvent(this);
        }

        protected void onAccelerationMovedHandle(IDispatchObject disoObj)
        {
            MAcceleration acceleration = disoObj as MAcceleration;
            (this.mEntity as Player).setDestRotate(acceleration.getOrient().eulerAngles, true);

            this.moveForward();

            Ctx.mInstance.mLogSys.log(string.Format("Acceleration orient is x = {0}, y = {1}, z = {2}", acceleration.getOrient().eulerAngles.x, acceleration.getOrient().eulerAngles.y, acceleration.getOrient().eulerAngles.z), LogTypeId.eLogAcceleration);
        }

        public void addOrientChangedHandle(MAction<IDispatchObject> handle)
        {
            mOrientChangedDisp.addEventHandle(null, handle);
        }

        public void removeOrientChangedHandle(MAction<IDispatchObject> handle)
        {
            mOrientChangedDisp.removeEventHandle(null, handle);
        }

        public void addPosChangedHandle(MAction<IDispatchObject> handle)
        {
            mPosChangedDisp.addEventHandle(null, handle);
        }

        public void removePosChangedHandle(MAction<IDispatchObject> handle)
        {
            mPosChangedDisp.removeEventHandle(null, handle);
        }

        public void addOrientStopChangedHandle(MAction<IDispatchObject> handle)
        {
            mOrientStopChangedDisp.addEventHandle(null, handle);
        }

        public void removeOrientStopChangedHandle(MAction<IDispatchObject> handle)
        {
            mOrientStopChangedDisp.removeEventHandle(null, handle);
        }

        public void addPosStopChangedHandle(MAction<IDispatchObject> handle)
        {
            mPosStopChangedDisp.addEventHandle(null, handle);
        }

        public void removePosStopChangedHandle(MAction<IDispatchObject> handle)
        {
            mPosStopChangedDisp.removeEventHandle(null, handle);
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

        //---------------------- Flock Start-----------------------------
        //public UnityEngine.Vector3 bound;

        //// Use this for initialization
        //protected void Start()
        //{
        //    this.bound = new UnityEngine.Vector3(5, 0, 5);
        //    CalculateNextMovementPoint();
        //}

        //protected void CalculateNextMovementPoint()
        //{
        //    UnityEngine.Vector3 initialPosition = this.mEntity.getPos();
        //    float posX = UnityEngine.Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
        //    float posY = UnityEngine.Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
        //    float posZ = UnityEngine.Random.Range(initialPosition.z - bound.z, initialPosition.z + bound.z);

        //    UnityEngine.Vector3 nextMovementPoint = initialPosition + new UnityEngine.Vector3(posX, posY, posZ);

        //    (this.mEntity as BeingEntity).setDestPos(nextMovementPoint, false);

        //    Ctx.mInstance.mPlayerMgr.setPlayerTargetPos(nextMovementPoint);
        //}

        //public bool mIsCalcNextPoint = false;
        //// Update is called once per frame
        //protected void Update()
        //{
        //    //transform.Translate(UnityEngine.Vector3.forward * speed * Ctx.mInstance.mSystemTimeData.deltaSec);
        //    //transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, UnityEngine.Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Ctx.mInstance.mSystemTimeData.deltaSec);

        //    //if (UnityEngine.Vector3.Distance(this.mDestPos, this.mEntity.getPos()) <= 10.0f)
        //    if (!this.isMoveToDest())
        //    {
        //        if (!mIsCalcNextPoint)
        //        {
        //            mIsCalcNextPoint = true;
        //            CalculateNextMovementPoint();
        //        }
        //    }
        //}
        //---------------------- Flock End --------------------------
    }
}