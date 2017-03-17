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
        protected float mMass;          // 质量

        public SceneEntityMovement mMovement;    // 移动组件
        public BeingEntityAttack mAttack;
        protected int reliveseconds; // 复活时间
        protected HudItemBase mHud; // HUD

        protected string mName;     // 名字
        public BeingAnimatorControl mAnimatorControl;
        public AnimFSM mAnimFSM;
        protected bool mIsFreezeXZ;     // 是否锁定 XZ 位置
        protected bool mIsEatedByOther;        // 是否被吃掉
        protected bool mIsEatedByServer;       // 自己吐的雪块是否服务器返回被自己吃掉

        protected string mTexPath;  // 纹理目录
        protected TileInfo mTileInfo;    // 纹理偏移信息
        protected string mPrefabPath;       // 预制的目录

        protected UnityEngine.Vector3 mHudPos;
        protected float mInvincibleTime;

        public BeingEntity()
        {
            //mSkinAniModel = new SkinModelSkelAnim();
            //mSkinAniModel.handleCB = onSkeletonLoaded;
            this.mBeingState = BeingState.eBSIdle;
            this.mBeingSubState = BeingSubState.eBSSNone;

            //this.mMoveSpeed = 1;
            this.mRotateSpeed = 10;
            this.mScaleSpeed = Ctx.mInstance.mSnowBallCfg.mScaleSpeed;

            this.setBallRadius(0, true, true);
            
            this.mName = "";
            this.mMoveSpeedFactor = 1;
            this.mIsFreezeXZ = false;
            this.mTexPath = "";

            //this.mMoveSpeed = 2;
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
            if (null != this.mHud)
            {
                this.mHud.dispose();
                this.mHud = null;
            }

            if (null != this.mMovement)
            {
                this.mMovement.dispose();
                this.mMovement = null;
            }

            if (null != this.mAttack)
            {
                this.mAttack.dispose();
                this.mAttack = null;
            }

            base.onDestroy();
        }

        virtual public void setMoveSpeed(float value)
        {
            this.mMoveSpeed = value;
            //this.mMoveSpeed = 2;
            this.setName(this.getName());

            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setMoveSpeed, BasicInfo is = {0}, MoveSpeed = {1}", this.getBasicInfoStr(), this.mMoveSpeed), LogTypeId.eLogBeingMove);
            }
        }

        public void setContactNotMergeSpeed(float value)
        {
            this.mMoveSpeedFactor = value / this.mMoveSpeed;
        }

        public float getMoveSpeed(bool isOrig = false)
        {
            if (isOrig)
            {
                return this.mMoveSpeed;
            }
            else
            {
                // 如果向中间靠拢，速度需要很慢
                if (BeingState.eBSMoveCenter == this.mBeingState)
                {
                    return Ctx.mInstance.mSnowBallCfg.mSlowMoveSpeed;
                }
                else
                {
                    return this.mMoveSpeed * this.mMoveSpeedFactor;
                }
            }
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

        // 强制移动到目标点,主要是遇到物理和逻辑不一致的情况，强制移动
        public void forceMoveDest(UnityEngine.Vector3 pos, bool immePos)
        {
            if (immePos)
            {
                this.setPos(pos);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).forceMoveDest(pos);
            }
        }

        public void setDestPos_FromKBE(UnityEngine.Vector3 pos, bool immePos)
        {
            pos = UtilApi.convPosByMode(pos);
            this.setDestPos(pos, immePos);
        }

        virtual public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {
            if (immePos)
            {
                this.setPos(pos);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestPos(pos);
            }
        }

        public UnityEngine.Vector3 getDestPos()
        {
            if (null != this.mMovement)
            {
                return (this.mMovement as BeingEntityMovement).getDestPos();
            }

            return UnityEngine.Vector3.zero;
        }

        public void setDestPosForBirth_FromKBE(UnityEngine.Vector3 pos, bool immePos)
        {
            pos = UtilApi.convPosByMode(pos);
            this.setDestPosForBirth(pos, immePos);
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

        public void setDestPosForMerge(UnityEngine.Vector3 pos, bool immePos)
        {
            if (immePos)
            {
                this.setPos(pos);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestPosForMerge(pos);
            }
        }

        // 向中心移动
        public void setDestPosForMoveCenter(UnityEngine.Vector3 pos, bool immePos)
        {
            if (immePos)
            {
                this.setPos(pos);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestPosForMoveCenter(pos);
            }
        }

        public void setDestRotate_FromKBE(UnityEngine.Vector3 rotate, bool immeRotate)
        {
            rotate = UtilApi.convRotByMode(rotate);
            this.setDestRotate(rotate, immeRotate);
        }

        public void setDestRotate(UnityEngine.Vector3 rotate, bool immeRotate)
        {
            if (immeRotate)
            {
                this.setRotateEulerAngle(rotate);
            }
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setDestRotate(rotate);
            }
        }

        // 设置分裂移动时候的前向移动方向
        public void setSeparateForwardRotate(UnityEngine.Vector3 rotate)
        {
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).setSeparateForwardRotate(rotate);
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
            if (immeScale)
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

        virtual public void setBallRadius(float size, bool immScale = false, bool isCalcMass = false)
        {
            if (this.mBallRadius != size && size > 0 && !UtilMath.isInvalidNum(size))
            {
                this.mBallRadius = size;
                //this.setDestScale(size, immScale);

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setBallRadius, BasicInfo is = {0}, BallRadius = {1},", this.getBasicInfoStr(), this.mBallRadius), LogTypeId.eLogBeingMove);
                }

                if (isCalcMass)
                {
                    this.mMass = UtilMath.getMassByRadius(this.mBallRadius);
                }
            }
        }

        public void setMass(float mass, bool isCalcRadius = true)
        {
            if (this.mMass != mass && mass > 0 && !UtilMath.isInvalidNum(mass))
            {
                this.mMass = mass;

                if (isCalcRadius)
                {
                    this.setBallRadius(UtilMath.getRadiusByMass(this.mMass));
                }

                // 如果全部打日志会直接卡掉的
                if (EntityType.ePlayerMainChild == this.mEntityType)
                {
                    if (MacroDef.ENABLE_LOG)
                    {
                        Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setMass, BasicInfo is = {0}, mass = {1}", this.getBasicInfoStr(), this.mMass), LogTypeId.eLogScene);
                    }
                }
            }
        }

        public float getMass()
        {
            return this.mMass;
        }

        public float getBallRadius()
        {
            return this.mBallRadius;
        }

        // 获取 mBallRadius 在世界空间真正的大小，这个是与碰撞大小相同的
        virtual public float getBallWorldRadius()
        {
            return this.mBallRadius;
        }

        virtual public void setName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.mName = name;

                if (null != this.mHud)
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

        // 获取分裂半径
        virtual public float getSplitWorldRadius()
        {
            float selfMass = UtilMath.getMassByRadius(this.mBallRadius);
            selfMass /= 2;
            float splitRadius = UtilMath.getRadiusByMass(selfMass);
            return splitRadius;
        }

        override protected void onPreInit()
        {
            this.setBallRadius(0);  // 初始小球的半径设为0，服务器会同步1过来

            // 基类初始化
            base.onPreInit();
            // 自动处理，例如添加到管理器
            this.autoHandle();
            // 初始化渲染器
            this.initRender();

            if (!MacroDef.ENABLE_SCENE2D_CLIP)  // 如果不使用场景裁剪，就直接加载资源，否则只有场景裁剪可视的时候，才加载资源
            {
                // 加载渲染器资源
                this.loadRenderRes();
            }
            else if(EntityType.ePlayerMain == this.mEntityType)     // PlayerMain 也要立即加载，因为有些依赖这个资源
            {
                this.loadRenderRes();
            }
        }

        protected override void onPostInit()
        {
            base.onPostInit();
        }

        override public void loadRenderRes()
        {
            if (null != this.mRender)
            {
                this.mRender.load();
            }
        }

        override public void onTick(float delta, TickMode tickMode)
        {
            base.onTick(delta, tickMode);
        }

        // Tick 第一阶段执行
        override protected void onPreTick(float delta, TickMode tickMode)
        {
            base.onPreTick(delta, tickMode);
        }

        // Tick 第二阶段执行
        override protected void onPostTick(float delta, TickMode tickMode)
        {
            if (null != this.mMovement)
            {
                this.mMovement.onTick(delta, tickMode);
            }

            if (null != this.mAttack)
            {
                this.mAttack.onTick(delta, tickMode);
            }
        }

        // 获取 Hud 场景中的位置
        virtual public UnityEngine.Vector3 getHudPos()
        {
            this.mHudPos = this.mPos;
            this.mHudPos.y += this.mBallRadius;

            return this.mHudPos;
        }

        virtual public void setBeingState(BeingState state)
        {
            if (this.mBeingState != state)
            {
                this.mBeingState = state;

                if (null != this.mAnimFSM)
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

        // 是否可以吃掉对方
        virtual public bool canEatOther(BeingEntity other)
        {
            bool ret = false;

            // 判断半径
            if (this.mBallRadius > other.getBallRadius())
            {
                if (this.mBallRadius >= other.getBallRadius() * Ctx.mInstance.mSnowBallCfg.mCanAttackRate)
                {
                    // 判断中心点距离
                    if (UtilMath.squaredDistance(this.mPos, other.getPos()) <= this.mBallRadius * this.mBallRadius)
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        // 是否可以被吃掉
        virtual public bool canEatByOther(BeingEntity other)
        {
            bool ret = false;

            if (this.mBallRadius < other.getBallRadius())
            {
                if (this.mBallRadius * Ctx.mInstance.mSnowBallCfg.mCanAttackRate <= other.getBallRadius())
                {
                    // 判断中心点距离
                    if (UtilMath.squaredDistance(this.mPos, other.getPos()) <= other.getBallRadius() * other.getBallRadius())
                    {
                        ret = true;
                    }
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

        public void overlapToEnter2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            this.mAttack.overlapToEnter2D(bBeingEntity, collisionInfo);
        }

        public void overlapToStay2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            this.mAttack.overlapToStay2D(bBeingEntity, collisionInfo);
        }

        public void overlapToExit2D(BeingEntity bBeingEntity, UnityEngine.Collision2D collisionInfo)
        {
            this.mAttack.overlapToExit2D(bBeingEntity, collisionInfo);
        }

        // 是否需要分离
        public bool isNeedSeparate(BeingEntity other)
        {
            UnityEngine.Vector3 direction = other.getPos() - this.getPos();

            if (direction.magnitude <= this.getBallRadius() + other.getBallRadius() + SnowBallCfg.msSeparateFactor)
            {
                return true;
            }

            return false;
        }

        // 通过当前状态判断是否可以进行分离
        public bool canSeparateByState()
        {
            if (BeingState.eBSIdle == this.mBeingState ||
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
            return this.mMass >= Ctx.mInstance.mSnowBallCfg.mCanEmitSnowMass;
        }

        public float getEmitSnowSize()
        {
            return UtilMath.getRadiusByMass(Ctx.mInstance.mSnowBallCfg.mEmitSnowMass);        // 需要转换成半径
        }

        virtual public float getEmitSnowWorldSize()
        {
            return UtilMath.getRadiusByMass(Ctx.mInstance.mSnowBallCfg.mEmitSnowMass);        // 需要转换成半径
        }

        // 是否可以分裂
        virtual public bool canSplit()
        {
            bool ret = false;
            ret = this.mMass >= Ctx.mInstance.mSnowBallCfg.mCanSplitMass;

            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::canSplit, BasicInfo is = {0}, current mass = {1}, SplitMass = {2}", this.getBasicInfoStr(), this.mMass, Ctx.mInstance.mSnowBallCfg.mCanSplitMass), LogTypeId.eLogScene);
            }

            return ret;
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

        // 是否可以交互
        public bool canInterActive(BeingEntity bBeingEntity)
        {
            bool can = true;

            if (UtilApi.isInFakePos(this.getPos()))
            {
                can = false;
            }
            else if (this.isClientDispose())
            {
                can = false;
            }
            else if (UtilApi.isInFakePos(bBeingEntity.getPos()))
            {
                can = false;
            }
            else if (bBeingEntity.isClientDispose())
            {
                can = false;
            }

            return can;
        }

        // 是否可以与其它的进行合并
        virtual public bool canMergeWithOther(BeingEntity bBeingEntity)
        {
            return false;
        }

        virtual public void mergeWithOther(BeingEntity bBeingEntity)
        {

        }

        virtual public void addParentOrientChangedhandle()
        {

        }

        // 设置纹理
        virtual public void setTexture(string path)
        {
            if (path != mTexPath)
            {
                this.mTexPath = path;

                if (null != this.mRender)
                {
                    (this.mRender as BeingEntityRender).setTexture(path);
                }
            }
        }

        virtual public void setTexTile(TileInfo tileInfo)
        {
            this.mTileInfo = tileInfo;

            if (null != this.mRender)
            {
                (this.mRender as BeingEntityRender).setTexTile(tileInfo);
            }
        }

        public TileInfo getTileInfo()
        {
            return this.mTileInfo;
        }

        virtual public void setLastMergedTime()
        {

        }

        // 接触跟随，但是不能融合
        virtual public void contactWithAndFollowButNotMerge(BeingEntity bBeing)
        {

        }

        public UnityEngine.Quaternion getDestRotate()
        {
            if (null != this.mMovement)
            {
                return (this.mMovement as BeingEntityMovement).getDestRotate();
            }

            return UtilMath.UnitQuat;
        }

        // 设置接触不融合跟随方向
        public void setNotMergeRotate(UnityEngine.Quaternion quat)
        {
            if (null != this.mMovement)
            {
                (this.mMovement as BeingEntityMovement).setNotMergeRotate(quat);
            }
        }

        public void clearContactNotMerge()
        {
            this.mMoveSpeedFactor = 1;

            if (null != this.mMovement)
            {
                (this.mMovement as BeingEntityMovement).clearNotMerge();
            }
        }

        public void setFreezeXZ(bool freeze)
        {
            this.mIsFreezeXZ = freeze;
            UtilApi.freezeRigidBodyXZPos(this.getGameObject(), this.mIsFreezeXZ);
        }

        public bool isFreezeXZ()
        {
            return this.mIsFreezeXZ;
        }

        public void setIsEatedByOther(bool value)
        {
            this.mIsEatedByOther = value;
        }

        public bool getIsEatedByOther()
        {
            return this.mIsEatedByOther;
        }

        public void setIsEatedByServer(bool value)
        {
            this.mIsEatedByServer = value;
        }

        public bool getIsEatedByServer()
        {
            return this.mIsEatedByServer;
        }

        // 是否正在请求服务器融合
        public bool isReqServerMerge()
        {
            return this.getBeingSubState() == BeingSubState.eBSSReqServerMerge;
        }

        // 预制目录是否有效
        public bool isPrefabPathValid()
        {
            return !string.IsNullOrEmpty(this.mPrefabPath);
        }

        public void setPrefabPath(string path)
        {
            this.mPrefabPath = path;
        }

        public string getPrefabPath()
        {
            return this.mPrefabPath;
        }

        // 移动到中心点
        virtual public void moveToCenter()
        {

        }

        public void enableRigid(bool enable)
        {
            if(null != this.mRender)
            {
                this.mRender.enableRigid(enable);
            }
        }

        public float getInvincibleTime()
        {
            return this.mInvincibleTime;
        }

        public void setInvincibleTime(float value)
        {
            this.mInvincibleTime = value;
        }
    }
}