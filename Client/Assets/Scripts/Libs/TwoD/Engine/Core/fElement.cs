using System;
using System.Security;

namespace SDK.Lib
{
	public class fElement : EventDispatchGroup, ITickedObject, IDelayHandleItem, IDispatchObject
    {
		private static int count = 0;
		public string id;
		public SecurityElement xmlObj;
		public int uniqueId;
		public float x;
		public float y;
		public float z;

		public fCell cell;
		// KBEN: 元素所在的地形区域
		public fFloor m_district;		
		public Object customData;
        public fSpriteElementRenderer flash9Renderer;

        // elementmove 元素移动事件
        public static int MOVE = 0;
        // elementnewcell 元素进入新的单元格子
        public static int NEWCELL = 1;
		
		protected float destx = 0;
		protected float desty = 0;
		protected float destz = 0;
		
		protected float offx = 0;
		protected float offy = 0;
		protected float offz = 0;

		protected float elasticity = 0;
		
		//控制器
		private fEngineElementController _controller = null;
		// KBEN: fFloor 
		
		public fElement(SecurityElement defObj)
		{
			this.xmlObj = defObj;
            string temp = "";
            UtilXml.getXmlAttrStr(defObj, "id", ref temp);

            this.uniqueId = fElement.count++;
			if (temp.Length == 1)
				this.id = temp;
			else
				this.id = "fElement_" + this.uniqueId;

			// 当前 Cell 
			this.cell = null;
			
			// 基本坐标
            UtilXml.getXmlAttrFloat(defObj, "x", ref this.x);
            UtilXml.getXmlAttrFloat(defObj, "y", ref this.y);
            UtilXml.getXmlAttrFloat(defObj, "z", ref this.z);
			
			this.customData = new Object();
		}
		
		public void setController(fEngineElementController controller)
		{
			if (this._controller != null)
				this._controller.disable();
			this._controller = controller;
			if (this._controller != null)
				this._controller.assignElement(this);
		}
		
		public fEngineElementController getController()
		{
			return this._controller;
		}
		
		virtual public void moveTo(float x, float y, float z)
		{			
			// 设置新的坐标	   
			this.x = x;
			this.y = y;
			this.z = z;			
		}

		virtual public void follow(fElement target, float elasticity = 0)
		{
			this.offx = target.x - this.x;
			this.offy = target.y - this.y;
			this.offz = target.z - this.z;
			
			this.elasticity = 1 + elasticity;
			// KBEN: 如果这个地方跟随者没有移动，moveListener 这个函数就不会被调用
			target.addEventHandle(fElement.MOVE, this.moveListener);
		}
		
		public void stopFollowing(fElement target)
		{
			target.removeEventHandle(fElement.MOVE, this.moveListener);
		}
		
		virtual public void moveListener(IDispatchObject dispObj)
		{
            fMoveEvent evt = dispObj as fMoveEvent;
            if (this.elasticity == 1)
				this.moveTo(evt.dx - this.offx, evt.dy - this.offy, evt.dz - this.offz);
			else
			{
				this.destx = evt.dx - this.offx;
				this.desty = evt.dy - this.offy;
				this.destz = evt.dz - this.offz;
                Ctx.m_instance.m_tickMgr.addTick(this);
            }
		}
		
		virtual public void followListener(IDispatchObject dispObj)
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
			
			// 停止
			if (dx < 1 && dx > -1 && dy < 1 && dy > -1 && dz < 1 && dz > -1)
			{
                Ctx.m_instance.m_tickMgr.delTick(this);
			}
		}
		
		public float distanceTo(float x, float y, float z)
		{
			return MathUtils.distance(x, y, this.x, this.y);
		}
		
		public void disposeElement()
		{
			this.xmlObj = null;
			this.cell = null;
			this._controller = null;
            Ctx.m_instance.m_tickMgr.delTick(this);
        }
		
		virtual public void dispose()
		{
			this.flash9Renderer = null;
			this.disposeElement();
		}
		
		virtual public void onTick(float deltaTime)
		{
            this.followListener(null);
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}