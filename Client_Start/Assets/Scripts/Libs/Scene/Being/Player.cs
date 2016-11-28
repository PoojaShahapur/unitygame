namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class Player : BeingEntity
	{
        protected MList<Player> mChildrenList;    // 保存分裂的 Child

        public Player()
            : base()
		{
            //m_skinAniModel.m_modelList = new SkinSubModel[(int)ePlayerModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)ePlayerModelType.eModelTotal)
            //{
            //    m_skinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}
            mChildrenList = new MList<Player>();
        }

        override public void init()
        {
            base.init();

            Ctx.mInstance.mPlayerMgr.addEntity(this);
        }

        // 构造完成 Player 后，在初始化 PlayerRender
        public override void onInit()
        {
            base.onInit();

            m_render = new PlayerRender(this);
            m_render.init();
            m_render.load();
        }

        override public void dispose()
        {
            base.dispose();
            Ctx.mInstance.mPlayerMgr.removeEntity(this);
        }
    }
}