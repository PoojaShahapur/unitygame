namespace SDK.Lib
{
    public class PlayerChildMovement : PlayerMovement
    {
        public PlayerChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        // 移动暂停
        override public void movePause()
        {
            base.movePause();
            this.setIsMoveToDest(false);
        }
    }
}