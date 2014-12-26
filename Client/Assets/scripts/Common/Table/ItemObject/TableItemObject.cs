namespace SDK.Common
{
    public class TableItemObject : TableItemBase
    {
        override public void parseBodyByteArray(IByteArray bytes, uint offset)
        {
            if (null == m_itemBody)
            {
                m_itemBody = new TableObjectItemBody();
            }

            m_itemBody.parseBodyByteArray(bytes, offset);
        }
    }
}