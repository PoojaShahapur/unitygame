using SDK.Lib;
namespace SDK.Common
{
    public class AuxDynImageDynGoButton : AuxBasicButton
    {
        protected AuxDynImageDynGOImage m_auxDynImageDynGOImage;        // 这个图片和 Prefab

        public AuxDynImageDynGoButton()
        {
            m_auxDynImageDynGOImage = new AuxDynImageDynGOImage();
            m_auxDynImageDynGOImage.imageLoadedDisp.addEventHandle(updateBtnCom);
        }

        public AuxDynImageDynGOImage auxDynImageDynGOImage
        {
            get
            {
                return m_auxDynImageDynGOImage;
            }
            set
            {
                m_auxDynImageDynGOImage = value;
            }
        }

        override protected void updateBtnCom(IDispatchObject dispObj)
        {
            bool bGoChange = false;
            if (m_selfGo != m_auxDynImageDynGOImage.selfGo)
            {
                m_selfGo = m_auxDynImageDynGOImage.selfGo;
                bGoChange = true;
            }
            if (m_pntGo == m_auxDynImageDynGOImage.pntGo)
            {
                m_pntGo = m_auxDynImageDynGOImage.pntGo;
                bGoChange = true;
            }
            if (bGoChange)
            {
                base.updateBtnCom(dispObj);
            }
        }

        public override void dispose()
        {
            base.dispose();
            m_auxDynImageDynGOImage.imageLoadedDisp.removeEventHandle(updateBtnCom);
            m_auxDynImageDynGOImage.dispose();
        }
    }
}