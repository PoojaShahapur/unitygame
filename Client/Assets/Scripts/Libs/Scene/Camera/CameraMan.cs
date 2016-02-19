using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 控制摄像机运动
     */
    public class CameraMan
    {
        protected GameObject m_targetGo;
        protected CameraController m_cameraController;

        public CameraMan(GameObject targetGo)
        {
            m_targetGo = targetGo;
            if (m_targetGo == null)
            {
                m_targetGo = UtilApi.createGameObject("CameraGo");
            }
        }

        public void setActor(GameObject targetGo)
        {
            m_targetGo = targetGo;
            if (m_targetGo == null)
            {
                m_targetGo = UtilApi.createGameObject("CameraGo");
            }
        }

        public void setCameraController(CameraController controller)
        {
            m_cameraController = controller;
        }
    }
}