using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SDK.Lib
{
    /**
     * @brief 视频剪辑播放管理器
     */
    public class MovieMgr
    {
        protected bool m_bPlaying;
#if UNITY_STANDALONE || UNITY_EDITOR
        protected MovieTexture m_movieTexture;
#endif
        protected TimerItemBase m_timer;
        protected Action m_handle;

        public MovieMgr()
        {
            m_bPlaying = false;
        }

        public void PlayMovie(string movieName, int time, Action acton)
        {
            m_handle = acton;
#if UNITY_EDITOR
            m_bPlaying = true;
            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath("Assets/res/Movie/" + movieName + ".mp4", typeof(MovieTexture));
            if(o != null)
            {
                m_movieTexture = o as MovieTexture;
                m_movieTexture.Play();
            }
#elif UNITY_STANDALONE
            //UnityEngine.Handheld.PlayFullScreenMovie("Movie/" + movieName + ".mp4", Color.black, FullScreenMovieControlMode.Full | FullScreenMovieControlMode.Hidden);
#endif

            if (m_timer == null)
            {
                m_timer = new TimerItemBase();
            }
            else
            {
                m_timer.reset();
            }
            m_timer.m_internal = time;        // 一分钟遍历一次
            m_timer.m_timerDisp.setFuncObject(OnPlayEnd);
            Ctx.m_instance.m_timerMgr.addTimer(m_timer);
        }

        // 视频结束，定时器回调
        protected void OnPlayEnd(TimerItemBase time)
        {
            if (m_handle != null)
            {
                m_handle();
            }
#if UNITY_STANDALONE || UNITY_EDITOR
            if (m_movieTexture != null)
            {
                m_movieTexture.Stop();
                m_movieTexture = null;
            }
#endif
        }

        // 绘制视频纹理到窗口上
        void OnGUI()
        {
#if UNITY_EDITOR
            if(m_bPlaying && m_movieTexture != null)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_movieTexture, ScaleMode.ScaleToFit);
            }
#endif
        }
    }
}