namespace SDK.Common
{
    public interface IBeingEntity
    {
        void setSkeleton(string name);
        void setPartModel(PlayerModelDef modelDef, string assetBundleName, string partName);
    }
}
