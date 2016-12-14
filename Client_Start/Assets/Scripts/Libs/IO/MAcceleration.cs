namespace SDK.Lib
{
    public enum MAccelerationMode
    {
        eNone = 0,
        eBegan = 1,
        eMoved = 2,
        eEnded = 3,
    }

    public class MAcceleration : IDispatchObject
    {
        public static MAcceleration mAccelerationOne = new MAcceleration();

        protected UnityEngine.Vector3 mPos;
        protected UnityEngine.Vector3 mLastPos;

        protected float mTimestamp;

        protected MAccelerationMode mAccelerationMode;
        protected float mSensitivity;   // 灵敏度

        // 是 began 不是 begin
        private AddOnceEventDispatch mOnAccelerationBeganDispatch; // Begin
        private AddOnceEventDispatch mOnAccelerationMovedDispatch; // Move
        private AddOnceEventDispatch mOnAccelerationEndedDispatch; // End

        public MAcceleration()
        {
            this.mPos = UnityEngine.Vector3.zero;
            this.mLastPos = UnityEngine.Vector3.zero;

            this.mTimestamp = 0;
            this.mAccelerationMode = MAccelerationMode.eNone;
            this.mSensitivity = 1;

            this.mOnAccelerationBeganDispatch = new AddOnceEventDispatch();
            this.mOnAccelerationMovedDispatch = new AddOnceEventDispatch();
            this.mOnAccelerationEndedDispatch = new AddOnceEventDispatch();
        }

        public UnityEngine.Quaternion getOrient()
        {
            UnityEngine.Vector3 dir = UnityEngine.Vector3.zero;

            // unity的X轴的正方向是向左的
            dir.x = -mPos.x;
            dir.y = 0;
            dir.z = mPos.z;

            //if (dir.sqrMagnitude > 1)
            //{
            //    dir.Normalize();
            //}

            UnityEngine.Quaternion orient = UnityEngine.Quaternion.LookRotation(dir);

            return orient;
        }

        public void onTick(float delta)
        {
            this.mLastPos = this.mPos;
            // TODO:Test
            //this.mPos = UnityEngine.Input.mousePosition;
            this.mPos = UnityEngine.Input.acceleration * this.mSensitivity;

            Ctx.mInstance.mLogSys.log(string.Format("LastPos is x = {0}, y = {1}, z = {2}, Pos is x = {3}, y = {4}, z = {5}", this.mLastPos.x, this.mLastPos.y, this.mLastPos.z, this.mPos.x, this.mPos.y, this.mPos.z), LogTypeId.eLogCommon);

            // 没有重力感应
            if (UnityEngine.Vector3.zero != this.mPos)
            {
                if (UnityEngine.Vector3.zero == this.mLastPos)
                {
                    this.mAccelerationMode = MAccelerationMode.eBegan;
                    Ctx.mInstance.mLogSys.log("Enter Began", LogTypeId.eLogCommon);
                }
                else
                {
                    this.mAccelerationMode = MAccelerationMode.eMoved;
                    Ctx.mInstance.mLogSys.log("Enter Move", LogTypeId.eLogCommon);
                }
            }
            else
            {
                if (UnityEngine.Vector3.zero != this.mLastPos)
                {
                    this.mAccelerationMode = MAccelerationMode.eEnded;
                    Ctx.mInstance.mLogSys.log("Enter End", LogTypeId.eLogCommon);
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("Enter None", LogTypeId.eLogCommon);
                }
            }

            if (MAccelerationMode.eBegan == this.mAccelerationMode)
            {
                this.handleAccelerationBegan();
            }
            else if (MAccelerationMode.eMoved == this.mAccelerationMode)
            {
                this.handleAccelerationMoved();
            }
            else if (MAccelerationMode.eEnded == this.mAccelerationMode)
            {
                this.handleAccelerationEnded();
            }
        }

        public void addAccelerationListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.ACCELERATIONBEGAN_EVENT == evtID)
            {
                this.mOnAccelerationBeganDispatch.addEventHandle(null, handle);
            }
            else if (EventId.ACCELERATIONMOVED_EVENT == evtID)
            {
                this.mOnAccelerationMovedDispatch.addEventHandle(null, handle);
            }
            else if (EventId.ACCELERATIONENDED_EVENT == evtID)
            {
                this.mOnAccelerationEndedDispatch.addEventHandle(null, handle);
            }
        }

        public void removeAccelerationListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.ACCELERATIONBEGAN_EVENT == evtID)
            {
                this.mOnAccelerationBeganDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.ACCELERATIONMOVED_EVENT == evtID)
            {
                this.mOnAccelerationMovedDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.ACCELERATIONENDED_EVENT == evtID)
            {
                this.mOnAccelerationEndedDispatch.removeEventHandle(null, handle);
            }
        }

        // 是否还有需要处理的事件
        public bool hasEventHandle()
        {
            if (this.mOnAccelerationBeganDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAccelerationMovedDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnAccelerationEndedDispatch.hasEventHandle())
            {
                return true;
            }

            return false;
        }

        public void handleAccelerationBegan()
        {
            if (null != this.mOnAccelerationBeganDispatch)
            {
                this.mOnAccelerationBeganDispatch.dispatchEvent(this);
            }
        }

        public void handleAccelerationMoved()
        {
            if (null != this.mOnAccelerationMovedDispatch)
            {
                this.mOnAccelerationMovedDispatch.dispatchEvent(this);
            }
        }

        public void handleAccelerationEnded()
        {
            if (null != this.mOnAccelerationEndedDispatch)
            {
                this.mOnAccelerationEndedDispatch.dispatchEvent(this);
            }

            this.mAccelerationMode = MAccelerationMode.eNone;   // 重置 None 标志
        }
    }
}