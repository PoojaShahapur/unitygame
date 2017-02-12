namespace SDK.Lib
{
	/**
	 * @brief 场景中的玩家
	 */
	public class PlayerTarget : Player
    {
        public PlayerTarget()
        {
            this.mTypeId = "PlayerTarget";
            this.mEntityType = EntityType.ePlayerTarget;
        }

        protected override void onPostInit()
        {
            base.onPostInit();
        }

        override public void initRender()
        {
            mRender = new PlayerTargetRender(this);
            mRender.init();
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mPlayerMgr.mPlayerTarget = null;
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mPlayerMgr.mPlayerTarget = this;
        }
    }
}