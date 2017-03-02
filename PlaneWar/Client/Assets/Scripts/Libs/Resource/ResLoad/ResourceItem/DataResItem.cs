namespace SDK.Lib
{
    public class DataResItem : ResItem
    {
        protected byte[] mBytes;
        protected string mLocalPath;

        override public void init(LoadItem item)
        {
            base.init(item);

            mBytes = (item as DataLoadItem).mBytes;

            mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        override public byte[] getBytes(string resName)
        {
            return mBytes;
        }

        override public string getText(string resName)
        {
            string text = "";
            System.Text.Encoding encode = UtilApi.convGkEncode2EncodingEncoding(GkEncode.eUTF8);

            if(mBytes != null)
            {
                text = encode.GetString(mBytes);
            }

            return text;
        }
    }
}