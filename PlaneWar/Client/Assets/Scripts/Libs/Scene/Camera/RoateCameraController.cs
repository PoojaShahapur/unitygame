using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 旋转相机控制器
     */
    public class RoateCameraController : CameraController
    {
        private Transform transform;

        public RoateCameraController(Camera camera, GameObject go, SceneEntityBase actor)
            : base(camera, go)
        {
            this.transform = camera.gameObject.GetComponent<Transform>();
            Ctx.mInstance.mGlobalDelegate.addMainChildChangedHandle(null, onTargetOrientPosChanged);

            this.onTargetOrientPosChanged(null);
        }

        public void init()
        {
           Screen.sleepTimeout = SleepTimeout.NeverSleep;//设置屏幕永远亮着
        }

        public void dispose()
        {

        }

        public void onTargetOrientPosChanged(IDispatchObject dispObj)
        {
            this.SetCameraPosition(true);
        }

        private void SetCameraPosition(bool isDispMove = false)
        {
            PlayerMain playerMain = Ctx.mInstance.mPlayerMgr.getHero();

            if(null != playerMain && !playerMain.getIsDead())
            {
                //中心位置
                Vector3 centerPos = playerMain.getPos();
                //缩放参照距离
                float radius = playerMain.mPlayerSplitMerge.getMaxCameraLength();
                //缩放相机距离
                float viewScale = radius * Ctx.mInstance.mSnowBallCfg.mCameraChangeFactor_Y;

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