using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : BeingMgr
	{
        protected PlayerMain m_hero;

        public PlayerMgr()
		{

		}

        public PlayerMain createHero()
        {
            return new PlayerMain();
        }

        public void addHero(PlayerMain hero)
        {
            m_hero = hero as PlayerMain;
            add(m_hero);
        }

        public PlayerMain getHero()
        {
            return m_hero;
        }
	}
}