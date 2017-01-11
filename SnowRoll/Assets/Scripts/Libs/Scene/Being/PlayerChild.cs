namespace SDK.Lib
{
    public class PlayerChild : Player
    {
        public Player mParentPlayer; // Parent 
        protected UnityEngine.Vector3 mHudPos;
        protected string mTexPath;  // 纹理目录

        public PlayerChild(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;

            this.mAnimatorControl = new BeingAnimatorControl(this);
            this.mAnimFSM = new AnimFSM(this);
            this.mTexPath = "";
        }

        override public void dispose()
        {
            base.dispose();

            this.mParentPlayer.mPlayerSplitMerge.removeFormParent(this);
        }

        public override void onDestroy()
        {
            if(null != this.mAnimatorControl)
            {
                this.mAnimatorControl.dispose();
                this.mAnimatorControl = null;
            }
            if(null != this.mAnimFSM)
            {
                this.mAnimFSM.dispose();
                this.mAnimFSM = null;
            }

            base.onDestroy();
        }

        override public void autoHandle()
        {
            base.autoHandle();

            this.mParentPlayer.mPlayerSplitMerge.addToParent(this);
        }

        public override void postInit()
        {
            base.postInit();

            //this.mAnimFSM.UpdateFSM();

            this.mHud = Ctx.mInstance.mHudSystem.createHud(this);
        }

        override public UnityEngine.Vector3 getHudPos()
        {
            this.mHudPos = this.mPos;
            this.mHudPos.y += this.mBallRadius;

            return this.mHudPos;
        }

        override public void setTexture(string path)
        {
            if(path != mTexPath)
            {
                this.mTexPath = path;

                base.setTexture(this.mTexPath);
            }
        }
    }
}