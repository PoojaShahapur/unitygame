using System;
using System.Collections;
using System.Collections.Generic;

namespace SDK.Lib
{
	public class fFlash9RenderEngine : fEngineRenderEngine
	{
		private fScene scene;

		private SpriteContainer container;

		private Dictionary<int, fFlash9ElementRenderer> renderers;
		
		private float viewWidth = 0;
		
		private float viewHeight = 0;
		
		// KBEN: 渲染引擎层，这个仅仅是场景， UI 单独一层 
		private MList<SpriteLayer> m_SceneLayer;
		// 保存的裁剪矩形
		//public Rectangle m_scrollRect;

		public fFlash9RenderEngine(fScene scene, SpriteContainer container, MList<SpriteLayer> sceneLayer = null)
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
		public void updateEffectPosition(EffectEntity effect)
		{
			effect.flash9Renderer.place();
		}

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

		public void startOcclusion(fRenderableElement element, fCharacter character)
		{
            fFlash9ElementRenderer r = element.flash9Renderer;
			if (r.screenVisible)
				r.startOcclusion(character);
		}

		public void updateOcclusion(fRenderableElement element, fCharacter character)
		{
            fFlash9ElementRenderer r = element.flash9Renderer;
			if (r.screenVisible)
				r.updateOcclusion(character);
		}

		public void stopOcclusion(fRenderableElement element, fCharacter character)
		{
            fFlash9ElementRenderer r = element.flash9Renderer;
			if (r.screenVisible)
				r.stopOcclusion(character);
		}

		// 直接获取对象
		public ArrayList translateStageCoordsToElements(float x, float y)
		{
            //var ret:Array = [];
            //var p:Point = new Point();

            //// 查找 fFloor
            //var floor:fFloor;
            //var spt:Point = this.scene.convertG2S(x, y);
            //floor = this.scene.getFloorAtByPos(spt.x, spt.y);
            //if (floor == null)
            //{
            //	return null;
            //}

            //var buildentity:Array;	// 在 SLBuild 层的 npc
            //buildentity = new Array();

            //var el:fRenderableElement = null;
            //var i:String = "";
            //// 查找其它
            //for (i in this.scene.renderManager.charactersV)
            //{
            //	el = this.scene.renderManager.charactersV[i];

            //	if (el.customData.flash9Renderer.hitTest(x, y))
            //	{
            //		p.x = x;
            //		p.y = y;
            //		p = el.container.globalToLocal(p);
            //		if(EntityCValue.SLBuild != el.layer)	// 如果不是在 SLBuild 层的 npc ,需要放在地形上面,所有其它的下面
            //		{
            //			ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
            //		}
            //		else
            //		{
            //			buildentity[buildentity.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
            //		}
            //	}
            //}

            //// 查找其它
            //for (i in this.scene.renderManager.elementsV)
            //{
            //	el = this.scene.renderManager.elementsV[i];
            //	// KBEN: 特效直接忽略   
            //	if (el is EffectEntity)
            //	{
            //		continue;
            //	}
            //	else if (el is fObject)
            //	{	
            //		if (el.customData.flash9Renderer.hitTest(x, y))
            //		{
            //			p.x = x;
            //			p.y = y;
            //			p = el.container.globalToLocal(p);
            //			ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
            //		}
            //	}
            //}

            //// Sort elements by depth, closest to camera first
            //var sortOnDepth:Function = function(a:fCoordinateOccupant, b:fCoordinateOccupant):Number
            //{
            //	if (a.element._depth < b.element._depth)
            //		return 1;
            //	else if (a.element._depth > b.element._depth)
            //		return -1;
            //	else
            //		return 0;
            //}
            //ret.sort(sortOnDepth);
            //// 放入 SLBuild 层的 npc
            //if(buildentity.length)
            //{
            //	ret = ret.concat(buildentity);
            //}
            //p.x = x;
            //p.y = y;

            //p = floor.container.globalToLocal(p);
            //ret[ret.length] = (new fCoordinateOccupant(floor, floor.x + p.x,floor.y + p.y, floor.z));

            //// Return
            //if (ret.length == 0)
            //	return null;
            //else
            //	return ret;

            return null;
		}

		public void dispose()
		{
			// Delete resources
			for (var i:String in this.renderers)
			{
				this.renderers[i].element.customData.flash9Renderer = null;
				this.renderers[i].dispose();
				if (this.renderers[i].element)
				{
					fFlash9RenderEngine.recursiveDelete(this.renderers[i].element.container);
					objectPool.returnInstance(this.renderers[i].element.container);
				}
				delete this.renderers[i];
			}
			this.renderers = null;
			// KBEN: 这个地方会递归移除 container 上的所有节点  
			fFlash9RenderEngine.recursiveDelete(this.container);
			
			this.m_SceneLayer = null;
		}
   
		private fFlash9ElementRenderer createRendererFor(fRenderableElement element)
		{
			if (!this.renderers.ContainsKey(element.uniqueId))
			{
                fElementContainer spr = objectPool.getInstanceOf(fElementContainer) as fElementContainer;
				
				if (element is fFloor)
				{
					// KBEN: 添加到自己层，这一句一定要添加在后面语句的前面      
					m_SceneLayer[EntityCValue.SLTerrain].addChild(spr);
					element.flash9Renderer = new fFlash9FloorRenderer(this, spr, element as fFloor);
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