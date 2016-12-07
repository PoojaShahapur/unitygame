namespace SDK.Lib
{
	/**
	 * @brief 其它玩家
	 */
	public class PlayerOther : Player
	{
		public PlayerOther()
		{
            this.mTypeId = "PlayerOther";
            this.mEntityType = EntityType.ePlayerOther;
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genNewStrId();
            this.mMovement = new PlayerOtherMovement(this);
            this.mAttack = new PlayerOtherAttack(this);
            this.mPlayerSplitMerge = new PlayerOtherSplitMerge(this);
            this.mPlayerSplitMerge.setParentPlayer(this);
        }

        public override void postInit()
        {
            base.postInit();

            this.mPlayerSplitMerge.startSplit();
        }

        override public void initRender()
        {
            mRender = new PlayerOtherRender(this);
            mRender.init();
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mPlayerMgr.removeEntity(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mPlayerMgr.addEntity(this);
        }
    }
}