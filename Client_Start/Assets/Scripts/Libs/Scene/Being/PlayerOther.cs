namespace SDK.Lib
{
	/**
	 * @brief 其它玩家
	 */
	public class PlayerOther : Player
	{
		public PlayerOther()
		{
            mTypeId = "PlayerOther";
            this.mEntityType = EntityType.ePlayerOther;
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genNewStrId();
            this.mMovement = new PlayerOtherMovement(this);
            this.mAttack = new PlayerOtherAttack(this);
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