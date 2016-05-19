using UnityEngine;

namespace SDK.Lib
{
    public class AuxDynImageStaticGoButton : AuxBasicButton
    {
        protected AuxDynImageStaticGOImage m_auxDynImageStaticGOImage;        // 这个图片和 Prefab

        public AuxDynImageStaticGoButton(GameObject pntNode = null, string path = "", BtnStyleID styleId = BtnStyleID.eBSID_None) :
            base(pntNode, path, styleId)
        {
            m_auxDynImageStaticGOImage = new AuxDynImageStaticGOImage();
            m_auxDynImageStaticGOImage.selfGo = this.selfGo;
        }

        override public void dispose()
        {
            m_auxDynImageStaticGOImage.dispose();
            base.dispose();
        }

        public AuxDynImageStaticGOImage auxDynImageStaticGOImage
        {
            get
            {
                return m_auxDynImageStaticGOImage;
            }
            set
            {
                m_auxDynImageStaticGOImage = value;
            }
        }
    }
}