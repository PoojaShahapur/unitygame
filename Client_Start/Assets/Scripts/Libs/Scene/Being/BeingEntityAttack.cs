﻿namespace SDK.Lib
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

        virtual public void onTick(float delta)
        {

        }
    }
}