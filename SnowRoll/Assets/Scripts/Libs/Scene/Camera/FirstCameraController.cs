using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 第一人称摄像机控制器
     */
    public class FirstCameraController : CameraController
    {
        public FirstCameraController(Camera camera, GameObject target)
            : base(camera, target)
        {

        }
    }
}