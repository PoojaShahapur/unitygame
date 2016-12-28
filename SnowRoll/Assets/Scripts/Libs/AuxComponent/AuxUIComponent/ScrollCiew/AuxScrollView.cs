namespace SDK.Lib
{
    public class AuxScrollView : AuxWindow
    {


        public void setSelfGo(UnityEngine.GameObject pntNode, string path)
        {
            this.selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        }
    }
}