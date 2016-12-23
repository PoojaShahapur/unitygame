namespace SDK.Lib
{
    public class PlayerMainChild : PlayerChild
    {
        protected uint mLastMergedTime;    // 最后一次融合时间

        public PlayerMainChild(Player parentPlayer)
            : base(parentPlayer)
        {
            this.mTypeId = "PlayerMainChild";
            this.mEntityType = EntityType.ePlayerMainChild;
            this.mMovement = new PlayerMainChildMovement(this);
            this.mAttack = new PlayerMainChildAttack(this);
            this.mEntityUniqueId = Ctx.mInstance.mPlayerMgr.genChildNewStrId();

            this.mLastMergedTime = 0;
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

        override public void setBeingState(BeingState state)
        {
            base.setBeingState(state);

            if(BeingState.eBSBirth == this.mBeingState)
            {
                this.mMoveSpeedFactor = 5;
            }
            else
            {
                this.mMoveSpeedFactor = 1;
            }
        }

        public uint getLastMergedTime()
        {
            return this.mLastMergedTime;
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

        // 是否可以执行合并操作，能否合并只有一个冷却时间条件
        override public bool canMerge()
        {
            return UtilLogic.canMerge(this.mLastMergedTime);
        }
    }
}