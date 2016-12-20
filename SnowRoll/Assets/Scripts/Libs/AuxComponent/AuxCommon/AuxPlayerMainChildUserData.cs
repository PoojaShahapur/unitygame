namespace SDK.Lib
{
    public class AuxPlayerMainChildUserData : AuxPlayerUserData
    {
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