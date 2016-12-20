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
            mGo = go_ as GameObject;
            updateParam();
        }

        // 卸载
        public override void unload()
        {
            if (bInCurState(SoundPlayState.eSS_Play))
            {
                Stop();
            }

            if (mGo != null)
            {
                UtilApi.Destroy(mGo);
                //UtilApi.UnloadUnusedAssets();
            }
        }
    }
}