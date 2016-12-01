namespace SDK.Lib
{
    public class SnowBlock : BeingEntity
    {
        public SnowBlock()
        {
            mTypeId = "SnowBlock";
            this.mEntityType = EntityType.eSnowBlock;
            this.mEntityUniqueId = Ctx.mInstance.mSnowBlockMgr.genNewStrId();
        }

        override public void dispose()
        {
            base.dispose();

            Ctx.mInstance.mSnowBlockMgr.removeEntity(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            Ctx.mInstance.mSnowBlockMgr.addEntity(this);
        }

        override public void initRender()
        {
            mRender = new SnowBlockRender(this);
            mRender.init();
        }
    }
}