namespace SDK.Lib
{
    public interface ITickedObject
    {
        void onTick(float delta, TickMode tickMode);
    }
}