using SDK.Common;
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

            if (onLoaded != null)
            {
                onLoaded(this);
            }

            clearListener();
        }

        public byte[] getBytes()
        {
            if(m_bytes == null)
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(m_localPath);
            }

            return m_bytes;
        }
    }
}