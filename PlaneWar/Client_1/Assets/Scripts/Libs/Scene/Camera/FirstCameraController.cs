using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 第一人称摄像机控制器
     */
    public class FirstCameraController : CameraController
    {
        public FirstCameraController(CamEntity camera, ICamTargetEntiry targetEntity)
            : base(camera, targetEntity)
        {

        }
    }
}