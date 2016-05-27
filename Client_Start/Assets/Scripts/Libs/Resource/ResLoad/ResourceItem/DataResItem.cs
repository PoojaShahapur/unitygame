using System.IO;

namespace SDK.Lib
{
    public class DataResItem : ResItem
    {
        protected byte[] m_bytes;
        protected string m_localPath;

        override public void init(LoadItem item)
        {
            base.init(item);

            m_bytes = (item as DataLoadItem).m_bytes;
            m_localPath = (item as DataLoadItem).m_localPath;

            refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public byte[] getBytes()
        {
            if(m_bytes == null)
            {
                MDataStream mDataStream = new MDataStream(m_localPath);
                m_bytes = mDataStream.readByte();
                mDataStream.dispose();
                mDataStream = null;
            }

            return m_bytes;
        }
    }
}