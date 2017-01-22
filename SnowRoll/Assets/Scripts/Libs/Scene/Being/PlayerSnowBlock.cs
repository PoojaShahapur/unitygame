namespace SDK.Lib
{
    /**
     * @brief Player 吐出来的积雪块
     */
    public class PlayerSnowBlock : BeingEntity
    {
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

            Ctx.mInstance.mPlayerSnowBlockMgr.removePlayerSnowBlock(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

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
    }
}