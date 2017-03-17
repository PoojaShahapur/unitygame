namespace SDK.Lib
{
    /**
     * @brief Tick 的优先级
     * @brief TP TickPriority 缩写
     */
    public class TickPriority
    {
        public static float eTPPlayerMgr = 10;       // PlayerMgr
        public static float eTPSnowBlockMgr = 10;    // SnowBlockMgr
        public static float eTPPlayerSnowBlockMgr = 10;    // PlayerSnowBlockMgr
        public static float eTPComputerBallMgr = 10;    // TPComputerBallMgr
        public static float eTPRobotMgr = 10;        // RobotMgr
        public static float eTPFlyBulletMgr = 10;        // FlyBulletMgr
        public static float eTPCamController = 10;   // 相机控制器
        public static float eTPInputMgr = 10000;   // 相机控制器
        public static float eTPResizeMgr = 100000;   // 窗口大小改变
        public static float eTPJoyStick = 1f;   // 摇杆控制器
        public static float eTPForwardForce = 1f;   // 反重力控制器
        public static float eTPCameraMgr = 1f;   // 相机位置更新控制器
        public static float eTPDelayTaskMgr = 1f;   // 延迟任务
        public static float eTPLoadProgressMgr = 1f;   // 更新加载进度
        public static float eTPSoundLoadStateCheckMgr = 1f;   // 更新音乐状态检查
        public static float eTPClipRect = 1f;
    }
}