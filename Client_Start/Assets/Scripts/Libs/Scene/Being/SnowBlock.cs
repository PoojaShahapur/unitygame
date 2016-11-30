namespace SDK.Lib
{
    public class SnowBlock : BeingEntity
    {
        public SnowBlock()
        {
            mTypeId = "SnowBlock";
            this.mEntityType = EntityType.eSnowBlock;
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
            m_render = new SnowBlockRender(this);
            m_render.init();
        }
    }
}