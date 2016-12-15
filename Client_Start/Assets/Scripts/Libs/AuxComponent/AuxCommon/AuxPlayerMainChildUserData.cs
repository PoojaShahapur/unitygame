namespace SDK.Lib
{
    public class AuxPlayerMainChildUserData : AuxPlayerUserData
    {
        void OnTriggerEnter(UnityEngine.Collider collider)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collider.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                {
                    aBeingEntity.overlapToEnter(bBeingEntity, collider);
                }
            }
        }

        void OnCollisionStay(UnityEngine.Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                {
                    aBeingEntity.overlapToStay(bBeingEntity, collision);
                }
            }
        }

        void OnCollisionExit(UnityEngine.Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                if (!Ctx.mInstance.mFrameCollideMgr.isOrAddCollidedInCurFrame(aBeingEntity.getEntityUniqueId(), bBeingEntity.getEntityUniqueId()))
                {
                    aBeingEntity.overlapToExit(bBeingEntity, collision);
                }
            }
        }
    }
}