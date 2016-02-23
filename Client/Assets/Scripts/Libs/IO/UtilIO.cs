using UnityEngine;

namespace SDK.Lib
{
    public class UtilIO
    {
        static protected Vector2 m_lastFirPos;
        static protected Vector2 m_curFirPos;

        static protected Vector2 m_lastSndPos;
        static protected Vector2 m_curSndPos;

        static public KeyCode[] keys = new KeyCode[]
        {
            KeyCode.Backspace, // 8,
		    KeyCode.Tab, // 9,
		    KeyCode.Clear, // 12,
		    KeyCode.Return, // 13,
		    KeyCode.Pause, // 19,
		    KeyCode.Escape, // 27,
		    KeyCode.Space, // 32,
		    KeyCode.Exclaim, // 33,
		    KeyCode.DoubleQuote, // 34,
		    KeyCode.Hash, // 35,
		    KeyCode.Dollar, // 36,
		    KeyCode.Ampersand, // 38,
		    KeyCode.Quote, // 39,
		    KeyCode.LeftParen, // 40,
		    KeyCode.RightParen, // 41,
		    KeyCode.Asterisk, // 42,
		    KeyCode.Plus, // 43,
		    KeyCode.Comma, // 44,
		    KeyCode.Minus, // 45,
		    KeyCode.Period, // 46,
		    KeyCode.Slash, // 47,
		    KeyCode.Alpha0, // 48,
		    KeyCode.Alpha1, // 49,
		    KeyCode.Alpha2, // 50,
		    KeyCode.Alpha3, // 51,
		    KeyCode.Alpha4, // 52,
		    KeyCode.Alpha5, // 53,
		    KeyCode.Alpha6, // 54,
		    KeyCode.Alpha7, // 55,
		    KeyCode.Alpha8, // 56,
		    KeyCode.Alpha9, // 57,
		    KeyCode.Colon, // 58,
		    KeyCode.Semicolon, // 59,
		    KeyCode.Less, // 60,
		    KeyCode.Equals, // 61,
		    KeyCode.Greater, // 62,
		    KeyCode.Question, // 63,
		    KeyCode.At, // 64,
		    KeyCode.LeftBracket, // 91,
		    KeyCode.Backslash, // 92,
		    KeyCode.RightBracket, // 93,
		    KeyCode.Caret, // 94,
		    KeyCode.Underscore, // 95,
		    KeyCode.BackQuote, // 96,
		    KeyCode.A, // 97,
		    KeyCode.B, // 98,
		    KeyCode.C, // 99,
		    KeyCode.D, // 100,
		    KeyCode.E, // 101,
		    KeyCode.F, // 102,
		    KeyCode.G, // 103,
		    KeyCode.H, // 104,
		    KeyCode.I, // 105,
		    KeyCode.J, // 106,
		    KeyCode.K, // 107,
		    KeyCode.L, // 108,
		    KeyCode.M, // 109,
		    KeyCode.N, // 110,
		    KeyCode.O, // 111,
		    KeyCode.P, // 112,
		    KeyCode.Q, // 113,
		    KeyCode.R, // 114,
		    KeyCode.S, // 115,
		    KeyCode.T, // 116,
		    KeyCode.U, // 117,
		    KeyCode.V, // 118,
		    KeyCode.W, // 119,
		    KeyCode.X, // 120,
		    KeyCode.Y, // 121,
		    KeyCode.Z, // 122,
		    KeyCode.Delete, // 127,
		    KeyCode.Keypad0, // 256,
		    KeyCode.Keypad1, // 257,
		    KeyCode.Keypad2, // 258,
		    KeyCode.Keypad3, // 259,
		    KeyCode.Keypad4, // 260,
		    KeyCode.Keypad5, // 261,
		    KeyCode.Keypad6, // 262,
		    KeyCode.Keypad7, // 263,
		    KeyCode.Keypad8, // 264,
		    KeyCode.Keypad9, // 265,
		    KeyCode.KeypadPeriod, // 266,
		    KeyCode.KeypadDivide, // 267,
		    KeyCode.KeypadMultiply, // 268,
		    KeyCode.KeypadMinus, // 269,
		    KeyCode.KeypadPlus, // 270,
		    KeyCode.KeypadEnter, // 271,
		    KeyCode.KeypadEquals, // 272,
		    KeyCode.UpArrow, // 273,
		    KeyCode.DownArrow, // 274,
		    KeyCode.RightArrow, // 275,
		    KeyCode.LeftArrow, // 276,
		    KeyCode.Insert, // 277,
		    KeyCode.Home, // 278,
		    KeyCode.End, // 279,
		    KeyCode.PageUp, // 280,
		    KeyCode.PageDown, // 281,
		    KeyCode.F1, // 282,
		    KeyCode.F2, // 283,
		    KeyCode.F3, // 284,
		    KeyCode.F4, // 285,
		    KeyCode.F5, // 286,
		    KeyCode.F6, // 287,
		    KeyCode.F7, // 288,
		    KeyCode.F8, // 289,
		    KeyCode.F9, // 290,
		    KeyCode.F10, // 291,
		    KeyCode.F11, // 292,
		    KeyCode.F12, // 293,
		    KeyCode.F13, // 294,
		    KeyCode.F14, // 295,
		    KeyCode.F15, // 296,
		    KeyCode.Numlock, // 300,
		    KeyCode.CapsLock, // 301,
		    KeyCode.ScrollLock, // 302,
		    KeyCode.RightShift, // 303,
		    KeyCode.LeftShift, // 304,
		    KeyCode.RightControl, // 305,
		    KeyCode.LeftControl, // 306,
		    KeyCode.RightAlt, // 307,
		    KeyCode.LeftAlt, // 308,
		    //KeyCode.Mouse0, // 323,
		    //KeyCode.Mouse1, // 324,
		    //KeyCode.Mouse2, // 325,
		    KeyCode.Mouse3, // 326,
		    KeyCode.Mouse4, // 327,
		    KeyCode.Mouse5, // 328,
		    KeyCode.Mouse6, // 329,
		    KeyCode.JoystickButton0, // 330,
		    KeyCode.JoystickButton1, // 331,
		    KeyCode.JoystickButton2, // 332,
		    KeyCode.JoystickButton3, // 333,
		    KeyCode.JoystickButton4, // 334,
		    KeyCode.JoystickButton5, // 335,
		    KeyCode.JoystickButton6, // 336,
		    KeyCode.JoystickButton7, // 337,
		    KeyCode.JoystickButton8, // 338,
		    KeyCode.JoystickButton9, // 339,
		    KeyCode.JoystickButton10, // 340,
		    KeyCode.JoystickButton11, // 341,
		    KeyCode.JoystickButton12, // 342,
		    KeyCode.JoystickButton13, // 343,
		    KeyCode.JoystickButton14, // 344,
		    KeyCode.JoystickButton15, // 345,
		    KeyCode.JoystickButton16, // 346,
		    KeyCode.JoystickButton17, // 347,
		    KeyCode.JoystickButton18, // 348,
		    KeyCode.JoystickButton19, // 349,
	    };

        static public float time
        {
            get
            {
                return Time.unscaledTime;
            }
        }

        // 两个或多个滑动事件
        static public void onMultiTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                m_curFirPos = touch.position;

                if (Input.touchCount > 1)
                {
                    touch = Input.GetTouch(1);
                    m_curSndPos = touch.position;

                    float curDist = Vector2.Distance(m_curFirPos, m_curSndPos);
                    float lastDist = Vector2.Distance(m_lastFirPos, m_lastSndPos);
                    float delta = curDist - lastDist;
                    // 分发多触屏改变
                    // disp
                }
            }

            m_lastFirPos = m_curFirPos;
            m_lastSndPos = m_curSndPos;
        }

        // 模拟停止拖放
        static public void simuStopDrag(IOController.MouseOrTouch currentTouch)
        {
            if (currentTouch != null)
            {
                currentTouch.dragStarted = false;
                currentTouch.pressed = null;
                currentTouch.dragged = null;
            }
        }

        // 播放协程
        //public Coroutine StartCoroutine(IEnumerator routine)
        //{
        //    return Ctx.m_instance.m_coroutineMgr.StartCoroutine(routine);
        //}
    }
}