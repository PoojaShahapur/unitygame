namespace SDK.Lib
{
    public class ModelMgr : ResMgrBase
    {
        // ¼ÇÂ¼ÃÉÆ¤ÐÅÏ¢
        //protected Dictionary<string, Dictionary<string, string[]> > m_skinDic = new Dictionary<string,Dictionary<string,string[]>>();

        public ModelMgr()
        {

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
        //    LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
        //    param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathBeingPath] + "BonesList";
        //    param.m_loadEventHandle = onSkinLoadEventHandle;
        //    Ctx.m_instance.m_resLoadMgr.loadBundle(param);
        //    Ctx.m_instance.m_poolSys.deleteObj(param);
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