using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class AuxScrollbar : AuxWindow
    {
        protected Scrollbar mScrollbar;       // 滚动条

        public AuxScrollbar(GameObject pntNode, string path, BtnStyleID styleId = BtnStyleID.eBSID_None)
        {
            this.mSelfGo = UtilApi.TransFindChildByPObjAndPath(pntNode, path);
            this.mScrollbar = UtilApi.getComByP<Scrollbar>(pntNode, path);
        }

        public float value
        {
            get
            {
                return this.mScrollbar.value;
            }
            set
            {
                this.mScrollbar.value = 0;
            }
        }
    }
}