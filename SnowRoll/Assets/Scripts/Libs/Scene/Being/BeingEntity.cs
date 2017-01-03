namespace SDK.Lib
{
	/**
	 * @brief 生物 Entity，有感知，可以交互的
	 */
    public class BeingEntity : SceneEntityBase
    {
        protected SkinModelSkelAnim mSkinAniModel;      // 模型数据
        protected BeingState mBeingState;       // 当前的状态
        protected BeingSubState mBeingSubState; // 当前子状态

        protected float mMoveSpeed;     // 移动速度
        protected float mRotateSpeed;   // 旋转速度
        protected float mScaleSpeed;    // 缩放速度

        protected float mMoveSpeedFactor;   // 移动速度因子

        protected float mBallRadius;    // 吃的大小，使用这个字段判断是否可以吃，以及吃后的大小
        protected BeingEntityAttack mAttack;
        protected int reliveseconds; // 复活时间
        protected HudItemBase mHud; // HUD

        protected string mName;     // 名字
        public BeingAnimatorControl mAnimatorControl;
        public AnimFSM mAnimFSM;

        public BeingEntity()
        {
            //mSkinAniModel = new SkinModelSkelAnim();
            //mSkinAniModel.handleCB = onSkeletonLoaded;
            this.mBeingState = BeingState.eBSIdle;
            this.mBeingSubState = BeingSubState.eBSSNone;

            this.mMoveSpeed = 1;
            this.mRotateSpeed = 10;
            this.mScaleSpeed = 10;

            this.mBallRadius = 1;

            this.mMoveSpeed = Ctx.mInstance.mSnowBallCfg.mMoveSpeed_k / mScale.x + Ctx.mInstance.mSnowBallCfg.mMoveSpeed_b;

            this.mName = "";
            this.mMoveSpeedFactor = 1;
        }

        public SkinModelSkelAnim skinAniModel
        {
            get
            {
                return mSkinAniModel;
            }
        }

        public int ReliveSeconds
        {
            set { reliveseconds = value; }
            get { return reliveseconds; }
        }

        // 骨骼设置，骨骼不能更换
        public void setSkeleton(string name)
        {
            //if(string.IsNullOrEmpty(mSkinAniModel.m_skeletonName))
            //{
            //    mSkinAniModel.m_skeletonName = name;
            //    mSkinAniModel.loadSkeleton();
            //}
        }

        public void setPartModel(int modelDef, string assetBundleName, string partName)
        {
            //mSkinAniModel.m_modelList[modelDef].m_bundleName = string.Format("{0}{1}", assetBundleName, ".prefab");
            //mSkinAniModel.m_modelList[modelDef].m_partName = partName;
            //mSkinAniModel.loadPartModel(modelDef);
        }

        public virtual void onSkeletonLoaded()
        {
            
        }

        // 目前只有怪有 Steerings ,加载这里是为了测试，全部都有 Steerings
        virtual protected void initSteerings()
        {

        }

        virtual public string getDesc()
        {
            return "";
        }

        public EffectControl effectControl
        {
            get
            {
                return getEffectControl();
            }
        }

        virtual public EffectControl getEffectControl()
        {
            return null;
        }

        public void playFlyNum(int num)
        {

        }

        public override void onDestroy()
        {
            base.onDestroy();

            if (null != this.mHud)
            {
                this.mHud.dispose();
            }
        }

        override public void setPos(UnityEngine.Vector3 pos)
        {
            if (!UtilMath.isEqualVec3(this.mPos, pos))
            {
                pos = Ctx.mInstance.mSceneSys.adjustPosInRange(pos);

                this.mPos = pos;
                this.mPos.y = 1.3f;     // TODO: 先固定

                if (null != mRender)
                {
                    mRender.setPos(pos);
                }
            }
        }

        public void setMoveSpeed(float value)
        {
            this.mMoveSpeed = value;
        }

        public float getMoveSpeed()
        {
            return this.mMoveSpeed * this.mMoveSpeedFactor;
        }

        public void setRotateSpeed(float value)
        {
            this.mRotateSpeed = value;
        }

        public float getRotateSpeed()
        {
            return this.mRotateSpeed;
        }

        public void setScaleSpeed(float value)
        {
            this.mScaleSpeed = value;
        }

        public float getScaleSpeed()
        {
            return this.mScaleSpeed;
        }

        virtual public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {
            if(immePos)
            {
                this.setPos(pos);
            }
            if(null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestPos(pos);
            }
        }

        public void setDestPosForBirth(UnityEngine.Vector3 pos, bool immePos)
        {
            if (immePos)
            {
                this.setPos(pos);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestPosForBirth(pos);
            }
        }

        public void setDestRotate(UnityEngine.Vector3 rotate, bool immeRotate)
        {
            if(immeRotate)
            {
                this.setRotateEulerAngle(rotate);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestRotate(rotate);
            }
        }

        public void setDestPosAndDestRotate(UnityEngine.Vector3 targetPt, bool immePos, bool immeRotate)
        {
            if (immePos)
            {
                this.setPos(targetPt);
            }

            UnityEngine.Quaternion retQuat = UtilMath.getRotateByStartAndEndPoint(this.getPos(), targetPt);
            if (immeRotate)
            {
                this.setRotateEulerAngle(retQuat.eulerAngles);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestPos(targetPt);
            }
        }

        public void setDestScale(float scale, bool immeScale)
        {
            if(immeScale)
            {
                this.setScale(new UnityEngine.Vector3(scale, scale, scale));
            }

            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestScale(scale);
            }
        }

        // 设置前向旋转
        public void setForwardRotate(UnityEngine.Vector3 rotate)
        {
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setForwardRotate(rotate);
            }
        }

        virtual public void setBallRadius(float size, bool immScale = false)
        {
            if (0 == size) return;

            if (this.mBallRadius != size)
            {
                this.mBallRadius = size;

                if (UtilMath.isInvalidNum(this.mBallRadius))
                {
                    this.mBallRadius = 1;

                    Ctx.mInstance.mLogSys.log("BeingEntity::setBallRadius is InValid num", LogTypeId.eLogSplitMergeEmit);
                }

                this.setDestScale(size, immScale);
            }
        }

        public float getBallRadius()
        {
            return this.mBallRadius;
        }

        virtual public void setName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.mName = name;

                if(null != this.mHud)
                {
                    this.mHud.onNameChanged();
                }
            }
        }

        public string getName()
        {
            return this.mName;
        }

        // 减少质量
        public void reduceMassBy(float mass)
        {
            float selfMass = UtilMath.getMassByRadius(this.mBallRadius);
            selfMass -= mass;
            this.setBallRadius(UtilMath.getRadiusByMass(selfMass));
        }

        // 获取分裂半径
        public float getSplitRadius()
        {
            float selfMass = UtilMath.getMassByRadius(this.mBallRadius);
            selfMass /= 2;
            float splitRadius = UtilMath.getRadiusByMass(selfMass);
            return splitRadius;
        }

        override public void preInit()
        {
            this.setBallRadius(Ctx.mInstance.mSnowBallCfg.mInitSnowRadius);  // 初始小球的半径是配置的

            // 基类初始化
            base.preInit();
            // 自动处理，例如添加到管理器
            this.autoHandle();
            // 初始化渲染器
            this.initRender();
            // 加载渲染器资源
            this.loadRenderRes();
            // 更新位置
            this.updateTransform();
        }

        public override void postInit()
        {
            base.postInit();
        }

        override public void loadRenderRes()
        {
            mRender.load();
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
        }

        // Tick 第一阶段执行
        override public void onPreTick(float delta)
        {
            base.onPreTick(delta);            
            mMoveSpeed = Ctx.mInstance.mSnowBallCfg.mMoveSpeed_k / mScale.x + Ctx.mInstance.mSnowBallCfg.mMoveSpeed_b;
        }

        // Tick 第二阶段执行
        override public void onPostTick(float delta)
        {
            if (null != this.mMovement)
            {
                this.mMovement.onTick(delta);
            }

            if(null != this.mAttack)
            {
                this.mAttack.onTick(delta);
            }
        }

        // 获取 Hud 场景中的位置
        virtual public UnityEngine.Vector3 getHudPos()
        {
            return this.mPos;
        }

        virtual public void setBeingState(BeingState state)
        {
            if (this.mBeingState != state)
            {
                this.mBeingState = state;

                if(null != this.mAnimFSM)
                {
                    this.mAnimFSM.MoveToState(AnimStateId.getStateIdByBeingState(this.mBeingState));
                }
            }
        }

        public BeingState getBeingState()
        {
            return this.mBeingState;
        }

        public void setBeingSubState(BeingSubState subState)
        {
            if (this.mBeingSubState != subState)
            {
                this.mBeingSubState = subState;
            }
        }

        public BeingSubState getBeingSubState()
        {
            return this.mBeingSubState;
        }

        public void clearBeingSubState()
        {
            this.mBeingSubState = BeingSubState.eBSSNone;
        }

        virtual public bool canEatOther(BeingEntity other)
        {
            bool ret = false;

            if(this.mBallRadius > other.getBallRadius())
            {
                if (this.mBallRadius >= other.getBallRadius() * Ctx.mInstance.mSnowBallCfg.mCanAttackRate)
                {
                    ret = true;
                }
            }
            else if(this.mBallRadius < other.getBallRadius())
            {
                if (this.mBallRadius * Ctx.mInstance.mSnowBallCfg.mCanAttackRate <= other.getBallRadius())
                {
                    ret = true;
                }
            }

            return ret;
        }

        public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            this.mAttack.overlapToEnter(bBeingEntity, collisionInfo);
        }

        public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            this.mAttack.overlapToStay(bBeingEntity, collisionInfo);
        }

        public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            this.mAttack.overlapToExit(bBeingEntity, collisionInfo);
        }

        // 是否需要分离
        public bool isNeedSeparate(BeingEntity other)
        {
            UnityEngine.Vector3 direction = other.getPos() - this.getPos();

            if(direction.magnitude <= this.getBallRadius() + other.getBallRadius() + SnowBallCfg.msSeparateFactor)
            {
                return true;
            }

            return false;
        }

        // 通过当前状态判断是否可以进行分离
        public bool canSeparateByState()
        {
            if(BeingState.eBSIdle == this.mBeingState ||
               BeingState.eBSSeparation == this.mBeingState)
            {
                return true;
            }

            return false;
        }

        // 通过当前状态判断是否需要减速
        public bool isNeedReduceSpeed()
        {
            //if (BeingState.BSWalk == this.mBeingState)
            //{
            //    return true;
            //}

            //return false;

            return !this.canMerge();
        }

        // 是否可以进行融合
        virtual public bool canMerge()
        {
            return true;
        }

        // 是否可以吐积雪块
        virtual public bool canEmitSnow()
        {
            return this.mBallRadius >= Ctx.mInstance.mSnowBallCfg.mEmitSnowRadius;
        }

        virtual public float getEmitSnowSize()
        {
            return UtilMath.getRadiusByMass(Ctx.mInstance.mSnowBallCfg.mEmitSnowMass);        // 需要转换成半径
        }

        // 是否可以分裂
        virtual public bool canSplit()
        {
            return this.mBallRadius >= Ctx.mInstance.mSnowBallCfg.mCanSplitFactor * Ctx.mInstance.mSnowBallCfg.mInitSnowRadius;
        }

        // 是否可以 IO 控制向前移动
        virtual public bool canIOControlMoveForward()
        {
            if (BeingState.eBSBirth != this.getBeingState())
            {
                return true;
            }

            return false;
        }
    }
}