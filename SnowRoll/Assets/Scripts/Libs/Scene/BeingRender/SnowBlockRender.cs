using UnityEngine;

namespace SDK.Lib
{
    public class SnowBlockRender : BeingEntityRender
    {
        public SnowBlockRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/SnowBlockTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            AuxSnowBlockUserData auxData = UtilApi.AddComponent<AuxSnowBlockUserData>(collide);

            auxData.setUserData(this.mEntity);
        }
    }
}
