namespace SDK.Lib
{
    public class AuxDynTexDynGOButton : AuxBasicButton
    {
        protected AuxDynTexDynGOImage mAuxDynTexDynGOImage;        // 这个图片和 Prefab

        public AuxDynTexDynGOButton()
            : base(null, "")
        {
            this.mAuxDynTexDynGOImage = new AuxDynTexDynGOImage();
            this.mAuxDynTexDynGOImage.texLoadedDisp.addEventHandle(null, updateBtnCom);
        }

        public string prefabPath
        {
            set
            {
                this.mAuxDynTexDynGOImage.prefabPath = value;
            }
        }

        public string texPath
        {
            set
            {
                this.mAuxDynTexDynGOImage.texPath = value;
            }
        }

        public EventDispatch imageLoadedDisp
        {
            get
            {
                return this.mAuxDynTexDynGOImage.texLoadedDisp;
            }
        }

        override protected void updateBtnCom(IDispatchObject dispObj)
        {
            bool bGoChange = false;
            if (this.mSelfGo != this.mAuxDynTexDynGOImage.selfGo)
            {
                this.mSelfGo = this.mAuxDynTexDynGOImage.selfGo;
                bGoChange = true;
            }
            if (this.mPntGo == this.mAuxDynTexDynGOImage.pntGo)
            {
                this.mPntGo = this.mAuxDynTexDynGOImage.pntGo;
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
            this.mAuxDynTexDynGOImage.texLoadedDisp.removeEventHandle(null, updateBtnCom);
            this.mAuxDynTexDynGOImage.dispose();
        }

        override public void syncUpdateCom()
        {
            this.mAuxDynTexDynGOImage.syncUpdateCom();
            base.syncUpdateCom();
        }
    }
}