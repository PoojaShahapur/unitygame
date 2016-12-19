namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class Player : BeingEntity
	{
        public PlayerSplitMerge mPlayerSplitMerge;

        public Player()
		{
            //mSkinAniModel.m_modelList = new SkinSubModel[(int)ePlayerModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)ePlayerModelType.eModelTotal)
            //{
            //    mSkinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}
        }

        override public void preInit()
        {
            base.preInit();

            //this.Start_Player();
            if (null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.init();
            }
        }

        // 构造完成 Player 后，在初始化 PlayerRender
        public override void onInit()
        {
            base.onInit();
        }

        override public void dispose()
        {
            base.dispose();
            //Ctx.mInstance.mPlayerMgr.removeEntity(this);
        }

        override public void onDestroy()
        {
            base.onDestroy();
            if (null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.dispose();
            }
        }

        override public void autoHandle()
        {
            base.autoHandle();

            //Ctx.mInstance.mPlayerMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new PlayerRender(this);
            mRender.init();
        }

        public override void onPreTick(float delta)
        {
            base.onPreTick(delta);

            //this.onLoop();
        }

        public override void onPostTick(float delta)
        {
            base.onPostTick(delta);

            if(null != this.mPlayerSplitMerge)
            {
                this.mPlayerSplitMerge.onTick(delta);
            }
        }

        override public bool canEatOther(BeingEntity other)
        {
            bool ret = false;
            // 雪快必然可以被吃掉
            if(other.getEntityType() == EntityType.eSnowBlock)
            {
                ret = true;
            }
            else
            {
                ret = base.canEatOther(other);
            }

            return ret;
        }

        // 获取所有的 ChildMovement
        public PlayerMovement[] getAllChildMovement()
        {
            if(null != this.mPlayerSplitMerge)
            {
                return this.mPlayerSplitMerge.getAllChildMovement();
            }

            return null;
        }

        public MList<SceneEntityBase> getChildList()
        {
            if (null != this.mPlayerSplitMerge)
            {
                return this.mPlayerSplitMerge.mPlayerChildMgr.getSceneEntityList();
            }

            return null;
        }
    }
}