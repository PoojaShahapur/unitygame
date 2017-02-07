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
            this.mGo = go_ as GameObject;
            this.updateParam();
        }

        // 卸载
        public override void unload()
        {
            if (this.isInCurState(SoundPlayState.eSS_Play))
            {
                this.Stop();
            }

            if (this.mGo != null)
            {
                UtilApi.Destroy(this.mGo);
                //UtilApi.UnloadUnusedAssets();
            }
        }
    }
}