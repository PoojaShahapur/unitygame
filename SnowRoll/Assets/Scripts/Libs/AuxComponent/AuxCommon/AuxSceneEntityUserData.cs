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
            if (null != aUserData)
            {
                BeingEntity aBeingEntity = aUserData.getUserData() as BeingEntity;
                if (null != aBeingEntity)
                {
                    AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
                    if (null != bUserData)
                    {
                        BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                        if (null != bBeingEntity)
                        {
                            if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                            {
                                if ((aBeingEntity as BeingEntity).canInterActive(bBeingEntity))
                                {
                                    aBeingEntity.overlapToEnter(bBeingEntity, collisionInfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        void OnCollisionStay(UnityEngine.Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            if (null != aUserData)
            {
                BeingEntity aBeingEntity = aUserData.getUserData() as BeingEntity;
                if (null != aBeingEntity)
                {
                    AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
                    if (null != bUserData)
                    {
                        BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                        if (null != bBeingEntity)
                        {
                            if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                            {
                                if ((aBeingEntity as BeingEntity).canInterActive(bBeingEntity))
                                {
                                    aBeingEntity.overlapToStay(bBeingEntity, collisionInfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        void OnCollisionExit(UnityEngine.Collision collisionInfo)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            if (null != aUserData)
            {
                BeingEntity aBeingEntity = aUserData.getUserData() as BeingEntity;
                if (null != aBeingEntity)
                {
                    AuxSceneEntityUserData bUserData = collisionInfo.gameObject.GetComponent<AuxSceneEntityUserData>();
                    if (null != bUserData)
                    {
                        BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                        if (null != bBeingEntity)
                        {
                            if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                            {
                                if ((aBeingEntity as BeingEntity).canInterActive(bBeingEntity))
                                {
                                    aBeingEntity.overlapToExit(bBeingEntity, collisionInfo);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}