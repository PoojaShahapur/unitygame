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

        public PlayerMgr()
		{
            mUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerPrefix, 0);
            mChildUniqueStrIdGen = new UniqueStrIdGen(UniqueStrIdGen.PlayerChildPrefix, 0);
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
            this.addEntity(this.mHero);
        }

        public PlayerMain getHero()
        {
            return mHero;
        }

        override public void init()
        {
            
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
    }
}