namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : EntityMgrBase
	{
        protected PlayerMain mHero;
        protected UniqueStrIdGen mChildUniqueStrIdGen;

        public PlayerTarget mPlayerTarget;
        protected TimerItemBase mRepeatTimer;

        protected int mCurNum;
        protected int mMaxNum;

        public PlayerMgr()
		{
            this.mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerPrefix, 0);
            this.mChildUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerChildPrefix, 0);

            this.mCurNum = 0;
            this.mMaxNum = 10;
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public PlayerMain createHero()
        {
            return new PlayerMain();
        }

        public void addHero(PlayerMain hero)
        {
            this.mHero = hero as PlayerMain;
            this.addPlayer(this.mHero);
        }

        public PlayerMain getHero()
        {
            return mHero;
        }

        override public void init()
        {
            base.init();
            //this.startCreatOtherTimer();
        }

        public void addPlayer(Player player)
        {
            this.addEntity(player);
        }

        public void removePlayer(Player player)
        {
            this.removeEntity(player);
            --mMaxNum;
        }

        public string genChildNewStrId()
        {
            return mChildUniqueStrIdGen.genNewStrId();
        }

        public void createPlayerMain()
        {
            mHero = new PlayerMain();
            mHero.init();
            mHero.setDestPos(new UnityEngine.Vector3(50, 1.3f, 50f), true);
            mHero.setDestRotate(UnityEngine.Quaternion.identity.eulerAngles, true);
        }

        // 进行分裂
        public void startSplit()
        {
            if(null != this.mHero)
            {
                this.mHero.mPlayerSplitMerge.startSplit();
            }
        }

        public void setPlayerTargetPos(UnityEngine.Vector3 pos)
        {
            if(null == this.mPlayerTarget)
            {
                this.mPlayerTarget = new PlayerTarget();
                this.mPlayerTarget.init();
            }

            this.mPlayerTarget.setPos(pos);
        }

        // 吐雪球
        public void emitSnowBlock()
        {
            if (null != this.mHero)
            {
                this.mHero.emitSnowBlock();
            }
        }

        protected void startCreatOtherTimer()
        {
            mRepeatTimer = new TimerItemBase();
            mRepeatTimer.mInternal = 1.0f;
            mRepeatTimer.mIsInfineLoop = true;
            mRepeatTimer.mTimerDisp.setFuncObject(onRepeatTimerTick);
            this.mRepeatTimer.startTimer();
        }

        protected void onRepeatTimerTick(TimerItemBase timer)
        {
            this.createOneOtherPlayer();
        }

        // 创建一个 Other Player
        protected void createOneOtherPlayer()
        {
            if(null != this.mHero && this.mCurNum < this.mMaxNum)
            {
                UnityEngine.Vector3 pos = this.mHero.getPos() +  this.mHero.getRotate() * (UtilMath.UnitCircleRandom() * 10);
                pos.y = 1.3f;

                PlayerOther player = new PlayerOther();
                player.setPos(pos);
                player.init();

                ++this.mCurNum;
            }
        }

        public bool isHeroMoving()
        {
            if (null != this.mHero)
            {
                return (BeingState.eBSWalk == this.mHero.getBeingState() ||
                        BeingState.eBSSeparation == this.mHero.getBeingState() ||
                        BeingState.eBSBirth == this.mHero.getBeingState() ||
                        BeingState.eBSIOControlWalk == this.mHero.getBeingState()
                    );
            }

            return false;
        }
    }
}