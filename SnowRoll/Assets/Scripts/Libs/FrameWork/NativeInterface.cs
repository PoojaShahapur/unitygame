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
	// 必须要 OC 的实现才行，否则在导出 AssetBundles 的时候就会报错，而这个时候并没有编译 ipa 包
    //[DllImport("__Internal")]
    //private static extern void aaa();
#endif
}