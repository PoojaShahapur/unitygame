using UnityEngine;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @屏幕自适应
     */
    public class CanvasMatch
    {
        protected int mRefWidth;
        protected int mRefHeight;
        protected CanvasScaler mCanvasScaler;
        protected int mScreenWidth = 0;
        protected int mScreenHeight = 0;
        protected GameObject mGo;

        public CanvasMatch()
        {

        }

        public void init()
        {
            mCanvasScaler = mGo.GetComponent<CanvasScaler>();
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
            mScreenWidth = Screen.width;
            mScreenHeight = Screen.height;

            float refRate = ((float)mRefWidth / mRefHeight);
            float screenRate = ((float)mScreenWidth / mScreenHeight);
            mCanvasScaler.matchWidthOrHeight = screenRate >= refRate ? 1 : 0;
        }
    }
}