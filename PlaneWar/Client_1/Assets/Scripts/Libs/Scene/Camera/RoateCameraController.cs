using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 旋转相机控制器
     */
    public class RoateCameraController : CameraController
    {
        private Transform transform;

        public RoateCameraController(CamEntity camera, ICamTargetEntiry targetEntity)
            : base(camera, targetEntity)
        {
            //this.transform = camera.gameObject.GetComponent<Transform>();
            camera.mCameraCreatedDispatch.addEventHandle(null, this.onCamCreated);
            Ctx.mInstance.mGlobalDelegate.addMainChildChangedHandle(null, this.onTargetOrientPosChanged);

            this.onTargetOrientPosChanged(null);
        }

        public void init()
        {
           Screen.sleepTimeout = SleepTimeout.NeverSleep;//设置屏幕永远亮着
        }

        override public void dispose()
        {

        }

        // 相机创建事件
        override public void onCamCreated(IDispatchObject dispObj)
        {
            base.onCamCreated(dispObj);

            this.transform = this.mCamera.GetComponent<Transform>();
            this.onTargetOrientPosChanged(null);
        }

        public void onTargetOrientPosChanged(IDispatchObject dispObj)
        {
            this.SetCameraPosition(true);
        }

        private void SetCameraPosition(bool isDispMove = false)
        {
            if (null != this.transform && null != this.mCamera)
            {
                PlayerMain playerMain = Ctx.mInstance.mPlayerMgr.getHero();

                if (null != playerMain && !playerMain.getIsDead())
                {
                    //中心位置
                    Vector3 centerPos = playerMain.getPos();
                    //缩放参照距离
                    float radius = playerMain.mPlayerSplitMerge.getMaxCameraLength();
                    //缩放相机距离
                    float viewScale = radius * Ctx.mInstance.mSnowBallCfg.mCameraChangeFactor_Y;
                    this.mCamera.GetComponent<Camera>().orthographicSize = 3 + viewScale;

                    Ctx.mInstance.mClipRect.setCam(this.mCamera.GetComponent<Camera>());

                    //更改主相机的旋转角度和位置
                    centerPos.z = -10;
                    this.transform.position = centerPos;

                    if (isDispMove)
                    {
                        Ctx.mInstance.mGlobalDelegate.mCameraOrientChangedDispatch.dispatchEvent(null);
                    }
                }
            }
        }
    }
}