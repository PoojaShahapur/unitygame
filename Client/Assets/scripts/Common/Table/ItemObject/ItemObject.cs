namespace SDK.Common
{
    public class ItemObject : ItemBase
    {
        override public void parseBodyByteArray(IByteArray bytes, uint offset)
        {
            if (null == m_itemBody)
            {
                m_itemBody = new ObjectItemBody();
            }

            m_itemBody.parseBodyByteArray(bytes, offset);
        }
    }
}