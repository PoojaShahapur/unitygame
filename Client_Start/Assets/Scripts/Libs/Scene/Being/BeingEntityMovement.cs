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
        protected bool mIsRotateToDest; // 是否需要旋转到目标方向
        protected bool mIsScaleToDest;  // 是否需要缩放到目标大小

        protected float mAcceleration;  // 线性加速度
        protected bool mIsAutoPath;     // 是否是自动寻路，或者是通过控制向前移动

        public BeingEntityMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mIsMoveToDest = false;
            this.mIsRotateToDest = false;
            this.mIsScaleToDest = false;
            this.mIsAutoPath = false;
        }

        public bool isMoveToDest()
        {
            return this.mIsMoveToDest;
        }

        public void setIsMoveToDest(bool isMove)
        {
            this.mIsMoveToDest = isMove;
        }

        public bool isRotateToDest()
        {
            return this.mIsRotateToDest;
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            if (this.mIsRotateToDest)
            {
                this.rotateToDest(delta);
            }

            if (this.mIsScaleToDest)
            {
                this.scaleToDest(delta);
            }

            if (this.mIsMoveToDest)
            {
                if (this.mIsAutoPath)
                {
                    this.moveToDest(delta);
                }
                else
                {
                    // 其它控制向前移动
                    this.moveForwardToDest(delta);
                }
            }
        }

        // 局部空间移动位置
        virtual public void addActorLocalOffset(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = mEntity.getRotate() * DeltaLocation;
            mEntity.setOriginal(mEntity.getPos() + localOffset);

            this.sendMoveMsg();
        }

        // 向目的前向移动
        virtual public void addActorLocalDestOffset(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = this.mDestRotate * DeltaLocation;
            mEntity.setOriginal(mEntity.getPos() + localOffset);

            this.sendMoveMsg();
        }

        // 局部空间旋转
        virtual public void addLocalRotation(UnityEngine.Vector3 DeltaRotation)
        {
            mEntity.setRotation(mEntity.getRotate() * UtilApi.convQuatFromEuler(DeltaRotation));

            this.sendMoveMsg();
        }

        // 向前移动
        virtual public void moveForward()
        {
            (this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

            this.setIsMoveToDest(true);
            this.mIsAutoPath = false;
        }

        // 向前移动进行分离
        public void moveForwardSeparate()
        {
            (this.mEntity as BeingEntity).setBeingState(BeingState.BSSeparation);

            this.setIsMoveToDest(true);
            this.mIsAutoPath = false;
        }

        // 向后移动
        //virtual public void moveBack()
        //{
        //    (this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

        //    UnityEngine.Quaternion destRotate = this.mEntity.getRotate() * UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 180, 0));
        //    (this.mEntity as BeingEntity).setDestRotate(destRotate.eulerAngles);

        //    this.setIsMoveToDest(true);
        //    this.mIsAutoPath = false;
        //}

        // 向左旋转
        public void rotateLeft()
        {
            //(this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, (mEntity as BeingEntity).mRotateSpeed * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 向右旋转
        public void rotateRight()
        {
            //(this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

            float delta = Ctx.mInstance.mSystemTimeData.deltaSec;
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, -(mEntity as BeingEntity).mRotateSpeed * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 停止移动
        virtual public void stopMove()
        {
            if (BeingState.BSIdle != (this.mEntity as BeingEntity).getBeingState())
            {
                (this.mEntity as BeingEntity).setBeingState(BeingState.BSIdle);
                this.setIsMoveToDest(false);
            }
        }

        virtual public void stopRotate()
        {

        }

        // 控制向前移动
        public void moveForwardToDest(float delta)
        {
            //(this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

            UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).mMoveSpeed * delta);
            this.addActorLocalOffset(localMove);
        }

        // 自动寻路移动
        public void moveToDest(float delta)
        {
            UtilApi.DrawLine(mEntity.getPos(), mDestPos, UnityEngine.Color.red);
            // 新的移动
            float dist = 0.0f;
            dist = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(mDestPos.x, 0f, mDestPos.z),
                    new UnityEngine.Vector3(mEntity.getPos().x, 0f, mEntity.getPos().z));

            float deltaSpeed = (mEntity as BeingEntity).mMoveSpeed * delta;

            if (dist > deltaSpeed)
            {
                (this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

                UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).mMoveSpeed * delta);
                this.addActorLocalDestOffset(localMove);
            }
            else
            {
                mEntity.setOriginal(this.mDestPos);
                this.onArriveDestPos();
            }
        }

        // 旋转到目标方向
        public void rotateToDest(float delta)
        {
            // 方向插值
            if (!UtilMath.isVectorEqual(mEntity.getRotateEulerAngle(), this.mDestRotate.eulerAngles))
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
            if (!UtilMath.isVectorEqual(this.mDestScale, this.mEntity.getScale()))
            {
                float dist = 0.0f;
                dist = UnityEngine.Vector3.Distance(this.mDestScale, this.mEntity.getScale());

                float deltaSpeed = (mEntity as BeingEntity).mScaleSpeed * delta;

                UnityEngine.Vector3 scale = this.mDestScale - mEntity.getScale();
                scale.Normalize();

                scale *= deltaSpeed;

                // 如果需要缩放
                if (dist > scale.magnitude)
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
            this.mIsAutoPath = false;
            (this.mEntity as BeingEntity).setBeingState(BeingState.BSIdle);
        }

        // 旋转到目标方向
        public void onArriveDesRatate()
        {
            this.mIsRotateToDest = false;
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

            if (!UtilMath.isVectorEqual(mDestPos, mEntity.getPos()))
            {
                this.mIsMoveToDest = true;
                this.mIsAutoPath = true;
                (this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

                // 计算最终方向
                this.setDestRotate(UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), this.mDestPos).eulerAngles);
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
            this.mIsMoveToDest = false;

            if (!UtilMath.isVectorEqual(mDestPos, mEntity.getPos()))
            {
                // 计算最终方向
                this.setDestRotate(UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), this.mDestPos).eulerAngles);
                this.mEntity.setRotation(this.mDestRotate);
                this.mEntity.setOriginal(this.mDestPos);

                this.sendMoveMsg();
            }
        }

        virtual public void setDestRotate(UnityEngine.Vector3 destRotate)
        {
            this.mDestRotate = UnityEngine.Quaternion.Euler(destRotate);

            if (!UtilMath.isVectorEqual(mEntity.getRotateEulerAngle(), destRotate))
            {
                this.mIsRotateToDest = true;
            }
            else
            {
                this.mIsRotateToDest = false;
            }
        }

        public void setDestScale(float scale)
        {
            if(UtilMath.isInvalidNum(scale))
            {
                Ctx.mInstance.mLogSys.log("Invalid num", LogTypeId.eLogCommon);
            }
            this.mDestScale = new UnityEngine.Vector3(scale, scale, scale);

            if (!UtilMath.isVectorEqual(this.mDestScale, this.mEntity.getScale()))
            {
                this.mIsScaleToDest = true;
            }
            else
            {
                this.mIsScaleToDest = false;
            }
        }

        virtual public void lookAt(UnityEngine.Vector3 targetPt)
        {
            UnityEngine.Quaternion retQuat = UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), targetPt);
            (this.mEntity as BeingEntity).setDestRotate(retQuat.eulerAngles, true);
        }

        virtual public void movePause()
        {

        }

        virtual public void sendMoveMsg()
        {
            // 移动后，更新 KBE 中的 Avatar 数据
            //KBEngine.Event.fireIn(
            //    "updatePlayer", 
            //    mEntity.getPos().x, 
            //    mEntity.getPos().y, 
            //    mEntity.getPos().z, 
            //    mEntity.getRotateEulerAngle().y
            //    );
        }
    }
}