namespace SDK.Lib
{
    /**
     * @brief HUD Item
     */
    public class HudItemBase : AuxComponent
    {
        protected BeingEntity mEntity;
        protected UnityEngine.Vector3 mPos;
        protected UnityEngine.Quaternion mRotate;

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
                this.mAuxPrefabLoader.dispose();
                this.mAuxPrefabLoader = null;
            }

            Ctx.mInstance.mHudSystem.removeHud(this);

            base.onDestroy();
        }

        // 资源加载
        virtual public void load()
        {
            if (null == this.mAuxPrefabLoader)
            {
                this.mAuxPrefabLoader = new AuxPrefabLoader("");
                this.mAuxPrefabLoader.setIsNeedInsPrefab(true);
                this.mAuxPrefabLoader.setIsInsNeedCoroutine(false);
                this.mAuxPrefabLoader.setDestroySelf(true); // 自己释放 GmmeObject
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

            if (this.mEntity.isEnableVisible() && this.mEntity.IsVisible())
            {
                this.show();
            }
            else if (!this.mEntity.IsVisible())
            {
                this.hide();
            }
        }

        public void setBeing(BeingEntity being)
        {
            this.mEntity = being;
        }

        // 位置发生改变
        public void onPosChanged()
        {
            //if (null != Ctx.mInstance.mCamSys.mMainCamera && null != Ctx.mInstance.mCamSys.mUguiCam)
            if (Ctx.mInstance.mCamSys.mMainCamera && Ctx.mInstance.mCamSys.mUguiCam)
            {
                // 坐标位置转换太耗时，不再转换位置坐标，直接在世界空间调整位置
                this.mPos = UtilApi.convWorldToUIPos(Ctx.mInstance.mUiMgr.mHudCanvas, Ctx.mInstance.mCamSys.mMainCamera, this.mEntity.getHudPos(), Ctx.mInstance.mCamSys.mUguiCam);
                //this.mPos = this.mEntity.getHudPos();
                //this.mRotate = UtilMath.getRotateByStartAndEndPoint(this.mPos, Ctx.mInstance.mCamSys.mMainCamera.transform.localPosition);

                this.setPos(this.mPos);
                //this.setRotate(this.mRotate);
            }
            else
            {
                this.mIsPosDirty = true;
            }
        }

        virtual public void onNameChanged()
        {
            this.mName.setText(this.mEntity.getName());
            this.mName.changeSize(UnityEngine.Mathf.Pow(this.mEntity.getBallRadius(), 1/3.0f));
        }

        override public void updateLocalTransform()
        {
            // 执行太频繁，这个判断会很卡
            //if (null != this.mSelfGo)
            //if (this.mSelfGo.Equals(null))
            if(this.mSelfGo)
            {
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;
                    UtilApi.setPos(this.mSelfGo.transform, this.mPos);
                }
                //if (this.mIsRotDirty)
                //{
                //    this.mIsRotDirty = false;
                //    UtilApi.setRot(this.mSelfGo.transform, this.mRotate);
                //}
            }
        }
    }
}