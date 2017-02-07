using UnityEngine;

namespace SDK.Lib
{
    public class AuxDynImageStaticGoButton : AuxButton
    {
        protected AuxDynImageStaticGOImage mAuxDynImageStaticGOImage;        // 这个图片和 Prefab

        public AuxDynImageStaticGoButton(GameObject pntNode = null, string path = "", BtnStyleID styleId = BtnStyleID.eBSID_None) :
            base(pntNode, path, styleId)
        {
            this.mAuxDynImageStaticGOImage = new AuxDynImageStaticGOImage();
            this.mAuxDynImageStaticGOImage.selfGo = this.selfGo;
        }

        override public void dispose()
        {
            this.mAuxDynImageStaticGOImage.dispose();

            base.dispose();
        }

        public AuxDynImageStaticGOImage auxDynImageStaticGOImage
        {
            get
            {
                return this.mAuxDynImageStaticGOImage;
            }
            set
            {
                this.mAuxDynImageStaticGOImage = value;
            }
        }
    }
}