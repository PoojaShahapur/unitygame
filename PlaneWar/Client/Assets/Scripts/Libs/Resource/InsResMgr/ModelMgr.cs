namespace SDK.Lib
{
    public class ModelMgr : InsResMgrBase
    {
        // ��¼��Ƥ��Ϣ
        //protected Dictionary<string, Dictionary<string, string[]> > m_skinDic = new Dictionary<string,Dictionary<string,string[]>>();

        public ModelMgr()
        {

        }

        public ModelRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndSyncLoad<ModelRes>(path, handle, progressHandle);
        }

        public ModelRes getAndSyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndSyncLoad<ModelRes>(path, luaTable, luaFunction, progressLuaFunction);
        }

        public ModelRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle, MAction<IDispatchObject> progressHandle)
        {
            return getAndAsyncLoad<ModelRes>(path, handle, progressHandle);
        }

        public ModelRes getAndAsyncLoadRes(string path, LuaInterface.LuaTable luaTable = null, LuaInterface.LuaFunction luaFunction = null, LuaInterface.LuaFunction progressLuaFunction = null)
        {
            return getAndAsyncLoad<ModelRes>(path, luaTable, luaFunction, progressLuaFunction, true);
        }

        //public string[] getBonesListByName(string name)
        //{
        //    if(m_skinDic["DefaultAvata"].ContainsKey(name))
        //    {
        //        return m_skinDic["DefaultAvata"][name];
        //    }

        //    return null;
        //}

        //public void loadSkinInfo()
        //{
        //    LoadParam param = Ctx.mInstance.mPoolSys.newObject<LoadParam>();
        //    param.mPath = Ctx.mInstance.mCfg.m_pathLst[(int)ResPathType.ePathBeingPath] + "BonesList";
        //    param.mLoadEventHandle = onSkinLoadEventHandle;
        //    Ctx.mInstance.mResLoadMgr.loadBundle(param);
        //    Ctx.mInstance.mPoolSys.deleteObj(param);
        //}

        //public void onSkinLoadEventHandle(IDispatchObject dispObj)
        //{
        //    ResItem res = dispObj as ResItem;
        //    string text = res.getText("BoneList");
        //    SecurityParser xmlDoc = new SecurityParser();
        //    xmlDoc.LoadXml(text);

        //    SecurityElement rootNode = xmlDoc.ToXml();
        //    ArrayList itemMeshList = rootNode.Children;
        //    SecurityElement itemMesh;

        //    ArrayList itemSubMeshList;
        //    SecurityElement itemSubMesh;
        //    string meshName = "";
        //    string subMeshName = "";
        //    string bonesList = "";

        //    foreach (SecurityElement itemNode1f in itemMeshList)
        //    {
        //        itemMesh = itemNode1f;
        //        meshName = UtilXml.getXmlAttrStr(itemMesh, "name");
        //        m_skinDic[meshName] = new Dictionary<string, string[]>();

        //        itemSubMeshList = itemMesh.Children;
        //        foreach (SecurityElement itemNode2f in itemSubMeshList)
        //        {
        //            itemSubMesh = itemNode2f;
        //            subMeshName = UtilXml.getXmlAttrStr(itemSubMesh, "name");
        //            bonesList = UtilXml.getXmlAttrStr(itemSubMesh, "bonelist");
        //            m_skinDic[meshName][subMeshName] = bonesList.Split(',');
        //        }
        //    }
        //}
    }
}