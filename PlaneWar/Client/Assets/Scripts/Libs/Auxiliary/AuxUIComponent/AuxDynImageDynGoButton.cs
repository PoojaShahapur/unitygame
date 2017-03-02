using System;

namespace SDK.Lib
{
    public class AuxDynImageDynGoButton : AuxButton
    {
        protected AuxDynImageDynGOImage mAuxDynImageDynGOImage;        // 这个图片和 Prefab

        public AuxDynImageDynGoButton()
            :base(null, "")
        {
            this.mAuxDynImageDynGOImage = new AuxDynImageDynGOImage();
            this.mAuxDynImageDynGOImage.imageLoadedDisp.addEventHandle(null, updateBtnCom);
        }

        public string prefabPath
        {
            set
            {
                this.mAuxDynImageDynGOImage.prefabPath = value;
            }
        }

        public void setImageInfo(string atlasName, string imageName)
        {
            this.mAuxDynImageDynGOImage.setImageInfo(atlasName, imageName);
        }

        public void addImageLoadedHandle(MAction<IDispatchObject> imageLoadedHandle)
        {
            this.mAuxDynImageDynGOImage.imageLoadedDisp.addEventHandle(null, imageLoadedHandle);
        }

        override protected void updateBtnCom(IDispatchObject dispObj)
        {
            bool bGoChange = false;
            if (this.mSelfGo != this.mAuxDynImageDynGOImage.selfGo)
            {
                this.mSelfGo = this.mAuxDynImageDynGOImage.selfGo;
                bGoChange = true;
            }
            if (this.mPntGo == this.mAuxDynImageDynGOImage.pntGo)
            {
                this.mPntGo = this.mAuxDynImageDynGOImage.pntGo;
                bGoChange = true;
            }
            if (bGoChange)
            {
                base.updateBtnCom(dispObj);
            }
        }

        public override void dispose()
        {
            this.mAuxDynImageDynGOImage.imageLoadedDisp.removeEventHandle(null, updateBtnCom);
            this.mAuxDynImageDynGOImage.dispose();

            base.dispose();
        }

        override public void syncUpdateCom()
        {
            this.mAuxDynImageDynGOImage.syncUpdateCom();
            base.syncUpdateCom();
        }
    }
}