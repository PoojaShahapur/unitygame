using BehaviorLibrary;
using SDK.Common;
using UnityEngine;

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
            m_skinAniModel.m_modelList = new PartInfo[(int)PlayerModelDef.eModelTotal];
            int idx = 0;
            while (idx < (int)PlayerModelDef.eModelTotal)
            {
                m_skinAniModel.m_modelList[idx] = new PartInfo();
                ++idx;
            }
		}

        override public void addAiByID(string id)
        {
            base.addAiByID(id);
            m_vehicle.sceneGo = m_skinAniModel.m_modelList[(int)PlayerModelDef.eModelWaist].m_partGo;
        }
	}
}