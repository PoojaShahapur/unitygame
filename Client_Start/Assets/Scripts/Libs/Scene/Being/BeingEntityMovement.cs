namespace SDK.Lib
{
    public class BeingEntityMovement : SceneEntityMovement
    {
        protected UnityEngine.Vector3 mLastPos;         // 之前位置信息
        protected UnityEngine.Quaternion mLastRotate;   // 之前方向信息

        protected UnityEngine.Vector3 mDestPos;         // 目的位置信息
        protected UnityEngine.Quaternion mDestRotate;   // 目的方向信息，欧拉角
        protected UnityEngine.Vector3 mDestScale;         // 目标缩放大小

        protected bool mIsMoveToDest;   // 是否需要移动到目标点
        protected bool mIsRatateToDest; // 是否需要旋转到目标方向
        protected bool mIsScaleToDest;  // 是否需要缩放到目标大小

        public BeingEntityMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mIsMoveToDest = false;
            this.mIsMoveToDest = false;
            this.mIsScaleToDest = false;
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            if(this.mIsMoveToDest)
            {
                this.moveToDest(delta);
            }
            if(this.mIsMoveToDest)
            {
                this.rotateToDest(delta);
            }
            if(this.mIsScaleToDest)
            {
                this.scaleToDest(delta);
            }
        }

        // 局部空间移动位置
        public void addActorLocalOffset(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = mEntity.getRotate() * DeltaLocation;
            mEntity.setOriginal(mEntity.getPos() + localOffset);
        }

        // 局部空间旋转
        public void addLocalRotation(UnityEngine.Vector3 DeltaRotation)
        {
            mEntity.setRotation(mEntity.getRotate() * UtilApi.convQuatFromEuler(DeltaRotation));
        }

        // 向前移动
        public void moveForward()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).mMoveSpeed * delta);
            this.addActorLocalOffset(localMove);
        }

        // 向后移动
        public void moveBack()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, -(mEntity as BeingEntity).mMoveSpeed * delta);
            this.addActorLocalOffset(localMove);
        }

        // 向左旋转
        public void rotateLeft()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, (mEntity as BeingEntity).mRotateSpeed * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 向右旋转
        public void rotateRight()
        {
            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, -(mEntity as BeingEntity).mRotateSpeed * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 根据目标点信息移动
        public void moveToDest(float delta)
        {
            float currY = 1.0f;
            bool isOnGround = true;
            bool isArrived = false;

            float dist = 0.0f;

            if (isOnGround)
            {
                dist = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(mDestPos.x, 0f, mDestPos.z),
                    new UnityEngine.Vector3(mEntity.getPos().x, 0f, mEntity.getPos().z));
            }
            else
            {
                dist = UnityEngine.Vector3.Distance(mDestPos, mEntity.getPos());
            }

            float deltaSpeed = (mEntity as BeingEntity).mMoveSpeed * delta;

            if (dist > UtilMath.EPSILON)
            {
                UnityEngine.Vector3 pos = mEntity.getPos();

                UnityEngine.Vector3 movement = mDestPos - pos;
                movement.y = 0f;
                movement.Normalize();

                movement *= deltaSpeed;

                // 如果需要移动
                if (dist > deltaSpeed || movement.magnitude > deltaSpeed)
                {
                    pos += movement;
                }
                else
                {
                    pos = mDestPos;
                    isArrived = true;
                }

                if (isOnGround)
                {
                    pos.y = currY;
                }

                mEntity.setOriginal(pos);

                if(isArrived)
                {
                    // 移动到终点
                    this.onArriveDestPos();
                }
            }
        }

        // 旋转到目标方向
        public void rotateToDest(float delta)
        {
            // 方向插值
            if (UnityEngine.Vector3.Distance(mEntity.getRotateEulerAngle(), this.mDestRotate.eulerAngles) > UtilMath.EPSILON)
            {
                mEntity.setRotation(UnityEngine.Quaternion.Slerp(mEntity.getRotate(), this.mDestRotate, (mEntity as BeingEntity).mRotateSpeed * delta));
            }
            else
            {
                this.onArriveDesRatate();
            }
        }

        // 旋转到目标方向
        public void scaleToDest(float delta)
        {
            float dist = 0.0f;
            dist = UnityEngine.Vector3.Distance(mDestPos, mEntity.getPos());

            float deltaSpeed = (mEntity as BeingEntity).mScaleSpeed * delta;

            if (dist > UtilMath.EPSILON)
            {
                UnityEngine.Vector3 scale = this.mDestScale - mEntity.getScale();
                scale.Normalize();

                scale *= deltaSpeed;

                // 如果需要移动
                if (dist > deltaSpeed || scale.magnitude > deltaSpeed)
                {
                    mEntity.setScale(mEntity.getScale() + scale);
                }
                else
                {
                    mEntity.setScale(this.mDestScale);
                    // 移动到终点
                    this.onArriveDestScale();
                }
            }
        }

        // 到达终点
        public void onArriveDestPos()
        {
            this.mIsMoveToDest = false;
        }

        // 旋转到目标方向
        public void onArriveDesRatate()
        {
            this.mIsMoveToDest = false;
        }

        // 缩放到目标带你
        public void onArriveDestScale()
        {
            this.mIsScaleToDest = false;
        }

        // 移动到最终地点
        public void moveToPos(UnityEngine.Vector3 destPos)
        {
            this.mDestPos = destPos;
            float dist = UnityEngine.Vector3.Distance(mDestPos, mEntity.getPos());

            if (dist > UtilMath.EPSILON)
            {
                this.mIsMoveToDest = true;

                // 计算最终方向
                this.mDestRotate = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.forward, this.mDestPos);
            }
            else
            {
                this.mIsMoveToDest = false;
            }
        }

        // 直接到具体位置，不用移动
        public void gotoPos(UnityEngine.Vector3 destPos)
        {
            this.mDestPos = destPos;
            float dist = UnityEngine.Vector3.Distance(mDestPos, mEntity.getPos());
            this.mIsMoveToDest = false;

            if (dist > UtilMath.EPSILON)
            {
                // 计算最终方向
                this.mDestRotate = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.forward, this.mDestPos);
                this.mEntity.setRotation(this.mDestRotate);
                this.mEntity.setOriginal(this.mDestPos);
            }
        }

        public void setDestRotate(UnityEngine.Vector3 destRotate)
        {
            this.mDestRotate = UnityEngine.Quaternion.Euler(destRotate);

            this.mIsMoveToDest = false;
        }

        public void setDestScale(float scale)
        {
            this.mDestScale = new UnityEngine.Vector3(scale, scale, scale);

            float dist = UnityEngine.Vector3.Distance(this.mDestScale, this.mEntity.getScale());

            if (dist > UtilMath.EPSILON)
            {
                this.mIsScaleToDest = true;
            }
            else
            {
                this.mIsScaleToDest = false;
            }
        }
    }
}