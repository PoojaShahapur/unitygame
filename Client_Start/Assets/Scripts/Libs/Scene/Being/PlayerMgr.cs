namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : EntityMgrBase
	{
        static public float xlimit_min = 100;
        static public float xlimit_max = 900;
        static public float zlimit_min = 100;
        static public float zlimit_max = 900;
        static public float y_height = 1.0f;

        protected PlayerMain mHero;
        protected UniqueStrIdGen mChildUniqueStrIdGen;

        public float mK;     // 目标点 K 因子
        public float mN;     // 目标点 N 因子

        public PlayerTarget mPlayerTarget;

        public PlayerMgr()
		{
            mUniqueStrIdGen = new UniqueStrIdGen("PL", 0);
            mChildUniqueStrIdGen = new UniqueStrIdGen("PC", 0);

            mK = 10;
            mN = 10;
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
            float x = UtilApi.rangRandom(PlayerMgr.xlimit_min, PlayerMgr.xlimit_max);
            float z = UtilApi.rangRandom(PlayerMgr.zlimit_min, PlayerMgr.zlimit_max);

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

            this.mPlayerTarget.setOriginal(pos);
        }
    }
}