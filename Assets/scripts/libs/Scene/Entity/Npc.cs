using UnityEngine;

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
            m_skinAniModel.m_modelList = new GameObject[(int)NpcModelDef.eModelTotal];
		}
	}
}