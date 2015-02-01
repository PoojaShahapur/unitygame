namespace SDK.Common
{
    public interface ITimerMgr : IDelayHandleMgrBase
    {
        void Advance(float delta);
    }
}