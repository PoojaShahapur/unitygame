namespace SDK.Lib
{
    public class LabelStyleBase : WidgetStyle
    {
        public LabelStyleBase()
        {

        }

        // 是否需要清除文本
        virtual public bool needClearText()
        {
            return true;
        }
    }
}