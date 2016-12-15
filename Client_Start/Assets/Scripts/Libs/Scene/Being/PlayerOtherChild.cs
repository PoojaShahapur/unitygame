namespace SDK.Lib
{
    public class PlayerOtherChild : PlayerChild
    {
        public PlayerOtherChild(Player parentPlayer)
            : base(parentPlayer)
        {
            this.mTypeId = "PlayerOtherChild";
            this.mEntityType = EntityType.ePlayerOtherChild;
            this.mMovement = new PlayerOtherChildMovement(this);
            this.mAttack = new PlayerOtherChildAttack(this);
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genChildNewStrId();
        }

        override public void initRender()
        {
            mRender = new PlayerOtherChildRender(this);
            mRender.init();
        }
    }
}