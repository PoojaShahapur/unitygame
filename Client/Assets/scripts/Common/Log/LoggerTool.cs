using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 日志系统，只有工具在使用
     */
    public class LoggerTool
    {
        static public void log(string message)
        {
            Debug.Log(message);
        }

        static public void warn(string message)
        {
            Debug.LogWarning(message);
        }

        static public void error(string message)
        {
            Debug.LogError(message);
        }
    }
}