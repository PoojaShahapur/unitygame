using System.Collections.Generic;
using System.Security;

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
		public Dictionary<int, TextureRes> _resDic;	//[act,Dictionary]的集合。其中Dictionary的是[direction, ]
		// fObjectDefinition 是否初始化
		public TextRes m_ObjDefRes;
		
		// KBEN: 可绘制的元素属于的 fFloor 索引 
		//public var m_floorIdx:int = -1;	// -1 表示没有 floor ，按一行一行
		// KBEN: 资源类型，决定资源加载的目录  
		public uint m_resType = 0;
		protected bool m_bDisposed = false;
		protected bool m_needDepthSort = true;

        // renderableElementDepthChange
        public static int DEPTHCHANGE = 0;
        // renderableElementShow
        public static int SHOW = 1;
        // renderableElementHide
        public static int HIDE = 2;
        // renderableElementEnable
        public static int ENABLE = 3;
        // renderableElementDisable
        public static int DISABLE = 4;
        // renderableElementAssetsCreated
        public static int ASSETS_CREATED = 5;
        // renderableElementAssetsDestroyed
        public static int ASSETS_DESTROYED = 6;

		public fRenderableElement(SecurityElement defObj, bool noDepthSort = false)
            : base(defObj)
		{
			_resDic = new Dictionary<int, TextureRes>();
		}
		
		public void disableMouseEvents()
		{
			dispatchEvent(fRenderableElement.DISABLE, this);
		}
		
		public void enableMouseEvents()
		{
			dispatchEvent(fRenderableElement.ENABLE, this);
		}
		
		public void show()
		{
			if (!this._visible)
			{
                this._visible = true;
                dispatchEvent(fRenderableElement.SHOW, this);
			}
		}
		
		public void hide()
		{
			if (this._visible)
			{
				this._visible = false;
				dispatchEvent(fRenderableElement.HIDE, this);
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
				this.dispatchEvent(fRenderableElement.DEPTHCHANGE, this);
			}
		}

		public float distance2d(float x, float y, float z)
		{
            return 0;
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
		
		virtual public void loadRes(uint act, uint direction)
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
			//var resact:Dictionary;
			//resact = this._resDic[act];
			//if (resact)
			//{
			//	if (resact[direction])
			//	{
			//		if (resact[direction].isLoaded)
			//		{
			//			if (!resact[direction].didFail)
			//			{
			//				return true;
			//			}
			//		}
			//	}
			//}
			
			return false;
		}
		
		public uint actByRes(TextureRes res)
		{
			//for (var key:String in _resDic)
			//{
			//	if (_resDic[key] == res)
			//	{
			//		return parseInt(key);
			//	}
			//}
			
			return 0;
		}
		
		public Dictionary<int, TextureRes> getResDic() 
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
		public void onTick(float deltaTime)
		{
			// KBEN: 可视化判断，需要添加
			if (this._visible && this.isVisibleNow)
			{
				if (this.flash9Renderer != null)
				{
                    this.flash9Renderer.onTick(deltaTime);
				}
			}
		}
		
		public void setNeedDepthSort(bool bNeed)
		{
			m_needDepthSort = bNeed;
		}
	}
}