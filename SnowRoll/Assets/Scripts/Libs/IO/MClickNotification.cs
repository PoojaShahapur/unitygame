namespace SDK.Lib
{
    public enum MClickNotification
    {
        None,
        Always,
        BasedOnDelta,   // Click 事件中间有间隔产生的，例如 Down ，然后移动很小距离，再 Up ，这个时候就产生 Click 事件
    }
}