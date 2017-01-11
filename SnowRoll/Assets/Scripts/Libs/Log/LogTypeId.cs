namespace SDK.Lib
{
    public enum LogTypeId
    {
        eLogCommon,         // 通用日志
        eLogSceneCull,      // 场景裁剪日志
        eLogMSceneManager,  // 场景管理器
        eLogTest,           // 测试日志
        eLogTestRL,         // 测试资源加载
        eLogResLoader,      // 资源加载
        eLogLocalFile,      // 本地文件系统， MDataStream
        eLogAcceleration,   // 重力
        eLogSplitMergeEmit,         // 分裂融合
        eLogSceneInterActive,       // 场景交互
        eLogBeingMove,      // Being 移动
        eLogKBE,            // KBEngine 日志
    }
}