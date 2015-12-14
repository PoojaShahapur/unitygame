using System;

namespace SDK.Lib
{
	public class fCamera : fElement
	{
		private static float count = 0;
		private fScene m_scene;
		private RectangleF m_rect;			// 不用经常申请释放资源

		public bool m_bInit = false;
		
		public fCamera(fScene scene)
            : base(null)
		{
			string myId = "fCamera_" + (fCamera.count++);
			
			this.m_scene = scene;
			//base(<camera id={myId}/>);

			m_rect = new RectangleF(0, 0, 100, 100);
			// 这里宽度和高度赋值一个具体的值，裁剪的时候使用这个句型
			//m_rect.width = this.m_context.m_config.m_curWidth;
			//m_rect.height = this.m_context.m_config.m_curHeight;
		}
		
		override public void follow(fElement target, float elasticity = 0)
		{
			// 摄像机要根据逻辑上的点进行计算，跟随的时候设置成和跟随者一个位置
			this.offx = 0;
			this.offy = 0;
			this.offz = 0;
			
			this.elasticity = 1 + elasticity;
			// KBEN: 如果这个地方跟随者没有移动，moveListener 这个函数就不会被调用
			target.addEventHandle(fElement.MOVE, this.moveListener);

			// 检查位置是否有问题
			bool berr = false;
			berr = adjustPos(target.x, target.y, target.z);
			
			if(berr)
			{
				this.destx = m_rect.x + m_rect.width/2;
				this.desty = m_rect.y + m_rect.height/2;
				this.destz = 0;
				
				float dx = this.destx - this.x;
                float dy = this.desty - this.y;
                float dz = this.destz - this.z;

				if (!(dx < 1 && dx > -1 && dy < 1 && dy > -1 && dz < 1 && dz > -1))	// 如果很小的移动就不移动了
				{
					this.moveTo(this.destx, this.desty, target.z - this.offz);
				}
			}
		}
		
		// 摄像机重载跟随，因为摄像机不能靠近地图的边缘
		override public void moveListener(IDispatchObject dispObj)
		{
            fMoveEvent evt = dispObj as fMoveEvent;
            // 先检测是否能到达地图边缘
            // bug 在这个地方判断有点问题，裁剪是根据摄像机的位置裁剪的，不是根据视口，视口仅仅是显示
            bool berr = false;
			berr = adjustPos(evt.dx, evt.dy, evt.dz);
			
			if(!berr)
			{
				if (this.elasticity == 1)
				{
					this.moveTo(evt.dx - this.offx, evt.dy - this.offy, evt.dz - this.offz);
				}
				else
				{
					this.destx = evt.dx - this.offx;
					this.desty = evt.dy - this.offy;
					this.destz = evt.dz - this.offz;

                    Ctx.m_instance.m_tickMgr.addTick(this);
				}
			}
			else
			{
				this.destx = m_rect.x + m_rect.width/2;
				this.desty = m_rect.y + m_rect.height/2;
				this.destz = 0;

                float dx = this.destx - this.x;
                float dy = this.desty - this.y;
				float dz = this.destz - this.z;
				
				if (this.elasticity == 1)
				{
					this.moveTo(this.destx, this.desty, evt.dz - this.offz);
				}
				else if (!(dx < 1 && dx > -1 && dy < 1 && dy > -1 && dz < 1 && dz > -1))	// 如果很小的移动就不移动了
				{
                    Ctx.m_instance.m_tickMgr.addTick(this);
                }
			}
		}
		
		override public void followListener(IDispatchObject dispObj)
		{
			float dx = this.destx - this.x;
            float dy = this.desty - this.y;
            float dz = this.destz - this.z;
			try
			{
				this.moveTo(this.x + dx / this.elasticity, this.y + dy / this.elasticity, this.z + dz / this.elasticity);
			}
			catch (Exception e)
			{
			}
			
			// Stop ?
			if (dx < 1 && dx > -1 && dy < 1 && dy > -1 && dz < 1 && dz > -1)
			{
                Ctx.m_instance.m_tickMgr.delTick(this);
            }
		}
		
        // 调用 moveTo 函数之前一定要先调用 adjustPos 这个函数，如果没有调用，一定要在外面手工调用一次
		override public void moveTo(float x, float y, float z)
		{
			// Last position
			float dx = this.x;
            float dy = this.y;
            float dz = this.z;
			
			// Set new coordinates			   
			this.x = x;
			this.y = y;
			this.z = z;

            // Check if element moved into a different cell
            fCell cell = this.m_scene.translateToCell(x, y, z);
			if (cell == null)
			{
                // 日志
			}
			if (this.cell == null || cell == null || cell != this.cell)
			{
				// 修真相机的裁剪视口
				this.cell = cell;

                dispatchEvent(fElement.NEWCELL, new fNewCellEvent());
			}
						
			// Dispatch event			
			this.dispatchEvent(fElement.MOVE, new fMoveEvent(this.x - dx, this.y - dy, this.z - dz));
		}
		
		// 如果需要调整位置返回 true
		protected bool adjustPos(float targetx, float targety, float targetz)
		{
			bool berr = false;

            float lcdestx = targetx - this.offx;
            float lcdesty = targety - this.offy;
			float lcdestz = targetz - this.offz;
			// 假设不会进行坐标转换，不用调用这个函数了
			//m_rect.width = m_context.m_config.m_curWidth;
			//m_rect.height = m_context.m_config.m_curHeight;
			//m_rect.x = Math.round(-m_context.m_config.m_curWidth / 2 + lcdestx);
			//m_rect.y = Math.round(-m_context.m_config.m_curHeight / 2 + lcdesty);
			//if (m_rect.x < 0)
			//{
			//	m_rect.x = 0;
			//	berr = true;
			//}
			//else if (m_rect.x > this.m_scene.m_scenePixelXOff + this.m_scene.widthpx() - m_rect.width)		// 战斗地形需要添加 m_scenePixelXOff
			//{
			//	m_rect.x = this.m_scene.m_scenePixelXOff + this.m_scene.widthpx() - m_rect.width;
			//	berr = true;
			//}
			//if (m_rect.y < 0)
			//{
			//	m_rect.y = 0;
			//	berr = true;
			//}
			//else if (m_rect.y > this.m_scene.heightpx() - m_rect.height)
			//{
			//	m_rect.y = this.m_scene.heightpx() - m_rect.height;
			//	berr = true;
			//}
			
			return berr;
		}
		
		// 这个也是移动相机到指定位置，类似 moveTo ，只不过很多函数都要走 moveTo ，如果很多逻辑写到 moveTo 里面就重复了
		public void gotoPos(float xtarget, float ytarget, float ztarget)
		{
			// 第一次调用这个函数，就说明初始化了
			m_bInit = true;
			if(adjustPos(xtarget, ytarget, ztarget))
			{
				moveTo(m_rect.x + m_rect.width/2, m_rect.y + m_rect.height/2, 0);
			}
			else
			{
				moveTo(xtarget, ytarget, ztarget);
			}
		}
		
		// 相机也保存一份数据视口数据，其它地方会用到
		public void setViewportSize(float width, float height)
		{
			gotoPos(this.x, this.y, this.z);
		}
	}
}