namespace SDK.Lib
{
    /**
	 * @brief 玩家的 Child 管理器
	 */
    public class PlayerChildMgr : EntityMgrBase
    {
        protected Player mParentPlayer;     // 最初分裂者

        public PlayerChildMgr()
        {
            
        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

        public void postUpdate()
        {
            int idx = 0;
            int len = this.getEntityCount();

            while(idx < len)
            {
                (this.mSceneEntityList[idx] as PlayerMainChild).postUpdate();
                ++idx;
            }
        }

        override public void init()
        {
            base.init();
        }

        public override void dispose()
        {
            base.dispose();

            this.mParentPlayer = null;
        }

        public void addPlayerChild(PlayerChild child)
        {
            this.addEntity(child);
        }

        public void removePlayerChild(PlayerChild child)
        {
            this.removeEntity(child);
        }

        public void setParentPlayer(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;
        }

        // 创建 Child
        public void createChild(Player splitParentPlayer)
        {
            Player child;
            if (EntityType.ePlayerMain == mParentPlayer.getEntityType())
            {
                child = new PlayerMainChild(this.mParentPlayer);
            }
            else
            {
                child = new PlayerMainChild(this.mParentPlayer);
            }

            child.init();
            child.setPos(splitParentPlayer.getPos());
        }
    }
}