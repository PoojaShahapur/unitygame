namespace SDK.Lib
{
    public class PlayerOtherMovement : BeingEntityMovement
    {
        public PlayerOtherMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void onTick(float delta, TickMode tickMode)
        {
            base.onTick(delta, tickMode);
        }
    }
}