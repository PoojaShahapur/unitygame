namespace SDK.Lib
{
    public class MacroDef
    {
        // Release 自己使用的定义
        //NET_MULTHREAD;MSG_ENCRIPT;MSG_COMPRESS

        // Debug 自己使用的定义
        //NET_MULTHREAD;MSG_ENCRIPT;MSG_COMPRESS;THREAD_CALLCHECK;ENABLE_WINLOG;ENABLE_NETLOG;UNIT_TEST;ENABLE_FILELOG

        // 宏定义开始
        // 调试不需要网络
        static public bool DEBUG_NOTNET = false;

        // 网络处理多线程，主要是调试的时候使用单线程，方便调试，运行的时候使用多线程
        static public bool NET_MULTHREAD = true;

        // 是否检查函数接口调用线程
        static public bool THREAD_CALLCHECK = true;

        // 消息加密
        static public bool MSG_ENCRIPT = false;

        // 消息压缩
        static public bool MSG_COMPRESS = false;

        // 开启窗口日志
        static public bool ENABLE_WINLOG = true;

        // 开启网络日志
        static public bool ENABLE_NETLOG = false;

        // 开启文件日志
        static public bool ENABLE_FILELOG = false;

        // 使用打包模式加载资源
        static public bool PKG_RES_LOAD = false;

        // 非打包文件系统资源加载
        static public bool UNPKG_RES_LOAD = false;

        // 是否开启 EnableProtoBuf
        static public bool ENABLE_PROTOBUF = false;

        // 是否开启 SharpZipLib
        static public bool ENABLE_SHARP_ZIP_LIB = false;

        // 单元测试，这个需要宏定义
        static public bool UNIT_TEST = true;

        // 不使用的代码
        static public bool DEPRECATE_CODE = false;

        // 多线程裁剪场景
        static public bool MULTITHREADING_CULL = false;

        // Lua 加载方式
        static public bool LUA_EDITOR = true;

        // 开启日志
        static public bool ENABLE_LOG = true;

        // 开启警告
        static public bool ENABLE_WARN = false;

        // 开启错误
        static public bool ENABLE_ERROR = false;

        // 绘制调试信息
        static public bool DRAW_DEBUG = false;

#if UNITY_IPHONE || UNITY_IOS
        // 开启 Bugly
        static public bool ENABLE_BUGLY = true;
#elif UNITY_ANDROID
        // 开启 Bugly
        static public bool ENABLE_BUGLY = true;
#else
        // 开启 Bugly
        static public bool ENABLE_BUGLY = false;
#endif
        // 坐标模式
        static public bool XZ_MODE = false;
        static public bool XY_MODE = true;

        // 物理运行
        static public bool PHYSIX_MOVE = false;

        // 宏定义结束
    }
}