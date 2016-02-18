using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 第三人称摄像机控制器
     */
    public class ThirdCameraController : CameraController
    {
        protected SphericalCoordinate m_coord;  // 坐标系统
        protected Transform m_cameraTrans;            // 摄像机的转换

        public ThirdCameraController(Camera camera, GameObject target)
            : base(camera, target)
        {
            m_coord = new SphericalCoordinate();
            m_cameraTrans = m_camera.GetComponent<Transform>();

            this.setParam(5, Mathf.PI / 4, Mathf.PI);
        }

        public void setParam(float radius, float theta, float fai)
        {
            m_coord.setParam(radius, theta, fai);
            m_coord.syncTrans(m_cameraTrans);
            m_cameraTrans.LookAt(0, 0, 0);
        }
    }
}