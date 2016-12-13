using UnityEngine;

namespace SDK.Lib
{
    public class PlayerMainRender : PlayerRender
    {
        public PlayerMainRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        override public void onInit()
        {
            this.mResPath = "World/Model/PlayerTest.prefab";
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            AuxPlayerMainUserData auxData = UtilApi.AddComponent<AuxPlayerMainUserData>(collide);
            auxData.setUserData(this.mEntity);

            Ctx.mInstance.mCamSys.setCameraActor(this.mEntity);
        }
    }
}