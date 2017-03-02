using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxStaticImage : AuxWindow
    {
        protected Image mImage;

        public AuxStaticImage(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            mImage = UtilApi.getComByP<Image>(pntNode, path);
        }

        public AuxStaticImage(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        public void setSelfGo(GameObject selfNode)
        {
            this.mSelfGo = selfNode;
            this.mImage = UtilApi.getComByP<Image>(this.mSelfGo);
        }
    }
}