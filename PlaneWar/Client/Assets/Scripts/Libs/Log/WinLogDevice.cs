using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 文件日志
     */
    public class WinLogDevice : LogDeviceBase
    {
        public override void logout(string message, LogColor type = LogColor.eLC_LOG)
        {
            if (type == LogColor.eLC_LOG)
            {
                Debug.Log(message);
            }
            else if (type == LogColor.eLC_WARN)
            {
                Debug.LogWarning(message);
            }
            else if (type == LogColor.eLC_ERROR)
            {
                Debug.LogError(message);
            }
        }
    }
}