namespace SDK.Common
{
    public interface ITickMgr
    {
        void Advance(float delta);
        void AddTickObj(ITickedObject obj);
    }
}