using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 第三人称摄像机控制器
     */
    public class ThirdCameraController : CameraController
    {
        protected SphericalCoordinate m_coord;  // 坐标系统

        public ThirdCameraController(Camera camera)
            : base(camera)
        {
            m_coord = new SphericalCoordinate();
        }

        public void setParam(float radius, float theta, float fai)
        {
            m_coord.setParam(radius, theta, fai);
        }
    }
}