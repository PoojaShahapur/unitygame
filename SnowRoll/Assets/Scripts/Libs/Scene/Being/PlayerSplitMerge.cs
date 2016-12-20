namespace SDK.Lib
{
    /**
     * @brief Player 的分裂和融合
     */
    public class PlayerSplitMerge
    {
        public PlayerChildMgr mPlayerChildMgr;    // 保存分裂的 Child
        protected Player mEntity;           // 分裂玩家
        protected bool mIsFitstSplited;     // 是否分裂

        public MRangeBox mRangeBox;
        protected float mTargetLength;      // 分裂时刻记录目标长度
        UnityEngine.Vector3 mTargetPoint;   // 目标点

        public PlayerSplitMerge(Player mPlayer)
        {
            this.mEntity = mPlayer;
            this.mPlayerChildMgr = new PlayerChildMgr();
            this.mIsFitstSplited = true;
            mRangeBox = new MRangeBox();
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        public virtual void onTick(float delta)
        {
            this.mPlayerChildMgr.onTick(delta);
        }

        public float getTargetLength()
        {
            return this.mTargetLength;
        }

        public UnityEngine.Vector3 getTargetPoint()
        {
            return this.mTargetPoint;
        }

        public void addToParent(Player childPlayer)
        {
            this.mPlayerChildMgr.addEntity(childPlayer);
        }

        public void removeFormParent(Player childPlayer)
        {
            this.mPlayerChildMgr.removeEntity(childPlayer);
        }

        public PlayerMovement[] getAllChildMovement()
        {
            int total = this.mPlayerChildMgr.getEntityCount();
            int index = 0;
            Player player = null;
            PlayerMovement[] movement = new PlayerMovement[total];
            while (index < total)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                movement[index] = player.mMovement as PlayerMovement;
                ++index;
            }

            return movement;
        }

        // 分裂
        public void startSplit()
        {
            if (this.mIsFitstSplited)
            {
                // 如果没有分裂，就是第一次分裂
                this.onFirstSplit();
            }
            else
            {
                // 已经分裂
                this.onNoFirstSplit();
            }
        }

        virtual protected void onFirstSplit()
        {
            this.mIsFitstSplited = false;
        }

        virtual protected void onNoFirstSplit()
        {
            
        }

        protected void calcTargetLength()
        {
            this.mTargetLength = mRangeBox.getHalfZ() * Ctx.mInstance.mSnowBallCfg.mK + Ctx.mInstance.mSnowBallCfg.mN;
        }

        //public void reduceTargetLength(float length)
        //{
        //    mTargetLength -= length;
        //    if(mTargetLength <= 0)
        //    {
        //        mTargetLength = 0;
        //    }
        //}

        // 计算目标点
        public void calcTargetPoint()
        {
            if (this.mTargetLength > 0)
            {
                this.mTargetPoint = mEntity.getPos() + mEntity.getRotate() * new UnityEngine.Vector3(0, 0, this.mTargetLength);

                Ctx.mInstance.mPlayerMgr.setPlayerTargetPos(this.mTargetPoint);
            }
        }

        public void updateChildDestDir()
        {
            // 设置查看目标点
            int idx = 0;
            int num = this.mPlayerChildMgr.getEntityCount();
            PlayerChild player;

            while (idx < num)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerChild;
                player.setDestPosAndDestRotate(this.mTargetPoint, false, true);

                ++idx;
            }
        }

        virtual public MergeItem addMerge(PlayerChild aChild, PlayerChild bChild)
        {
            return null;
        }

        virtual public void removeMerge(PlayerChild aChild, PlayerChild bChild)
        {

        }

        virtual public bool isExistMerge(PlayerChild aChild, PlayerChild bChild)
        {
            return false;
        }

        virtual public void emitSnowBlock()
        {

        }

        virtual public void setDestPos(UnityEngine.Vector3 pos, bool immePos)
        {

        }
    }
}