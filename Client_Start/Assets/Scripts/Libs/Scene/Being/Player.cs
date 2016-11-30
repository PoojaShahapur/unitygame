namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class Player : BeingEntity
	{
        protected MList<PlayerChild> mChildrenList;    // 保存分裂的 Child

        public Player()
		{
            //m_skinAniModel.m_modelList = new SkinSubModel[(int)ePlayerModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)ePlayerModelType.eModelTotal)
            //{
            //    m_skinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}
            mChildrenList = new MList<PlayerChild>();
        }

        override public void init()
        {
            base.init();
        }

        // 构造完成 Player 后，在初始化 PlayerRender
        public override void onInit()
        {
            base.onInit();
        }

        override public void dispose()
        {
            base.dispose();
            Ctx.mInstance.mPlayerMgr.removeEntity(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mPlayerMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new PlayerRender(this);
            mRender.init();
        }

        override public void loadRenderRes()
        {
            mRender.load();
        }
    }
}