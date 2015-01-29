namespace SDK.Common
{
    public interface ITickedObject : IDelayHandleItem
    {
        void OnTick(float delta);
    }
}