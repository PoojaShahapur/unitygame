using SDK.Common;
using System.IO;
namespace SDK.Lib
{
    public class DataResItem : ResItem
    {
        protected byte[] m_bytes;

        override public void init(LoadItem item)
        {
            m_bytes = (item as DataLoadItem).m_bytes;

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
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(m_path);
            }

            return m_bytes;
        }
    }
}