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
        protected string mGoName;              // GameObject 的名字
        protected UICanvasID mCanvasID;        // CanvasID
        protected MList<UILayer> mLayerList;     // Canvas 中的 Layer

        public UICanvas(UICanvasID canvasID)
        {
            mCanvasID = canvasID;

            mLayerList = new MList<UILayer>();

            int idx = 0;
            for (idx = 0; idx < (int)UILayerID.eMaxLayer; ++idx)
            {
                mLayerList.Add(new UILayer((UILayerID)idx));
            }

            if (UICanvasID.eFirstCanvas == mCanvasID)
            {
                mLayerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_FirstCanvas;
                mLayerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_FirstCanvas;
                mLayerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_FirstCanvas;
                mLayerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_FirstCanvas;
                mLayerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_FirstCanvas;
                mLayerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_FirstCanvas;
            }
            else if(UICanvasID.eSecondCanvas == mCanvasID)
            {
                mLayerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_SecondCanvas;
                mLayerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_SecondCanvas;
                mLayerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_SecondCanvas;
                mLayerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_SecondCanvas;
                mLayerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_SecondCanvas;
                mLayerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_SecondCanvas;
            }
            else if (UICanvasID.eHudCanvas == mCanvasID)
            {
                mLayerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_HudCanvas;
                mLayerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_HudCanvas;
                mLayerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_HudCanvas;
                mLayerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_HudCanvas;
                mLayerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_HudCanvas;
                mLayerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_HudCanvas;
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
            int idx = 0;
            for (idx = 0; idx < (int)UILayerID.eMaxLayer; ++idx)
            {
                mLayerList[idx].findLayerGOAndTrans();
            }
        }
    }
}