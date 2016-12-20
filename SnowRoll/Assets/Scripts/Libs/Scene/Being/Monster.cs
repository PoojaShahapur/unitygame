namespace SDK.Lib
{
    public class Monster : BeingEntity
    {
        public Monster()
            : base()
        {
            mTypeId = "Monster";
        }

        override public void dispose()
        {
            base.dispose();
            Ctx.mInstance.mMonsterMgr.removeEntity(this);
        }
    }
}