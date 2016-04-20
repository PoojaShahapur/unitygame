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
        }

        override public void run()
        {
            Ctx.m_instance.m_sceneManager.cullScene();
        }
    }
}