namespace SDK.Lib
{
	/**
	 * @brief 玩家管理器
	 */
    public class PlayerMgr : EntityMgrBase
	{
        protected PlayerMain m_hero;

        public PlayerMgr()
		{

		}

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public PlayerMain createHero()
        {
            return new PlayerMain();
        }

        public void addHero(PlayerMain hero)
        {
            m_hero = hero as PlayerMain;
            addEntity(m_hero);
        }

        public PlayerMain getHero()
        {
            return m_hero;
        }
	}
}