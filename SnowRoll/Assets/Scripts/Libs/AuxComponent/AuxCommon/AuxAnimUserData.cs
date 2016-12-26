namespace SDK.Lib
{
    /**
     * @breif 动画事件
     */
    public class AuxAnimUserData : AuxUserData
    {
        protected BeingEntity mEntity;

        public void setBeingEntity(BeingEntity entity)
        {
            this.mEntity = entity;
        }

        /**
         * @breif 事件回调函数
         */
        public void AnimEventCall(string funcName)
        {
            // 执行事件
            if("eat" == funcName)
            {
                this.mEntity.setBeingState(BeingState.eBSIdle);
            }
            else if ("split" == funcName)
            {
                this.mEntity.setBeingState(BeingState.eBSIdle);
            }
        }
    }
}