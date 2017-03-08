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
        protected MoveWay mMoveWay;     // 移动方式

        public BeingEntityMovement(SceneEntityBase entity)
            : base(entity)
        {
            this.mIsMoveToDest = false;
            this.mIsRotateToDest = false;
            this.mIsScaleToDest = false;
            this.mMoveWay = MoveWay.eNone;
        }

        public bool isMoveToDest()
        {
            return this.mIsMoveToDest;
        }

        public void setIsMoveToDest(bool isMove)
        {
            if (this.mIsMoveToDest != isMove)
            {
                this.mIsMoveToDest = isMove;

                if(false == this.mIsMoveToDest)
                {
                    this.mMoveWay = MoveWay.eNone;
                }
            }
        }

        public bool isRotateToDest()
        {
            return this.mIsRotateToDest;
        }

        public void setRotateToDest(bool isRotate)
        {
            if (this.mIsRotateToDest != isRotate)
            {
                this.mIsRotateToDest = isRotate;
            }
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
                if (MoveWay.eAutoPathMove == this.mMoveWay ||
                    MoveWay.eBirthMove == this.mMoveWay)
                {
                    // 设置目标点移动
                    this.moveToDest(delta);
                }
                else if(MoveWay.eSeparateMove == this.mMoveWay)
                {
                    // 设置前向方向移动
                    this.moveForwardToDest(delta);
                    //this.moveSeparateForwardToDest(delta);
                }
                else if(MoveWay.eIOControlMove == this.mMoveWay)
                {
                    this.moveForwardToDestIOControl(delta);
                }
                else if(MoveWay.eMergeMove == this.mMoveWay)
                {
                    this.moveToDestNoOrient(delta);
                }
            }
        }

        // 局部空间移动位置
        virtual public void addActorLocalOffset(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = mEntity.getRotate() * DeltaLocation;
            mEntity.setPos(mEntity.getPos() + localOffset);

            this.sendMoveMsg();
        }

        // 向目的前向移动
        virtual public void addActorLocalDestOffset(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = this.mDestRotate * DeltaLocation;
            mEntity.setPos(mEntity.getPos() + localOffset);

            this.sendMoveMsg();
        }

        // 向目的前向移动
        virtual public void addActorLocalDestOffsetNoOrient(UnityEngine.Vector3 DeltaLocation)
        {
            UnityEngine.Vector3 localOffset = DeltaLocation;
            mEntity.setPos(mEntity.getPos() + localOffset);

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
            if (BeingState.eBSWalk != (this.mEntity as BeingEntity).getBeingState())
            {
                Game.Game.ReqSceneInteractive.setMainChildStop(false);
                (this.mEntity as BeingEntity).setBeingState(BeingState.eBSWalk);
                this.setIsMoveToDest(true);
                this.mMoveWay = MoveWay.eIOControlMove;
            }

            //(this.mEntity as BeingEntity).setBeingState(BeingState.eBSIOControlWalk);

            //UnityEngine.Vector3 localMove = new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).getMoveSpeed() * Ctx.mInstance.mSystemTimeData.deltaSec);

            //this.addActorLocalDestOffset(localMove);
        }

        // 向前移动进行分离
        public void moveForwardSeparate()
        {
            if (BeingState.eBSSeparation != (this.mEntity as BeingEntity).getBeingState())
            {
                (this.mEntity as BeingEntity).setBeingState(BeingState.eBSSeparation);

                this.setIsMoveToDest(true);
                this.mMoveWay = MoveWay.eSeparateMove;
            }
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
            float delta = Ctx.mInstance.mSystemTimeData.getFixedTimestep();
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, (mEntity as BeingEntity).getRotateSpeed() * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 向右旋转
        public void rotateRight()
        {
            //(this.mEntity as BeingEntity).setBeingState(BeingState.BSWalk);

            float delta = Ctx.mInstance.mSystemTimeData.getFixedTimestep();
            UnityEngine.Vector3 deltaRotation = new UnityEngine.Vector3(0.0f, -(mEntity as BeingEntity).getRotateSpeed() * delta, 0.0f);
            this.addLocalRotation(deltaRotation);
        }

        // 停止移动
        virtual public void stopMove()
        {
            if (BeingState.eBSIdle != (this.mEntity as BeingEntity).getBeingState())
            {
                Game.Game.ReqSceneInteractive.setMainChildStop(true);
                (this.mEntity as BeingEntity).setBeingState(BeingState.eBSIdle);
                this.setIsMoveToDest(false);
            }
        }

        virtual public void stopRotate()
        {
            this.setRotateToDest(false);
        }

        // 控制向前移动
        public void moveForwardToDest(float delta)
        {
            UnityEngine.Vector3 localMove = UtilApi.convPosByMode(new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).getMoveSpeed() * delta));
            this.addActorLocalOffset(localMove);
        }

        public void moveForwardToDestIOControl(float delta)
        {
            UnityEngine.Vector3 localMove = UtilApi.convPosByMode(new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).getMoveSpeed() * Ctx.mInstance.mSystemTimeData.getFixedTimestep()));

            this.addActorLocalDestOffset(localMove);
        }

        // 分裂向前移动，但是不根据真正的方向
        virtual public void moveSeparateForwardToDest(float delta)
        {

        }

        // 自动寻路移动
        public void moveToDest(float delta)
        {
            if (MacroDef.DRAW_DEBUG)
            {
                UtilApi.DrawLine(mEntity.getPos(), mDestPos, UnityEngine.Color.red);
            }

            //this.checkAndUpdateDestRotate();

            float dist = 0.0f;

            if (MacroDef.XZ_MODE)
            {
                dist = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(mDestPos.x, 0f, mDestPos.z),
                        new UnityEngine.Vector3(mEntity.getPos().x, 0f, mEntity.getPos().z));
            }
            else if (MacroDef.XY_MODE)
            {
                dist = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(mDestPos.x, mDestPos.y, 0),
                        new UnityEngine.Vector3(mEntity.getPos().x, mEntity.getPos().y, 0f));
            }

            float deltaSpeed = (mEntity as BeingEntity).getMoveSpeed() * delta;

            if (dist > deltaSpeed)
            {
                UnityEngine.Vector3 localMove = UtilApi.convPosByMode(new UnityEngine.Vector3(0.0f, 0.0f, (mEntity as BeingEntity).getMoveSpeed() * delta));
                this.addActorLocalDestOffset(localMove);
            }
            else
            {
                mEntity.setPos(this.mDestPos);
                this.onArriveDestPos();
            }
        }

        // 移动到目标点，不用判断方向
        public void moveToDestNoOrient(float delta)
        {
            if (MacroDef.DRAW_DEBUG)
            {
                UtilApi.DrawLine(mEntity.getPos(), mDestPos, UnityEngine.Color.red);
            }

            UnityEngine.Vector3 normal = mDestPos - mEntity.getPos();
            normal.Normalize();

            float dist = 0.0f;
            if (MacroDef.XZ_MODE)
            {
                dist = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(mDestPos.x, 0f, mDestPos.z),
                    new UnityEngine.Vector3(mEntity.getPos().x, 0f, mEntity.getPos().z));
            }
            else if (MacroDef.XY_MODE)
            {
                dist = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(mDestPos.x, mDestPos.y, 0),
                    new UnityEngine.Vector3(mEntity.getPos().x, mEntity.getPos().y, 0f));
            }

            float deltaSpeed = (mEntity as BeingEntity).getMoveSpeed() * delta;

            if (dist > deltaSpeed)
            {
                UnityEngine.Vector3 localMove = normal * deltaSpeed;
                this.addActorLocalDestOffsetNoOrient(localMove);
            }
            else
            {
                mEntity.setPos(this.mDestPos);
                this.onArriveDestPos();
            }
        }

        protected void checkAndUpdateDestRotate()
        {
            UnityEngine.Quaternion quat = UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), this.mDestPos);
            if (!UtilMath.isEqualQuat(this.mDestRotate, quat))
            {
                // 计算最终方向
                (this.mEntity as BeingEntity).setDestRotate(quat.eulerAngles, true);
            }
        }

        // 旋转到目标方向
        public void rotateToDest(float delta)
        {
            // 方向插值
            if (!UtilMath.isEqualVec3(mEntity.getRotateEulerAngle(), this.mDestRotate.eulerAngles))
            {
                mEntity.setRotation(UnityEngine.Quaternion.Slerp(mEntity.getRotate(), this.mDestRotate, (mEntity as BeingEntity).getRotateSpeed() * delta));
            }
            else
            {
                this.onArriveDesRatate();
            }
        }

        // 旋转到目标方向
        public void scaleToDest(float delta)
        {
            if (!UtilMath.isEqualVec3(this.mDestScale, this.mEntity.getScale()))
            {
                float dist = 0.0f;
                dist = UnityEngine.Vector3.Distance(this.mDestScale, this.mEntity.getScale());

                float deltaSpeed = (mEntity as BeingEntity).getScaleSpeed() * delta;

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
        virtual public void onArriveDestPos()
        {
            this.setIsMoveToDest(false);
            (this.mEntity as BeingEntity).setBeingState(BeingState.eBSIdle);
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

        public void forceMoveDest(UnityEngine.Vector3 destPos)
        {
            destPos = Ctx.mInstance.mSceneSys.adjustPosInRange(this.mEntity, destPos);

            this.mDestPos = destPos;

            this.setIsMoveToDest(true);
            this.mMoveWay = MoveWay.eAutoPathMove;
        }

        // 移动到最终地点
        //public void moveToPos(UnityEngine.Vector3 destPos)
        public void setDestPos(UnityEngine.Vector3 destPos)
        {
            if (!UtilMath.isEqualVec3(this.mDestPos, destPos))
            {
                destPos = Ctx.mInstance.mSceneSys.adjustPosInRange(this.mEntity, destPos);

                this.mDestPos = destPos;

                if (!UtilMath.isEqualVec3(mDestPos, mEntity.getPos()))
                {
                    this.setIsMoveToDest(true);
                    this.mMoveWay = MoveWay.eAutoPathMove;

                    (this.mEntity as BeingEntity).setBeingState(BeingState.eBSWalk);

                    // 计算最终方向
                    this.setDestRotate(UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), this.mDestPos).eulerAngles);
                }
                else
                {
                    this.setIsMoveToDest(false);
                }
            }
        }

        // 向前移动出生
        public void setDestPosForBirth(UnityEngine.Vector3 destPos)
        {
            if (!UtilMath.isEqualVec3(this.mDestPos, destPos))
            {
                destPos = Ctx.mInstance.mSceneSys.adjustPosInRange(this.mEntity, destPos);

                this.mDestPos = destPos;

                if (!UtilMath.isEqualVec3(mDestPos, mEntity.getPos()))
                {
                    this.setIsMoveToDest(true);
                    this.mMoveWay = MoveWay.eBirthMove;

                    (this.mEntity as BeingEntity).setBeingState(BeingState.eBSBirth);

                    // 计算最终方向
                    this.setDestRotate(UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), this.mDestPos).eulerAngles);
                }
                else
                {
                    this.setIsMoveToDest(false);
                }
            }
        }

        // 向前移动出生
        public void setDestPosForMerge(UnityEngine.Vector3 destPos)
        {
            if (!UtilMath.isEqualVec3(this.mDestPos, destPos))
            {
                destPos = Ctx.mInstance.mSceneSys.adjustPosInRange(this.mEntity, destPos);

                this.mDestPos = destPos;

                if (!UtilMath.isEqualVec3(mDestPos, mEntity.getPos()))
                {
                    this.setIsMoveToDest(true);
                    this.mMoveWay = MoveWay.eMergeMove;

                    (this.mEntity as BeingEntity).setBeingState(BeingState.eBSWalk);
                }
                else
                {
                    this.setIsMoveToDest(false);

                    this.onArriveDestPos();
                }
            }
        }

        // 向前移动出生
        public void setDestPosForMoveCenter(UnityEngine.Vector3 destPos)
        {
            if (!UtilMath.isEqualVec3(this.mDestPos, destPos))
            {
                destPos = Ctx.mInstance.mSceneSys.adjustPosInRange(this.mEntity, destPos);

                this.mDestPos = destPos;

                if (!UtilMath.isEqualVec3(mDestPos, mEntity.getPos()))
                {
                    this.setIsMoveToDest(true);
                    this.mMoveWay = MoveWay.eAutoPathMove;

                    (this.mEntity as BeingEntity).setBeingState(BeingState.eBSMoveCenter);
                }
                else
                {
                    this.setIsMoveToDest(false);

                    this.onArriveDestPos();
                }
            }
        }

        // 直接到具体位置，不用移动
        public void gotoPos(UnityEngine.Vector3 destPos)
        {
            if (!UtilMath.isEqualVec3(this.mDestPos, destPos))
            {
                destPos = Ctx.mInstance.mSceneSys.adjustPosInRange(this.mEntity, destPos);

                this.mDestPos = destPos;
                this.setIsMoveToDest(false);

                if (!UtilMath.isEqualVec3(mDestPos, mEntity.getPos()))
                {
                    // 计算最终方向
                    this.setDestRotate(UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), this.mDestPos).eulerAngles);
                    this.mEntity.setRotation(this.mDestRotate);
                    this.mEntity.setPos(this.mDestPos);

                    this.sendMoveMsg();
                }
            }
        }

        virtual public void setDestRotate(UnityEngine.Vector3 destRotate)
        {
            // 只能绕 Y 轴旋转
            if (MacroDef.XZ_MODE)
            {
                destRotate.x = 0;
                destRotate.z = 0;
            }
            else if (MacroDef.XY_MODE)
            {
                // 只能绕 Z 轴旋转
                destRotate.x = 0;
                destRotate.y = 0;
            }

            this.mDestRotate = UnityEngine.Quaternion.Euler(destRotate);

            if (!UtilMath.isEqualVec3(mEntity.getRotateEulerAngle(), destRotate))
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
                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log("Invalid num", LogTypeId.eLogCommon);
                }
            }
            this.mDestScale = new UnityEngine.Vector3(scale, scale, scale);

            if (!UtilMath.isEqualVec3(this.mDestScale, this.mEntity.getScale()))
            {
                this.mIsScaleToDest = true;
            }
            else
            {
                this.mIsScaleToDest = false;
            }
        }

        virtual public void setDestPosAndDestRotate(UnityEngine.Vector3 targetPt, bool immePos, bool immeRotate)
        {
            UnityEngine.Quaternion retQuat = UtilMath.getRotateByStartAndEndPoint(this.mEntity.getPos(), targetPt);
            (this.mEntity as BeingEntity).setDestRotate(retQuat.eulerAngles, immeRotate);
            (this.mEntity as BeingEntity).setDestRotate(retQuat.eulerAngles, immeRotate);
        }

        virtual public void movePause()
        {
            (this.mEntity as BeingEntity).setBeingState(BeingState.eBSIdle);
        }

        virtual public void setForwardRotate(UnityEngine.Vector3 rotate)
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

        virtual public void setSeparateForwardRotate(UnityEngine.Vector3 rotate)
        {

        }

        public UnityEngine.Quaternion getDestRotate()
        {
            return this.mDestRotate;
        }

        public UnityEngine.Vector3 getDestPos()
        {
            return this.mDestPos;
        }

        virtual public void setNotMergeRotate(UnityEngine.Quaternion quat)
        {

        }

        virtual public void clearNotMerge()
        {

        }
    }
}