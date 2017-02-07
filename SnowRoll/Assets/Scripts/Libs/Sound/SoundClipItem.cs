using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 直接加载声音文件
     */
    public class SoundClipItem : SoundItem
    {
        public AudioClip mClip;            // 声音资源放在 prefab 中国

        public override void setResObj(UnityEngine.Object go_)
        {
            this.mClip = go_ as AudioClip;
            this.mGo = UtilApi.createGameObject("SoundGO");

            if (this.mClip == null)
            {
                return;
            }

            this.mAudio = mGo.GetComponent<AudioSource>();

            if (this.mAudio == null)
            {
                this.mAudio = (AudioSource)mGo.AddComponent<AudioSource>();
            }

            this.mAudio.clip = mClip;

            this.updateParam();
        }

        public override void unload()
        {
            if (this.isInCurState(SoundPlayState.eSS_Play))
            {
                this.Stop();
            }

            if (this.mGo != null)
            {
                //mClip.UnloadAudioData();
                UtilApi.Destroy(this.mGo);
                //UtilApi.UnloadUnusedAssets();
            }
        }
    }
}