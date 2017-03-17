namespace SDK.Lib
{		
	/**
	 * @brief »ù±¾ NPC
	 */
	public class Npc : BeingEntity 
	{
		public Npc()
            : base()
		{
            //mSkinAniModel.m_modelList = new SkinSubModel[(int)eNpcModelType.eModelTotal];
            //int idx = 0;
            //while (idx < (int)eNpcModelType.eModelTotal)
            //{
            //    mSkinAniModel.m_modelList[idx] = new SkinSubModel();
            //    ++idx;
            //}
		}

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mNpcMgr.removeEntity(this);
        }
    }
}