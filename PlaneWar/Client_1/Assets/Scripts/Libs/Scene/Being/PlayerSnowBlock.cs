namespace SDK.Lib
{
    /**
     * @brief Player 吐出来的积雪块
     */
    public class PlayerSnowBlock : BeingEntity
    {
        protected uint mOwnerThisId;

        public PlayerSnowBlock()
        {
            this.mTypeId = "PlayerSnowBlock";
            this.mEntityType = EntityType.ePlayerSnowBlock;
            this.mEntityUniqueId = Ctx.mInstance.mPlayerSnowBlockMgr.genNewStrId();
            this.mMovement = new PlayerSnowBlockMovement(this);
            this.mAttack = new PlayerSnowBlockAttack(this);

            this.mMoveSpeed = 50;
        }

        override public void dispose()
        {
            base.dispose();

            if (null != Ctx.mInstance.mPlayerSnowBlockMgr)
            {
                Ctx.mInstance.mPlayerSnowBlockMgr.removePlayerSnowBlock(this);
            }
        }

        override public void autoHandle()
        {
            base.autoHandle();

            if (Ctx.mInstance.mPlayerSnowBlockMgr != null)
                Ctx.mInstance.mPlayerSnowBlockMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new PlayerSnowBlockRender(this);
            mRender.init();
        }

        // 雪块不能吃，只能被吃
        override public bool canEatOther(BeingEntity other)
        {
            return false;
        }

        override public void setMoveSpeed(float value)
        {
            
        }

        override public float getBallWorldRadius()
        {
            return this.mBallRadius * Ctx.mInstance.mSnowBallCfg.mShitCollideRadius;
        }

        public void setOwnerThisId(uint value)
        {
            this.mOwnerThisId = value;
        }

        public void sendEat()
        {
            if (this.mOwnerThisId > 0)
            {
                PlayerChild child = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr.getEntityByThisId(this.mOwnerThisId) as PlayerChild;
                if (null != child)
                {
                    ((child as PlayerMainChild).mAttack as PlayerMainChildAttack).eatPlayerSnowBlock(this);
                }
            }
        }
    }
}