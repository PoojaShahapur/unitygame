using System.Collections.Generic;

namespace SDK.Lib
{
    public enum UICanvasID
    {
        eFirstCanvas,
        eSecondCanvas,
        eHudCanvas,

        eCanvas_Total,
    }

    /**
     * @brief 描述一个 Canvas
     */
    public class UICanvas
    {
        protected string mGoName;               // GameObject 的名字
        protected UICanvasID mCanvasID;         // CanvasID
        protected MList<UILayer> mLayerList;    // Canvas 中的 Layer
        protected UnityEngine.Canvas mCanvas;   // Canvas 组件

        public UICanvas(UICanvasID canvasID)
        {
            mCanvasID = canvasID;

            mLayerList = new MList<UILayer>();

            int idx = 0;
            for (idx = 0; idx < (int)UILayerId.eMaxLayer; ++idx)
            {
                mLayerList.Add(new UILayer((UILayerId)idx));
            }

            if (UICanvasID.eFirstCanvas == mCanvasID)
            {
                mLayerList[(int)UILayerId.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_FirstCanvas;
                mLayerList[(int)UILayerId.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_FirstCanvas;
                mLayerList[(int)UILayerId.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_FirstCanvas;
                mLayerList[(int)UILayerId.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_FirstCanvas;
                mLayerList[(int)UILayerId.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_FirstCanvas;
                mLayerList[(int)UILayerId.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_FirstCanvas;
            }
            else if(UICanvasID.eSecondCanvas == mCanvasID)
            {
                mLayerList[(int)UILayerId.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_SecondCanvas;
                mLayerList[(int)UILayerId.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_SecondCanvas;
                mLayerList[(int)UILayerId.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_SecondCanvas;
                mLayerList[(int)UILayerId.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_SecondCanvas;
                mLayerList[(int)UILayerId.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_SecondCanvas;
                mLayerList[(int)UILayerId.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_SecondCanvas;
            }
            else if (UICanvasID.eHudCanvas == mCanvasID)
            {
                mLayerList[(int)UILayerId.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_HudCanvas;
                mLayerList[(int)UILayerId.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_HudCanvas;
                mLayerList[(int)UILayerId.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_HudCanvas;
                mLayerList[(int)UILayerId.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_HudCanvas;
                mLayerList[(int)UILayerId.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_HudCanvas;
                mLayerList[(int)UILayerId.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_HudCanvas;
            }
        }

        public string goName
        {
            set
            {
                mGoName = value;
            }
        }

        public MList<UILayer> layerList
        {
            get
            {
                return mLayerList;
            }
        }

        public void findCanvasGO()
        {
            this.mCanvas = UtilApi.TransFindChildByPObjAndPath(Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_Root], mGoName).GetComponent<UnityEngine.Canvas>();

            int idx = 0;
            for (idx = 0; idx < (int)UILayerId.eMaxLayer; ++idx)
            {
                mLayerList[idx].findLayerGOAndTrans();
            }
        }

        public UnityEngine.Canvas getCanvas()
        {
            return this.mCanvas;
        }
    }
}