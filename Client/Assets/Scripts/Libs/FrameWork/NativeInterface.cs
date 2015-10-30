using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Unity 调用本机原生接口
     */
    public class NativeInterface
    {
        // 调用 Java API
        public void callJavaMethod(string method, object[] args)
        {
#if UNITY_ANDROID
            if(Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        jo.Call(method, args);
                    }
                }
            }
#endif
        }
    }

    // IOS Native 导入区域
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _getAPNType();
#endif
}