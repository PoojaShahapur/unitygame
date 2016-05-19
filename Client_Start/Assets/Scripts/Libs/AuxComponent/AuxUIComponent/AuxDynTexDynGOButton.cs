namespace SDK.Lib
{
    public class AuxDynTexDynGOButton : AuxBasicButton
    {
        protected AuxDynTexDynGOImage m_auxDynTexDynGOImage;        // 这个图片和 Prefab

        public AuxDynTexDynGOButton()
        {
            m_auxDynTexDynGOImage = new AuxDynTexDynGOImage();
            m_auxDynTexDynGOImage.texLoadedDisp.addEventHandle(null, updateBtnCom);
        }

        public string prefabPath
        {
            set
            {
                m_auxDynTexDynGOImage.prefabPath = value;
            }
        }

        public string texPath
        {
            set
            {
                m_auxDynTexDynGOImage.texPath = value;
            }
        }

        public EventDispatch imageLoadedDisp
        {
            get
            {
                return m_auxDynTexDynGOImage.texLoadedDisp;
            }
        }

        override protected void updateBtnCom(IDispatchObject dispObj)
        {
            bool bGoChange = false;
            if (m_selfGo != m_auxDynTexDynGOImage.selfGo)
            {
                m_selfGo = m_auxDynTexDynGOImage.selfGo;
                bGoChange = true;
            }
            if (m_pntGo == m_auxDynTexDynGOImage.pntGo)
            {
                m_pntGo = m_auxDynTexDynGOImage.pntGo;
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
            m_auxDynTexDynGOImage.texLoadedDisp.removeEventHandle(null, updateBtnCom);
            m_auxDynTexDynGOImage.dispose();
        }

        override public void syncUpdateCom()
        {
            m_auxDynTexDynGOImage.syncUpdateCom();
            base.syncUpdateCom();
        }
    }
}