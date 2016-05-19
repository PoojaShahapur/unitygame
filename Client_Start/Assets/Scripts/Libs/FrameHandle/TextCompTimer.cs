using UnityEngine.UI;

namespace SDK.Lib
{
    public class TextCompTimer : DaoJiShiTimer
    {
        protected Text m_text;

        protected override void onPreCallBack()
        {
            base.onPreCallBack();
            m_text.text = UtilLogic.formatTime((int)m_curRunTime);
        }
    }
}