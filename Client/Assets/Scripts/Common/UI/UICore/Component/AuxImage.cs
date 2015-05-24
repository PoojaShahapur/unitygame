using UnityEngine;
using UnityEngine.UI;

namespace SDK.Common
{
    public class AuxImage : AuxComponent
    {
        protected Image m_image;

        public AuxImage(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            m_selfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            m_image = UtilApi.getComByP<Image>(pntNode, path);
        }

        public AuxImage(LabelStyleID styleId = LabelStyleID.eLSID_None)
        {
            
        }

        public void setSelfGo(GameObject selfNode)
        {
            m_selfGo = selfNode;
            m_image = UtilApi.getComByP<Image>(m_selfGo);
        }
    }
}