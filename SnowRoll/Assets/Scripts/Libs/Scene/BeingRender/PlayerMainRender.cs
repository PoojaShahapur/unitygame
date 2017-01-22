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

            //GameObject collide = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
            //AuxPlayerMainUserData auxData = UtilApi.AddComponent<AuxPlayerMainUserData>(collide);
            AuxPlayerMainUserData auxData = UtilApi.AddComponent<AuxPlayerMainUserData>(this.selfGo);
            auxData.setUserData(this.mEntity);

            //UtilApi.setLayer(this.selfGo, "PlayerMain");

            Ctx.mInstance.mCamSys.setCameraActor(this.mEntity);
        }

        override public void show()
        {
            if (!IsVisible())
            {
                UtilApi.SetActive(this.mSelfGo, true);
            }
        }

        override public void hide()
        {
            if (this.IsVisible())
            {
                UtilApi.SetActive(this.mSelfGo, false);
            }
        }
    }
}