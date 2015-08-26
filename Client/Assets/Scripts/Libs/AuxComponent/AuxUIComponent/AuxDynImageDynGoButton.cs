using SDK.Lib;
using System;

namespace SDK.Lib
{
    public class AuxDynImageDynGoButton : AuxBasicButton
    {
        protected AuxDynImageDynGOImage m_auxDynImageDynGOImage;        // 这个图片和 Prefab

        public AuxDynImageDynGoButton()
        {
            m_auxDynImageDynGOImage = new AuxDynImageDynGOImage();
            m_auxDynImageDynGOImage.imageLoadedDisp.addEventHandle(updateBtnCom);
        }

        public string prefabPath
        {
            set
            {
                m_auxDynImageDynGOImage.prefabPath = value;
            }
        }

        public void setImageInfo(string atlasName, string imageName)
        {
            m_auxDynImageDynGOImage.setImageInfo(atlasName, imageName);
        }

        public void addImageLoadedHandle(Action<IDispatchObject> imageLoadedHandle)
        {
            m_auxDynImageDynGOImage.imageLoadedDisp.addEventHandle(imageLoadedHandle);
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

        override public void syncUpdateCom()
        {
            m_auxDynImageDynGOImage.syncUpdateCom();
            base.syncUpdateCom();
        }
    }
}