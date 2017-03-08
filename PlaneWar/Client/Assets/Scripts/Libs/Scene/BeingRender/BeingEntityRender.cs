using UnityEngine;

namespace SDK.Lib
{
    public class BeingEntityRender : EntityRenderBase
    {
        protected string mResPath;  // 资源目录
        protected AuxPrefabLoader mAuxPrefabLoader;
        //public UnityEngine.CharacterController characterController;
        protected GameObject mModel;    // Model 节点
        protected GameObject mModelRender;    // ModelRender 节点
        protected Transform mColliderTrans; // 碰撞转换
        protected AuxTextureLoader mAuxTextureLoader;

        /**
         * @brief 资源加载之类的基本操作写在这里
         */
        public BeingEntityRender(SceneEntityBase entity_)
            : base(entity_)
        {

        }

        public void setResPath(string path)
        {
            this.mResPath = path;
        }

        override public void onDestroy()
        {
            this.releaseRes();

            base.onDestroy();
        }

        // 释放资源
        protected void releaseRes()
        {
            if (null != this.mAuxPrefabLoader)
            {
                if (this.mIsUsePool)
                {
                    this.onRetPool();
                    this.mAuxPrefabLoader.deleteObj();
                }
                else
                {
                    this.mAuxPrefabLoader.dispose();
                }

                this.mAuxPrefabLoader = null;
            }

            if (null != this.mAuxTextureLoader)
            {
                if (this.mIsUsePool)
                {
                    this.mAuxTextureLoader.deleteObj();
                }
                else
                {
                    this.mAuxTextureLoader.dispose();
                }

                this.mAuxTextureLoader = null;
            }
        }

        override public void onEnterScreenRange()
        {
            // 进入显示的时候必然是空，这里再判断一次
            if (null == this.mAuxPrefabLoader)
            {
                this.load();
            }
        }

        override public void onLeaveScreenRange()
        {
            this.releaseRes();

            this.selfGo = null;
        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;

                    // 只有自己才是物理移动
                    if (MacroDef.PHYSIX_MOVE && 
                        (null != this.mRigidbody || null != this.mRigidbody2D) && EntityType.ePlayerMainChild == this.mEntity.getEntityType())
                    {
                        UtilApi.setRigidbodyPos(this.mRigidbody, this.mEntity.getPos());
                        UtilApi.setRigidbody2DPos(this.mRigidbody2D, this.mEntity.getPos());
                    }
                    else
                    {
                        UtilApi.setPos(this.mSelfGo.transform, this.mEntity.getPos());
                    }
                    // 内部 2D 物理组件总是会移动，因此重置一下
                    UtilApi.resetRST(this.mColliderTrans);
                }
                if (this.mIsRotDirty)
                {
                    this.mIsRotDirty = false;

                    UtilApi.setRot(this.mSelfGo.transform, this.mEntity.getRotate());
                }
                if (this.mIsScaleDirty)
                {
                    this.mIsScaleDirty = false;

                    //UtilApi.setScale(this.mSelfGo.transform, this.mEntity.getScale());
                }
            }
        }

        // 资源加载
        override public void load()
        {
            if(null == this.mAuxPrefabLoader)
            {
                //this.mAuxPrefabLoader = new AuxPrefabLoader("", true, false);
                this.mAuxPrefabLoader = AuxPrefabLoader.newObject(this.mResPath);
                this.mAuxPrefabLoader.setDestroySelf(true);
                this.mAuxPrefabLoader.setIsNeedInsPrefab(true);
                this.mAuxPrefabLoader.setIsInsNeedCoroutine(true);
                this.mAuxPrefabLoader.setIsInitOrientPos(true);
                this.mAuxPrefabLoader.setIsFakePos(true);
                this.mAuxPrefabLoader.setIsUsePool(true);
            }

            this.mAuxPrefabLoader.asyncLoad(this.mResPath, this.onResLoaded);
        }

        public void onResLoaded(IDispatchObject dispObj)
        {
            this.selfGo = this.mAuxPrefabLoader.getGameObject();
        }

        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            //characterController = ((UnityEngine.GameObject)this.gameObject()).GetComponent<UnityEngine.CharacterController>();
            //if (null == characterController)
            //{
            //    characterController = ((UnityEngine.GameObject)this.gameObject()).AddComponent<UnityEngine.CharacterController>();
            //}

            this.mModel = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.MODEL_NAME);
            this.mColliderTrans = UtilApi.TransFindChildTransByPObjAndPath(this.selfGo, UtilApi.COLLIDE_NAME);
        }

        override public Bounds getBounds()
        {
            if (null == this.mModel)
            {
                this.mModel = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.MODEL_NAME);
            }

            return UtilApi.getComByP<MeshFilter>(this.mModel).mesh.bounds;
        }

        // 获取 Model GameObject
        virtual public GameObject getModelObject()
        {
            if (null == this.mModel)
            {
                this.mModel = UtilApi.TransFindChildByPObjAndPath(this.selfGo, UtilApi.MODEL_NAME);
            }

            return this.mModel;
        }

        override public void setTexture(string path)
        {
            if (null == this.mAuxTextureLoader)
            {
                //this.mAuxTextureLoader = new AuxTextureLoader();
                this.mAuxTextureLoader = AuxTextureLoader.newObject();
            }

            this.mAuxTextureLoader.asyncLoad(path, onTextureLoaded);
        }

        override public void setTexTile(TileInfo tileInfo)
        {
            this.setModelTexTile();
        }

        public void onTextureLoaded(IDispatchObject dispObj)
        {
            if (this.mAuxTextureLoader.hasSuccessLoaded())
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
            if (null != this.mModelRender &&
                null != this.mAuxTextureLoader &&
                this.mAuxTextureLoader.hasSuccessLoaded())
            {
                UtilApi.setGameObjectMainTexture(this.mModelRender, this.mAuxTextureLoader.getTexture());
            }
        }

        protected void setModelTexTile()
        {
            if (null != this.mModelRender)
            {
                UtilApi.setGoTile(this.mModelRender, (this.mEntity as BeingEntity).getTileInfo());
            }
        }
    }
}