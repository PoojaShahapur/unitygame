using SDK.Common;
using System.IO;
namespace SDK.Lib
{
    public class DataResItem : ResItem
    {
        protected byte[] m_bytes;
        protected string m_rootPath;

        override public void init(LoadItem item)
        {
            m_bytes = (item as DataLoadItem).m_bytes;
            m_rootPath = (item as DataLoadItem).m_rootPath;

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
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(Path.Combine(m_rootPath, m_path));
            }

            return m_bytes;
        }
    }
}