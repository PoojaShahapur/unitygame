namespace SDK.Lib
{
    public class PlayerChildMovement : PlayerMovement
    {
        public PlayerChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void lookAt(UnityEngine.Vector3 targetPt)
        {
            base.lookAt(targetPt);

            this.mDestPos = targetPt;
        }

        // 继续向前移动
        override public void moveAlong()
        {
            base.moveAlong();
            this.setIsMoveToDest(true);
        }

        // 移动暂停
        override public void movePause()
        {
            base.movePause();
            this.setIsMoveToDest(false);
        }
    }
}