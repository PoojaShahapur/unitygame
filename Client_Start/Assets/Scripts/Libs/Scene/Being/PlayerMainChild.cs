namespace SDK.Lib
{
    public class PlayerMainChild : PlayerChild
    {
        public PlayerMainChild(Player parentPlayer)
            : base(parentPlayer)
        {
            this.mTypeId = "PlayerMainChild";
            this.mEntityType = EntityType.ePlayerChild;
            this.mMovement = new PlayerMainChildMovement(this);
            this.mAttack = new PlayerMainChildAttack(this);
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genChildNewStrId();
        }

        override public void initRender()
        {
            mRender = new PlayerMainChildRender(this);
            mRender.init();
        }
    }
}