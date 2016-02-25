using UnityEngine;

namespace SDK.Lib
{
    public class TerrainCameraMan : CameraMan
    {
        public TerrainCameraMan(GameObject targetGo)
            : base(targetGo)
        {

        }

        override public void onKeyPress(KeyCode key)
        {
            if (KeyCode.UpArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.z = m_localPos.z + 0.1f;
                float height = Ctx.m_instance.m_sceneSys.getHeightAt(m_localPos.x, m_localPos.z);
                m_localPos.y = height;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.DownArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.z = m_localPos.z - 0.1f;
                float height = Ctx.m_instance.m_sceneSys.getHeightAt(m_localPos.x, m_localPos.z);
                m_localPos.y = height;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.RightArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.x = m_localPos.x + 0.1f;
                float height = Ctx.m_instance.m_sceneSys.getHeightAt(m_localPos.x, m_localPos.z);
                m_localPos.y = height;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.LeftArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.x = m_localPos.x - 0.1f;
                float height = Ctx.m_instance.m_sceneSys.getHeightAt(m_localPos.x, m_localPos.z);
                m_localPos.y = height;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else
            {
                base.onKeyPress(key);
            }
        }
    }
}