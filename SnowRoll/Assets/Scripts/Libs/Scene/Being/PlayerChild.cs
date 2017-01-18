﻿namespace SDK.Lib
{
    public class PlayerChild : Player
    {
        public Player mParentPlayer; // Parent 
        protected UnityEngine.Vector3 mHudPos;
        protected string mTexPath;  // 纹理目录
        private UnityEngine.Vector3 preSendPosition; //上一次发送时的位置

        public PlayerChild(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;

            this.mAnimatorControl = new BeingAnimatorControl(this);
            this.mAnimFSM = new AnimFSM(this);
            this.mTexPath = "";
            this.preSendPosition = UnityEngine.Vector3.zero;
        }

        public void setPreSendPosition(UnityEngine.Vector3 _position)
        {
            this.preSendPosition = _position;
        }

        public UnityEngine.Vector3 getPreSendPosition()
        {
            return this.preSendPosition;
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

        //override public void onTick(float delta)
        //{
        //    base.onTick(delta);

        //    if (null != this.mRender)
        //    {
        //        this.mRender.onTick(delta);
        //    }
        //}

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

        override public void show()
        {
            base.show();

            if (this.mWillVisible && this.mIsVisible)
            {
                if (null != this.mHud)
                {
                    this.mHud.show();
                }
            }
        }

        override public void hide()
        {
            base.hide();

            if (null != this.mHud)
            {
                this.mHud.hide();
            }
        }
    }
}