namespace SDK.Lib
{
    public class TextResBase : InsResBase
    {
        protected string mText;

        public TextResBase()
        {

        }

        override protected void initImpl(ResItem res)
        {
            // 获取资源单独保存
            mText = res.getText(res.getPrefabName());
            base.initImpl(res);
        }

        override public void unload()
        {
            mText = null;
            base.unload();
        }

        public string text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
            }
        }

        public string getText(string path)
        {
            return mText;
        }
    }
}