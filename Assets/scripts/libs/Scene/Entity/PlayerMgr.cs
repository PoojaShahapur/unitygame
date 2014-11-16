using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : BeingMgr, IPlayerMgr
	{
        protected PlayerMain m_hero;

        public PlayerMgr()
		{

		}

        public IPlayerMain createHero()
        {
            return new PlayerMain();
        }

        public void addHero(IPlayerMain hero)
        {
            m_hero = hero as PlayerMain;
            add(m_hero);
        }

        public IPlayerMain getHero()
        {
            return m_hero;
        }
	}
}