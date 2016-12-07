namespace SDK.Lib
{
    public class PlayerMainMovement : PlayerMovement
    {
        public PlayerMainMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void onTick(float delta)
        {
            base.onTick(delta);

            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            if (horizontal > 0.0f)
            {
                this.rotateLeft();
            }
            else if (horizontal < 0.0f)
            {
                this.rotateRight();
            }
            else
            {
                if(!this.isMoveToDest())
                {
                    this.stopMove();
                }
            }

            float vertical = UnityEngine.Input.GetAxis("Vertical");
            if (vertical > 0.0f)
            {
                this.moveForward();
            }
            else if(vertical < 0.0f)
            {
                this.moveBack();
            }
            else
            {
                if (!this.isRotateToDest())
                {
                    this.stopMove();
                }
            }
        }
    }
}