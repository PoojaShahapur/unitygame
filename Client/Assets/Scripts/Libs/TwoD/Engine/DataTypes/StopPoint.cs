using System.Security;

namespace SDK.Lib
{
	/**
	 * KBEN: 阻挡点信息  
	 */
	public class StopPoint 
	{
		protected SecurityElement m_xmlObj;	// 阻挡点的 XML 描述 
		protected int m_type;		// 阻挡点类型     
		protected bool m_isStop;	// 是否是阻挡点，主要是为了统一流程才加这个变量，只有这个变量是 true 的时候，里面的其它内容才是有效的，否则是无效的     
		
		public StopPoint(SecurityElement defObj, fScene scene)
		{
            UtilXml.getXmlAttrInt(defObj, "type", ref m_type);
			m_isStop = true;
		}
		
		public bool getIsStop() 
		{
			return m_isStop;
		}
		
		public void setIsStop(bool value) 
		{
			m_isStop = value;
		}
	}
}