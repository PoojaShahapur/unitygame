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
            m_skinAniModel.m_modelList = new GameObject[(int)PlayerModelDef.eModelTotal];
		}
	}
}