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
            mClip = go_ as AudioClip;
            mGo = UtilApi.createGameObject("SoundGO");

            if (mClip == null)
            {
                return;
            }

            mAudio = mGo.GetComponent<AudioSource>();
            if (mAudio == null)
            {
                mAudio = (AudioSource)mGo.AddComponent<AudioSource>();
            }
            mAudio.clip = mClip;

            updateParam();
        }

        public override void unload()
        {
            if (bInCurState(SoundPlayState.eSS_Play))
            {
                Stop();
            }

            if (mGo != null)
            {
                //mClip.UnloadAudioData();
                UtilApi.Destroy(mGo);
                //UtilApi.UnloadUnusedAssets();
            }
        }
    }
}