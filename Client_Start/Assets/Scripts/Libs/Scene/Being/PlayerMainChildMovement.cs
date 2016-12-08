namespace SDK.Lib
{
    public class PlayerMainChildMovement : PlayerChildMovement
    {
        public PlayerMainChildMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            //float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            //if (horizontal > 0.0f)
            //{
            //    this.rotateLeft();
            //}
            //else if (horizontal < 0.0f)
            //{
            //    this.rotateRight();
            //}
            //else
            //{
            //    if(!this.isMoveToDest())
            //    {
            //        this.stopMove();
            //    }
            //}

            //float vertical = UnityEngine.Input.GetAxis("Vertical");
            //if (vertical > 0.0f)
            //{
            //    //this.moveForward();
            //    this.moveAlong();
            //}
            //else if (vertical < 0.0f)
            //{
            //    //this.moveBack();
            //    this.moveAlong();
            //}
            //else
            //{
            //    this.movePause();
            //}
        }

        // Parent Player 方向改变事件处理器
        public void handleParentOrientChanged(IDispatchObject dispObj)
        {
            this.updateDir();
        }

        // Parent Player 位置改变事件处理器
        public void handleParentPosChanged(IDispatchObject dispObj)
        {
            this.moveAlong();
        }

        // 方向停止改变
        public void handleParentOrientStopChanged(IDispatchObject dispObj)
        {
            this.movePause();
        }

        // 位置停止改变
        public void handleParentPosStopChanged(IDispatchObject dispObj)
        {
            this.movePause();
        }

        protected void updateDir()
        {
            this.lookAt((this.mEntity as PlayerChild).mParentPlayer.mPlayerSplitMerge.getTargetPoint());
        }
    }
}