using UnityEngine;

namespace SDK.Lib
{
    public class CamEntity
    {
        public GameObject mCamGo;

        public void onSceneLoaded()
        {
            mCamGo = GameObject.FindGameObjectWithTag("MainCamera");
        }

        public void setTarget(Transform tran)
        {
            //mCamGo.GetComponent<SmoothFollow>().target = tran;
        }
    }
}