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
            if(null != this.mAuxPrefabLoader)
            {
                //this.mAuxPrefabLoader.dispose();
                this.mAuxPrefabLoader.deleteObj();
                this.mAuxPrefabLoader = null;
            }

            if (null != this.mAuxTextureLoader)
            {
                //this.mAuxTextureLoader.dispose();
                this.mAuxTextureLoader.deleteObj();
                this.mAuxTextureLoader = null;
            }

            base.onDestroy();
        }

        // 资源加载
        override public void load()
        {
            if(null == this.mAuxPrefabLoader)
            {
                //this.mAuxPrefabLoader = new AuxPrefabLoader("", true, false);
                this.mAuxPrefabLoader = AuxPrefabLoader.newObject(this.mResPath);
                //this.mAuxPrefabLoader.setDestroySelf(false); // 自己释放 GmmeObject
                this.mAuxPrefabLoader.setIsNeedInsPrefab(true);
                this.mAuxPrefabLoader.setIsInsNeedCoroutine(true);
                this.mAuxPrefabLoader.setDestroySelf(true); // 自己释放 GmmeObject
                this.mAuxPrefabLoader.setIsInitOrientPos(true);
                this.mAuxPrefabLoader.setIsFakePos(true);
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