namespace SDK.Lib
{
    public class PlayerMainAttack : BeingEntityAttack
    {
        public PlayerMainAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            // 处理触碰小球吃掉小球的情况
        }
    }
}