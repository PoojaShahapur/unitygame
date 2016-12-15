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

        public override void preInit()
        {
            base.preInit();

            this.mMovement.init();
            this.mAttack.init();
        }

        // 自己当前是否在分裂目标点的后面
        public bool isBehindTargetPoint()
        {
            if (UtilMath.isABehindC(this.getPos(), this.mParentPlayer.getPos(), this.mParentPlayer.mPlayerSplitMerge.getTargetPoint()))
            {
                return true;
            }

            return false;
        }
    }
}