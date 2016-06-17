namespace SDK.Lib
{
    public class DataResItem : ResItem
    {
        protected byte[] m_bytes;
        protected string m_localPath;

        override public void init(LoadItem item)
        {
            base.init(item);

            m_bytes = (item as DataLoadItem).mBytes;

            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override public byte[] getBytes(string resName)
        {
            return m_bytes;
        }

        override public string getText(string resName)
        {
            string text = "";
            if(m_bytes != null)
            {
                text = GkEncode.UTF8.GetString(m_bytes);
            }

            return text;
        }
    }
}