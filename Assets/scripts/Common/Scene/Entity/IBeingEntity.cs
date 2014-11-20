namespace SDK.Common
{
    public interface IBeingEntity
    {
        void setSkeleton(string name);
        void setPartModel(int modelDef, string assetBundleName, string partName);
        void addAiByID(string id);
    }
}
