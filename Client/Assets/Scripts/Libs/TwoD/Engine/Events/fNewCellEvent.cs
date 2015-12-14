namespace SDK.Lib
{
	public class fNewCellEvent :IDispatchObject
	{
		public bool m_needDepthSort;	//dispatch本事件时，需要深度排序

		public fNewCellEvent(bool bNotDepthSort = true)
		{
			m_needDepthSort = bNotDepthSort;
		}	
	}
}