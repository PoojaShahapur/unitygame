using SDK.Common;
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
            m_skinAniModel.m_modelList = new PartInfo[(int)NpcModelDef.eModelTotal];
		}
	}
}