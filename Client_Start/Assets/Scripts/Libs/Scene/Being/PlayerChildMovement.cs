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
            //this.moveToPos(targetPt);
        }

        // 移动暂停
        override public void movePause()
        {
            base.movePause();
            this.setIsMoveToDest(false);
        }
    }
}