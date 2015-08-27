using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 直接加载声音文件
     */
    public class SoundClipItem : SoundItem
    {
        public AudioClip m_clip;            // 声音资源放在 prefab 中国

        public override void setResObj(UnityEngine.Object go_)
        {
            m_clip = go_ as AudioClip;
            m_go = UtilApi.createGameObject("SoundGO");

            if (m_clip == null)
            {
                return;
            }

            m_audio = m_go.GetComponent<AudioSource>();
            if (m_audio == null)
            {
                m_audio = (AudioSource)m_go.AddComponent<AudioSource>();
            }
            m_audio.clip = m_clip;

            updateParam();
        }

        public override void unload()
        {
            if (bInCurState(SoundPlayState.eSS_Play))
            {
                Stop();
            }

            if (m_go != null)
            {
                //m_clip.UnloadAudioData();
                UtilApi.Destroy(m_go);
                //UtilApi.UnloadUnusedAssets();
            }
        }
    }
}