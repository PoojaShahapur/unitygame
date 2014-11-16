using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 主角
	 */
    public class PlayerMain : Player, IPlayerMain
	{
		public PlayerMain()
		{
            m_skinAniModel.handleCB = onSkeletonLoaded;
		}

        public void onSkeletonLoaded()
        {
            Transform tran = m_skinAniModel.transform.FindChild("Reference/Hips");
            if(tran)
            {
                Ctx.m_instance.m_camSys.m_sceneCam.setTarget(tran);
            }
        }
	}
}