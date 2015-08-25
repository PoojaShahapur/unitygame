using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxStaticImage : AuxComponent
    {
        protected Image m_image;

        public AuxStaticImage(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_image = UtilApi.getComByP<Image>(pntNode, path);
        }

        public AuxStaticImage(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        public void setSelfGo(GameObject selfNode)
        {
            m_selfGo = selfNode;
            m_image = UtilApi.getComByP<Image>(m_selfGo);
        }
    }
}