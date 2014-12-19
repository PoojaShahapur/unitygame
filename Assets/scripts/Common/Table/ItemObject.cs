namespace SDK.Common
{
	/**
	 * @brief 掉落物表   
	 */
	public class ObjectItem : ItemBase
	{
		public string m_strModel;
		public int m_type;		//0 - 手动拾取；1- 自动拾取
		
		override public void parseByteArray(ByteArray bytes)
		{
			base.parseByteArray(bytes);

            m_strModel = UtilTable.readString(bytes);
			m_type = bytes.readByte();
		}
	}
}