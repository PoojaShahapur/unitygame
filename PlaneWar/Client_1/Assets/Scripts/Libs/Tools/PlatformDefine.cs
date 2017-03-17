namespace SDK.Lib
{
    public class PlatformDefine
    {
        static public string PlatformFolder;

        // http://docs.unity3d.com/Manual/PlatformDependentCompilation.html
        static public void init()
        {
#if UNITY_EDITOR_WIN
            PlatformFolder = "Windows";
#elif UNITY_EDITOR_OSX
            PlatformFolder = "OSX";
#elif UNITY_STANDALONE_OSX
            PlatformFolder = "OSX";
#elif UNITY_DASHBOARD_WIDGET
            PlatformFolder = "OSX";
#elif UNITY_STANDALONE_WIN
            PlatformFolder = "Windows";
#elif UNITY_STANDALONE_LINUX
            PlatformFolder = "Linux";
#elif UNITY_IOS
            PlatformFolder = "iOS";
#elif UNITY_IPHONE
            PlatformFolder = "iOS";
#elif UNITY_ANDROID
            PlatformFolder = "Android";
#elif UNITY_WEBPLAYER
            PlatformFolder = "WebPlayer";
#elif UNITY_WII
            PlatformFolder = "Wii";
#elif UNITY_PS3
            PlatformFolder = "Ps3";
#elif UNITY_XBOX360
            PlatformFolder = "Xbox360";
#elif UNITY_NACL
            PlatformFolder = "Nacl";
#else
            PlatformFolder = "Windows";
#endif
        }
    }
}