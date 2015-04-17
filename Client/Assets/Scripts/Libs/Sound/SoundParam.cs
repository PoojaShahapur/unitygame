using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class SoundParam : IRecycle
    {
        public string m_path = "";      // 音乐路径
        public bool m_bLoop = true;    // 是否循环播放
        public Transform m_trans;       // 播放音乐的对象的位置信息

        public void resetDefault()
        {
            m_path = "";
            m_bLoop = true;
            m_trans = null;
        }
    }
}