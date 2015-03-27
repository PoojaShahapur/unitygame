namespace SDK.Common
{
    public interface ITickMgr : IDelayHandleMgrBase
    {
        void Advance(float delta);
    }
}