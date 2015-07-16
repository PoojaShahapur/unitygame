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
            int idx = 0;
            while (idx < (int)NpcModelDef.eModelTotal)
            {
                m_skinAniModel.m_modelList[idx] = new PartInfo();
                ++idx;
            }
		}
	}
}