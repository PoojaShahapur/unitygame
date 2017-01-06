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
            if (null != mPlayerChildMgr)
            {
                mPlayerChildMgr.dispose();
                mPlayerChildMgr = null;
            }

            mEntity = null;
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

        public float getMaxCameraLength()
        {
            float length = 0;
            if(null != this.mPlayerChildMgr)
            {
                UnityEngine.Vector3 center = Ctx.mInstance.mPlayerMgr.getHero().getPos(); ; //中心
                int total = this.mPlayerChildMgr.getEntityCount();
                int index = 0;
                Player player = null;

                if (1 == total)//只有一个根据直径缩放
                {
                    player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                    length = player.getScale().x * 2;
                }
                else//多个根据中心到最远子物体的距离与最大鱼直径之和缩放
                {
                    float maxradius = 0;
                    while (index < total)
                    {
                        player = this.mPlayerChildMgr.getEntityByIndex(index) as Player;
                        if (maxradius < player.getBallRadius() * 2)//直径
                        {
                            maxradius = player.getBallRadius() * 2;
                        }
                        float templen = UtilMath.Sqrt(UtilMath.Sqr(center.x - player.getPos().x) + UtilMath.Sqr(center.z - player.getPos().z));
                        if (templen > length) length = templen;
                        ++index;
                    }

                    length += maxradius;
                }
            }

            return 0 == length ? 5 : length;
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

        //virtual public void addSplitChild(PlayerChild playerChild)
        //{

        //}

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
            if (this.mTargetLength > 0 && mEntity != null)
            {
                this.mTargetPoint = mEntity.getPos() + mEntity.getRotate() * new UnityEngine.Vector3(0, 0, this.mTargetLength);

                //Ctx.mInstance.mPlayerMgr.setPlayerTargetPos(this.mTargetPoint);
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

        virtual public void setName()
        {

        }

        virtual public void updateCenterPos()
        {

        }

        virtual public float getAllChildMass()
        {
            return 0;
        }
    }
}