using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @屏幕自适应
     */
    public class CanvasMatch
    {
        protected int m_refWidth;
        protected int m_refHeight;
        protected CanvasScaler m_canvasScaler;
        protected int m_screenWidth = 0;
        protected int m_screenHeight = 0;
        protected GameObject m_go;

        public CanvasMatch()
        {

        }

        public void init()
        {
            m_canvasScaler = m_go.GetComponent<CanvasScaler>();
            updateScaleMatch();
        }

        public void update()
        {
            if(Application.isEditor)
            {
                updateScaleMatch();
            }
        }

        public void updateScaleMatch()
        {
            m_screenWidth = Screen.width;
            m_screenHeight = Screen.height;

            float refRate = ((float)m_refWidth / m_refHeight);
            float screenRate = ((float)m_screenWidth / m_screenHeight);
            m_canvasScaler.matchWidthOrHeight = screenRate >= refRate ? 1 : 0;
        }
    }
}