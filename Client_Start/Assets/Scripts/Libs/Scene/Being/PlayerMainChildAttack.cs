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
            if (EntityType.ePlayerMainChild == bBeingEntity.getEntityType())
            {

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