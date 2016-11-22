using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxStaticImageStaticGoImage : AuxWindow
    {
        protected Image mImage;

        public AuxStaticImageStaticGoImage(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.mImage = UtilApi.getComByP<Image>(pntNode, path);
        }

        public AuxStaticImageStaticGoImage(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        public void setSelfGo(GameObject selfNode)
        {
            this.mSelfGo = selfNode;
            this.mImage = UtilApi.getComByP<Image>(this.mSelfGo);
        }
    }
}