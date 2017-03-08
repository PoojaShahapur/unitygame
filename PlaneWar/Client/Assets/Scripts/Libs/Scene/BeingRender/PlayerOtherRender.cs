using UnityEngine;

namespace SDK.Lib
{
    public class PlayerOtherRender : PlayerRender
    {
        public PlayerOtherRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/PlayerOther.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            AuxPlayerOtherUserData auxData = UtilApi.AddComponent<AuxPlayerOtherUserData>(collide);
            auxData.setUserData(this.mEntity);
        }
    }
}