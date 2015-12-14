using System.Collections;
using System.Collections.Generic;

namespace SDK.Lib
{
	public class fCell
	{
		public float zIndex;
		public float i;
		public float j;
		public float k;
		
		/**
		 * 格子中心点的位置
		 */
		public float x;
		public float y;
		public float z;

		public MList<fElement> visibleElements;		// 这个存放的是 fVisibilityInfo 这个数据结构

		public MList<fFloor> m_visibleFloor;		// 这个存放的是 fFloor 这个数据结构，但是可视化的内容与 visibleElements 都是一样的
		
		public float visibleRange = 0;

		// KBEN: 阻挡点，本来没有必要在这里再存储一个阻挡点信息了，但是为了找到 fCell 直接找到阻挡点。其实可以从 fScene 中查找的      
		private StopPoint m_stoppoint;
		//public Rectangle m_scrollRect;		// 这个是这个单元格子最大可见的范围，注意是格子左上角和右下角两个可见范围的并集
		public Dictionary<fCell, MList<fFloor>> m_updateDistrict;	// 当从 另外一个格子进入当前格子时，记录需要更新的内容区域(fFloor对象)列表，其实就是在前一次基础上迭代更新
		public Dictionary<fCell, MList<fFloor>> m_hideDistrict;	// 这个是进入当前格子需要隐藏的区域
		public fScene m_scene;
		
		public StopPoint getStoppoint()
		{
			return m_stoppoint;
		}
		
		public void setStoppoint(StopPoint value)
		{
			m_stoppoint = value;
		}
		
		public fCell(fScene scene)
		{
			m_scene = scene;
			m_updateDistrict = new Dictionary<fCell, MList<fFloor>>();
			m_hideDistrict = new Dictionary<fCell, MList<fFloor>>();
		}
		
		public void dispose()
		{
			if (this.visibleElements != null)
			{
                this.visibleElements.Clear();
                this.visibleElements = null;
			}
			
			clearClip();
			m_updateDistrict = null;
			m_hideDistrict = null;
		}
		
		// 由于相机从之前的格子进入当前的格子，需要更新当前格子的显示区域列表
		public void updateByPreInCur(fCell pregrid)
		{
            if (!m_updateDistrict.ContainsKey(pregrid))
            {
				if(pregrid != null)
				{
					m_updateDistrict[pregrid] = new MList<fFloor>();	// 兼容代码，用 Array
				}
				m_hideDistrict[pregrid] = new MList<fFloor>();
			}
			
			// 如果前一个格子存在
			if(pregrid != null)
			{
                // 更新更新列表
                MList<fFloor> updateDistrictList = m_updateDistrict[pregrid];
				foreach(fFloor floor in m_visibleFloor.list)
				{

				}
                MList<fFloor> hideDistrict = m_hideDistrict[pregrid];
				// 更新隐藏列表
				foreach(fFloor floor in pregrid.m_visibleFloor.list)
				{
					// 如果上一个格子可见，这个格子不可见，必然是隐藏的区域
					if(m_visibleFloor.IndexOf(floor) == -1)
					{
						hideDistrict.push(floor);
					}
				}
			}
			else
			{
				m_updateDistrict[pregrid] = m_visibleFloor;
			}
		}
		
		// 清理裁剪数据，以便重新生成
		public void clearClip()
		{
			if(visibleElements != null)
			{
				visibleElements.Clear();
				visibleElements = null;
			}
			if(m_visibleFloor != null)
			{
				m_visibleFloor.Clear();
				m_visibleFloor = null;
			}
			
			if(m_updateDistrict != null)
			{
				foreach(fCell key in m_updateDistrict.Keys)
				{
					if(m_updateDistrict[key] != null)
					{
						m_updateDistrict[key].Clear();
					}
					m_updateDistrict[key] = null;
				}
			}
			if(m_hideDistrict != null)
			{
				foreach(fCell key in m_hideDistrict.Keys)
				{
					if(m_hideDistrict[key] != null)
					{
						m_hideDistrict[key].Clear();
					}
					m_hideDistrict[key] = null;
				}
			}
		}
	}
}