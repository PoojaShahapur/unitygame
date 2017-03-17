namespace SDK.Lib
{
    public class FlyBulletMovement : BeingEntityMovement
    {
        public FlyBulletMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        public override void onTick(float delta, TickMode tickMode)
        {
            base.onTick(delta, tickMode);
        }

        // 到达终点
        override public void onArriveDestPos()
        {            
            base.onArriveDestPos();
            this.mEntity.dispose();
        }
    }
}