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
        }
	}
}