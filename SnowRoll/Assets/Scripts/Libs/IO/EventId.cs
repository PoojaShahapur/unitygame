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

        AXIS_EVENT = 9,

        TOUCHBEGIN_EVENT = 10,
        TOUCHMOVED_EVENT = 11,
        TOUCHSTATIONARY_EVENT = 12,     // 
        TOUCHENDED_EVENT = 13,          // 
        TOUCHCANCELED_EVENT = 14,       // 

        ACCELERATIONBEGAN_EVENT = 15,
        ACCELERATIONMOVED_EVENT = 16,
        ACCELERATIONENDED_EVENT = 17,
    }
}