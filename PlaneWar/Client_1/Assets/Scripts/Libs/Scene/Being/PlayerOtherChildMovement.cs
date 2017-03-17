namespace SDK.Lib
{
    public class PlayerOtherChildMovement : PlayerChildMovement
    {
        public PlayerOtherChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
        }
    }
}