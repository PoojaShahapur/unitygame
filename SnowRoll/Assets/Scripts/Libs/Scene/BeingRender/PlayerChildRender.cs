namespace SDK.Lib
{
    public class PlayerChildRender : PlayerRender
    {
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

            this.setModelMat();
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