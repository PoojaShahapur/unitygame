using System;
using System.Security;

namespace SDK.Lib
{
	public class fCharacter : fSceneObject, fMovingElement
	{
        // charactercollide
        public static int COLLIDE = 0;
        // characterwalkover
		public static int WALKOVER = 1;
        // charactereventin
		public static int EVENT_IN = 2;
        // charactereventout
		public static int EVENT_OUT = 3;
		
		public fCharacter(SecurityElement defObj, fScene scene)
            : base(defObj, scene)
		{
			
		}
		
		public void teleportTo(float x, float y, float z)
		{
			this.moveTo(x, y, z);
		}

		// 从一个点直接跳到另外一个点   
		public override void moveTo(float x, float y, float z)
		{			
			// KBEN: 防止 z 小于 0 
			if (z < 0)
			{
				z = 0;
			}
            // Last position
            float lx = this.x;
            float ly = this.y;
            float lz = this.z;

            // Movement
            float dx = x - lx;
            float dy = y - ly;
            float dz = z - lz;
			
			// bug: 这个地方如果返回，没有 exit 
			if (dx == 0 && dy == 0 && dz == 0)
			{
				return;
			}
			
			try
			{		   
				this.x = x;
				this.y = y;
				this.z = z;

                float radius = this.radius;
                float height = this.height;
				
				this.top = this.z + height;

                fCell cell = this.scene.translateToCell(this.x, this.y, this.z);
				
				if (cell != this.cell || this.cell == null)
				{
                    fCell lastCell = this.cell;
					this.cell = cell;
                    fNewCellEvent newCell = new fNewCellEvent();
					if (lastCell != null && cell.y == lastCell.y)
					{						
						newCell.m_needDepthSort = false;
					}				
					dispatchEvent(fElement.NEWCELL, newCell);

                    // 继续判断是否是新的 district
                    fFloor dist = this.scene.getFloorAtByPos(this.x, this.y);

					if(m_district == null || dist == null || m_district != dist)
					{
						// 到达新的区域没有什么好做的，仅仅是将自己的信息移动到新的区域
						if (m_district != null)
						{
							m_district.clearCharacter(this.id);
						}
						m_district = dist;
						if (m_district != null)
						{
							m_district.addCharacter(this.id);
						}
					}
				}
				 
				if (this.x != lx || this.y != ly || this.z != lz)
					dispatchEvent(fElement.MOVE, new fMoveEvent(this.x - lx, this.y - ly, this.z - lz));
			}
			catch (Exception e)
            {
				this.x = lx;
				this.y = ly;
				this.z = lz;
				dispatchEvent(fCharacter.COLLIDE, new fCollideEvent(this));
			}
		}
		
		public void disposeCharacter()
		{
			// KBEN: 这个一定要放在最后，他会销毁自身的很多变量，这些变量在销毁之前很可能会用到
			this.disposeObject();
		}
		
		//在从场景对象(fScene)上移除时，调用此函数
		public void onRemoveFormScene()
		{
			
		}
		
		/** @private */
		public override void dispose()
		{
			this.disposeCharacter();
			base.dispose();
		}
	}
}