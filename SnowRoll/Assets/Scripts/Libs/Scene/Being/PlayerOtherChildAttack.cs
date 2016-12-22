namespace SDK.Lib
{
    public class PlayerOtherChildAttack : PlayerChildAttack
    {
        public PlayerOtherChildAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if(EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToEnter(this.mEntity, collisionInfo);
            }
        }

        override public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToStay(this.mEntity, collisionInfo);
            }
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collisionInfo)
        {
            if (UtilApi.isInFakePos(this.mEntity.getPos()))
            {
                return;
            }

            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {
                bBeingEntity.overlapToExit(this.mEntity, collisionInfo);
            }
        }
    }
}