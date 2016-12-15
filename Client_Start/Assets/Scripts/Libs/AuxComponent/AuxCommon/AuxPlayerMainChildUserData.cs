namespace SDK.Lib
{
    public class AuxPlayerMainChildUserData : AuxPlayerUserData
    {
        void OnCollisionStay(UnityEngine.Collision collision)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            PlayerMainChild aBeingEntity = aUserData.getUserData() as PlayerMainChild;

            AuxSceneEntityUserData bUserData = collision.gameObject.GetComponent<AuxSceneEntityUserData>();
            if (null != bUserData)
            {
                BeingEntity bBeingEntity = bUserData.getUserData() as BeingEntity;
                aBeingEntity.overlapToStay(bBeingEntity, collision);
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
                aBeingEntity.overlapToExit(bBeingEntity, collision);
            }
        }
    }
}