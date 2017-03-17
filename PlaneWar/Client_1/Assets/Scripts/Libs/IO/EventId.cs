namespace SDK.Lib
{
    public enum EventId
    {
        KEYDOWN_EVENT = 2,
        KEYUP_EVENT = 3,
        KEYPRESS_EVENT = 4,

        MOUSEDOWN_EVENT = 5,
        MOUSEUP_EVENT = 6,
        MOUSEPRESS_EVENT = 7,       // 鼠标一直按下事件
        MOUSEMOVE_EVENT = 8,        // 鼠标移动事件
        MOUSEPRESS_MOVE_EVENT = 9,  // 鼠标按下移动事件

        AXIS_EVENT = 10,

        TOUCHBEGIN_EVENT = 11,
        TOUCHMOVED_EVENT = 12,
        TOUCHSTATIONARY_EVENT = 13,     // 触碰静止
        TOUCHENDED_EVENT = 14,          // 触碰结束
        TOUCHCANCELED_EVENT = 15,       // 触碰取消

        MULTI_TOUCHBEGIN_EVENT = 11,
        MULTI_TOUCHMOVED_EVENT = 12,    // 多触碰暂时只支持触碰移动
        MULTI_TOUCHSTATIONARY_EVENT = 13,
        MULTI_TOUCHENDED_EVENT = 14,
        MULTI_TOUCHCANCELED_EVENT = 15,

        ACCELERATIONBEGAN_EVENT = 17,
        ACCELERATIONMOVED_EVENT = 18,
        ACCELERATIONENDED_EVENT = 19,
    }
}