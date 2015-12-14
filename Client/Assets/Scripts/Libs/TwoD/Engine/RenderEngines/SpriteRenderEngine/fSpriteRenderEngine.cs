using System;
using System.Collections;
using System.Collections.Generic;

namespace SDK.Lib
{
	public class fSpriteRenderEngine : fEngineRenderEngine
	{
		private fScene scene;

		private SpriteContainer container;

		private Dictionary<int, fSpriteElementRenderer> renderers;
		
		private float viewWidth = 0;
		
		private float viewHeight = 0;
		
		// KBEN: 渲染引擎层，这个仅仅是场景， UI 单独一层 
		private MList<SpriteLayer> m_SceneLayer;
		// 保存的裁剪矩形
		//public Rectangle m_scrollRect;

		public fSpriteRenderEngine(fScene scene, MList<SpriteLayer> sceneLayer = null)
		{
			// Init items
			this.scene = scene;
			this.container = container;
			this.renderers = new Dictionary<int, fFlash9ElementRenderer>();
			m_SceneLayer = sceneLayer;
			//m_scrollRect = new Rectangle();
		}
		
		public void initialize()
		{
			try
			{				
				scene.m_timesOfShow++;
			}
			catch (Exception e)
			{
				string str = "fFlash9RenderEngine::initialize() ";
			}
		}
		
		public fElementContainer initRenderFor(fRenderableElement element)
		{
            fFlash9ElementRenderer renderer = this.createRendererFor(element);
			return element.flash9Renderer.container;
		}

		public void stopRenderFor(fRenderableElement element)
		{
			element.flash9Renderer = null;
			this.renderers[element.uniqueId].dispose();
			this.renderers.Remove(element.uniqueId);
			
			// fFlash9RenderEngine.recursiveDelete(element.container);
			//objectPool.returnInstance(element.container);
		}

		public void updateCharacterPosition(fCharacter character)
		{
            character.flash9Renderer.place();
		}

		// KBEN: 更新特效位置 
		//public void updateEffectPosition(EffectEntity effect)
		//{
		//	effect.flash9Renderer.place();
		//}

		public void showElement(fRenderableElement element)
		{
            fFlash9ElementRenderer r = element.flash9Renderer;
			// KBEN: 资源加载尽量放在这资源加载里面，不要放在显示隐藏里面  
			if (!r.assetsCreated)
			{
				r.createAssets();
			}
			
			r.screenVisible = true;
			r.show();
			
			element.dispatchEvent(fRenderableElement.ASSETS_CREATED, null);
		}

		public void hideElement(fRenderableElement element)
		{
            fFlash9ElementRenderer r = element.flash9Renderer;
			r.hide();
			r.screenVisible = false;
		}

		public void enableElement(fRenderableElement element)
		{
			element.flash9Renderer.enableMouseEvents();
		}

		public void disableElement(fRenderableElement element)
		{
			element.flash9Renderer.disableMouseEvents();
		}

		public void setCameraPosition(fCamera camera)
		{
			//// 滚动矩形方式
			//if (this.viewWidth > 0 && this.viewHeight > 0)
			//{
			//	var p:Point = fScene.translateCoords(camera.x, camera.y, camera.z);
			//	m_scrollRect.width = int(this.viewWidth);
			//	m_scrollRect.height = int(this.viewHeight);
			//	m_scrollRect.x = int(-this.viewWidth / 2 + p.x);
			//	m_scrollRect.y = int( -this.viewHeight / 2 + p.y);
			//	// bug 在这个地方判断有点问题，裁剪是根据摄像机的位置裁剪的，不是根据视口，视口仅仅是显示
			//	this.container.scrollRect = m_scrollRect;
			//}
			//else
			//{
			//	this.container.scrollRect = null;
			//}
		}

		public void setViewportSize(float width, float height)
		{
			this.viewWidth = width;
			this.viewHeight = height;
		}

		// 直接获取对象
		public ArrayList translateStageCoordsToElements(float x, float y)
		{
            return null;
		}

		public void dispose()
		{
			//// Delete resources
			//for (string i in this.renderers)
			//{
			//	this.renderers[i].element.customData.flash9Renderer = null;
			//	this.renderers[i].dispose();
			//	if (this.renderers[i].element)
			//	{
			//		fFlash9RenderEngine.recursiveDelete(this.renderers[i].element.container);
			//		objectPool.returnInstance(this.renderers[i].element.container);
			//	}
			//	delete this.renderers[i];
			//}
			//this.renderers = null;
			//// KBEN: 这个地方会递归移除 container 上的所有节点  
			//fFlash9RenderEngine.recursiveDelete(this.container);
			
			//this.m_SceneLayer = null;
		}
   
		private fFlash9ElementRenderer createRendererFor(fRenderableElement element)
		{
			if (!this.renderers.ContainsKey(element.uniqueId))
			{
                fElementContainer spr = new fElementContainer();
				
				if (element is fFloor)
				{
					// KBEN: 添加到自己层，这一句一定要添加在后面语句的前面      
					//m_SceneLayer[EntityCValue.SLTerrain].addChild(spr);
					element.flash9Renderer = new fFlash9FloorRenderer(this, element as fFloor, 0, 0);
				}
				else
				{
					// KBEN: 添加到自己层      
					//throw new Error("createRendererFor type cannot distinguish");
				}
				
				this.renderers[element.uniqueId] = element.flash9Renderer;
			}
			
			return this.renderers[element.uniqueId];
		}
		
		public SpriteLayer getSceneLayer(int id)
		{
			return m_SceneLayer[id];
		}
		
		public void setSceneLayer(MList<SpriteLayer> value)
		{
			m_SceneLayer = value;
		}
		
		//public Rectangle getScrollRect()
		//{
		//	return m_scrollRect;
		//}
	}
}