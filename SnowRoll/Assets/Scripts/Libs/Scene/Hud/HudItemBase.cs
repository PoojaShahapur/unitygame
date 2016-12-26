namespace SDK.Lib
{
    /**
     * @brief HUD Item
     */
    public class HudItemBase : AuxComponent
    {
        protected BeingEntity mEntity;
        protected UnityEngine.Vector3 mPos;

        protected string mResPath;  // 资源目录
        protected AuxPrefabLoader mAuxPrefabLoader;

        protected AuxLabel mName;

        public HudItemBase()
        {
            this.mName = new AuxLabel();
        }

        public override void init()
        {
            this.pntGo = Ctx.mInstance.mUiMgr.mHudParent;

            base.init();
            this.load();
        }

        public override void dispose()
        {
            base.dispose();
        }

        override public void onDestroy()
        {
            if (null != this.mAuxPrefabLoader)
            {
                mAuxPrefabLoader.dispose();
            }

            base.onDestroy();
        }

        // 资源加载
        virtual public void load()
        {
            if (null == this.mAuxPrefabLoader)
            {
                this.mAuxPrefabLoader = new AuxPrefabLoader("", true, false);
                this.mAuxPrefabLoader.setDestroySelf(false); // 自己释放 GmmeObject
                this.mAuxPrefabLoader.setIsInitOrientPos(true);
                this.mAuxPrefabLoader.setIsFakePos(true);
            }

            this.mAuxPrefabLoader.asyncLoad(mResPath, onResLoaded);
        }

        public void onResLoaded(IDispatchObject dispObj)
        {
            this.selfGo = this.mAuxPrefabLoader.getGameObject();
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            this.mName.setSelfGo(this.selfGo, "TextName");
        }

        public void setBeing(BeingEntity being)
        {
            this.mEntity = being;
        }

        // 位置发生改变
        public void onPosChanged()
        {
            if (null != Ctx.mInstance.mCamSys.mMainCamera && null != Ctx.mInstance.mCamSys.mUguiCam)
            {
                this.mPos = UtilApi.convWorldToUIPos(Ctx.mInstance.mUiMgr.mHudCanvas, Ctx.mInstance.mCamSys.mMainCamera, this.mEntity.getPos(), Ctx.mInstance.mCamSys.mUguiCam);

                this.setPos(this.mPos);
            }
            else
            {
                this.mIsPosDirty = true;
            }
        }

        virtual public void onNameChanged()
        {
            this.mName.setText(this.mEntity.getName());
        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;
                    UtilApi.setPos(this.mSelfGo.transform, this.mPos);
                }
            }
        }
    }
}