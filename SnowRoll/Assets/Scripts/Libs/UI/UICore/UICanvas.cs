using System.Collections.Generic;

namespace SDK.Lib
{
    public enum UICanvasID
    {
        eFirstCanvas,
        eSecondCanvas,

        eCanvas_Total,
    }

    /**
     * @brief 描述一个 Canvas
     */
    public class UICanvas
    {
        protected string m_goName;              // GameObject 的名字
        protected UICanvasID mCanvasID;        // CanvasID
        protected List<UILayer> m_layerList;     // Canvas 中的 Layer

        public UICanvas(UICanvasID canvasID)
        {
            mCanvasID = canvasID;

            m_layerList = new List<UILayer>();

            int idx = 0;
            for (idx = 0; idx < (int)UILayerID.eMaxLayer; ++idx)
            {
                m_layerList.Add(new UILayer((UILayerID)idx));
            }

            if (UICanvasID.eFirstCanvas == mCanvasID)
            {
                m_layerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_FirstCanvas;
                m_layerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_FirstCanvas;
                m_layerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_FirstCanvas;
                m_layerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_FirstCanvas;
                m_layerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_FirstCanvas;
                m_layerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_FirstCanvas;
            }
            else if(UICanvasID.eSecondCanvas == mCanvasID)
            {
                m_layerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_SecondCanvas;
                m_layerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_SecondCanvas;
                m_layerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_SecondCanvas;
                m_layerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_SecondCanvas;
                m_layerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_SecondCanvas;
                m_layerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_SecondCanvas;
            }
        }

        public string goName
        {
            set
            {
                m_goName = value;
            }
        }

        public List<UILayer> layerList
        {
            get
            {
                return m_layerList;
            }
        }

        public void findCanvasGO()
        {
            int idx = 0;
            for (idx = 0; idx < (int)UILayerID.eMaxLayer; ++idx)
            {
                m_layerList[idx].findLayerGOAndTrans();
            }
        }
    }
}