﻿using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * Enumeration class that maps friendly key names to their key code equivalent. This class
     * should not be instantiated directly, rather, one of the constants should be used.
     */   
    public class InputKey : IDispatchObject
    {
        /*
        if(Input.anyKeyDown)
        {
            // Event.current 为什么一直是 null
            if (Event.current != null && Event.current.isKey)
            {
                onKeyDown(Event.current.keyCode);
            }
        }
        */

        public static InputKey None = new InputKey(KeyCode.None, "None");
        public static InputKey Backspace = new InputKey(KeyCode.Backspace, "Backspace");
        public static InputKey Tab = new InputKey(KeyCode.Tab, "Tab");
        public static InputKey Clear = new InputKey(KeyCode.Clear, "Clear");
        public static InputKey Return = new InputKey(KeyCode.Return, "Return");
        public static InputKey Pause = new InputKey(KeyCode.Pause, "Pause");
        public static InputKey Escape = new InputKey(KeyCode.Escape, "Escape");
        public static InputKey Space = new InputKey(KeyCode.Space, "Space");
        public static InputKey Exclaim = new InputKey(KeyCode.Exclaim, "Exclaim");
        public static InputKey DoubleQuote = new InputKey(KeyCode.DoubleQuote, "DoubleQuote");
        public static InputKey Hash = new InputKey(KeyCode.Hash, "Hash");
        public static InputKey Dollar = new InputKey(KeyCode.Dollar, "Dollar");
        public static InputKey Ampersand = new InputKey(KeyCode.Ampersand, "Ampersand");
        public static InputKey Quote = new InputKey(KeyCode.Quote, "Quote");
        public static InputKey LeftParen = new InputKey(KeyCode.LeftParen, "LeftParen");
        public static InputKey RightParen = new InputKey(KeyCode.RightParen, "RightParen");
        public static InputKey Asterisk = new InputKey(KeyCode.Asterisk, "Asterisk");
        public static InputKey Plus = new InputKey(KeyCode.Plus, "Plus");
        public static InputKey Comma = new InputKey(KeyCode.Comma, "Comma");
        public static InputKey Minus = new InputKey(KeyCode.Minus, "Minus");
        public static InputKey Period = new InputKey(KeyCode.Period, "Period");
        public static InputKey Slash = new InputKey(KeyCode.Slash, "Slash");
        public static InputKey Alpha0 = new InputKey(KeyCode.Alpha0, "Alpha0");
        public static InputKey Alpha1 = new InputKey(KeyCode.Alpha1, "Alpha1");
        public static InputKey Alpha2 = new InputKey(KeyCode.Alpha2, "Alpha2");
        public static InputKey Alpha3 = new InputKey(KeyCode.Alpha3, "Alpha3");
        public static InputKey Alpha4 = new InputKey(KeyCode.Alpha4, "Alpha4");
        public static InputKey Alpha5 = new InputKey(KeyCode.Alpha5, "Alpha5");
        public static InputKey Alpha6 = new InputKey(KeyCode.Alpha6, "Alpha6");
        public static InputKey Alpha7 = new InputKey(KeyCode.Alpha7, "Alpha7");
        public static InputKey Alpha8 = new InputKey(KeyCode.Alpha8, "Alpha8");
        public static InputKey Alpha9 = new InputKey(KeyCode.Alpha9, "Alpha9");
        public static InputKey Colon = new InputKey(KeyCode.Colon, "Colon");
        public static InputKey Semicolon = new InputKey(KeyCode.Semicolon, "Semicolon");
        public static InputKey Less = new InputKey(KeyCode.Less, "Less");
        public static InputKey Equals = new InputKey(KeyCode.Equals, "Equals");
        public static InputKey Greater = new InputKey(KeyCode.Greater, "Greater");
        public static InputKey Question = new InputKey(KeyCode.Question, "Question");
        public static InputKey At = new InputKey(KeyCode.At, "At");
        public static InputKey LeftBracket = new InputKey(KeyCode.LeftBracket, "LeftBracket");
        public static InputKey Backslash = new InputKey(KeyCode.Backslash, "Backslash");
        public static InputKey RightBracket = new InputKey(KeyCode.RightBracket, "RightBracket");
        public static InputKey Caret = new InputKey(KeyCode.Caret, "Caret");
        public static InputKey Underscore = new InputKey(KeyCode.Underscore, "Underscore");
        public static InputKey BackQuote = new InputKey(KeyCode.BackQuote, "BackQuote");
        public static InputKey A = new InputKey(KeyCode.A, "A");
        public static InputKey B = new InputKey(KeyCode.B, "B");
        public static InputKey C = new InputKey(KeyCode.C, "C");
        public static InputKey D = new InputKey(KeyCode.D, "D");
        public static InputKey E = new InputKey(KeyCode.E, "E");
        public static InputKey F = new InputKey(KeyCode.F, "F");
        public static InputKey G = new InputKey(KeyCode.G, "G");
        public static InputKey H = new InputKey(KeyCode.H, "H");
        public static InputKey I = new InputKey(KeyCode.I, "I");
        public static InputKey J = new InputKey(KeyCode.J, "J");
        public static InputKey K = new InputKey(KeyCode.K, "K");
        public static InputKey L = new InputKey(KeyCode.L, "L");
        public static InputKey M = new InputKey(KeyCode.M, "M");
        public static InputKey N = new InputKey(KeyCode.N, "N");
        public static InputKey O = new InputKey(KeyCode.O, "O");
        public static InputKey P = new InputKey(KeyCode.P, "P");
        public static InputKey Q = new InputKey(KeyCode.Q, "Q");
        public static InputKey R = new InputKey(KeyCode.R, "R");
        public static InputKey S = new InputKey(KeyCode.S, "S");
        public static InputKey T = new InputKey(KeyCode.T, "T");
        public static InputKey U = new InputKey(KeyCode.U, "U");
        public static InputKey V = new InputKey(KeyCode.V, "V");
        public static InputKey W = new InputKey(KeyCode.W, "W");
        public static InputKey X = new InputKey(KeyCode.X, "X");
        public static InputKey Y = new InputKey(KeyCode.Y, "Y");
        public static InputKey Z = new InputKey(KeyCode.Z, "Z");
        public static InputKey Delete = new InputKey(KeyCode.Delete, "Delete");
        public static InputKey Keypad0 = new InputKey(KeyCode.Keypad0, "Keypad0");
        public static InputKey Keypad1 = new InputKey(KeyCode.Keypad1, "Keypad1");
        public static InputKey Keypad2 = new InputKey(KeyCode.Keypad2, "Keypad2");
        public static InputKey Keypad3 = new InputKey(KeyCode.Keypad3, "Keypad3");
        public static InputKey Keypad4 = new InputKey(KeyCode.Keypad4, "Keypad4");
        public static InputKey Keypad5 = new InputKey(KeyCode.Keypad5, "Keypad5");
        public static InputKey Keypad6 = new InputKey(KeyCode.Keypad6, "Keypad6");
        public static InputKey Keypad7 = new InputKey(KeyCode.Keypad7, "Keypad7");
        public static InputKey Keypad8 = new InputKey(KeyCode.Keypad8, "Keypad8");
        public static InputKey Keypad9 = new InputKey(KeyCode.Keypad9, "Keypad9");
        public static InputKey KeypadPeriod = new InputKey(KeyCode.KeypadPeriod, "KeypadPeriod");
        public static InputKey KeypadDivide = new InputKey(KeyCode.KeypadDivide, "KeypadDivide");
        public static InputKey KeypadMultiply = new InputKey(KeyCode.KeypadMultiply, "KeypadMultiply");
        public static InputKey KeypadMinus = new InputKey(KeyCode.KeypadMinus, "KeypadMinus");
        public static InputKey KeypadPlus = new InputKey(KeyCode.KeypadPlus, "KeypadPlus");
        public static InputKey KeypadEnter = new InputKey(KeyCode.KeypadEnter, "KeypadEnter");
        public static InputKey KeypadEquals = new InputKey(KeyCode.KeypadEquals, "KeypadEquals");
        public static InputKey UpArrow = new InputKey(KeyCode.UpArrow, "UpArrow");
        public static InputKey DownArrow = new InputKey(KeyCode.DownArrow, "DownArrow");
        public static InputKey RightArrow = new InputKey(KeyCode.RightArrow, "RightArrow");
        public static InputKey LeftArrow = new InputKey(KeyCode.LeftArrow, "LeftArrow");
        public static InputKey Insert = new InputKey(KeyCode.Insert, "Insert");
        public static InputKey Home = new InputKey(KeyCode.Home, "Home");
        public static InputKey End = new InputKey(KeyCode.End, "End");
        public static InputKey PageUp = new InputKey(KeyCode.PageUp, "PageUp");
        public static InputKey PageDown = new InputKey(KeyCode.PageDown, "PageDown");
        public static InputKey F1 = new InputKey(KeyCode.F1, "F1");
        public static InputKey F2 = new InputKey(KeyCode.F2, "F2");
        public static InputKey F3 = new InputKey(KeyCode.F3, "F3");
        public static InputKey F4 = new InputKey(KeyCode.F4, "F4");
        public static InputKey F5 = new InputKey(KeyCode.F5, "F5");
        public static InputKey F6 = new InputKey(KeyCode.F6, "F6");
        public static InputKey F7 = new InputKey(KeyCode.F7, "F7");
        public static InputKey F8 = new InputKey(KeyCode.F8, "F8");
        public static InputKey F9 = new InputKey(KeyCode.F9, "F9");
        public static InputKey F10 = new InputKey(KeyCode.F10, "F10");
        public static InputKey F11 = new InputKey(KeyCode.F11, "F11");
        public static InputKey F12 = new InputKey(KeyCode.F12, "F12");
        public static InputKey F13 = new InputKey(KeyCode.F13, "F13");
        public static InputKey F14 = new InputKey(KeyCode.F14, "F14");
        public static InputKey F15 = new InputKey(KeyCode.F15, "F15");
        public static InputKey Numlock = new InputKey(KeyCode.Numlock, "Numlock");
        public static InputKey CapsLock = new InputKey(KeyCode.CapsLock, "CapsLock");
        public static InputKey ScrollLock = new InputKey(KeyCode.ScrollLock, "ScrollLock");
        public static InputKey RightShift = new InputKey(KeyCode.RightShift, "RightShift");
        public static InputKey LeftShift = new InputKey(KeyCode.LeftShift, "LeftShift");
        public static InputKey RightControl = new InputKey(KeyCode.RightControl, "RightControl");
        public static InputKey LeftControl = new InputKey(KeyCode.LeftControl, "LeftControl");
        public static InputKey RightAlt = new InputKey(KeyCode.RightAlt, "RightAlt");
        public static InputKey LeftAlt = new InputKey(KeyCode.LeftAlt, "LeftAlt");
        public static InputKey RightCommand = new InputKey(KeyCode.RightCommand, "RightCommand");
        public static InputKey RightApple = new InputKey(KeyCode.RightApple, "RightApple");
        public static InputKey LeftCommand = new InputKey(KeyCode.LeftCommand, "LeftCommand");
        public static InputKey LeftApple = new InputKey(KeyCode.LeftApple, "LeftApple");
        public static InputKey LeftWindows = new InputKey(KeyCode.LeftWindows, "LeftWindows");
        public static InputKey RightWindows = new InputKey(KeyCode.RightWindows, "RightWindows");
        public static InputKey AltGr = new InputKey(KeyCode.AltGr, "AltGr");
        public static InputKey Help = new InputKey(KeyCode.Help, "Help");
        public static InputKey Print = new InputKey(KeyCode.Print, "Print");
        public static InputKey SysReq = new InputKey(KeyCode.SysReq, "SysReq");
        public static InputKey Break = new InputKey(KeyCode.Break, "Break");
        public static InputKey Menu = new InputKey(KeyCode.Menu, "Menu");
        public static InputKey Mouse0 = new InputKey(KeyCode.Mouse0, "Mouse0");
        public static InputKey Mouse1 = new InputKey(KeyCode.Mouse1, "Mouse1");
        public static InputKey Mouse2 = new InputKey(KeyCode.Mouse2, "Mouse2");
        public static InputKey Mouse3 = new InputKey(KeyCode.Mouse3, "Mouse3");
        public static InputKey Mouse4 = new InputKey(KeyCode.Mouse4, "Mouse4");
        public static InputKey Mouse5 = new InputKey(KeyCode.Mouse5, "Mouse5");
        public static InputKey Mouse6 = new InputKey(KeyCode.Mouse6, "Mouse6");
        public static InputKey JoystickButton0 = new InputKey(KeyCode.JoystickButton0, "JoystickButton0");
        public static InputKey JoystickButton1 = new InputKey(KeyCode.JoystickButton1, "JoystickButton1");
        public static InputKey JoystickButton2 = new InputKey(KeyCode.JoystickButton2, "JoystickButton2");
        public static InputKey JoystickButton3 = new InputKey(KeyCode.JoystickButton3, "JoystickButton3");
        public static InputKey JoystickButton4 = new InputKey(KeyCode.JoystickButton4, "JoystickButton4");
        public static InputKey JoystickButton5 = new InputKey(KeyCode.JoystickButton5, "JoystickButton5");
        public static InputKey JoystickButton6 = new InputKey(KeyCode.JoystickButton6, "JoystickButton6");
        public static InputKey JoystickButton7 = new InputKey(KeyCode.JoystickButton7, "JoystickButton7");
        public static InputKey JoystickButton8 = new InputKey(KeyCode.JoystickButton8, "JoystickButton8");
        public static InputKey JoystickButton9 = new InputKey(KeyCode.JoystickButton9, "JoystickButton9");
        public static InputKey JoystickButton10 = new InputKey(KeyCode.JoystickButton10, "JoystickButton10");
        public static InputKey JoystickButton11 = new InputKey(KeyCode.JoystickButton11, "JoystickButton11");
        public static InputKey JoystickButton12 = new InputKey(KeyCode.JoystickButton12, "JoystickButton12");
        public static InputKey JoystickButton13 = new InputKey(KeyCode.JoystickButton13, "JoystickButton13");
        public static InputKey JoystickButton14 = new InputKey(KeyCode.JoystickButton14, "JoystickButton14");
        public static InputKey JoystickButton15 = new InputKey(KeyCode.JoystickButton15, "JoystickButton15");
        public static InputKey JoystickButton16 = new InputKey(KeyCode.JoystickButton16, "JoystickButton16");
        public static InputKey JoystickButton17 = new InputKey(KeyCode.JoystickButton17, "JoystickButton17");
        public static InputKey JoystickButton18 = new InputKey(KeyCode.JoystickButton18, "JoystickButton18");
        public static InputKey JoystickButton19 = new InputKey(KeyCode.JoystickButton19, "JoystickButton19");
        public static InputKey Joystick1Button0 = new InputKey(KeyCode.Joystick1Button0, "Joystick1Button0");
        public static InputKey Joystick1Button1 = new InputKey(KeyCode.Joystick1Button1, "Joystick1Button1");
        public static InputKey Joystick1Button2 = new InputKey(KeyCode.Joystick1Button2, "Joystick1Button2");
        public static InputKey Joystick1Button3 = new InputKey(KeyCode.Joystick1Button3, "Joystick1Button3");
        public static InputKey Joystick1Button4 = new InputKey(KeyCode.Joystick1Button4, "Joystick1Button4");
        public static InputKey Joystick1Button5 = new InputKey(KeyCode.Joystick1Button5, "Joystick1Button5");
        public static InputKey Joystick1Button6 = new InputKey(KeyCode.Joystick1Button6, "Joystick1Button6");
        public static InputKey Joystick1Button7 = new InputKey(KeyCode.Joystick1Button7, "Joystick1Button7");
        public static InputKey Joystick1Button8 = new InputKey(KeyCode.Joystick1Button8, "Joystick1Button8");
        public static InputKey Joystick1Button9 = new InputKey(KeyCode.Joystick1Button9, "Joystick1Button9");
        public static InputKey Joystick1Button10 = new InputKey(KeyCode.Joystick1Button10, "Joystick1Button10");
        public static InputKey Joystick1Button11 = new InputKey(KeyCode.Joystick1Button11, "Joystick1Button11");
        public static InputKey Joystick1Button12 = new InputKey(KeyCode.Joystick1Button12, "Joystick1Button12");
        public static InputKey Joystick1Button13 = new InputKey(KeyCode.Joystick1Button13, "Joystick1Button13");
        public static InputKey Joystick1Button14 = new InputKey(KeyCode.Joystick1Button14, "Joystick1Button14");
        public static InputKey Joystick1Button15 = new InputKey(KeyCode.Joystick1Button15, "Joystick1Button15");
        public static InputKey Joystick1Button16 = new InputKey(KeyCode.Joystick1Button16, "Joystick1Button16");
        public static InputKey Joystick1Button17 = new InputKey(KeyCode.Joystick1Button17, "Joystick1Button17");
        public static InputKey Joystick1Button18 = new InputKey(KeyCode.Joystick1Button18, "Joystick1Button18");
        public static InputKey Joystick1Button19 = new InputKey(KeyCode.Joystick1Button19, "Joystick1Button19");
        public static InputKey Joystick2Button0 = new InputKey(KeyCode.Joystick2Button0, "Joystick2Button0");
        public static InputKey Joystick2Button1 = new InputKey(KeyCode.Joystick2Button1, "Joystick2Button1");
        public static InputKey Joystick2Button2 = new InputKey(KeyCode.Joystick2Button2, "Joystick2Button2");
        public static InputKey Joystick2Button3 = new InputKey(KeyCode.Joystick2Button3, "Joystick2Button3");
        public static InputKey Joystick2Button4 = new InputKey(KeyCode.Joystick2Button4, "Joystick2Button4");
        public static InputKey Joystick2Button5 = new InputKey(KeyCode.Joystick2Button5, "Joystick2Button5");
        public static InputKey Joystick2Button6 = new InputKey(KeyCode.Joystick2Button6, "Joystick2Button6");
        public static InputKey Joystick2Button7 = new InputKey(KeyCode.Joystick2Button7, "Joystick2Button7");
        public static InputKey Joystick2Button8 = new InputKey(KeyCode.Joystick2Button8, "Joystick2Button8");
        public static InputKey Joystick2Button9 = new InputKey(KeyCode.Joystick2Button9, "Joystick2Button9");
        public static InputKey Joystick2Button10 = new InputKey(KeyCode.Joystick2Button10, "Joystick2Button10");
        public static InputKey Joystick2Button11 = new InputKey(KeyCode.Joystick2Button11, "Joystick2Button11");
        public static InputKey Joystick2Button12 = new InputKey(KeyCode.Joystick2Button12, "Joystick2Button12");
        public static InputKey Joystick2Button13 = new InputKey(KeyCode.Joystick2Button13, "Joystick2Button13");
        public static InputKey Joystick2Button14 = new InputKey(KeyCode.Joystick2Button14, "Joystick2Button14");
        public static InputKey Joystick2Button15 = new InputKey(KeyCode.Joystick2Button15, "Joystick2Button15");
        public static InputKey Joystick2Button16 = new InputKey(KeyCode.Joystick2Button16, "Joystick2Button16");
        public static InputKey Joystick2Button17 = new InputKey(KeyCode.Joystick2Button17, "Joystick2Button17");
        public static InputKey Joystick2Button18 = new InputKey(KeyCode.Joystick2Button18, "Joystick2Button18");
        public static InputKey Joystick2Button19 = new InputKey(KeyCode.Joystick2Button19, "Joystick2Button19");
        public static InputKey Joystick3Button0 = new InputKey(KeyCode.Joystick3Button0, "Joystick3Button0");
        public static InputKey Joystick3Button1 = new InputKey(KeyCode.Joystick3Button1, "Joystick3Button1");
        public static InputKey Joystick3Button2 = new InputKey(KeyCode.Joystick3Button2, "Joystick3Button2");
        public static InputKey Joystick3Button3 = new InputKey(KeyCode.Joystick3Button3, "Joystick3Button3");
        public static InputKey Joystick3Button4 = new InputKey(KeyCode.Joystick3Button4, "Joystick3Button4");
        public static InputKey Joystick3Button5 = new InputKey(KeyCode.Joystick3Button5, "Joystick3Button5");
        public static InputKey Joystick3Button6 = new InputKey(KeyCode.Joystick3Button6, "Joystick3Button6");
        public static InputKey Joystick3Button7 = new InputKey(KeyCode.Joystick3Button7, "Joystick3Button7");
        public static InputKey Joystick3Button8 = new InputKey(KeyCode.Joystick3Button8, "Joystick3Button8");
        public static InputKey Joystick3Button9 = new InputKey(KeyCode.Joystick3Button9, "Joystick3Button9");
        public static InputKey Joystick3Button10 = new InputKey(KeyCode.Joystick3Button10, "Joystick3Button10");
        public static InputKey Joystick3Button11 = new InputKey(KeyCode.Joystick3Button11, "Joystick3Button11");
        public static InputKey Joystick3Button12 = new InputKey(KeyCode.Joystick3Button12, "Joystick3Button12");
        public static InputKey Joystick3Button13 = new InputKey(KeyCode.Joystick3Button13, "Joystick3Button13");
        public static InputKey Joystick3Button14 = new InputKey(KeyCode.Joystick3Button14, "Joystick3Button14");
        public static InputKey Joystick3Button15 = new InputKey(KeyCode.Joystick3Button15, "Joystick3Button15");
        public static InputKey Joystick3Button16 = new InputKey(KeyCode.Joystick3Button16, "Joystick3Button16");
        public static InputKey Joystick3Button17 = new InputKey(KeyCode.Joystick3Button17, "Joystick3Button17");
        public static InputKey Joystick3Button18 = new InputKey(KeyCode.Joystick3Button18, "Joystick3Button18");
        public static InputKey Joystick3Button19 = new InputKey(KeyCode.Joystick3Button19, "Joystick3Button19");
        public static InputKey Joystick4Button0 = new InputKey(KeyCode.Joystick4Button0, "Joystick4Button0");
        public static InputKey Joystick4Button1 = new InputKey(KeyCode.Joystick4Button1, "Joystick4Button1");
        public static InputKey Joystick4Button2 = new InputKey(KeyCode.Joystick4Button2, "Joystick4Button2");
        public static InputKey Joystick4Button3 = new InputKey(KeyCode.Joystick4Button3, "Joystick4Button3");
        public static InputKey Joystick4Button4 = new InputKey(KeyCode.Joystick4Button4, "Joystick4Button4");
        public static InputKey Joystick4Button5 = new InputKey(KeyCode.Joystick4Button5, "Joystick4Button5");
        public static InputKey Joystick4Button6 = new InputKey(KeyCode.Joystick4Button6, "Joystick4Button6");
        public static InputKey Joystick4Button7 = new InputKey(KeyCode.Joystick4Button7, "Joystick4Button7");
        public static InputKey Joystick4Button8 = new InputKey(KeyCode.Joystick4Button8, "Joystick4Button8");
        public static InputKey Joystick4Button9 = new InputKey(KeyCode.Joystick4Button9, "Joystick4Button9");
        public static InputKey Joystick4Button10 = new InputKey(KeyCode.Joystick4Button10, "Joystick4Button10");
        public static InputKey Joystick4Button11 = new InputKey(KeyCode.Joystick4Button11, "Joystick4Button11");
        public static InputKey Joystick4Button12 = new InputKey(KeyCode.Joystick4Button12, "Joystick4Button12");
        public static InputKey Joystick4Button13 = new InputKey(KeyCode.Joystick4Button13, "Joystick4Button13");
        public static InputKey Joystick4Button14 = new InputKey(KeyCode.Joystick4Button14, "Joystick4Button14");
        public static InputKey Joystick4Button15 = new InputKey(KeyCode.Joystick4Button15, "Joystick4Button15");
        public static InputKey Joystick4Button16 = new InputKey(KeyCode.Joystick4Button16, "Joystick4Button16");
        public static InputKey Joystick4Button17 = new InputKey(KeyCode.Joystick4Button17, "Joystick4Button17");
        public static InputKey Joystick4Button18 = new InputKey(KeyCode.Joystick4Button18, "Joystick4Button18");
        public static InputKey Joystick4Button19 = new InputKey(KeyCode.Joystick4Button19, "Joystick4Button19");
        public static InputKey Joystick5Button0 = new InputKey(KeyCode.Joystick5Button0, "Joystick5Button0");
        public static InputKey Joystick5Button1 = new InputKey(KeyCode.Joystick5Button1, "Joystick5Button1");
        public static InputKey Joystick5Button2 = new InputKey(KeyCode.Joystick5Button2, "Joystick5Button2");
        public static InputKey Joystick5Button3 = new InputKey(KeyCode.Joystick5Button3, "Joystick5Button3");
        public static InputKey Joystick5Button4 = new InputKey(KeyCode.Joystick5Button4, "Joystick5Button4");
        public static InputKey Joystick5Button5 = new InputKey(KeyCode.Joystick5Button5, "Joystick5Button5");
        public static InputKey Joystick5Button6 = new InputKey(KeyCode.Joystick5Button6, "Joystick5Button6");
        public static InputKey Joystick5Button7 = new InputKey(KeyCode.Joystick5Button7, "Joystick5Button7");
        public static InputKey Joystick5Button8 = new InputKey(KeyCode.Joystick5Button8, "Joystick5Button8");
        public static InputKey Joystick5Button9 = new InputKey(KeyCode.Joystick5Button9, "Joystick5Button9");
        public static InputKey Joystick5Button10 = new InputKey(KeyCode.Joystick5Button10, "Joystick5Button10");
        public static InputKey Joystick5Button11 = new InputKey(KeyCode.Joystick5Button11, "Joystick5Button11");
        public static InputKey Joystick5Button12 = new InputKey(KeyCode.Joystick5Button12, "Joystick5Button12");
        public static InputKey Joystick5Button13 = new InputKey(KeyCode.Joystick5Button13, "Joystick5Button13");
        public static InputKey Joystick5Button14 = new InputKey(KeyCode.Joystick5Button14, "Joystick5Button14");
        public static InputKey Joystick5Button15 = new InputKey(KeyCode.Joystick5Button15, "Joystick5Button15");
        public static InputKey Joystick5Button16 = new InputKey(KeyCode.Joystick5Button16, "Joystick5Button16");
        public static InputKey Joystick5Button17 = new InputKey(KeyCode.Joystick5Button17, "Joystick5Button17");
        public static InputKey Joystick5Button18 = new InputKey(KeyCode.Joystick5Button18, "Joystick5Button18");
        public static InputKey Joystick5Button19 = new InputKey(KeyCode.Joystick5Button19, "Joystick5Button19");
        public static InputKey Joystick6Button0 = new InputKey(KeyCode.Joystick6Button0, "Joystick6Button0");
        public static InputKey Joystick6Button1 = new InputKey(KeyCode.Joystick6Button1, "Joystick6Button1");
        public static InputKey Joystick6Button2 = new InputKey(KeyCode.Joystick6Button2, "Joystick6Button2");
        public static InputKey Joystick6Button3 = new InputKey(KeyCode.Joystick6Button3, "Joystick6Button3");
        public static InputKey Joystick6Button4 = new InputKey(KeyCode.Joystick6Button4, "Joystick6Button4");
        public static InputKey Joystick6Button5 = new InputKey(KeyCode.Joystick6Button5, "Joystick6Button5");
        public static InputKey Joystick6Button6 = new InputKey(KeyCode.Joystick6Button6, "Joystick6Button6");
        public static InputKey Joystick6Button7 = new InputKey(KeyCode.Joystick6Button7, "Joystick6Button7");
        public static InputKey Joystick6Button8 = new InputKey(KeyCode.Joystick6Button8, "Joystick6Button8");
        public static InputKey Joystick6Button9 = new InputKey(KeyCode.Joystick6Button9, "Joystick6Button9");
        public static InputKey Joystick6Button10 = new InputKey(KeyCode.Joystick6Button10, "Joystick6Button10");
        public static InputKey Joystick6Button11 = new InputKey(KeyCode.Joystick6Button11, "Joystick6Button11");
        public static InputKey Joystick6Button12 = new InputKey(KeyCode.Joystick6Button12, "Joystick6Button12");
        public static InputKey Joystick6Button13 = new InputKey(KeyCode.Joystick6Button13, "Joystick6Button13");
        public static InputKey Joystick6Button14 = new InputKey(KeyCode.Joystick6Button14, "Joystick6Button14");
        public static InputKey Joystick6Button15 = new InputKey(KeyCode.Joystick6Button15, "Joystick6Button15");
        public static InputKey Joystick6Button16 = new InputKey(KeyCode.Joystick6Button16, "Joystick6Button16");
        public static InputKey Joystick6Button17 = new InputKey(KeyCode.Joystick6Button17, "Joystick6Button17");
        public static InputKey Joystick6Button18 = new InputKey(KeyCode.Joystick6Button18, "Joystick6Button18");
        public static InputKey Joystick6Button19 = new InputKey(KeyCode.Joystick6Button19, "Joystick6Button19");
        public static InputKey Joystick7Button0 = new InputKey(KeyCode.Joystick7Button0, "Joystick7Button0");
        public static InputKey Joystick7Button1 = new InputKey(KeyCode.Joystick7Button1, "Joystick7Button1");
        public static InputKey Joystick7Button2 = new InputKey(KeyCode.Joystick7Button2, "Joystick7Button2");
        public static InputKey Joystick7Button3 = new InputKey(KeyCode.Joystick7Button3, "Joystick7Button3");
        public static InputKey Joystick7Button4 = new InputKey(KeyCode.Joystick7Button4, "Joystick7Button4");
        public static InputKey Joystick7Button5 = new InputKey(KeyCode.Joystick7Button5, "Joystick7Button5");
        public static InputKey Joystick7Button6 = new InputKey(KeyCode.Joystick7Button6, "Joystick7Button6");
        public static InputKey Joystick7Button7 = new InputKey(KeyCode.Joystick7Button7, "Joystick7Button7");
        public static InputKey Joystick7Button8 = new InputKey(KeyCode.Joystick7Button8, "Joystick7Button8");
        public static InputKey Joystick7Button9 = new InputKey(KeyCode.Joystick7Button9, "Joystick7Button9");
        public static InputKey Joystick7Button10 = new InputKey(KeyCode.Joystick7Button10, "Joystick7Button10");
        public static InputKey Joystick7Button11 = new InputKey(KeyCode.Joystick7Button11, "Joystick7Button11");
        public static InputKey Joystick7Button12 = new InputKey(KeyCode.Joystick7Button12, "Joystick7Button12");
        public static InputKey Joystick7Button13 = new InputKey(KeyCode.Joystick7Button13, "Joystick7Button13");
        public static InputKey Joystick7Button14 = new InputKey(KeyCode.Joystick7Button14, "Joystick7Button14");
        public static InputKey Joystick7Button15 = new InputKey(KeyCode.Joystick7Button15, "Joystick7Button15");
        public static InputKey Joystick7Button16 = new InputKey(KeyCode.Joystick7Button16, "Joystick7Button16");
        public static InputKey Joystick7Button17 = new InputKey(KeyCode.Joystick7Button17, "Joystick7Button17");
        public static InputKey Joystick7Button18 = new InputKey(KeyCode.Joystick7Button18, "Joystick7Button18");
        public static InputKey Joystick7Button19 = new InputKey(KeyCode.Joystick7Button19, "Joystick7Button19");
        public static InputKey Joystick8Button0 = new InputKey(KeyCode.Joystick8Button0, "Joystick8Button0");
        public static InputKey Joystick8Button1 = new InputKey(KeyCode.Joystick8Button1, "Joystick8Button1");
        public static InputKey Joystick8Button2 = new InputKey(KeyCode.Joystick8Button2, "Joystick8Button2");
        public static InputKey Joystick8Button3 = new InputKey(KeyCode.Joystick8Button3, "Joystick8Button3");
        public static InputKey Joystick8Button4 = new InputKey(KeyCode.Joystick8Button4, "Joystick8Button4");
        public static InputKey Joystick8Button5 = new InputKey(KeyCode.Joystick8Button5, "Joystick8Button5");
        public static InputKey Joystick8Button6 = new InputKey(KeyCode.Joystick8Button6, "Joystick8Button6");
        public static InputKey Joystick8Button7 = new InputKey(KeyCode.Joystick8Button7, "Joystick8Button7");
        public static InputKey Joystick8Button8 = new InputKey(KeyCode.Joystick8Button8, "Joystick8Button8");
        public static InputKey Joystick8Button9 = new InputKey(KeyCode.Joystick8Button9, "Joystick8Button9");
        public static InputKey Joystick8Button10 = new InputKey(KeyCode.Joystick8Button10, "Joystick8Button10");
        public static InputKey Joystick8Button11 = new InputKey(KeyCode.Joystick8Button11, "Joystick8Button11");
        public static InputKey Joystick8Button12 = new InputKey(KeyCode.Joystick8Button12, "Joystick8Button12");
        public static InputKey Joystick8Button13 = new InputKey(KeyCode.Joystick8Button13, "Joystick8Button13");
        public static InputKey Joystick8Button14 = new InputKey(KeyCode.Joystick8Button14, "Joystick8Button14");
        public static InputKey Joystick8Button15 = new InputKey(KeyCode.Joystick8Button15, "Joystick8Button15");
        public static InputKey Joystick8Button16 = new InputKey(KeyCode.Joystick8Button16, "Joystick8Button16");
        public static InputKey Joystick8Button17 = new InputKey(KeyCode.Joystick8Button17, "Joystick8Button17");
        public static InputKey Joystick8Button18 = new InputKey(KeyCode.Joystick8Button18, "Joystick8Button18");
        public static InputKey Joystick8Button19 = new InputKey(KeyCode.Joystick8Button19, "Joystick8Button19");

        public static InputKey[] mInputKeyArray;

        /**
         * A dictionary mapping the string names of all the keys to the InputKey they represent.
         */
        public static InputKey[] getInputKeyArray()
        {
            if (mInputKeyArray == null)
            {
                mInputKeyArray = new InputKey[(int)KeyCode.Joystick8Button19 + 1];
                mInputKeyArray[(int)KeyCode.None] = None;
                mInputKeyArray[(int)KeyCode.Backspace] = Backspace;
                mInputKeyArray[(int)KeyCode.Tab] = Tab;
                mInputKeyArray[(int)KeyCode.Clear] = Clear;
                mInputKeyArray[(int)KeyCode.Return] = Return;
                mInputKeyArray[(int)KeyCode.Pause] = Pause;
                mInputKeyArray[(int)KeyCode.Escape] = Escape;
                mInputKeyArray[(int)KeyCode.Space] = Space;
                mInputKeyArray[(int)KeyCode.Exclaim] = Exclaim;
                mInputKeyArray[(int)KeyCode.DoubleQuote] = DoubleQuote;
                mInputKeyArray[(int)KeyCode.Hash] = Hash;
                mInputKeyArray[(int)KeyCode.Dollar] = Dollar;
                mInputKeyArray[(int)KeyCode.Ampersand] = Ampersand;
                mInputKeyArray[(int)KeyCode.Quote] = Quote;
                mInputKeyArray[(int)KeyCode.LeftParen] = LeftParen;
                mInputKeyArray[(int)KeyCode.RightParen] = RightParen;
                mInputKeyArray[(int)KeyCode.Asterisk] = Asterisk;
                mInputKeyArray[(int)KeyCode.Plus] = Plus;
                mInputKeyArray[(int)KeyCode.Comma] = Comma;
                mInputKeyArray[(int)KeyCode.Minus] = Minus;
                mInputKeyArray[(int)KeyCode.Period] = Period;
                mInputKeyArray[(int)KeyCode.Slash] = Slash;
                mInputKeyArray[(int)KeyCode.Alpha0] = Alpha0;
                mInputKeyArray[(int)KeyCode.Alpha1] = Alpha1;
                mInputKeyArray[(int)KeyCode.Alpha2] = Alpha2;
                mInputKeyArray[(int)KeyCode.Alpha3] = Alpha3;
                mInputKeyArray[(int)KeyCode.Alpha4] = Alpha4;
                mInputKeyArray[(int)KeyCode.Alpha5] = Alpha5;
                mInputKeyArray[(int)KeyCode.Alpha6] = Alpha6;
                mInputKeyArray[(int)KeyCode.Alpha7] = Alpha7;
                mInputKeyArray[(int)KeyCode.Alpha8] = Alpha8;
                mInputKeyArray[(int)KeyCode.Alpha9] = Alpha9;
                mInputKeyArray[(int)KeyCode.Colon] = Colon;
                mInputKeyArray[(int)KeyCode.Semicolon] = Semicolon;
                mInputKeyArray[(int)KeyCode.Less] = Less;
                mInputKeyArray[(int)KeyCode.Equals] = Equals;
                mInputKeyArray[(int)KeyCode.Greater] = Greater;
                mInputKeyArray[(int)KeyCode.Question] = Question;
                mInputKeyArray[(int)KeyCode.At] = At;
                mInputKeyArray[(int)KeyCode.LeftBracket] = LeftBracket;
                mInputKeyArray[(int)KeyCode.Backslash] = Backslash;
                mInputKeyArray[(int)KeyCode.RightBracket] = RightBracket;
                mInputKeyArray[(int)KeyCode.Caret] = Caret;
                mInputKeyArray[(int)KeyCode.Underscore] = Underscore;
                mInputKeyArray[(int)KeyCode.BackQuote] = BackQuote;
                mInputKeyArray[(int)KeyCode.A] = A;
                mInputKeyArray[(int)KeyCode.B] = B;
                mInputKeyArray[(int)KeyCode.C] = C;
                mInputKeyArray[(int)KeyCode.D] = D;
                mInputKeyArray[(int)KeyCode.E] = E;
                mInputKeyArray[(int)KeyCode.F] = F;
                mInputKeyArray[(int)KeyCode.G] = G;
                mInputKeyArray[(int)KeyCode.H] = H;
                mInputKeyArray[(int)KeyCode.I] = I;
                mInputKeyArray[(int)KeyCode.J] = J;
                mInputKeyArray[(int)KeyCode.K] = K;
                mInputKeyArray[(int)KeyCode.L] = L;
                mInputKeyArray[(int)KeyCode.M] = M;
                mInputKeyArray[(int)KeyCode.N] = N;
                mInputKeyArray[(int)KeyCode.O] = O;
                mInputKeyArray[(int)KeyCode.P] = P;
                mInputKeyArray[(int)KeyCode.Q] = Q;
                mInputKeyArray[(int)KeyCode.R] = R;
                mInputKeyArray[(int)KeyCode.S] = S;
                mInputKeyArray[(int)KeyCode.T] = T;
                mInputKeyArray[(int)KeyCode.U] = U;
                mInputKeyArray[(int)KeyCode.V] = V;
                mInputKeyArray[(int)KeyCode.W] = W;
                mInputKeyArray[(int)KeyCode.X] = X;
                mInputKeyArray[(int)KeyCode.Y] = Y;
                mInputKeyArray[(int)KeyCode.Z] = Z;
                mInputKeyArray[(int)KeyCode.Delete] = Delete;
                mInputKeyArray[(int)KeyCode.Keypad0] = Keypad0;
                mInputKeyArray[(int)KeyCode.Keypad1] = Keypad1;
                mInputKeyArray[(int)KeyCode.Keypad2] = Keypad2;
                mInputKeyArray[(int)KeyCode.Keypad3] = Keypad3;
                mInputKeyArray[(int)KeyCode.Keypad4] = Keypad4;
                mInputKeyArray[(int)KeyCode.Keypad5] = Keypad5;
                mInputKeyArray[(int)KeyCode.Keypad6] = Keypad6;
                mInputKeyArray[(int)KeyCode.Keypad7] = Keypad7;
                mInputKeyArray[(int)KeyCode.Keypad8] = Keypad8;
                mInputKeyArray[(int)KeyCode.Keypad9] = Keypad9;
                mInputKeyArray[(int)KeyCode.KeypadPeriod] = KeypadPeriod;
                mInputKeyArray[(int)KeyCode.KeypadDivide] = KeypadDivide;
                mInputKeyArray[(int)KeyCode.KeypadMultiply] = KeypadMultiply;
                mInputKeyArray[(int)KeyCode.KeypadMinus] = KeypadMinus;
                mInputKeyArray[(int)KeyCode.KeypadPlus] = KeypadPlus;
                mInputKeyArray[(int)KeyCode.KeypadEnter] = KeypadEnter;
                mInputKeyArray[(int)KeyCode.KeypadEquals] = KeypadEquals;
                mInputKeyArray[(int)KeyCode.UpArrow] = UpArrow;
                mInputKeyArray[(int)KeyCode.DownArrow] = DownArrow;
                mInputKeyArray[(int)KeyCode.RightArrow] = RightArrow;
                mInputKeyArray[(int)KeyCode.LeftArrow] = LeftArrow;
                mInputKeyArray[(int)KeyCode.Insert] = Insert;
                mInputKeyArray[(int)KeyCode.Home] = Home;
                mInputKeyArray[(int)KeyCode.End] = End;
                mInputKeyArray[(int)KeyCode.PageUp] = PageUp;
                mInputKeyArray[(int)KeyCode.PageDown] = PageDown;
                mInputKeyArray[(int)KeyCode.F1] = F1;
                mInputKeyArray[(int)KeyCode.F2] = F2;
                mInputKeyArray[(int)KeyCode.F3] = F3;
                mInputKeyArray[(int)KeyCode.F4] = F4;
                mInputKeyArray[(int)KeyCode.F5] = F5;
                mInputKeyArray[(int)KeyCode.F6] = F6;
                mInputKeyArray[(int)KeyCode.F7] = F7;
                mInputKeyArray[(int)KeyCode.F8] = F8;
                mInputKeyArray[(int)KeyCode.F9] = F9;
                mInputKeyArray[(int)KeyCode.F10] = F10;
                mInputKeyArray[(int)KeyCode.F11] = F11;
                mInputKeyArray[(int)KeyCode.F12] = F12;
                mInputKeyArray[(int)KeyCode.F13] = F13;
                mInputKeyArray[(int)KeyCode.F14] = F14;
                mInputKeyArray[(int)KeyCode.F15] = F15;
                mInputKeyArray[(int)KeyCode.Numlock] = Numlock;
                mInputKeyArray[(int)KeyCode.CapsLock] = CapsLock;
                mInputKeyArray[(int)KeyCode.ScrollLock] = ScrollLock;
                mInputKeyArray[(int)KeyCode.RightShift] = RightShift;
                mInputKeyArray[(int)KeyCode.LeftShift] = LeftShift;
                mInputKeyArray[(int)KeyCode.RightControl] = RightControl;
                mInputKeyArray[(int)KeyCode.LeftControl] = LeftControl;
                mInputKeyArray[(int)KeyCode.RightAlt] = RightAlt;
                mInputKeyArray[(int)KeyCode.LeftAlt] = LeftAlt;
                mInputKeyArray[(int)KeyCode.RightCommand] = RightCommand;
                mInputKeyArray[(int)KeyCode.RightApple] = RightApple;
                mInputKeyArray[(int)KeyCode.LeftCommand] = LeftCommand;
                mInputKeyArray[(int)KeyCode.LeftApple] = LeftApple;
                mInputKeyArray[(int)KeyCode.LeftWindows] = LeftWindows;
                mInputKeyArray[(int)KeyCode.RightWindows] = RightWindows;
                mInputKeyArray[(int)KeyCode.AltGr] = AltGr;
                mInputKeyArray[(int)KeyCode.Help] = Help;
                mInputKeyArray[(int)KeyCode.Print] = Print;
                mInputKeyArray[(int)KeyCode.SysReq] = SysReq;
                mInputKeyArray[(int)KeyCode.Break] = Break;
                mInputKeyArray[(int)KeyCode.Menu] = Menu;
                mInputKeyArray[(int)KeyCode.Mouse0] = Mouse0;
                mInputKeyArray[(int)KeyCode.Mouse1] = Mouse1;
                mInputKeyArray[(int)KeyCode.Mouse2] = Mouse2;
                mInputKeyArray[(int)KeyCode.Mouse3] = Mouse3;
                mInputKeyArray[(int)KeyCode.Mouse4] = Mouse4;
                mInputKeyArray[(int)KeyCode.Mouse5] = Mouse5;
                mInputKeyArray[(int)KeyCode.Mouse6] = Mouse6;
                mInputKeyArray[(int)KeyCode.JoystickButton0] = JoystickButton0;
                mInputKeyArray[(int)KeyCode.JoystickButton1] = JoystickButton1;
                mInputKeyArray[(int)KeyCode.JoystickButton2] = JoystickButton2;
                mInputKeyArray[(int)KeyCode.JoystickButton3] = JoystickButton3;
                mInputKeyArray[(int)KeyCode.JoystickButton4] = JoystickButton4;
                mInputKeyArray[(int)KeyCode.JoystickButton5] = JoystickButton5;
                mInputKeyArray[(int)KeyCode.JoystickButton6] = JoystickButton6;
                mInputKeyArray[(int)KeyCode.JoystickButton7] = JoystickButton7;
                mInputKeyArray[(int)KeyCode.JoystickButton8] = JoystickButton8;
                mInputKeyArray[(int)KeyCode.JoystickButton9] = JoystickButton9;
                mInputKeyArray[(int)KeyCode.JoystickButton10] = JoystickButton10;
                mInputKeyArray[(int)KeyCode.JoystickButton11] = JoystickButton11;
                mInputKeyArray[(int)KeyCode.JoystickButton12] = JoystickButton12;
                mInputKeyArray[(int)KeyCode.JoystickButton13] = JoystickButton13;
                mInputKeyArray[(int)KeyCode.JoystickButton14] = JoystickButton14;
                mInputKeyArray[(int)KeyCode.JoystickButton15] = JoystickButton15;
                mInputKeyArray[(int)KeyCode.JoystickButton16] = JoystickButton16;
                mInputKeyArray[(int)KeyCode.JoystickButton17] = JoystickButton17;
                mInputKeyArray[(int)KeyCode.JoystickButton18] = JoystickButton18;
                mInputKeyArray[(int)KeyCode.JoystickButton19] = JoystickButton19;
                mInputKeyArray[(int)KeyCode.Joystick1Button0] = Joystick1Button0;
                mInputKeyArray[(int)KeyCode.Joystick1Button1] = Joystick1Button1;
                mInputKeyArray[(int)KeyCode.Joystick1Button2] = Joystick1Button2;
                mInputKeyArray[(int)KeyCode.Joystick1Button3] = Joystick1Button3;
                mInputKeyArray[(int)KeyCode.Joystick1Button4] = Joystick1Button4;
                mInputKeyArray[(int)KeyCode.Joystick1Button5] = Joystick1Button5;
                mInputKeyArray[(int)KeyCode.Joystick1Button6] = Joystick1Button6;
                mInputKeyArray[(int)KeyCode.Joystick1Button7] = Joystick1Button7;
                mInputKeyArray[(int)KeyCode.Joystick1Button8] = Joystick1Button8;
                mInputKeyArray[(int)KeyCode.Joystick1Button9] = Joystick1Button9;
                mInputKeyArray[(int)KeyCode.Joystick1Button10] = Joystick1Button10;
                mInputKeyArray[(int)KeyCode.Joystick1Button11] = Joystick1Button11;
                mInputKeyArray[(int)KeyCode.Joystick1Button12] = Joystick1Button12;
                mInputKeyArray[(int)KeyCode.Joystick1Button13] = Joystick1Button13;
                mInputKeyArray[(int)KeyCode.Joystick1Button14] = Joystick1Button14;
                mInputKeyArray[(int)KeyCode.Joystick1Button15] = Joystick1Button15;
                mInputKeyArray[(int)KeyCode.Joystick1Button16] = Joystick1Button16;
                mInputKeyArray[(int)KeyCode.Joystick1Button17] = Joystick1Button17;
                mInputKeyArray[(int)KeyCode.Joystick1Button18] = Joystick1Button18;
                mInputKeyArray[(int)KeyCode.Joystick1Button19] = Joystick1Button19;
                mInputKeyArray[(int)KeyCode.Joystick2Button0] = Joystick2Button0;
                mInputKeyArray[(int)KeyCode.Joystick2Button1] = Joystick2Button1;
                mInputKeyArray[(int)KeyCode.Joystick2Button2] = Joystick2Button2;
                mInputKeyArray[(int)KeyCode.Joystick2Button3] = Joystick2Button3;
                mInputKeyArray[(int)KeyCode.Joystick2Button4] = Joystick2Button4;
                mInputKeyArray[(int)KeyCode.Joystick2Button5] = Joystick2Button5;
                mInputKeyArray[(int)KeyCode.Joystick2Button6] = Joystick2Button6;
                mInputKeyArray[(int)KeyCode.Joystick2Button7] = Joystick2Button7;
                mInputKeyArray[(int)KeyCode.Joystick2Button8] = Joystick2Button8;
                mInputKeyArray[(int)KeyCode.Joystick2Button9] = Joystick2Button9;
                mInputKeyArray[(int)KeyCode.Joystick2Button10] = Joystick2Button10;
                mInputKeyArray[(int)KeyCode.Joystick2Button11] = Joystick2Button11;
                mInputKeyArray[(int)KeyCode.Joystick2Button12] = Joystick2Button12;
                mInputKeyArray[(int)KeyCode.Joystick2Button13] = Joystick2Button13;
                mInputKeyArray[(int)KeyCode.Joystick2Button14] = Joystick2Button14;
                mInputKeyArray[(int)KeyCode.Joystick2Button15] = Joystick2Button15;
                mInputKeyArray[(int)KeyCode.Joystick2Button16] = Joystick2Button16;
                mInputKeyArray[(int)KeyCode.Joystick2Button17] = Joystick2Button17;
                mInputKeyArray[(int)KeyCode.Joystick2Button18] = Joystick2Button18;
                mInputKeyArray[(int)KeyCode.Joystick2Button19] = Joystick2Button19;
                mInputKeyArray[(int)KeyCode.Joystick3Button0] = Joystick3Button0;
                mInputKeyArray[(int)KeyCode.Joystick3Button1] = Joystick3Button1;
                mInputKeyArray[(int)KeyCode.Joystick3Button2] = Joystick3Button2;
                mInputKeyArray[(int)KeyCode.Joystick3Button3] = Joystick3Button3;
                mInputKeyArray[(int)KeyCode.Joystick3Button4] = Joystick3Button4;
                mInputKeyArray[(int)KeyCode.Joystick3Button5] = Joystick3Button5;
                mInputKeyArray[(int)KeyCode.Joystick3Button6] = Joystick3Button6;
                mInputKeyArray[(int)KeyCode.Joystick3Button7] = Joystick3Button7;
                mInputKeyArray[(int)KeyCode.Joystick3Button8] = Joystick3Button8;
                mInputKeyArray[(int)KeyCode.Joystick3Button9] = Joystick3Button9;
                mInputKeyArray[(int)KeyCode.Joystick3Button10] = Joystick3Button10;
                mInputKeyArray[(int)KeyCode.Joystick3Button11] = Joystick3Button11;
                mInputKeyArray[(int)KeyCode.Joystick3Button12] = Joystick3Button12;
                mInputKeyArray[(int)KeyCode.Joystick3Button13] = Joystick3Button13;
                mInputKeyArray[(int)KeyCode.Joystick3Button14] = Joystick3Button14;
                mInputKeyArray[(int)KeyCode.Joystick3Button15] = Joystick3Button15;
                mInputKeyArray[(int)KeyCode.Joystick3Button16] = Joystick3Button16;
                mInputKeyArray[(int)KeyCode.Joystick3Button17] = Joystick3Button17;
                mInputKeyArray[(int)KeyCode.Joystick3Button18] = Joystick3Button18;
                mInputKeyArray[(int)KeyCode.Joystick3Button19] = Joystick3Button19;
                mInputKeyArray[(int)KeyCode.Joystick4Button0] = Joystick4Button0;
                mInputKeyArray[(int)KeyCode.Joystick4Button1] = Joystick4Button1;
                mInputKeyArray[(int)KeyCode.Joystick4Button2] = Joystick4Button2;
                mInputKeyArray[(int)KeyCode.Joystick4Button3] = Joystick4Button3;
                mInputKeyArray[(int)KeyCode.Joystick4Button4] = Joystick4Button4;
                mInputKeyArray[(int)KeyCode.Joystick4Button5] = Joystick4Button5;
                mInputKeyArray[(int)KeyCode.Joystick4Button6] = Joystick4Button6;
                mInputKeyArray[(int)KeyCode.Joystick4Button7] = Joystick4Button7;
                mInputKeyArray[(int)KeyCode.Joystick4Button8] = Joystick4Button8;
                mInputKeyArray[(int)KeyCode.Joystick4Button9] = Joystick4Button9;
                mInputKeyArray[(int)KeyCode.Joystick4Button10] = Joystick4Button10;
                mInputKeyArray[(int)KeyCode.Joystick4Button11] = Joystick4Button11;
                mInputKeyArray[(int)KeyCode.Joystick4Button12] = Joystick4Button12;
                mInputKeyArray[(int)KeyCode.Joystick4Button13] = Joystick4Button13;
                mInputKeyArray[(int)KeyCode.Joystick4Button14] = Joystick4Button14;
                mInputKeyArray[(int)KeyCode.Joystick4Button15] = Joystick4Button15;
                mInputKeyArray[(int)KeyCode.Joystick4Button16] = Joystick4Button16;
                mInputKeyArray[(int)KeyCode.Joystick4Button17] = Joystick4Button17;
                mInputKeyArray[(int)KeyCode.Joystick4Button18] = Joystick4Button18;
                mInputKeyArray[(int)KeyCode.Joystick4Button19] = Joystick4Button19;
                mInputKeyArray[(int)KeyCode.Joystick5Button0] = Joystick5Button0;
                mInputKeyArray[(int)KeyCode.Joystick5Button1] = Joystick5Button1;
                mInputKeyArray[(int)KeyCode.Joystick5Button2] = Joystick5Button2;
                mInputKeyArray[(int)KeyCode.Joystick5Button3] = Joystick5Button3;
                mInputKeyArray[(int)KeyCode.Joystick5Button4] = Joystick5Button4;
                mInputKeyArray[(int)KeyCode.Joystick5Button5] = Joystick5Button5;
                mInputKeyArray[(int)KeyCode.Joystick5Button6] = Joystick5Button6;
                mInputKeyArray[(int)KeyCode.Joystick5Button7] = Joystick5Button7;
                mInputKeyArray[(int)KeyCode.Joystick5Button8] = Joystick5Button8;
                mInputKeyArray[(int)KeyCode.Joystick5Button9] = Joystick5Button9;
                mInputKeyArray[(int)KeyCode.Joystick5Button10] = Joystick5Button10;
                mInputKeyArray[(int)KeyCode.Joystick5Button11] = Joystick5Button11;
                mInputKeyArray[(int)KeyCode.Joystick5Button12] = Joystick5Button12;
                mInputKeyArray[(int)KeyCode.Joystick5Button13] = Joystick5Button13;
                mInputKeyArray[(int)KeyCode.Joystick5Button14] = Joystick5Button14;
                mInputKeyArray[(int)KeyCode.Joystick5Button15] = Joystick5Button15;
                mInputKeyArray[(int)KeyCode.Joystick5Button16] = Joystick5Button16;
                mInputKeyArray[(int)KeyCode.Joystick5Button17] = Joystick5Button17;
                mInputKeyArray[(int)KeyCode.Joystick5Button18] = Joystick5Button18;
                mInputKeyArray[(int)KeyCode.Joystick5Button19] = Joystick5Button19;
                mInputKeyArray[(int)KeyCode.Joystick6Button0] = Joystick6Button0;
                mInputKeyArray[(int)KeyCode.Joystick6Button1] = Joystick6Button1;
                mInputKeyArray[(int)KeyCode.Joystick6Button2] = Joystick6Button2;
                mInputKeyArray[(int)KeyCode.Joystick6Button3] = Joystick6Button3;
                mInputKeyArray[(int)KeyCode.Joystick6Button4] = Joystick6Button4;
                mInputKeyArray[(int)KeyCode.Joystick6Button5] = Joystick6Button5;
                mInputKeyArray[(int)KeyCode.Joystick6Button6] = Joystick6Button6;
                mInputKeyArray[(int)KeyCode.Joystick6Button7] = Joystick6Button7;
                mInputKeyArray[(int)KeyCode.Joystick6Button8] = Joystick6Button8;
                mInputKeyArray[(int)KeyCode.Joystick6Button9] = Joystick6Button9;
                mInputKeyArray[(int)KeyCode.Joystick6Button10] = Joystick6Button10;
                mInputKeyArray[(int)KeyCode.Joystick6Button11] = Joystick6Button11;
                mInputKeyArray[(int)KeyCode.Joystick6Button12] = Joystick6Button12;
                mInputKeyArray[(int)KeyCode.Joystick6Button13] = Joystick6Button13;
                mInputKeyArray[(int)KeyCode.Joystick6Button14] = Joystick6Button14;
                mInputKeyArray[(int)KeyCode.Joystick6Button15] = Joystick6Button15;
                mInputKeyArray[(int)KeyCode.Joystick6Button16] = Joystick6Button16;
                mInputKeyArray[(int)KeyCode.Joystick6Button17] = Joystick6Button17;
                mInputKeyArray[(int)KeyCode.Joystick6Button18] = Joystick6Button18;
                mInputKeyArray[(int)KeyCode.Joystick6Button19] = Joystick6Button19;
                mInputKeyArray[(int)KeyCode.Joystick7Button0] = Joystick7Button0;
                mInputKeyArray[(int)KeyCode.Joystick7Button1] = Joystick7Button1;
                mInputKeyArray[(int)KeyCode.Joystick7Button2] = Joystick7Button2;
                mInputKeyArray[(int)KeyCode.Joystick7Button3] = Joystick7Button3;
                mInputKeyArray[(int)KeyCode.Joystick7Button4] = Joystick7Button4;
                mInputKeyArray[(int)KeyCode.Joystick7Button5] = Joystick7Button5;
                mInputKeyArray[(int)KeyCode.Joystick7Button6] = Joystick7Button6;
                mInputKeyArray[(int)KeyCode.Joystick7Button7] = Joystick7Button7;
                mInputKeyArray[(int)KeyCode.Joystick7Button8] = Joystick7Button8;
                mInputKeyArray[(int)KeyCode.Joystick7Button9] = Joystick7Button9;
                mInputKeyArray[(int)KeyCode.Joystick7Button10] = Joystick7Button10;
                mInputKeyArray[(int)KeyCode.Joystick7Button11] = Joystick7Button11;
                mInputKeyArray[(int)KeyCode.Joystick7Button12] = Joystick7Button12;
                mInputKeyArray[(int)KeyCode.Joystick7Button13] = Joystick7Button13;
                mInputKeyArray[(int)KeyCode.Joystick7Button14] = Joystick7Button14;
                mInputKeyArray[(int)KeyCode.Joystick7Button15] = Joystick7Button15;
                mInputKeyArray[(int)KeyCode.Joystick7Button16] = Joystick7Button16;
                mInputKeyArray[(int)KeyCode.Joystick7Button17] = Joystick7Button17;
                mInputKeyArray[(int)KeyCode.Joystick7Button18] = Joystick7Button18;
                mInputKeyArray[(int)KeyCode.Joystick7Button19] = Joystick7Button19;
                mInputKeyArray[(int)KeyCode.Joystick8Button0] = Joystick8Button0;
                mInputKeyArray[(int)KeyCode.Joystick8Button1] = Joystick8Button1;
                mInputKeyArray[(int)KeyCode.Joystick8Button2] = Joystick8Button2;
                mInputKeyArray[(int)KeyCode.Joystick8Button3] = Joystick8Button3;
                mInputKeyArray[(int)KeyCode.Joystick8Button4] = Joystick8Button4;
                mInputKeyArray[(int)KeyCode.Joystick8Button5] = Joystick8Button5;
                mInputKeyArray[(int)KeyCode.Joystick8Button6] = Joystick8Button6;
                mInputKeyArray[(int)KeyCode.Joystick8Button7] = Joystick8Button7;
                mInputKeyArray[(int)KeyCode.Joystick8Button8] = Joystick8Button8;
                mInputKeyArray[(int)KeyCode.Joystick8Button9] = Joystick8Button9;
                mInputKeyArray[(int)KeyCode.Joystick8Button10] = Joystick8Button10;
                mInputKeyArray[(int)KeyCode.Joystick8Button11] = Joystick8Button11;
                mInputKeyArray[(int)KeyCode.Joystick8Button12] = Joystick8Button12;
                mInputKeyArray[(int)KeyCode.Joystick8Button13] = Joystick8Button13;
                mInputKeyArray[(int)KeyCode.Joystick8Button14] = Joystick8Button14;
                mInputKeyArray[(int)KeyCode.Joystick8Button15] = Joystick8Button15;
                mInputKeyArray[(int)KeyCode.Joystick8Button16] = Joystick8Button16;
                mInputKeyArray[(int)KeyCode.Joystick8Button17] = Joystick8Button17;
                mInputKeyArray[(int)KeyCode.Joystick8Button18] = Joystick8Button18;
                mInputKeyArray[(int)KeyCode.Joystick8Button19] = Joystick8Button19;
            }

            return mInputKeyArray;
        }

        /**
         * Converts a key code to the string that represents it.
         */
        public static string codeToString(KeyCode value)
        {
            return mInputKeyArray[(int)value].getKeyDesc();
        }

        public InputKey(KeyCode keyCode, string keyDesc)
        {
            this.mKeyCode = keyCode;
            this.mKeyDesc = keyDesc;

            this.mKeyState = false;
            this.mKeyStateOld = false;
            this.mJustPressed = false;
            this.mJustReleased = false;

            this.mOnKeyUpDispatch = new AddOnceEventDispatch();
            this.mOnKeyDownDispatch = new AddOnceEventDispatch();
            this.mOnKeyPressDispatch = new AddOnceEventDispatch();
        }

        public KeyCode getKeyCode()
        {
            return mKeyCode;
        }

        public string getKeyDesc()
        {
            return mKeyDesc;
        }

        public void onTick(float delta)
        {
            //if (Input.GetKey(mKeyCode))
            //{
            //    mKeyState = true;
            //}
            //else
            //{
            //    mKeyState = false;
            //}

            //// 按下状态
            //if (mKeyState && !mKeyStateOld)
            //{
            //    mJustPressed = true;
            //}
            //else
            //{
            //    mJustPressed = false;
            //}

            //// 弹起状态
            //if (!mKeyState && mKeyStateOld)
            //{
            //    mJustReleased = true;
            //}
            //else
            //{
            //    mJustReleased = false;
            //}

            //mKeyStateOld = mKeyState;

            if (Input.GetKeyDown(this.mKeyCode))
            {
                this.onKeyDown(this.mKeyCode);
            }

            if (Input.GetKeyUp(this.mKeyCode))
            {
                this.onKeyUp(this.mKeyCode);
            }

            if (Input.GetKey(this.mKeyCode))
            {
                this.onKeyPress(this.mKeyCode);
            }
        }

        private void onKeyDown(KeyCode keyCode)
        {
            //if (this.mKeyState)
            //{
            //    return;
            //}

            //this.mKeyState = true;
            if (null != this.mOnKeyDownDispatch)
            {
                this.mOnKeyDownDispatch.dispatchEvent(this);
            }
        }

        private void onKeyUp(KeyCode keyCode)
        {
            //this.mKeyState = false;
            if (null != this.mOnKeyUpDispatch)
            {
                this.mOnKeyUpDispatch.dispatchEvent(this);
            }
        }

        private void onKeyPress(KeyCode keyCode)
        {
            if (null != this.mOnKeyPressDispatch)
            {
                this.mOnKeyPressDispatch.dispatchEvent(this);
            }
        }

        public void addKeyListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.KEYUP_EVENT == evtID)
            {
                mOnKeyUpDispatch.addEventHandle(null, handle);
            }
            else if (EventId.KEYDOWN_EVENT == evtID)
            {
                mOnKeyDownDispatch.addEventHandle(null, handle);
            }
            else if (EventId.KEYPRESS_EVENT == evtID)
            {
                mOnKeyPressDispatch.addEventHandle(null, handle);
            }
        }

        public void removeKeyListener(EventId evtID, MAction<IDispatchObject> handle)
        {
            if (EventId.KEYUP_EVENT == evtID)
            {
                mOnKeyUpDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.KEYDOWN_EVENT == evtID)
            {
                mOnKeyDownDispatch.removeEventHandle(null, handle);
            }
            else if (EventId.KEYPRESS_EVENT == evtID)
            {
                mOnKeyPressDispatch.removeEventHandle(null, handle);
            }
        }

        // 是否还有需要处理的事件
        public bool hasEventHandle()
        {
            if(this.mOnKeyUpDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnKeyDownDispatch.hasEventHandle())
            {
                return true;
            }
            if (this.mOnKeyPressDispatch.hasEventHandle())
            {
                return true;
            }

            return false;
        }

        /**
         * Returns whether or not a key was pressed since the last tick.
         */
        public bool keyJustPressed()
        {
            return this.mJustPressed;
        }

        /**
         * Returns whether or not a key was released since the last tick.
         */
        public bool keyJustReleased()
        {
            return this.mJustReleased;
        }

        /**
         * Returns whether or not a specific key is down.
         */
        public bool isKeyDown()
        {
            return this.mKeyState;
        }

        // 按键相关
        private KeyCode mKeyCode;
        private string mKeyDesc;

        // 键盘状态
        public bool mKeyState;
        public bool mKeyStateOld;
        public bool mJustPressed;
        public bool mJustReleased;

        // 事件处理
        private AddOnceEventDispatch mOnKeyUpDispatch;
        private AddOnceEventDispatch mOnKeyDownDispatch;
        private AddOnceEventDispatch mOnKeyPressDispatch;
    }
}