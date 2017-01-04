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
        TOUCHSTATIONARY_EVENT = 13,     // 
        TOUCHENDED_EVENT = 14,          // 
        TOUCHCANCELED_EVENT = 15,       // 

        ACCELERATIONBEGAN_EVENT = 16,
        ACCELERATIONMOVED_EVENT = 17,
        ACCELERATIONENDED_EVENT = 18,
    }
}