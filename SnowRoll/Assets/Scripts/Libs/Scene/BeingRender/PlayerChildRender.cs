namespace SDK.Lib
{
    public class PlayerChildRender : PlayerRender
    {
        //protected UnityEngine.SkinnedMeshRenderer mNativeRender;
        protected AuxTextureLoader mAuxTextureLoader;

        public PlayerChildRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        public override void dispose()
        {
            if(null != this.mAuxTextureLoader)
            {
                this.mAuxTextureLoader.dispose();
                this.mAuxTextureLoader = null;
            }

            base.dispose();
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

        override public void setTexture(string path)
        {
            if(null == this.mAuxTextureLoader)
            {
                this.mAuxTextureLoader = new AuxTextureLoader();
            }

            this.mAuxTextureLoader.asyncLoad(path, onTextureLoaded);
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            //if (null != this.mModelRender)
            //{
            //    this.mNativeRender = this.mModelRender.GetComponent<UnityEngine.SkinnedMeshRenderer>();
            //}

            this.setModelMat();

            AuxClipUserData auxData = UtilApi.AddComponent<AuxClipUserData>(this.mModelRender);
            auxData.setUserData(this.mEntity);

            this.setSelfName("" + mEntity.getThisId());
        }

        public void onTextureLoaded(IDispatchObject dispObj)
        {
            if(this.mAuxTextureLoader.hasSuccessLoaded())
            {
                this.setModelMat();
            }
            else
            {
                this.mAuxTextureLoader.dispose();
                this.mAuxTextureLoader = null;
            }
        }

        protected void setModelMat()
        {
            if (null != this.mModelRender && null != this.mAuxTextureLoader && this.mAuxTextureLoader.hasSuccessLoaded())
            {
                UtilApi.setGameObjectMainTexture(this.mModelRender, this.mAuxTextureLoader.getTexture());
            }
        }
    }
}