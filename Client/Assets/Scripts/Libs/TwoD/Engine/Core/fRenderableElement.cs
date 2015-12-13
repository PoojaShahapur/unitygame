namespace SDK.Lib
{
	public class fRenderableElement : fElement
	{
		public fElementContainer container;
		
		public bool _visible = true;
		public float x0;
		public float y0;
		public float x1;
		public float y1;
		public float top;

		public float _depth = 0;
		public int depthOrder;
		public bool isVisibleNow = false;
		public bool willBeVisible = false;

		// KBEN: 可渲染元素，必然有资源，字典中存放一个数组，里面存放是的每一个方向的资源      
		public var _resDic:Dictionary;	//[act,Dictionary]的集合。其中Dictionary的是[direction, SWFResource]
		// fObjectDefinition 是否初始化  
		public var m_ObjDefRes:SWFResource;
		
		// KBEN: 可绘制的元素属于的 fFloor 索引 
		//public var m_floorIdx:int = -1;	// -1 表示没有 floor ，按一行一行
		// KBEN: 资源类型，决定资源加载的目录  
		public uint m_resType = 0;
		protected bool m_bDisposed = false;
		protected bool m_needDepthSort = true;

		public static string DEPTHCHANGE = "renderableElementDepthChange";
		
		public static string SHOW = "renderableElementShow";

		public static string HIDE = "renderableElementHide";

		public static string ENABLE = "renderableElementEnable";

		public static string DISABLE = "renderableElementDisable";

		public static string ASSETS_CREATED = "renderableElementAssetsCreated";

		public static string ASSETS_DESTROYED = "renderableElementAssetsDestroyed";

		public fRenderableElement(XmlNode defObj, bool noDepthSort = false)
		{
			super(defObj);
			_resDic = new Dictionary();
		}
		
		public void disableMouseEvents()
		{
			dispatchEvent(new Event(fRenderableElement.DISABLE));
		}
		
		public void enableMouseEvents()
		{
			dispatchEvent(new Event(fRenderableElement.ENABLE));
		}
		
		public void show()
		{
			if (!this._visible)
			{
                this._visible = true;
				dispatchEvent(new Event(fRenderableElement.SHOW))
			}
		}
		
		public void hide()
		{
			if (this._visible)
			{
				this._visible = false;
				dispatchEvent(new Event(fRenderableElement.HIDE));
			}
		}

		// Depth 管理
		// dispatchmsg = true 是否分发消息
		public void setDepth(float d, bool dispatchmsg = true)
		{
			this._depth = d;
			
			// 记录所有的对象
			if(dispatchmsg)
			{
				this.dispatchEvent(new Event(fRenderableElement.DEPTHCHANGE));
			}
		}

		public float distance2d(float x, float y, float z)
		{
            Point p2d = fScene.translateCoords(x, y, z);
			return this.distance2dScreen(p2d.x, p2d.y);
		}

		public void distance2dScreen(float x, float y)
		{
			if (this is fMovingElement)
			{
				this.screenArea = this.bounds2d.clone();
				this.screenArea.offsetPoint(fScene.translateCoords(this.x, this.y, this.z));
			}
			
			// Test bounds
			var bounds:Rectangle = this.screenArea;
			var pos2D:Point = new Point(x, y);
			var dist:Number = Infinity;
			if (bounds.contains(pos2D.x, pos2D.y))
				return 0;
			
			var corner1:Point = new Point(bounds.left, bounds.top);
			var corner2:Point = new Point(bounds.left, bounds.bottom);
			var corner3:Point = new Point(bounds.right, bounds.bottom);
			var corner4:Point = new Point(bounds.right, bounds.top);
			
			var d:Number = mathUtils.distancePointToSegment(corner1, corner2, pos2D);
			if (d < dist)
				dist = d;
			d = mathUtils.distancePointToSegment(corner2, corner3, pos2D);
			if (d < dist)
				dist = d;
			d = mathUtils.distancePointToSegment(corner3, corner4, pos2D);
			if (d < dist)
				dist = d;
			d = mathUtils.distancePointToSegment(corner4, corner1, pos2D);
			if (d < dist)
				dist = d;
			
			return dist;
		}
		
		public void disposeRenderable()
		{

		}
		
		public override void dispose()
		{
			m_bDisposed = true;
			base.dispose();
			this.disposeRenderable();
		}
		
		public void loadRes(uint act, uint direction)
		{
			
		}
		
		public void loadObjDefRes()
		{
			
		}
		
		// KBEN: 渲染显示会回调这个函数     
		public void showRender()
		{
			
		}
		
		// KBEN: 渲染隐藏会回调这个函数     
		public void hideRender()
		{
			
		}	
		
		// 判断某个资源的动作是否加载 
		public bool actLoaded(uint act, uint direction)
		{
			var resact:Dictionary;
			resact = this._resDic[act];
			if (resact)
			{
				if (resact[direction])
				{
					if (resact[direction].isLoaded)
					{
						if (!resact[direction].didFail)
						{
							return true;
						}
					}
				}
			}
			
			return false;
		}
		
		public uint actByRes(res:SWFResource)
		{
			for (var key:String in _resDic)
			{
				if (_resDic[key] == res)
				{
					return parseInt(key);
				}
			}
			
			return 0;
		}
		
		public Dictionary getResDic() 
		{
			return _resDic;
		}
		
		public void onMouseEnter()
		{
			
		}
		public void onMouseLeave()
		{
			
		}
		
		// 这个主要用在有些 npc 需要放在的地物层，永远放在人物层下面，不排序
		public uint getLayer()
		{
			return 0;
		}
		
		public void setLayer(uint value)
		{
			
		}
		public bool getIsDisposed()
		{
			return m_bDisposed;
		}
		
		// KBEN:
		override public void onTick(float deltaTime)
		{
			// KBEN: 可视化判断，需要添加
			if (this._visible && this.isVisibleNow)
			{
				if (customData.flash9Renderer)
				{
					customData.flash9Renderer.onTick(deltaTime);
				}
			}
		}
		
		public void setNeedDepthSort(bool bNeed)
		{
			m_needDepthSort = bNeed;
		}
	}
}