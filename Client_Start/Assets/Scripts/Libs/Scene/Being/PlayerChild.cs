namespace SDK.Lib
{
    public class PlayerChild : Player
    {
        public Player mParentPlayer; // Parent Player

        public PlayerChild(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;
        }

        override public void dispose()
        {
            base.dispose();

            this.mParentPlayer.mPlayerSplitMerge.removeFormParent(this);
        }

        override public void autoHandle()
        {
            base.autoHandle();

            this.mParentPlayer.mPlayerSplitMerge.addToParent(this);
        }

        // 修改新的位置信息，转向目标点，Child 只能通过这个接口改变方向
        //public void lookAt(UnityEngine.Vector3 targetPt)
        //{
        //    (this.mMovement as BeingEntityMovement).lookAt(targetPt);
        //}
    }
}