namespace SDK.Lib
{
    /**
     * @brief SceneEntity 用户数据
     */
    public class AuxSceneEntityUserData : AuxUserData
    {
        private SceneEntityBase mUserData;

        public SceneEntityBase getUserData()
        {
            return mUserData;
        }

        public void setUserData(SceneEntityBase value)
        {
            mUserData = value;
        }

        void OnCollisionEnter(UnityEngine.Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                {
                    aBeingEntity.overlapToEnter(bBeingEntity, collisionInfo);
                }
            }
        }

        void OnCollisionStay(UnityEngine.Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                {
                    aBeingEntity.overlapToStay(bBeingEntity, collisionInfo);
                }
            }
        }

        void OnCollisionExit(UnityEngine.Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                {
                    aBeingEntity.overlapToExit(bBeingEntity, collisionInfo);
                }
            }
        }
    }
}