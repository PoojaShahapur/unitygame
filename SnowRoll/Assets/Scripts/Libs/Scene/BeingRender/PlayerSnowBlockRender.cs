using UnityEngine;

namespace SDK.Lib
{
    public class PlayerSnowBlockRender : BeingEntityRender
    {
        public PlayerSnowBlockRender(SceneEntityBase entity_)
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
            AuxPlayerSnowBlockUserData auxData = UtilApi.AddComponent<AuxPlayerSnowBlockUserData>(collide);

            auxData.setUserData(this.mEntity);
        }
    }
}
