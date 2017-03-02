namespace SDK.Lib
{
    public class CoroutineTaskBase
    {
        protected eCoroutineTaskState mState;
        protected bool mNeedRemove;

        public CoroutineTaskBase()
        {
            mNeedRemove = true;
        }

        public bool isRuning()
        {
            return mState == eCoroutineTaskState.eRunning;
        }

        public bool isPause()
        {
            return mState == eCoroutineTaskState.ePaused;
        }

        public bool isStop()
        {
            return mState == eCoroutineTaskState.eStopped;
        }

        public void setNeedRemove(bool value)
        {
            mNeedRemove = value;
        }

        public bool isNeedRemove()
        {
            return mNeedRemove;
        }

        public void Start()
        {
            mState = eCoroutineTaskState.eRunning;
        }

        public void Stop()
        {
            mState = eCoroutineTaskState.eStopped;
        }

        virtual public void run()
        {
            mState = eCoroutineTaskState.eStopped;
        }
    }
}