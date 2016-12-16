namespace SDK.Lib
{
    public class PlayerMainChildAttack : BeingEntityAttack
    {
        public PlayerMainChildAttack(BeingEntity entity)
            : base(entity)
        {

        }

        override public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {
            // 如果和 PlayerMainChild 碰撞
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType() ||
                EntityType.ePlayerMain == bBeingEntity.getEntityType())
            {
                if (this.mEntity.canReduceSpeed() || bBeingEntity.canReduceSpeed())
                {
                    if (UtilMath.isBehindCollidePoint(this.mEntity.getPos(), this.mEntity.getForward(), collision))
                    {
                        // 需要减小速度
                        this.mEntity.setMoveSpeed(bBeingEntity.getMoveSpeed());
                    }
                    else
                    {
                        bBeingEntity.setMoveSpeed(this.mEntity.getMoveSpeed());
                    }
                }
            }
            else if(EntityType.ePlayerOtherChild == bBeingEntity.getEntityType())
            {
                // 如果碰撞 PlayerOtherChild
            }
        }

        override public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {
            // 如果和 PlayerMainChild 碰撞
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {

            }
        }
    }
}