namespace SDK.Lib
{
    /**
     * @brief FlyBullet，Bullet 谁发射，谁控制，但是处理不了离开自己一屏，如果离开自己一屏，只能算是 Bullet 无效
     */
    public class FlyBullet : BeingEntity
    {
        protected uint mOwnerThisId;
        protected bool mIsSelfBullet;   // 是否是自己发射的 Bullet

        public FlyBullet()
        {
            this.mTypeId = "FlyBullet";
            this.mEntityType = EntityType.eFlyBullet;
            this.mEntityUniqueId = Ctx.mInstance.mFlyBulletMgr.genNewStrId();
            this.mMovement = new FlyBulletMovement(this);
            this.mAttack = new FlyBulletAttack(this);

            this.mIsSelfBullet = true;
        }

        override public void dispose()
        {
            base.dispose();

            if (null != Ctx.mInstance.mFlyBulletMgr)
            {
                Ctx.mInstance.mFlyBulletMgr.removeFlyBullet(this);
            }
        }

        override public void autoHandle()
        {
            base.autoHandle();

            if (null != Ctx.mInstance.mFlyBulletMgr)
            {
                Ctx.mInstance.mFlyBulletMgr.addFlyBullet(this);
            }
        }

        override public void initRender()
        {
            mRender = new FlyBulletRender(this);
            mRender.init();
        }

        // 雪块不能吃，只能被吃
        override public bool canEatOther(BeingEntity other)
        {
            return false;
        }

        override public void setMoveSpeed(float value)
        {
            base.setMoveSpeed(value);
        }

        public bool isSelfBullet()
        {
            return this.mIsSelfBullet;
        }

        override public float getBallWorldRadius()
        {
            return this.mBallRadius * Ctx.mInstance.mSnowBallCfg.mShitCollideRadius;
        }

        public void setOwnerThisId(uint value)
        {
            this.mOwnerThisId = value;

            this.mIsSelfBullet = Ctx.mInstance.mPlayerMgr.isHeroByThisId(this.mOwnerThisId);
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