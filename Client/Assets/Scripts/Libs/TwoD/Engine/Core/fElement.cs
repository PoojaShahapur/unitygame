using System;

namespace SDK.Lib
{
	public class fElement
	{
		private static int count = 0;
		public string id;
		public XmlNode xmlObj;
		public int uniqueId;
		public float x;
		public float y;
		public float z;

		public fCell cell;
		// KBEN: 元素所在的地形区域
		public fFloor m_district;
		
		public Object customData;
		
	
		public static string MOVE = "elementmove";

		public static string NEWCELL = "elementnewcell";
		
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
		
		public fElement(XmlNode defObj)
		{
			// Id
			this.xmlObj = defObj;
			var temp:XMLList = defObj.@id;
			
			this.uniqueId = fElement.count++;
			if (temp.length() == 1)
				this.id = temp.toString();
			else
				this.id = "fElement_" + this.uniqueId;

			// 当前 Cell 
			this.cell = null;
			
			// 基本坐标
			this.x = new Number(defObj.@x[0]);
			this.y = new Number(defObj.@y[0]);
			this.z = new Number(defObj.@z[0]);
			if (isNaN(this.x))
				this.x = 0;
			if (isNaN(this.y))
				this.y = 0;
			if (isNaN(this.z))
				this.z = 0;
			
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
		
		public void moveTo(float x, float y, float z)
		{			
			// 设置新的坐标	   
			this.x = x;
			this.y = y;
			this.z = z;			
		}

		public void follow(fElement target, float elasticity = 0)
		{
			this.offx = target.x - this.x;
			this.offy = target.y - this.y;
			this.offz = target.z - this.z;
			
			this.elasticity = 1 + elasticity;
			// KBEN: 如果这个地方跟随者没有移动，moveListener 这个函数就不会被调用
			target.addEventListener(fElement.MOVE, this.moveListener, false, 0, true);
		}
		
		public void stopFollowing(fElement target)
		{
			target.removeEventListener(fElement.MOVE, this.moveListener);
		}
		
		public fElement moveListener(fMoveEvent evt)
		{
			if (this.elasticity == 1)
				this.moveTo(evt.target.x - this.offx, evt.target.y - this.offy, evt.target.z - this.offz);
			else
			{
				this.destx = evt.target.x - this.offx;
				this.desty = evt.target.y - this.offy;
				this.destz = evt.target.z - this.offz;
				fEngine.stage.addEventListener('enterFrame', this.followListener, false, 0, true);
			}
		}
		
		public void followListener(Event evt)
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
				fEngine.stage.removeEventListener('enterFrame', this.followListener);
			}
		}
		
		public float distanceTo(float x, float y, float z)
		{
			return mathUtils.distance(x, y, this.x, this.y);
		}
		
		public void disposeElement()
		{
			this.xmlObj = null;
			this.cell = null;
			this._controller = null;
			if (fEngine.stage)
				fEngine.stage.removeEventListener('enterFrame', this.followListener);
		}
		
		public void dispose()
		{
			this.customData.flash9Renderer = null;
			this.disposeElement();
		}
		
		public void onTick(float deltaTime)
		{
			
		}
	}
}