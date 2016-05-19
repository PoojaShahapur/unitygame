namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class Player : BeingEntity
	{			
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
		}

        override public void dispose()
        {
            base.dispose();
            Ctx.m_instance.m_playerMgr.removeEntity(this);
        }
    }
}