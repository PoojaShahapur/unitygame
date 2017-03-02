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
        }

        protected override void onPostInit()
        {
            base.onPostInit();

            this.hide();

            this.mMovement.init();
            this.mAttack.init();
            this.mPlayerSplitMerge.init();

            //this.mPlayerSplitMerge.startSplit();
        }

        override public void initRender()
        {
            //this.mRender = new PlayerOtherRender(this);
            //this.mRender.init();
            this.mRender = null;
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mPlayerMgr.removePlayer(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mPlayerMgr.addPlayer(this);
        }
    }
}