namespace SDK.Lib
{
    public class AuxScrollView : AuxWindow
    {
        protected string mResPath;
        protected AuxPrefabLoader mPrefabLoader;
        protected MList<AuxScrollViewItemBase> mItemList;
        protected UnityEngine.GameObject mInsGo;

        public AuxScrollView()
        {
            this.mResPath = "";

            this.mPrefabLoader = new AuxPrefabLoader("");
            this.mPrefabLoader.setIsNeedInsPrefab(false);
            this.mPrefabLoader.setIsInsNeedCoroutine(false);
            this.mPrefabLoader.setDestroySelf(false);
            this.mPrefabLoader.setIsInitOrientPos(false);

            this.mItemList = new MList<AuxScrollViewItemBase>();
        }

        override public void init()
        {

        }

        override public void dispose()
        {

        }

        public void setResPath(string path)
        {
            this.mResPath = path;
        }

        public void setSelfGo(UnityEngine.GameObject pntNode, string path)
        {
            this.selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        }

        virtual public AuxScrollViewItemBase createDataItem()
        {
            return null;
        }

        protected void loadPrefabItem()
        {
            if(!this.mPrefabLoader.hasSuccessLoaded())
            {
                this.mPrefabLoader.syncLoad(this.mResPath, this.onResLoaded);
            }
        }

        public void onResLoaded(IDispatchObject dispObj)
        {
            if (MacroDef.ENABLE_LOG)
            {
                Ctx.mInstance.mLogSys.log("loaded", LogTypeId.eLogCommon);
            }
        }

        virtual public UnityEngine.GameObject createResItem()
        {
            this.loadPrefabItem();
            this.mPrefabLoader.InstantiateObject(this.onInstantiateObjectFinish);
            return this.mInsGo;
        }

        public void onInstantiateObjectFinish(IDispatchObject dispObj)
        {
            this.mInsGo = this.mPrefabLoader.getGameObject();
            UtilApi.SetParent(this.mInsGo, this.selfGo, false);
        }

        public void addItem(AuxScrollViewItemBase item)
        {
            this.mItemList.Add(item);
        }

        public void updateItemList()
        {
            AuxScrollViewItemBase item = null;
            int idx = 0;
            int len = this.mItemList.Count();

            while(idx < len)
            {
                item = this.mItemList[idx];

                this.mInsGo = this.createResItem();
                item.setGo(this.mInsGo);

                ++idx;
            }
        }
    }
}