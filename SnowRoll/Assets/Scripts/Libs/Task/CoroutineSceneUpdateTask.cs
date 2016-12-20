namespace SDK.Lib
{
    /**
     * @brief 场景更新任务
     */
    public class CoroutineSceneUpdateTask : CoroutineTaskBase
    {
        public CoroutineSceneUpdateTask()
        {
            mNeedRemove = false;
            Stop();
        }

        override public void run()
        {
            base.run();
            Ctx.mInstance.mSceneManager.cullScene();
        }
    }
}