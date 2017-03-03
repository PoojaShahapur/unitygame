namespace SDK.Lib
{
    public class PlayerChild : Player
    {
        public Player mParentPlayer; // Parent
        private UnityEngine.Vector3 preSendPosition; //上一次发送时的位置
        private UnityEngine.Vector3 preSendOrient; //上一次发送时的位置

        public PlayerChild(Player parentPlayer)
        {
            this.mParentPlayer = parentPlayer;

            //this.mAnimatorControl = new BeingAnimatorControl(this);
            this.mAnimFSM = new AnimFSM(this);
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

        public void setPreSendOrient(UnityEngine.Vector3 orient)
        {
            this.preSendOrient = orient;
        }

        public UnityEngine.Vector3 getPreSendOrient()
        {
            return this.preSendOrient;
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

        protected override void onPostInit()
        {
            base.onPostInit();

            //this.mAnimFSM.UpdateFSM();
            this.mHud = Ctx.mInstance.mHudSystem.createHud(this);
            //this.setTexture(Ctx.mInstance.mSnowBallCfg.getRandomBallTex());
            //this.setTexTile(Ctx.mInstance.mSnowBallCfg.getRandomBallTexTile());
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
            return base.getHudPos();
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

        override public float getBallWorldRadius()
        {
            return this.mBallRadius * Ctx.mInstance.mSnowBallCfg.mBallCollideRadius;
        }

        override public float getEmitSnowWorldSize()
        {
            return UtilMath.getRadiusByMass(Ctx.mInstance.mSnowBallCfg.mEmitSnowMass) * Ctx.mInstance.mSnowBallCfg.mShitCollideRadius;        // 需要转换成半径
        }

        override public float getSplitWorldRadius()
        {
            return this.getSplitRadius() * Ctx.mInstance.mSnowBallCfg.mBallCollideRadius;
        }
    }
}