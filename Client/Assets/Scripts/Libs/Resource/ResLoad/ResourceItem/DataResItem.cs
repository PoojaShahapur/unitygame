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
            return m_bytes;
        }
    }
}