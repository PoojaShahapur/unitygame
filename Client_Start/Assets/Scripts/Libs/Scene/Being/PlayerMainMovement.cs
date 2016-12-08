namespace SDK.Lib
{
    public class PlayerMainMovement : PlayerMovement
    {
        protected AddOnceEventDispatch mOrientChangedDisp;     // 方向改变分发器
        protected AddOnceEventDispatch mPosChangedDisp;        // 位置改变分发器
        protected AddOnceEventDispatch mOrientStopChangedDisp; // 方向改变停止分发器
        protected AddOnceEventDispatch mPosStopChangedDisp;        // 位置改变停止分发器

        protected bool mIsRotateDown;
        protected bool mIsRotateUp;
        protected bool mIsMoveDown;
        protected bool mIsMoveUp;

        public PlayerMainMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mOrientChangedDisp = new AddOnceEventDispatch();
            this.mPosChangedDisp = new AddOnceEventDispatch();
            this.mOrientStopChangedDisp = new AddOnceEventDispatch();
            this.mPosStopChangedDisp = new AddOnceEventDispatch();

            this.mIsRotateUp = true;
            this.mIsMoveUp = true;
        }

        override public void onTick(float delta)
        {
            // 绘制调试信息
            UtilApi.DrawLine(this.mEntity.getPos(), (this.mEntity as Player).mPlayerSplitMerge.getTargetPoint(), UnityEngine.Color.red);

            base.onTick(delta);

            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            //if (horizontal > 0.0f)
            //{
            //    mIsRotateUp = false;
            //    if (!mIsRotateDown)
            //    {
            //        //mIsRotateDown = true;
            //        this.rotateLeft();
            //    }
            //}
            //else if (horizontal < 0.0f)
            //{
            //    mIsRotateUp = false;
            //    if (!mIsRotateDown)
            //    {
            //        this.rotateRight();
            //        //mIsRotateDown = true;
            //    }
            //}
            //else
            //{
            //    mIsRotateDown = false;
            //    if (mIsRotateUp == false)
            //    {
            //        mIsRotateUp = true;
            //        this.stopRotate();
            //    }
            //}

            float vertical = UnityEngine.Input.GetAxis("Vertical");
            if (vertical > 0.0f)
            {
                mIsMoveUp = false;
                if (!mIsMoveDown)
                {
                    mIsMoveDown = true;
                    this.moveForward();
                }
            }
            else if(vertical < 0.0f)
            {
                mIsMoveUp = false;
                if (!mIsMoveDown)
                {
                    mIsMoveDown = true;
                    //this.moveBack();
                }
            }
            else
            {
                mIsMoveDown = false;
                if (mIsMoveUp == false)
                {
                    mIsMoveUp = true;
                    this.stopMove();
                }
            }
        }

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
            (this.mEntity as Player).mPlayerSplitMerge.reduceTargetLength(-DeltaLocation.z);
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
    }
}