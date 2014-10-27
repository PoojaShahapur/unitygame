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
            m_skinAniModel = new SkinAniModel[(int)NpcModelDef.eModelTotal];
		}
	}
}