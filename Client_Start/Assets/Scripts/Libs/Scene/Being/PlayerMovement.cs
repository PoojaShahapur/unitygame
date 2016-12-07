namespace SDK.Lib
{
    public class PlayerMovement : BeingEntityMovement
    {
        public PlayerMovement(SceneEntityBase entity)
            : base(entity)
        {

        }

        override public void addLocalRotation(UnityEngine.Vector3 DeltaRotation)
        {
            base.addLocalRotation(DeltaRotation);

            if (null != (this.mEntity as Player).mPlayerSplitMerge)
            {
                (this.mEntity as Player).mPlayerSplitMerge.updateChildDestDir();
            }
        }

        override public void setDestRotate(UnityEngine.Vector3 destRotate)
        {
            base.setDestRotate(destRotate);

            if(null != (this.mEntity as Player).mPlayerSplitMerge)
            {
                (this.mEntity as Player).mPlayerSplitMerge.updateChildDestDir();
            }
        }
    }
}