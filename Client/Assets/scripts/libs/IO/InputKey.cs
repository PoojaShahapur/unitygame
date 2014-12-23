using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * Enumeration class that maps friendly key names to their key code equivalent. This class
     * should not be instantiated directly, rather, one of the constants should be used.
     */   
    public class InputKey
    {
        public static InputKey INVALID = new InputKey(0);
        public static InputKey BACKSPACE = new InputKey(8);
        public static InputKey TAB = new InputKey(9);
        public static InputKey ENTER = new InputKey(13);
        public static InputKey COMMAND = new InputKey(15);
        public static InputKey SHIFT = new InputKey(16);
        public static InputKey CONTROL = new InputKey(17);
        public static InputKey ALT = new InputKey(18);
        public static InputKey PAUSE = new InputKey(19);
        public static InputKey CAPS_LOCK = new InputKey(20);
        public static InputKey ESCAPE = new InputKey(27);
        public static InputKey SPACE = new InputKey(32);
        public static InputKey PAGE_UP = new InputKey(33);
        public static InputKey PAGE_DOWN = new InputKey(34);
        public static InputKey END = new InputKey(35);
        public static InputKey HOME = new InputKey(36);
        public static InputKey LEFT = new InputKey(37);
        public static InputKey UP = new InputKey(38);
        public static InputKey RIGHT = new InputKey(39);
        public static InputKey DOWN = new InputKey(40);

        public static InputKey INSERT = new InputKey(45);
        public static InputKey DELETE = new InputKey(46);

        public static InputKey ZERO = new InputKey(48);
        public static InputKey ONE = new InputKey(49);
        public static InputKey TWO = new InputKey(50);
        public static InputKey THREE = new InputKey(51);
        public static InputKey FOUR = new InputKey(52);
        public static InputKey FIVE = new InputKey(53);
        public static InputKey SIX = new InputKey(54);
        public static InputKey SEVEN = new InputKey(55);
        public static InputKey EIGHT = new InputKey(56);
        public static InputKey NINE = new InputKey(57);

        public static InputKey A = new InputKey(65);
        public static InputKey B = new InputKey(66);
        public static InputKey C = new InputKey(67);
        public static InputKey D = new InputKey(68);
        public static InputKey E = new InputKey(69);
        public static InputKey F = new InputKey(70);
        public static InputKey G = new InputKey(71);
        public static InputKey H = new InputKey(72);
        public static InputKey I = new InputKey(73);
        public static InputKey J = new InputKey(74);
        public static InputKey K = new InputKey(75);
        public static InputKey L = new InputKey(76);
        public static InputKey M = new InputKey(77);
        public static InputKey N = new InputKey(78);
        public static InputKey O = new InputKey(79);
        public static InputKey P = new InputKey(80);
        public static InputKey Q = new InputKey(81);
        public static InputKey R = new InputKey(82);
        public static InputKey S = new InputKey(83);
        public static InputKey T = new InputKey(84);
        public static InputKey U = new InputKey(85);
        public static InputKey V = new InputKey(86);
        public static InputKey W = new InputKey(87);
        public static InputKey X = new InputKey(88);
        public static InputKey Y = new InputKey(89);
        public static InputKey Z = new InputKey(90);

        public static InputKey NUM0 = new InputKey(96);
        public static InputKey NUM1 = new InputKey(97);
        public static InputKey NUM2 = new InputKey(98);
        public static InputKey NUM3 = new InputKey(99);
        public static InputKey NUM4 = new InputKey(100);
        public static InputKey NUM5 = new InputKey(101);
        public static InputKey NUM6 = new InputKey(102);
        public static InputKey NUM7 = new InputKey(103);
        public static InputKey NUM8 = new InputKey(104);
        public static InputKey NUM9 = new InputKey(105);

        public static InputKey MULTIPLY = new InputKey(106);
        public static InputKey ADD = new InputKey(107);
        public static InputKey NUMENTER = new InputKey(108);
        public static InputKey SUBTRACT = new InputKey(109);
        public static InputKey DECIMAL = new InputKey(110);
        public static InputKey DIVIDE = new InputKey(111);

        public static InputKey F1 = new InputKey(112);
        public static InputKey F2 = new InputKey(113);
        public static InputKey F3 = new InputKey(114);
        public static InputKey F4 = new InputKey(115);
        public static InputKey F5 = new InputKey(116);
        public static InputKey F6 = new InputKey(117);
        public static InputKey F7 = new InputKey(118);
        public static InputKey F8 = new InputKey(119);
        public static InputKey F9 = new InputKey(120);
        // F10sidered 'reserved' by Flash
        public static InputKey F11 = new InputKey(122);
        public static InputKey F12 = new InputKey(123);

        public static InputKey NUM_LOCK = new InputKey(144);
        public static InputKey SCROLL_LOCK = new InputKey(145);

        public static InputKey COLON = new InputKey(186);
        public static InputKey PLUS = new InputKey(187);
        public static InputKey COMMA = new InputKey(188);
        public static InputKey MINUS = new InputKey(189);
        public static InputKey PERIOD = new InputKey(190);
        public static InputKey BACKSLASH = new InputKey(191);
        public static InputKey TILDE = new InputKey(192);

        public static InputKey LEFT_BRACKET = new InputKey(219);
        public static InputKey SLASH = new InputKey(220);
        public static InputKey RIGHT_BRACKET = new InputKey(221);
        public static InputKey QUOTE = new InputKey(222);

        public static InputKey MOUSE_BUTTON = new InputKey(253);
        public static InputKey MOUSE_X = new InputKey(254);
        public static InputKey MOUSE_Y = new InputKey(255);
        public static InputKey MOUSE_WHEEL = new InputKey(256);
        public static InputKey MOUSE_HOVER = new InputKey(257);

        /**
         * A dictionary mapping the string names of all the keys to the InputKey they represent.
         */
        public static Dictionary<string, InputKey> staticTypeMap()
        {
            if (_typeMap == null)
            {
                _typeMap = new Dictionary<string, InputKey>();
                _typeMap["BACKSPACE"] = BACKSPACE;
                _typeMap["TAB"] = TAB;
                _typeMap["ENTER"] = ENTER;
                _typeMap["RETURN"] = ENTER;
                _typeMap["SHIFT"] = SHIFT;
                _typeMap["COMMAND"] = COMMAND;
                _typeMap["CONTROL"] = CONTROL;
                _typeMap["ALT"] = ALT;
                _typeMap["OPTION"] = ALT;
                _typeMap["ALTERNATE"] = ALT;
                _typeMap["PAUSE"] = PAUSE;
                _typeMap["CAPS_LOCK"] = CAPS_LOCK;
                _typeMap["ESCAPE"] = ESCAPE;
                _typeMap["SPACE"] = SPACE;
                _typeMap["SPACE_BAR"] = SPACE;
                _typeMap["PAGE_UP"] = PAGE_UP;
                _typeMap["PAGE_DOWN"] = PAGE_DOWN;
                _typeMap["END"] = END;
                _typeMap["HOME"] = HOME;
                _typeMap["LEFT"] = LEFT;
                _typeMap["UP"] = UP;
                _typeMap["RIGHT"] = RIGHT;
                _typeMap["DOWN"] = DOWN;
                _typeMap["LEFT_ARROW"] = LEFT;
                _typeMap["UP_ARROW"] = UP;
                _typeMap["RIGHT_ARROW"] = RIGHT;
                _typeMap["DOWN_ARROW"] = DOWN;
                _typeMap["INSERT"] = INSERT;
                _typeMap["DELETE"] = DELETE;
                _typeMap["ZERO"] = ZERO;
                _typeMap["ONE"] = ONE;
                _typeMap["TWO"] = TWO;
                _typeMap["THREE"] = THREE;
                _typeMap["FOUR"] = FOUR;
                _typeMap["FIVE"] = FIVE;
                _typeMap["SIX"] = SIX;
                _typeMap["SEVEN"] = SEVEN;
                _typeMap["EIGHT"] = EIGHT;
                _typeMap["NINE"] = NINE;
                _typeMap["0"] = ZERO;
                _typeMap["1"] = ONE;
                _typeMap["2"] = TWO;
                _typeMap["3"] = THREE;
                _typeMap["4"] = FOUR;
                _typeMap["5"] = FIVE;
                _typeMap["6"] = SIX;
                _typeMap["7"] = SEVEN;
                _typeMap["8"] = EIGHT;
                _typeMap["9"] = NINE;
                _typeMap["NUMBER_0"] = ZERO;
                _typeMap["NUMBER_1"] = ONE;
                _typeMap["NUMBER_2"] = TWO;
                _typeMap["NUMBER_3"] = THREE;
                _typeMap["NUMBER_4"] = FOUR;
                _typeMap["NUMBER_5"] = FIVE;
                _typeMap["NUMBER_6"] = SIX;
                _typeMap["NUMBER_7"] = SEVEN;
                _typeMap["NUMBER_8"] = EIGHT;
                _typeMap["NUMBER_9"] = NINE;
                _typeMap["A"] = A;
                _typeMap["B"] = B;
                _typeMap["C"] = C;
                _typeMap["D"] = D;
                _typeMap["E"] = E;
                _typeMap["F"] = F;
                _typeMap["G"] = G;
                _typeMap["H"] = H;
                _typeMap["I"] = I;
                _typeMap["J"] = J;
                _typeMap["K"] = K;
                _typeMap["L"] = L;
                _typeMap["M"] = M;
                _typeMap["N"] = N;
                _typeMap["O"] = O;
                _typeMap["P"] = P;
                _typeMap["Q"] = Q;
                _typeMap["R"] = R;
                _typeMap["S"] = S;
                _typeMap["T"] = T;
                _typeMap["U"] = U;
                _typeMap["V"] = V;
                _typeMap["W"] = W;
                _typeMap["X"] = X;
                _typeMap["Y"] = Y;
                _typeMap["Z"] = Z;
                _typeMap["NUM0"] = NUM0;
                _typeMap["NUM1"] = NUM1;
                _typeMap["NUM2"] = NUM2;
                _typeMap["NUM3"] = NUM3;
                _typeMap["NUM4"] = NUM4;
                _typeMap["NUM5"] = NUM5;
                _typeMap["NUM6"] = NUM6;
                _typeMap["NUM7"] = NUM7;
                _typeMap["NUM8"] = NUM8;
                _typeMap["NUM9"] = NUM9;
                _typeMap["NUMPAD_0"] = NUM0;
                _typeMap["NUMPAD_1"] = NUM1;
                _typeMap["NUMPAD_2"] = NUM2;
                _typeMap["NUMPAD_3"] = NUM3;
                _typeMap["NUMPAD_4"] = NUM4;
                _typeMap["NUMPAD_5"] = NUM5;
                _typeMap["NUMPAD_6"] = NUM6;
                _typeMap["NUMPAD_7"] = NUM7;
                _typeMap["NUMPAD_8"] = NUM8;
                _typeMap["NUMPAD_9"] = NUM9;
                _typeMap["MULTIPLY"] = MULTIPLY;
                _typeMap["ASTERISK"] = MULTIPLY;
                _typeMap["NUMMULTIPLY"] = MULTIPLY;
                _typeMap["NUMPAD_MULTIPLY"] = MULTIPLY;
                _typeMap["ADD"] = ADD;
                _typeMap["NUMADD"] = ADD;
                _typeMap["NUMPAD_ADD"] = ADD;
                _typeMap["SUBTRACT"] = SUBTRACT;
                _typeMap["NUMSUBTRACT"] = SUBTRACT;
                _typeMap["NUMPAD_SUBTRACT"] = SUBTRACT;
                _typeMap["DECIMAL"] = DECIMAL;
                _typeMap["NUMDECIMAL"] = DECIMAL;
                _typeMap["NUMPAD_DECIMAL"] = DECIMAL;
                _typeMap["DIVIDE"] = DIVIDE;
                _typeMap["NUMDIVIDE"] = DIVIDE;
                _typeMap["NUMPAD_DIVIDE"] = DIVIDE;
                _typeMap["NUMENTER"] = NUMENTER;
                _typeMap["NUMPAD_ENTER"] = NUMENTER;
                _typeMap["F1"] = F1;
                _typeMap["F2"] = F2;
                _typeMap["F3"] = F3;
                _typeMap["F4"] = F4;
                _typeMap["F5"] = F5;
                _typeMap["F6"] = F6;
                _typeMap["F7"] = F7;
                _typeMap["F8"] = F8;
                _typeMap["F9"] = F9;
                _typeMap["F11"] = F11;
                _typeMap["F12"] = F12;
                _typeMap["NUM_LOCK"] = NUM_LOCK;
                _typeMap["SCROLL_LOCK"] = SCROLL_LOCK;
                _typeMap["COLON"] = COLON;
                _typeMap["SEMICOLON"] = COLON;
                _typeMap["PLUS"] = PLUS;
                _typeMap["EQUAL"] = PLUS;
                _typeMap["COMMA"] = COMMA;
                _typeMap["LESS_THAN"] = COMMA;
                _typeMap["MINUS"] = MINUS;
                _typeMap["UNDERSCORE"] = MINUS;
                _typeMap["PERIOD"] = PERIOD;
                _typeMap["GREATER_THAN"] = PERIOD;
                _typeMap["BACKSLASH"] = BACKSLASH;
                _typeMap["QUESTION_MARK"] = BACKSLASH;
                _typeMap["TILDE"] = TILDE;
                _typeMap["BACK_QUOTE"] = TILDE;
                _typeMap["LEFT_BRACKET"] = LEFT_BRACKET;
                _typeMap["LEFT_BRACE"] = LEFT_BRACKET;
                _typeMap["SLASH"] = SLASH;
                _typeMap["FORWARD_SLASH"] = SLASH;
                _typeMap["PIPE"] = SLASH;
                _typeMap["RIGHT_BRACKET"] = RIGHT_BRACKET;
                _typeMap["RIGHT_BRACE"] = RIGHT_BRACKET;
                _typeMap["QUOTE"] = QUOTE;
                _typeMap["MOUSE_BUTTON"] = MOUSE_BUTTON;
                _typeMap["MOUSE_X"] = MOUSE_X;
                _typeMap["MOUSE_Y"] = MOUSE_Y;
                _typeMap["MOUSE_WHEEL"] = MOUSE_WHEEL;
                _typeMap["MOUSE_HOVER"] = MOUSE_HOVER;
            }

            return _typeMap;
        }

        /**
         * Converts a key code to the string that represents it.
         */
        public static string codeToString(int value)
        {
            foreach (string name in _typeMap.Keys)
            {
                if (_typeMap[name.ToUpper()].keyCode() == value)
                    return name.ToUpper();
            }

            return null;
        }

        /**
         * Converts the name of a key to the keycode it represents.
         */
        public static int stringToCode(string value)
        {
            if (!_typeMap.ContainsKey(value.ToUpper()))
                return 0;

            return _typeMap[value.ToUpper()].keyCode();
        }

        /**
         * Converts the name of a key to the InputKey it represents.
         */
        public static InputKey stringToKey(string value)
        {
            return staticTypeMap()[value.ToUpper()];
        }

        private static Dictionary<string, InputKey> _typeMap = null;

        /**
         * The key code that this wraps.
         */
        public int keyCode()
        {
            return _keyCode;
        }

        public InputKey(int keyCode = 0)
        {
            _keyCode = keyCode;
        }

        public static Dictionary<string, InputKey> typeMap()
        {
            return staticTypeMap();
        }

        private int _keyCode = 0;
    }
}