namespace SDK.Lib
{
    public class AuxScrollView : AuxWindow
    {
        protected string mResPath;
        protected AuxPrefabLoader mPrefabLoader;
        protected MList<AuxScrollViewItemBase> mItemList;

        public AuxScrollView()
        {
            this.mResPath = "";

            this.mPrefabLoader = new AuxPrefabLoader("", false, false);
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
            Ctx.mInstance.mLogSys.log("loaded", LogTypeId.eLogCommon);
        }

        virtual public UnityEngine.GameObject createResItem()
        {
            UnityEngine.GameObject insGo = this.mPrefabLoader.InstantiateObject();
            return null;
        }

        public void onInstantiateObjectFinish(IDispatchObject dispObj)
        {
            UnityEngine.GameObject insGo = this.mPrefabLoader.getGameObject();
            UtilApi.SetParent(insGo, this.selfGo, false);
        }

        public void addItem(AuxScrollViewItemBase item)
        {
            this.mItemList.Add(item);
        }

        public void updateItemList()
        {
            UnityEngine.GameObject insGo = null;
            AuxScrollViewItemBase item = null;
            int idx = 0;
            int len = this.mItemList.Count();

            while(idx < len)
            {
                item = this.mItemList[idx];

                insGo = this.mPrefabLoader.InstantiateObject();
                item.setGo(insGo);

                UtilApi.SetParent(insGo, this.selfGo, false);

                ++idx;
            }
        }
    }
}