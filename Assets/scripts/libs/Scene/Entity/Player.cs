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
		}

        override public void addAi(BehaviorTree behaviorTree)
        {
            base.addAi(behaviorTree);
            m_vehicle.sceneGo = m_skinAniModel.m_modelList[(int)PlayerModelDef.eModelWaist].m_partGo;
        }
	}
}