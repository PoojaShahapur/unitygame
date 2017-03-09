namespace SDK.Lib
{
    /**
     * @brief 多刚体交互计算策略
     */
    public class MutilRigidCalcPolicy
    {
        protected MList<BeingEntity> mEnablePhysicsEntityList;      // 开启刚体实体列表
        protected MList<BeingEntity> mDisablePhysicsEntityList;     // 关闭刚体实体列表

        protected bool mIsEnablePolicy;   // 是否开启策略
        protected int mStartLimitNum;   // 开始进行显示的数量
        protected int mCurNum;          // 当前已经有的 Entity 数量
        protected TimeInterval mTimeInterval;   // 定时器间隔

        public MutilRigidCalcPolicy()
        {
            this.mEnablePhysicsEntityList = new MList<BeingEntity>();
            this.mEnablePhysicsEntityList.setIsSpeedUpFind(true);

            this.mDisablePhysicsEntityList = new MList<BeingEntity>();
            this.mDisablePhysicsEntityList.setIsSpeedUpFind(true);

            this.mIsEnablePolicy = true;
            this.mStartLimitNum = 20;
            this.mCurNum = 0;
            this.mTimeInterval = new TimeInterval();
            this.mTimeInterval.setInterval(2);
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void onTick(float delta)
        {
            if (this.mIsEnablePolicy)
            {
                if (this.mTimeInterval.canExec(delta))
                {
                    if (this.mDisablePhysicsEntityList.Count() > 0)
                    {
                        BeingEntity entity = this.mDisablePhysicsEntityList.get(0);
                        this.mDisablePhysicsEntityList.RemoveAt(0);

                        if (!this.mEnablePhysicsEntityList.Contains(entity))
                        {
                            this.mEnablePhysicsEntityList.Add(entity);
                        }

                        entity.enableRigid(true);
                    }
                }
            }
        }

        public void addBeingEntity(BeingEntity entity)
        {
            if (this.mIsEnablePolicy)
            {
                if (this.mCurNum < this.mStartLimitNum)
                {
                    if (!this.mEnablePhysicsEntityList.Contains(entity))
                    {
                        this.mCurNum += 1;
                        this.mEnablePhysicsEntityList.Add(entity);
                    }
                }
                else
                {
                    if (!this.mDisablePhysicsEntityList.Contains(entity))
                    {
                        this.mCurNum += 1;
                        this.mDisablePhysicsEntityList.Add(entity);
                    }
                }
            }
        }

        public void removeBeingEntity(BeingEntity entity)
        {
            if (this.mIsEnablePolicy)
            {
                if (this.mEnablePhysicsEntityList.Contains(entity))
                {
                    this.mCurNum -= 1;
                    this.mEnablePhysicsEntityList.Remove(entity);

                    if (this.mEnablePhysicsEntityList.Count() < this.mStartLimitNum)
                    {
                        if (this.mDisablePhysicsEntityList.Count() > 0)
                        {
                            entity = this.mDisablePhysicsEntityList.get(0);
                            this.mEnablePhysicsEntityList.Add(entity);
                            this.mDisablePhysicsEntityList.RemoveAt(0);
                            // 开启物理计算
                            entity.enableRigid(true);
                        }
                    }
                }
                if (this.mDisablePhysicsEntityList.Contains(entity))
                {
                    this.mCurNum -= 1;
                    this.mDisablePhysicsEntityList.Remove(entity);
                }
            }
        }

        // 检查 Policy
        public bool checkPolicy(BeingEntity entity)
        {
            if (this.mIsEnablePolicy)
            {
                bool canEnable = true;

                // 需要关闭物理计算
                if (this.mDisablePhysicsEntityList.Contains(entity))
                {
                    canEnable = false;

                    entity.enableRigid(false);
                }

                return canEnable;
            }

            return true;
        }

        public bool isChildEnableRigidByThisId(PlayerMainChild entity)
        {
            return this.mEnablePhysicsEntityList.Contains(entity);
        }
    }
}