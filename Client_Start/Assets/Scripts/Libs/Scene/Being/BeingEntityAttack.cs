namespace SDK.Lib
{
    /**
     * @brief 攻击控制器，控制攻击逻辑
     */
    public class BeingEntityAttack
    {
        protected BeingEntity mEntity;

        public BeingEntityAttack(BeingEntity entity)
        {
            mEntity = entity;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        virtual public void onTick(float delta)
        {

        }

        virtual public void overlapToEnter(BeingEntity bBeingEntity, UnityEngine.Collider collider)
        {

        }

        virtual public void overlapToStay(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {

        }

        virtual public void overlapToExit(BeingEntity bBeingEntity, UnityEngine.Collision collision)
        {

        }
    }
}