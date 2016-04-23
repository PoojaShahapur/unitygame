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
            Ctx.m_instance.m_sceneManager.cullScene();
        }
    }
}