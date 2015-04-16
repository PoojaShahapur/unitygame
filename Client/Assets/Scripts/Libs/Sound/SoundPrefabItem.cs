using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @将声音做成 prefab
     */
    public class SoundPrefabItem : SoundItem
    {
        public override void setResObj(UnityEngine.Object go_)
        {
            m_go = go_ as GameObject;
            updateParam();
        }

        // 卸载
        public virtual void unload()
        {
            if (bInCurState(SoundPlayState.eSS_Play))
            {
                Stop();
            }

            if (m_go != null)
            {
                UtilApi.Destroy(m_go);
                UtilApi.UnloadUnusedAssets();
            }
        }
    }
}