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
            this.mResPath = "World/Model/PlayerMain.prefab";
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

        // 资源加载
        override public void load()
        {
            if (null == this.mAuxPrefabLoader)
            {
                //this.mAuxPrefabLoader = new AuxPrefabLoader("", true, false);
                this.mAuxPrefabLoader = AuxPrefabLoader.newObject(this.mResPath);
                this.mAuxPrefabLoader.setDestroySelf(true);
                this.mAuxPrefabLoader.setIsNeedInsPrefab(true);
                this.mAuxPrefabLoader.setIsInsNeedCoroutine(false);
                this.mAuxPrefabLoader.setIsInitOrientPos(true);
                this.mAuxPrefabLoader.setIsFakePos(true);
                this.mAuxPrefabLoader.setIsUsePool(true);
            }

            this.mAuxPrefabLoader.asyncLoad(this.mResPath, this.onResLoaded);
        }
    }
}