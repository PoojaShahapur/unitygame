namespace SDK.Lib
{
    public class AuxClipUserData : AuxSceneEntityUserData
    {
        void OnBecameVisible()
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            if (null != aUserData)
            {
                BeingEntity aBeingEntity = aUserData.getUserData() as BeingEntity;
                if (null != aBeingEntity)
                {
                    //aBeingEntity.show();
                }
            }
        }

        void OnBecameInvisible()
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            if (null != aUserData)
            {
                BeingEntity aBeingEntity = aUserData.getUserData() as BeingEntity;
                if (null != aBeingEntity)
                {
                    //aBeingEntity.hide();
                }
            }
        }
    }
}