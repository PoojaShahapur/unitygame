using UnityEngine;

namespace SDK.Common
{
    public class CamEntity
    {
        public GameObject m_camGo;

        public void onSceneLoaded()
        {
            m_camGo = GameObject.FindGameObjectWithTag("MainCamera");
        }

        public void setTarget(Transform tran)
        {
            //m_camGo.GetComponent<SmoothFollow>().target = tran;
        }
    }
}