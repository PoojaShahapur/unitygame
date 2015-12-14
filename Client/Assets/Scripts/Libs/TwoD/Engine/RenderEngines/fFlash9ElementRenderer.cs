namespace SDK.Lib
{
	public class fFlash9ElementRenderer : ITickedObject
	{
		public fRenderableElement element;

		public fFlash9RenderEngine rEngine;

		public fScene scene;
		
		public fElementContainer container;
		
		// 这个不再判断资源是否加载完成，现在不同的动作在不同资源里   
		public bool assetsCreated = false;

		public bool screenVisible = false;
		
		public fFlash9ElementRenderer(fFlash9RenderEngine rEngine, fRenderableElement element)
		{
			this.element = element;			
			this.rEngine = rEngine;
			this.place();
		}

		public void createAssets()
		{
		}

		public void destroyAssets()
		{

		}

		public void place()
		{
            Point coords = fScene.translateCoords(this.element.x, this.element.y, this.element.z);
			this.container.x = Math.floor(coords.x);
			this.container.y = Math.floor(coords.y);
		}

		public void disableMouseEvents()
		{
			
		}

		public void enableMouseEvents()
		{
			
		}

		public void show()
		{
			// KBEN: 更新链接元素显示   
			element.showRender();
		}

		public void hide()
		{
			// 防止调用多次隐藏挂掉
			if (containerParent && container.parent == containerParent)
			{
				this.containerParent.removeChild(this.container);
			}
			
			// KBEN: 更新链接元素显示    
			element.hideRender();
		}

		public void startOcclusion(fCharacter character)
		{

		}

		public void updateOcclusion(fCharacter character)
		{

		}

		public void stopOcclusion(fCharacter character)
		{

		}

		public void disposeRenderer()
		{
			this.element = null;	
			this.rEngine = null;
		}
		
		public void dispose()
		{
			this.disposeRenderer();
		}
		
		// KBEN:
		// 每一帧更新 
		public void onTick(float deltaTime)
		{
			
		}
		
		// KBEN:主要是资源类初始化类常量  
		public void init(SWFResource res, uint act, uint direction)
		{
			
		}
		
		// KBEN: 切换渲染显示容器，一般是把特效关联到一个实体上面  
		public void changeContainerParent(DisplayObjectContainer pnt)
		{
			if (this.container.parent)
			{
				this.containerParent.removeChild(this.container)
			}
			
			this.containerParent = pnt;
		}
		
		// KBEN: 动作是否播放完，重复动作总是返回 false ，不重复的动作播放完了返回 true 
		public bool aniOver() 
		{
			return false;
		}
		
		// 在换外观的时候使用，将外观的属性释放了，目前只实现人物   
		public void disposeShow()
		{
			
		}
		
		// 获取当前动作的方向， 0 - 7 
		public float getActDir()
		{
			return 0;
		}
		
		public int getCurrentFrame()
		{
			return 0;
		}
		
		public void setCurrentFrame(int value)
		{
			
		}
		
		public DisplayObjectContainer layerContainer(uint layer)
		{
			return null;
		}
		
		public void onMouseEnter()
		{
			
		}
		
		public void onMouseLeave()
		{
			
		}
		
		public void changeAngle(float value)
		{
			
		}

		public int getCurFrame(float deltaTime)
		{
			return 0;
		}
		
		public void clearName()
		{
			
		}
	}
}