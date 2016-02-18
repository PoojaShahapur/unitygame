using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 摄像机控制器
     */
    public class CameraController
    {
        protected Camera m_camera;  // 摄像机

        public CameraController(Camera camera)
        {
            m_camera = camera;
        }
    }
}