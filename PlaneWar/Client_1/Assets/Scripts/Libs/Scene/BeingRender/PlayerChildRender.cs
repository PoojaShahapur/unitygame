namespace SDK.Lib
{
    public class PlayerChildRender : PlayerRender
    {
        //protected UnityEngine.SkinnedMeshRenderer mNativeRender;

        public PlayerChildRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        //override public void onTick(float delta)
        //{
        //    base.onTick(delta);

        //    if (null != this.mNativeRender)
        //    {
        //        if (this.mNativeRender.isVisible)
        //        {
        //            if (this.mEntity.isWillVisible())
        //            {
        //                this.mEntity.show();
        //            }
        //        }
        //        else
        //        {
        //            this.mEntity.hide();
        //        }
        //    }
        //}

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            this.mModelRender = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.MODEL_RENDER_NAME);

            //if (null != this.mModelRender)
            //{
            //    this.mNativeRender = this.mModelRender.GetComponent<UnityEngine.SkinnedMeshRenderer>();
            //}

            this.setModelMat();

            //AuxClipUserData auxData = UtilApi.AddComponent<AuxClipUserData>(this.mModelRender);
            //auxData.setUserData(this.mEntity);

            this.setSelfName("" + mEntity.getThisId());
        }
    }
}