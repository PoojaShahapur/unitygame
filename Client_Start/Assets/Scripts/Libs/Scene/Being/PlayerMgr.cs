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

        public void createPlayer()
        {
            float x = UtilApi.rangRandom(PlayerMgr.xlimit_min, PlayerMgr.xlimit_max);
            float z = UtilApi.rangRandom(PlayerMgr.zlimit_min, PlayerMgr.zlimit_max);

            mHero = new PlayerMain();
            mHero.init();
            mHero.setOriginal(new UnityEngine.Vector3(x, PlayerMgr.y_height, z));
            mHero.setRotation(UnityEngine.Quaternion.identity);

            if (mHero != null)
            {
                //mHero.SetIsJustCreate(true);
                //++mHero.create_times;
                //mHero.cur_auto_relive_seconds = mHero.auto_relive_seconds;

                string tempName = "";
                tempName = Ctx.mInstance.mSystemSetting.getString("myname");
                //if (tempName == "")
                //{
                //    tempName = mHero.playerName;
                //}

                //mHero.SetIsRobot(false);
                mHero.setSelfName(tempName);
                //mHero.m_charid = 0;//自己的charid为0
                //mHero.setMyName(tempName);
            }
        }

        // 进行分裂
        public void startSplit()
        {
            if(null != this.mHero)
            {
                this.mHero.mPlayerSplitMerge.startSplit();
            }
        }
    }
}