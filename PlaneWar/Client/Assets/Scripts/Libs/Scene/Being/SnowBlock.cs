namespace SDK.Lib
{
    public class SnowBlock : BeingEntity
    {
        public SnowBlock()
        {
            this.mTypeId = "SnowBlock";
            this.mEntityType = EntityType.eSnowBlock;
            this.mEntityUniqueId = Ctx.mInstance.mSnowBlockMgr.genNewStrId();

            this.mAttack = new SnowBlockAttack(this);
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mSnowBlockMgr.removeSnowBlock(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mSnowBlockMgr.addSnowBlock(this);
        }

        override public void initRender()
        {
            this.setPrefabPath(Ctx.mInstance.mSnowBallCfg.getRandomSnowBlockTex());

            mRender = new SnowBlockRender(this);
            mRender.init();
        }

        protected override void onPostInit()
        {
            base.onPostInit();

            //this.setTexture(Ctx.mInstance.mSnowBallCfg.getRandomSnowBlockTex());
            //this.setTexTile(Ctx.mInstance.mSnowBallCfg.getRandomSnowBlockTexTile());
        }

        // 雪块不能吃，只能被吃
        override public bool canEatOther(BeingEntity other)
        {
            return false;
        }

        override public float getBallWorldRadius()
        {
            return this.mBallRadius * Ctx.mInstance.mSnowBallCfg.mSnowBlockCollideRadius;
        }
    }
}