namespace SDK.Lib
{
    /**
     * @brief Player 的分裂和融合
     */
    public class PlayerSplitMerge
    {
        public PlayerChildMgr mPlayerChildMgr;    // 保存分裂的 Child
        protected Player mEntity;     // 分裂玩家
        protected bool mIsFitstSplited;    // 是否分裂

        public Player mParentPlayer;  // 真正的父
        public MRangeBox mRangeBox;
        protected float mTargetLength;     // 分裂时刻记录目标长度

        public PlayerSplitMerge(Player mPlayer)
        {
            this.mEntity = mPlayer;
            this.mPlayerChildMgr = new PlayerChildMgr();
            this.mIsFitstSplited = true;
            mRangeBox = new MRangeBox();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public virtual void onTick(float delta)
        {
            this.mPlayerChildMgr.onTick(delta);
        }

        public void setParentPlayer(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;
        }

        public void addToParent(Player childPlayer)
        {
            this.mParentPlayer.mPlayerSplitMerge.mPlayerChildMgr.addEntity(childPlayer);
        }

        public void removeFormParent(Player childPlayer)
        {
            this.mParentPlayer.mPlayerSplitMerge.mPlayerChildMgr.removeEntity(childPlayer);
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
            this.mTargetLength = mRangeBox.getHalfZ() * Ctx.mInstance.mPlayerMgr.mK + Ctx.mInstance.mPlayerMgr.mN;
        }

        // 计算目标点
        public UnityEngine.Vector3 getAndCalcTargetPoint()
        {
            UnityEngine.Vector3 targetPoint = mEntity.getRotate() * new UnityEngine.Vector3(0, 0, this.mTargetLength);
            return targetPoint;
        }

        public void updateChildDestDir()
        {
            // 设置查看目标点
            int idx = 0;
            int num = this.mPlayerChildMgr.getEntityCount();
            PlayerChild player;
            UnityEngine.Vector3 targetPoint = this.getAndCalcTargetPoint();

            while (idx < num)
            {
                player = this.mPlayerChildMgr.getEntityByIndex(idx) as PlayerChild;
                (player.mMovement as BeingEntityMovement).lookAt(targetPoint);

                ++idx;
            }
        }
    }
}