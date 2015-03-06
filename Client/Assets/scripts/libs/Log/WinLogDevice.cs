using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 文件日志
     */
    public class WinLogDevice : LogDeviceBase
    {
        public override void logout(string message, LogColor type = LogColor.LOG)
        {
            if (type == LogColor.LOG)
            {
                Debug.Log(message);
            }
            else if (type == LogColor.WARN)
            {
                Debug.LogWarning(message);
            }
            else if (type == LogColor.ERROR)
            {
                Debug.LogError(message);
            }
        }
    }
}