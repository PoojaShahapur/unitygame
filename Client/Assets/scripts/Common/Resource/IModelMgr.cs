namespace SDK.Common
{
    public interface IModelMgr
    {
        void loadSkinInfo();
        string[] getBonesListByName(string name);
    }
}