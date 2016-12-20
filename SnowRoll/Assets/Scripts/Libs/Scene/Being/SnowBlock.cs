﻿namespace SDK.Lib
{
    public class SnowBlock : BeingEntity
    {
        public SnowBlock()
        {
            this.mTypeId = "SnowBlock";
            this.mEntityType = EntityType.eSnowBlock;
            this.mEntityUniqueId = Ctx.mInstance.mSnowBlockMgr.genNewStrId();
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
            mRender = new SnowBlockRender(this);
            mRender.init();
        }

        // 雪块不能吃，只能被吃
        override public bool canEatOther(BeingEntity other)
        {
            return false;
        }
    }
}