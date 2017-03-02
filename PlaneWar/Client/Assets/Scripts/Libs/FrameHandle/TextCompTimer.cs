using UnityEngine.UI;

namespace SDK.Lib
{
    public class TextCompTimer : DaoJiShiTimer
    {
        protected Text mText;

        protected override void onPreCallBack()
        {
            base.onPreCallBack();
            this.mText.text = UtilLogic.formatTime((int)this.mCurRunTime);
        }
    }
}