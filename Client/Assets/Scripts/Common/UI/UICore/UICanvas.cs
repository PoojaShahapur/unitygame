using System.Collections.Generic;

namespace SDK.Common
{
    public enum UICanvasID
    {
        eCanvas_50,
        eCanvas_100,

        eeCanvas_Total,
    }

    /**
     * @brief 描述一个 Canvas
     */
    public class UICanvas
    {
        protected string m_goName;              // GameObject 的名字
        protected UICanvasID m_canvasID;        // CanvasID
        protected List<UILayer> m_layerList;     // Canvas 中的 Layer

        public UICanvas(UICanvasID canvasID)
        {
            m_canvasID = canvasID;

            m_layerList = new List<UILayer>();

            int idx = 0;
            for (idx = 0; idx < (int)UILayerID.eMaxLayer; ++idx)
            {
                m_layerList.Add(new UILayer((UILayerID)idx));
            }

            if (UICanvasID.eCanvas_50 == m_canvasID)
            {
                m_layerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_Canvas_50;
                m_layerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_Canvas_50;
                m_layerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_Canvas_50;
                m_layerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_Canvas_50;
                m_layerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_Canvas_50;
                m_layerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_Canvas_50;
            }
            else if(UICanvasID.eCanvas_100 == m_canvasID)
            {
                m_layerList[(int)UILayerID.eBtmLayer].goName = NotDestroyPath.ND_CV_UIBtmLayer_Canvas_100;
                m_layerList[(int)UILayerID.eFirstLayer].goName = NotDestroyPath.ND_CV_UIFirstLayer_Canvas_100;
                m_layerList[(int)UILayerID.eSecondLayer].goName = NotDestroyPath.ND_CV_UISecondLayer_Canvas_100;
                m_layerList[(int)UILayerID.eThirdLayer].goName = NotDestroyPath.ND_CV_UIThirdLayer_Canvas_100;
                m_layerList[(int)UILayerID.eForthLayer].goName = NotDestroyPath.ND_CV_UIForthLayer_Canvas_100;
                m_layerList[(int)UILayerID.eTopLayer].goName = NotDestroyPath.ND_CV_UITopLayer_Canvas_100;
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