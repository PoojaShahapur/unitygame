namespace SDK.Lib
{
	/**
	 * @brief 生物 Entity，有感知，可以交互的
	 */
    public class BeingEntity : SceneEntityBase
    {
        protected SkinModelSkelAnim mSkinAniModel;      // 模型数据
        protected BeingState mBeingState;       // 当前的状态

        public float mMoveSpeed;     // 移动速度
        public float mRotateSpeed;   // 旋转速度
        public float mScaleSpeed;    // 缩放速度

        protected float mEatSize;    // 吃的大小，使用这个字段判断是否可以吃，以及吃后的大小
        protected BeingEntityAttack mAttack;

        public BeingEntity()
        {
            //mSkinAniModel = new SkinModelSkelAnim();
            //mSkinAniModel.handleCB = onSkeletonLoaded;
            this.mBeingState = BeingState.BSIdle;

            this.mMoveSpeed = 1;
            this.mRotateSpeed = 5;
            this.mScaleSpeed = 1;

            this.mEatSize = 10;
        }

        public SkinModelSkelAnim skinAniModel
        {
            get
            {
                return mSkinAniModel;
            }
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

        public BeingBehaviorControl behaviorControl
        {
            get
            {
                return getBeingBehaviorControl();
            }
        }

        virtual public BeingBehaviorControl getBeingBehaviorControl()
        {
            return null;
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

        public uint qwThisID
        {
            get
            {
                return 0;
            }
        }

        public void playFlyNum(int num)
        {

        }

        public void setMoveSpeed(float value)
        {
            this.mMoveSpeed = value;
        }

        public float getMoveSpeed()
        {
            return this.mMoveSpeed;
        }

        public void setRotateSpeed(float value)
        {
            this.mRotateSpeed = value;
        }

        public void setScaleSpeed(float value)
        {
            this.mScaleSpeed = value;
        }

        public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {
            if(immePos)
            {
                this.setOriginal(pos);
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
                this.setOriginal(pos);
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
                this.setOriginal(targetPt);
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

        public void setDestScale(float scale)
        {
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestScale(scale);
            }
        }

        public void setEatSize(float size)
        {
            this.mEatSize = size;

            this.setDestScale(size);
        }

        public float getEatSize()
        {
            return this.mEatSize;
        }

        override public void preInit()
        {
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

        public void setBeingState(BeingState state)
        {
            if (this.mBeingState != state)
            {
                this.mBeingState = state;
            }
        }

        public BeingState getBeingState()
        {
            return this.mBeingState;
        }

        virtual public bool canEatOther(BeingEntity other)
        {
            bool ret = false;

            if(this.mEatSize > other.getEatSize())
            {
                if (this.mEatSize >= other.getEatSize() * Ctx.mInstance.mSnowBallCfg.mCanEatRate)
                {
                    ret = true;
                }
            }
            else if(this.mEatSize < other.getEatSize())
            {
                if (this.mEatSize * Ctx.mInstance.mSnowBallCfg.mCanEatRate <= other.getEatSize())
                {
                    ret = true;
                }
            }

            return ret;
        }

        public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collider collider)
        {
            this.mAttack.overlapToEnter(bBeingEntity, collider);
        }

        public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {
            this.mAttack.overlapToStay(bBeingEntity, collision);
        }

        public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {
            this.mAttack.overlapToExit(bBeingEntity, collision);
        }

        // 是否需要分离
        public bool isNeedSeparate(BeingEntity other)
        {
            UnityEngine.Vector3 direction = other.getPos() - this.getPos();
            if(direction.magnitude < this.getEatSize() + other.getEatSize())
            {
                return true;
            }

            return false;
        }

        // 通过当前状态判断是否可以进行分离
        public bool canSeparateByState()
        {
            if(BeingState.BSIdle == this.mBeingState ||
               BeingState.BSSeparation == this.mBeingState)
            {
                return true;
            }

            return false;
        }

        // 通过当前状态判断是否需要减速
        public bool canReduceSpeed()
        {
            if (BeingState.BSWalk == this.mBeingState)
            {
                return true;
            }

            return false;
        }
    }
}