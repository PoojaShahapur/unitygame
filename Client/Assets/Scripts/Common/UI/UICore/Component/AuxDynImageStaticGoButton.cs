using UnityEngine;

namespace SDK.Common
{
    public class AuxDynImageStaticGoButton : AuxBasicButton
    {
        protected AuxDynImageStaticGOImage m_auxDynImageStaticGOImage;        // 这个图片和 Prefab

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