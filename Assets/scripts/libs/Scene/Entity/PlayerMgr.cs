using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : BeingMgr, IPlayerMgr
	{
        public PlayerMgr()
		{

		}

        public IPlayerMain createHero()
        {
            return new PlayerMain();
        }
	}
}