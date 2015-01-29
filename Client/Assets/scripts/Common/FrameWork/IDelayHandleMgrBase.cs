namespace SDK.Common
{
    public interface IDelayHandleMgrBase
    {
        void addObject(IDelayHandleItem delayObject, float priority = 0.0f);
        void delObject(IDelayHandleItem delayObject);
    }
}