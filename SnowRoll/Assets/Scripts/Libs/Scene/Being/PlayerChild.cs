namespace SDK.Lib
{
    public class PlayerChild : Player
    {
        public Player mParentPlayer; // Parent 

        public PlayerChild(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;

            this.mAnimatorControl = new BeingAnimatorControl(this);
            this.mAnimFSM = new AnimFSM(this);
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

        public override void postInit()
        {
            base.postInit();

            this.mAnimFSM.UpdateFSM();

            this.mHud = Ctx.mInstance.mHudSystem.createHud(this);
        }
    }
}