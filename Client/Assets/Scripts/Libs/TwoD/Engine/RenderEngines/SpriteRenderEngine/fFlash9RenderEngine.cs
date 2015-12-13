package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	// Imports
	import com.bit101.components.Component;
	import com.pblabs.engine.entity.EffectEntity;
	import com.pblabs.engine.entity.EntityCValue;
	import com.util.DebugBox;
	import org.ffilmation.engine.helpers.fUtil;
	
	import flash.display.Bitmap;
	import flash.display.DisplayObject;
	import flash.display.DisplayObjectContainer;
	import flash.display.MovieClip;
	import flash.display.Shape;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	
	import org.ffilmation.engine.core.fCamera;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fGlobalLight;
	import org.ffilmation.engine.core.fLight;
	//import org.ffilmation.engine.core.fPlane;
	import org.ffilmation.engine.core.fRenderableElement;
	import org.ffilmation.engine.core.fScene;
	import org.ffilmation.engine.datatypes.fCoordinateOccupant;
	import org.ffilmation.engine.elements.fCharacter;
	import org.ffilmation.engine.elements.fEmptySprite;
	import org.ffilmation.engine.elements.fFloor;
	import org.ffilmation.engine.elements.fFogPlane;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.helpers.fEngineCValue;
	import org.ffilmation.engine.interfaces.fEngineRenderEngine;
	import org.ffilmation.utils.objectPool;
	
	/**
	 * This is ffilmation's default flash9 isometric renderer
	 * @private
	 */
	public class fFlash9RenderEngine implements fEngineRenderEngine
	{
		// Private properties
		
		/** The scene rendered by this renderer */
		private var scene:fScene;
		
		/** The Sprite where this scene will be drawn  */
		private var container:Sprite;
		
		/** An array of all elementRenderers in this scene. An elementRenderer is a class that renders an specific element, for example a wallRenderer is associated to a fWall	*/
		private var renderers:Array;
		
		/** Viewport width */
		private var viewWidth:Number = 0;
		
		/** Viewport height */
		private var viewHeight:Number = 0;
		
		// KBEN: 渲染引擎层，这个仅仅是场景， UI 单独一层 
		private var m_SceneLayer:Vector.<Sprite>;
		// 保存的裁剪矩形
		public var m_scrollRect:Rectangle;
		
		/**
		 * Class constructor
		 */
		public function fFlash9RenderEngine(scene:fScene, container:Sprite, sceneLayer:Vector.<Sprite> = null):void
		{
			// Init items
			this.scene = scene;
			this.container = container;
			this.renderers = new Array;
			m_SceneLayer = sceneLayer;
			m_scrollRect = new Rectangle();
		}
		
		/**
		 * This method is called when the scene is to be displayed.
		 */
		public function initialize():void
		{
			try
			{				
				scene.m_timesOfShow++;
				this.scene.environmentLight.addEventListener(fLight.COLORCHANGE, this.processGlobalColorChange, false, 0, true);
				this.scene.environmentLight.addEventListener(fLight.INTENSITYCHANGE, this.processGlobalIntensityChange, false, 0, true);
				this.scene.environmentLight.addEventListener(fLight.RENDER, this.processGlobalIntensityChange, false, 0, true);
			}
			catch (e:Error)
			{
				var str:String = "fFlash9RenderEngine::initialize() ";
				if (scene)
				{
					str += "scene:" + fUtil.keyValueToString("m_disposed", scene.m_disposed, "m_timesOfShow", scene.m_timesOfShow, "sceneid", scene.m_serverSceneID);
					var key:String;
					for (key in scene.m_dicDebugInfo)
					{
						str += ", " + key + "=" + scene.m_dicDebugInfo[key];
					}
					if (scene.environmentLight)
					{
						str += " environmentLight";
					}
				}
				str += e.getStackTrace();
				
				DebugBox.sendToDataBase(str);
			}
		}
		
		/**
		 * This method initializes the render engine for an element in the scene.
		 */
		public function initRenderFor(element:fRenderableElement):fElementContainer
		{
			var renderer:fFlash9ElementRenderer = this.createRendererFor(element);
			return element.customData.flash9Renderer.container;
		}
		
		/**
		 * This method removes an element from the render engine
		 */
		public function stopRenderFor(element:fRenderableElement):void
		{
			// Delete renderer
			element.customData.flash9Renderer = null;
			this.renderers[element.uniqueId].dispose();
			delete this.renderers[element.uniqueId];
			
			// Free graphics
			fFlash9RenderEngine.recursiveDelete(element.container);
			objectPool.returnInstance(element.container);
		}
		
		/**
		 * This method returns the asset from the library that was used to display the element.
		 * It gets written as the "flashClip" property of the element.
		 */
		public function getAssetFor(element:fRenderableElement):MovieClip
		{
			return element.customData.flash9Renderer.flashClip;
		}
		
		/**
		 * This method updates the position of a character's sprite
		 */
		public function updateCharacterPosition(char:fCharacter):void
		{
			char.customData.flash9Renderer.place();
		}
		
		/**
		 * This method updates the position of an epmty Sprite's sprite
		 */
		public function updateEmptySpritePosition(spr:fEmptySprite):void
		{
			spr.customData.flash9Renderer.place();
		}
		
		/**
		 * This method updates the position of a bullet's sprite
		 */
		//public function updateBulletPosition(bullet:fBullet):void
		//{
		//	bullet.customData.flash9Renderer.place();
		//}
		
		// KBEN: 更新特效位置 
		public function updateEffectPosition(effect:EffectEntity):void
		{
			effect.customData.flash9Renderer.place();
		}
		
		// KBEN: 更新掉落物位置 
		public function updateFObjectPosition(fobj:fObject):void
		{
			fobj.customData.flash9Renderer.place();
		}
		
		// KBEN: 更新雾区域位置  
		public function updateFogPosition(fog:fFogPlane):void
		{
			fog.customData.flash9Renderer.place();
		}
		
		/**
		 * This method renders an element visible
		 **/
		public function showElement(element:fRenderableElement):void
		{			
			var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
			// KBEN: 资源加载尽量放在这资源加载里面，不要放在显示隐藏里面  
			if (!r.assetsCreated)
			{
				r.createAssets();
				r.renderGlobalLight(this.scene.environmentLight);
				if (element.hasOwnProperty("onCreateAssets"))
				{
					element["onCreateAssets"].apply(null, null);					
				}
				// KBEN: 资源在加载完成并且初始化后才设置加载完成标志位    
				//r.assetsCreated = true;
			}
			
			r.screenVisible = true;
			// KBEN: 不再有这些消息
			//this.applyPendingRenderMessages(element);
			r.show();
			
			// Dispatch creation event
			element.dispatchEvent(new Event(fRenderableElement.ASSETS_CREATED));
		}
		
		// This method applies pending render messages
		/*
		private function applyPendingRenderMessages(element:fRenderableElement):void
		{
			var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
			var messages:Array = r.renderMessages.getMessages();
			var l:int = messages.length;
			for (var i:int = 0; i < l; i++)
			{
				var messageObj:fRenderMessage = messages[i];
				var messageType:int = messageObj.message;
				try
				{
					switch (messageType)
					{
						//case fAllRenderMessages.LIGHT_IN: 
							//r.lightIn(messageObj.target as fLight);
							//break;
						//case fAllRenderMessages.LIGHT_OUT: 
							//r.lightOut(messageObj.target as fLight);
							//break;
						//case fAllRenderMessages.LIGHT_RESET: 
							//r.lightReset(messageObj.target as fLight);
							//break;
						case fAllRenderMessages.RENDER_START: 
							r.renderStart(messageObj.target as fLight);
							break;
						//case fAllRenderMessages.RENDER_LIGHT: 
							//r.renderLight(messageObj.target as fLight);
							//break;
						//case fAllRenderMessages.RENDER_SHADOW: 
							//r.renderShadow(messageObj.target as fLight, messageObj.target2 as fRenderableElement);
							//break;
						case fAllRenderMessages.RENDER_FINISH: 
							r.renderFinish(messageObj.target as fLight);
							break;
						//case fAllRenderMessages.UPDATE_SHADOW: 
							//r.updateShadow(messageObj.target as fLight, messageObj.target2 as fRenderableElement);
							//break;
						//case fAllRenderMessages.REMOVE_SHADOW: 
							//r.removeShadow(messageObj.target as fLight, messageObj.target2 as fRenderableElement);
							//break;
						case fAllRenderMessages.GLOBAL_INTESITY_CHANGE: 
							r.processGlobalIntensityChange(messageObj.target as fGlobalLight);
							break;
						case fAllRenderMessages.GLOBAL_COLOR_CHANGE: 
							r.processGlobalColorChange(messageObj.target as fGlobalLight);
							break;
						//case fAllRenderMessages.START_OCCLUSION: 
							//r.startOcclusion(messageObj.target as fCharacter);
							//break;
						//case fAllRenderMessages.UPDATE_OCCLUSION: 
							//r.updateOcclusion(messageObj.target as fCharacter);
							//break;
						//case fAllRenderMessages.STOP_OCCLUSION: 
							//r.stopOcclusion(messageObj.target as fCharacter);
							//break;
					}
				}
				catch (e:Error)
				{
					
				}
			}
			// Clear pending
			if (!fEngine.conserveMemory)
				r.renderMessages.reset();
		}
		*/
		
		/**
		 * This method renders an element invisible
		 **/
		public function hideElement(element:fRenderableElement):void
		{
			var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
			r.hide();
			r.screenVisible = false;
		/*if(fEngine.conserveMemory && r.assetsCreated) {
		   r.destroyAssets()
		   r.assetsCreated = false
		   // Dispatch destruction event
		   element.dispatchEvent(new Event(fRenderableElement.ASSETS_DESTROYED))
		 }*/
		}
		
		/**
		 * This method enables mouse events for an element
		 **/
		public function enableElement(element:fRenderableElement):void
		{
			element.customData.flash9Renderer.enableMouseEvents();
		}
		
		/**
		 * This method disables mouse events for an element
		 **/
		public function disableElement(element:fRenderableElement):void
		{
			element.customData.flash9Renderer.disableMouseEvents();
		}
		
		/**
		 * When a moving light reaches an element, this method is executed
		 */
		//public function lightIn(element:fRenderableElement, light:fOmniLight):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.lightIn(light);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.LIGHT_IN, light);
		//}
		
		/**
		 * When a moving light moves out of an element, this method is executed
		 */
		//public function lightOut(element:fRenderableElement, light:fOmniLight):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.lightOut(light);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.LIGHT_OUT, light, null, true);
		//}
		
		/**
		 * When a light is to be reset ( new size )
		 */
		//public function lightReset(element:fRenderableElement, light:fOmniLight):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.lightReset(light);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.LIGHT_RESET, light);
		//}
		
		/**
		 * This is the renderStart call.
		 */
		//public function renderStart(element:fRenderableElement, light:fOmniLight):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.renderStart(light);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.RENDER_START, light);
		//}
		
		/**
		 * This is the renderLight call.
		 */
		//public function renderLight(element:fRenderableElement, light:fOmniLight):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.renderLight(light);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.RENDER_LIGHT, light);
		//}
		
		/**
		 * This is the renderShadow call.
		 */
		//public function renderShadow(element:fRenderableElement, light:fOmniLight, shadow:fRenderableElement):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.renderShadow(light, shadow);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.RENDER_SHADOW, light, shadow);
		//}
		
		/**
		 * This is the renderFinish call.
		 */
		//public function renderFinish(element:fRenderableElement, light:fOmniLight):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.renderFinish(light);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.RENDER_FINISH, light);
		//}
		
		/**
		 * This is the updateShadow call.
		 */
		//public function updateShadow(element:fRenderableElement, light:fOmniLight, shadow:fRenderableElement):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.updateShadow(light, shadow);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.UPDATE_SHADOW, light, shadow);
		//}
		
		/**
		 * When an element is removed or hidden, or moves out of another element's range, its shadows need to be removed too
		 */
		//public function removeShadow(element:fRenderableElement, light:fOmniLight, shadow:fRenderableElement):void
		//{
		//	var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
		//	if (r.screenVisible)
		//		r.removeShadow(light, shadow);
		//	if (!r.screenVisible || fEngine.conserveMemory)
		//		r.renderMessages.addMessage(fAllRenderMessages.REMOVE_SHADOW, light, shadow, true);
		//}
		
		/**
		 * When the quality settings for the engine's shadows are changed, this method is called so old shadows are removed.
		 * There is no need for the renderer to redraw all shadows in this method: The engine rerenders the whole scene after
		 * this has been executed.
		 */
		//public function resetShadows():void
		//{
		//	for (var i:String in this.renderers)
		//		if (this.renderers[i].assetsCreated)
		//			this.renderers[i].resetShadows();
		//}
		
		/**
		 * Updates the render to show a given camera's position
		 */
		public function setCameraPosition(camera:fCamera):void
		{
			// 滚动矩形方式
			if (this.viewWidth > 0 && this.viewHeight > 0)
			{
				var p:Point = fScene.translateCoords(camera.x, camera.y, camera.z);
				// 这个就不每一次都申请了
				//var rect:Rectangle = new Rectangle();
				m_scrollRect.width = int(this.viewWidth);
				m_scrollRect.height = int(this.viewHeight);
				m_scrollRect.x = int(-this.viewWidth / 2 + p.x);
				m_scrollRect.y = int( -this.viewHeight / 2 + p.y);
				// bug 在这个地方判断有点问题，裁剪是根据摄像机的位置裁剪的，不是根据视口，视口仅仅是显示
				//if(scene.m_sceneType != EntityCValue.SCFight)
				//{
				//	if (rect.x < 0)
				//	{
				//		rect.x = 0;					
				//	}
				//	else if (rect.x > scene.widthpx() - rect.width)
				//	{
				//		rect.x = scene.widthpx() - rect.width;
				//	}
				//	if (rect.y < 0)
				//	{
				//		rect.y = 0;
				//	}
				//	else if (rect.y > scene.heightpx() - rect.height)
				//	{
				//		rect.y = scene.heightpx() - rect.height;
				//	}
				//}
				this.container.scrollRect = m_scrollRect;				
			}
			else
			{
				this.container.scrollRect = null;
			}

			/*
			// 坐标变换方式
			if (this.viewWidth > 0 && this.viewHeight > 0)
			{
				var p:Point = fScene.translateCoords(camera.x, camera.y, camera.z);
				var rect:Rectangle = new Rectangle();
				rect.width = this.viewWidth;
				rect.height = this.viewHeight;
				rect.x = Math.round(-this.viewWidth / 2 + p.x);
				rect.y = Math.round( -this.viewHeight / 2 + p.y);
				if(scene.m_sceneType != EntityCValue.SCFight)
				{
					if (rect.x < 0)
					{
						rect.x = 0;					
					}
					else if (rect.x > scene.widthpx() - rect.width)
					{
						rect.x = scene.widthpx() - rect.width;
					}
					if (rect.y < 0)
					{
						rect.y = 0;
					}
					else if (rect.y > scene.heightpx() - rect.height)
					{
						rect.y = scene.heightpx() - rect.height;
					}
				}
				//this.container.scrollRect = rect;
				// 计算位置
				
				for each(var con:Sprite in scene.m_SceneLayer)
				{
					if(con)
					{
						con.x = -rect.x;
						con.y = -rect.y;
					}
				}
			}
			else
			{
				//this.container.scrollRect = null;
			}
			*/
		}
		
		/**
		 * Updates the viewport size. This call will be immediately followed by a setCameraPosition call
		 * @see org.ffilmation.engine.interfaces.fRenderEngine#setCameraPosition
		 */
		public function setViewportSize(width:Number, height:Number):void
		{
			this.viewWidth = width;
			this.viewHeight = height;
		}
		
		/**
		 * Starts acclusion related to one character
		 * @param element Element where occlusion is applied
		 * @param character Character causing the occlusion
		 */
		public function startOcclusion(element:fRenderableElement, character:fCharacter):void
		{
			var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
			if (r.screenVisible)
				r.startOcclusion(character);
			// KBEN: 去掉 rendermessage
			//if (!r.screenVisible || fEngine.conserveMemory)
			//	r.renderMessages.addMessage(fAllRenderMessages.START_OCCLUSION, character);
		}
		
		/**
		 * Updates acclusion related to one character
		 * @param element Element where occlusion is applied
		 * @param character Character causing the occlusion
		 */
		public function updateOcclusion(element:fRenderableElement, character:fCharacter):void
		{
			var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
			if (r.screenVisible)
				r.updateOcclusion(character);
			// KBEN: 去掉 rendermessage
			//if (!r.screenVisible || fEngine.conserveMemory)
			//	r.renderMessages.addMessage(fAllRenderMessages.UPDATE_OCCLUSION, character);
		}
		
		/**
		 * Stops acclusion related to one character
		 * @param element Element where occlusion is applied
		 * @param character Character causing the occlusion
		 */
		public function stopOcclusion(element:fRenderableElement, character:fCharacter):void
		{
			var r:fFlash9ElementRenderer = element.customData.flash9Renderer;
			if (r.screenVisible)
				r.stopOcclusion(character);
			// KBEN: 去掉 rendermessage
			//if (!r.screenVisible || fEngine.conserveMemory)
			//	r.renderMessages.addMessage(fAllRenderMessages.STOP_OCCLUSION, character, null, true);
		}
		
		/**
		 * This method returns the element under a Stage coordinate, and a 3D translation of the 2D coordinates passed as input.
		 */

//		// 这个点选遍历太多了
//		public function translateStageCoordsToElements(x:Number, y:Number):Array
//		{
//			var found:Array = [];
//			var ret:Array = [];
//			var p:Point = new Point();
//			for (var i:String in this.renderers)
//			{
//				var el:fRenderableElement = null;
//				if (this.renderers[i].container)
//					el = this.renderers[i].container.fElement;
//					
//				// 雾面板不做任何处理     
//				if (el is fFogPlane)
//				{
//					continue;
//				}
//				// KBEN: 特效直接忽略   
//				else if (el is EffectEntity)
//				{
//					continue;
//				}
//				// fPlane 使用 hitTestPoint 测试， fObject 使用 bitmapData.hitTest 测试
//				else if (el is fFloor)
//				{
//					if (found.indexOf(el) < 0 && el.container.hitTestPoint(x, y, true) /*&& this.currentOccluding.indexOf(el)<0*/)
//					{					
//						// Avoid repeated results
//						found[found.length] = el;
//						
//						// Get local coordinate
//						p.x = x;
//						p.y = y;						
//						if (el is fPlane)
//						{
//							var r:fFlash9PlaneRenderer = (el.customData.flash9Renderer as fFlash9PlaneRenderer);
//							// KBEN: 坐标转换到局部坐标
//							if (this.scene.m_sceneConfig.mapType == fEngineCValue.Engine2d)
//							{
//								p = r.container.globalToLocal(p);
//							}
//							//else
//							//{
//							//	p = r.deformedSimpleShadowsLayer.globalToLocal(p);
//							//}
//							
//							if (r.scrollR.containsPoint(p))
//							{
//								// Push data
//								if (el is fFloor)
//									ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y + p.y, el.z));
//								// (el is fWall)
//								//{
//								//	var w:fWall = el as fWall;
//								//	if (w.vertical)
//								//		ret[ret.length] = (new fCoordinateOccupant(w, w.x, w.y0 + p.x, w.z - p.y));
//								//	else
//								//		ret[ret.length] = (new fCoordinateOccupant(w, w.x0 + p.x, w.y, w.z - p.y));
//								//}	
//							}
//						}
//						
//						//if (el is fObject)
//						//{
//							//p = el.container.globalToLocal(p);
//							//ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
//						//}
//					}
//				}
//				else if (el is fObject)
//				{	
//					if (found.indexOf(el) < 0 && this.renderers[i].hitTest(x, y))
//					{
//						found[found.length] = el;
//						p.x = x;
//						p.y = y;
//						p = el.container.globalToLocal(p);
//						ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
//					}
//				}
//			}
//			
//			// Sort elements by depth, closest to camera first
//			var sortOnDepth:Function = function(a:fCoordinateOccupant, b:fCoordinateOccupant):Number
//			{
//				if (a.element._depth > b.element._depth)
//					return 1;
//				else if (a.element._depth < b.element._depth)
//					return -1;
//				else
//					return 0;
//			}
//			ret.sort(sortOnDepth);
//			
//			// Return
//			if (ret.length == 0)
//				return null;
//			else
//				return ret;
//		}

		// 直接获取对象
		public function translateStageCoordsToElements(x:Number, y:Number):Array
		{
			var ret:Array = [];
			var p:Point = new Point();
			
			// 查找 fFloor
			var floor:fFloor;
			var spt:Point = this.scene.convertG2S(x, y);
			floor = this.scene.getFloorAtByPos(spt.x, spt.y);
			if (floor == null)
			{
				return null;
			}

			var buildentity:Array;	// 在 SLBuild 层的 npc
			buildentity = new Array();
			
			var el:fRenderableElement = null;
			var i:String = "";
			// 查找其它
			for (i in this.scene.renderManager.charactersV)
			{
				el = this.scene.renderManager.charactersV[i];
				
				if (el.customData.flash9Renderer.hitTest(x, y))
				{
					p.x = x;
					p.y = y;
					p = el.container.globalToLocal(p);
					if(EntityCValue.SLBuild != el.layer)	// 如果不是在 SLBuild 层的 npc ,需要放在地形上面,所有其它的下面
					{
						ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
					}
					else
					{
						buildentity[buildentity.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
					}
				}
			}
			
			// 查找其它
			for (i in this.scene.renderManager.elementsV)
			{
				el = this.scene.renderManager.elementsV[i];
				// KBEN: 特效直接忽略   
				if (el is EffectEntity)
				{
					continue;
				}
				else if (el is fObject)
				{	
					if (el.customData.flash9Renderer.hitTest(x, y))
					{
						p.x = x;
						p.y = y;
						p = el.container.globalToLocal(p);
						ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
					}
				}
			}
			
			// Sort elements by depth, closest to camera first
			var sortOnDepth:Function = function(a:fCoordinateOccupant, b:fCoordinateOccupant):Number
			{
				if (a.element._depth < b.element._depth)
					return 1;
				else if (a.element._depth > b.element._depth)
					return -1;
				else
					return 0;
			}
			ret.sort(sortOnDepth);
			// 放入 SLBuild 层的 npc
			if(buildentity.length)
			{
				ret = ret.concat(buildentity);
			}
			p.x = x;
			p.y = y;
			//var r:fFlash9PlaneRenderer = (floor.customData.flash9Renderer as fFlash9PlaneRenderer);
			//p = r.container.globalToLocal(p);
			p = floor.container.globalToLocal(p);
			ret[ret.length] = (new fCoordinateOccupant(floor, floor.x + p.x,floor.y + p.y, floor.z));
			
			// Return
			if (ret.length == 0)
				return null;
			else
				return ret;
		}

		/*
		// 使用这个点选, getObjectsUnderPoint 在窗口大小改变的时候经常出现点选不了的 bug
		public function translateStageCoordsToElements(x:Number, y:Number):Array
		{
			var parent:DisplayObject;
			var eleCon:fElementContainer;
			var el:fRenderableElement = null;
			
			var found:Array = [];
			var ret:Array = [];
			var p:Point = new Point();
			
			// 获取场景中鼠标下面的绘制对象
			var lst:Array = this.scene.container.getObjectsUnderPoint(new Point(x, y));
			// 从最后一个遍历，最上面的在最后一个
			// 这里先遍历最后一个，因为外部是从最后一个开始遍历的 onClick(evt:MouseEvent) 
			var idx:int = 0;
			while(idx < lst.length)
			{
				// 获取 fElementContainer 对象
				parent = null;
				eleCon = null;
				eleCon = getRenderContainer(lst[idx], parent, eleCon);
				
				if(eleCon)
				{
					el = null;
					el = eleCon.fElement;
					
					// 雾面板不做任何处理     
					if (el is fFogPlane)
					{
						// bug: 这个地方会死循环
						++idx;
						continue;
					}
					// KBEN: 特效直接忽略   
					else if (el is EffectEntity)
					{
						// bug: 这个地方会死循环
						++idx;
						continue;
					}
					// fPlane 使用 hitTestPoint 测试， fObject 使用 bitmapData.hitTest 测试
					else if (el is fFloor)
					{
						if (found.indexOf(el) < 0 && el.container.hitTestPoint(x, y, true))
						{					
							// Avoid repeated results
							found[found.length] = el;
							
							// Get local coordinate
							p.x = x;
							p.y = y;						
							if (el is fPlane)
							{
								var r:fFlash9PlaneRenderer = (el.customData.flash9Renderer as fFlash9PlaneRenderer);
								// KBEN: 坐标转换到局部坐标
								if (this.scene.m_sceneConfig.mapType == fEngineCValue.Engine2d)
								{
									p = r.container.globalToLocal(p);
								}
								
								if (r.scrollR.containsPoint(p))
								{
									// Push data
									if (el is fFloor)
									{
										ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y + p.y, el.z));
									}
								}
							}
						}
					}
					else if (el is fObject)
					{	
						if (found.indexOf(el) < 0 && eleCon.fElement.customData.flash9Renderer.hitTest(x, y))
						{
							p.x = x;
							p.y = y;
							p = el.container.globalToLocal(p);
							ret[ret.length] = (new fCoordinateOccupant(el, el.x + p.x, el.y, el.z - p.y));
						}
					}
				}
				
				++idx;
			}
			
			// Return
			if (ret.length == 0)
				return null;
			else
				return ret;
		}
		*/

		// 根据显示对象，查找其容器
		public function getRenderContainer(obj:DisplayObject, parent:DisplayObject, eleCon:fElementContainer):fElementContainer
		{
			if(!(obj is Bitmap))	// 只有图片才能接受场景消息
			{
				return null;
			}
			
			parent = obj.parent;
			eleCon = parent as fElementContainer;
			while(!eleCon && parent)
			{
				parent = parent.parent;
				eleCon = parent as fElementContainer;
			}
			
			return eleCon;
		}
		
		/**
		 * Frees all allocated resources. This is called when the scene is hidden or destroyed.
		 */
		public function dispose():void
		{
			// Stop listeners
			this.scene.environmentLight.removeEventListener(fLight.COLORCHANGE, this.processGlobalColorChange);
			this.scene.environmentLight.removeEventListener(fLight.INTENSITYCHANGE, this.processGlobalIntensityChange);
			this.scene.environmentLight.removeEventListener(fLight.RENDER, this.processGlobalIntensityChange);
			
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
			this.renderers = new Array;
			// KBEN: 这个地方会递归移除 container 上的所有节点  
			fFlash9RenderEngine.recursiveDelete(this.container);
			
			this.m_SceneLayer = null;
		}
		
		// INTERNAL
		
		/**
		 * This method retrieves the projected Sprite corresponding to a given element and floor size
		 * @private
		 */
		//public function getObjectSpriteProjection(element:fObject, floorz:Number, x:Number, y:Number, z:Number):fObjectProjection
		//{
		//	return element.customData.flash9Renderer.getSpriteProjection(floorz, x, y, z);
		//}
		
		/**
		 * This method retrieves the Sprite representing the shadow of a given fObject
		 * @private
		 */
		//public function getObjectShadow(element:fObject, request:fRenderableElement):fObjectShadow
		//{
		//	return element.customData.flash9Renderer.getShadow(request);
		//}
		
		/**
		 * This method returns an unused shadow to the pool
		 * @private
		 */
		//public function returnObjectShadow(sh:fObjectShadow):void
		//{
		//	sh.object.customData.flash9Renderer.returnShadow(sh);
		//}
		
		/**
		 * This event listener is executed when the global light changes its intensity
		 */
		private function processGlobalIntensityChange(evt:Event):void
		{
			for (var i:String in this.renderers)
			{
				if (this.renderers[i].screenVisible)
					this.renderers[i].processGlobalIntensityChange(evt.target as fGlobalLight);
				// KBEN: 去掉 rendermessage
				//if (!this.renderers[i].screenVisible || fEngine.conserveMemory)
				//	this.renderers[i].renderMessages.addMessage(fAllRenderMessages.GLOBAL_INTESITY_CHANGE, evt.target as fGlobalLight);
			}
		}
		
		/**
		 * This event listener is executed when the global light changes its color
		 */
		private function processGlobalColorChange(evt:Event):void
		{
			for (var i:String in this.renderers)
			{
				if (this.renderers[i].screenVisible)
					this.renderers[i].processGlobalColorChange(evt.target as fGlobalLight);
				// KBEN: 去掉 rendermessage
				//if (!this.renderers[i].screenVisible || fEngine.conserveMemory)
				//	this.renderers[i].renderMessages.addMessage(fAllRenderMessages.GLOBAL_COLOR_CHANGE, evt.target as fGlobalLight);
			}
		}
		
		/**
		 * Creates the renderer associated to a renderableElement. The renderer is created if it doesn't exist.
		 */
		private function createRendererFor(element:fRenderableElement):fFlash9ElementRenderer
		{
			//Create renderer if it doesn't exist
			if (!this.renderers[element.uniqueId])
			{
				var spr:fElementContainer = objectPool.getInstanceOf(fElementContainer) as fElementContainer;
				// KBEN: 不添加到根节点  
				//this.container.addChild(spr);
				
				if (element is fFloor)
				{
					// KBEN: 添加到自己层，这一句一定要添加在后面语句的前面      
					m_SceneLayer[EntityCValue.SLTerrain].addChild(spr);
					element.customData.flash9Renderer = new fFlash9FloorRenderer(this, spr, element as fFloor);
				}
				//else if (element is fWall)
				//	element.customData.flash9Renderer = new fFlash9WallRenderer(this, spr, element as fWall);
				else if (element is fObject)
				{
					// KBEN: 添加到自己层
					if(EntityCValue.SLBuild != element.layer)		// 如果不是地物层
					{
						m_SceneLayer[EntityCValue.SLObject].addChild(spr);
					}
					else
					{
						m_SceneLayer[EntityCValue.SLBuild].addChild(spr);
					}
					//if ((element as fObject).definition.objType == fEngineCValue.MdMovieClip)
					//{
					//	element.customData.flash9Renderer = new fFlash9ObjectRenderer(this, spr, element as fObject);
					//}
					if ((element as fObject).definition.objType == fEngineCValue.MdPicSeq)
					{
						element.customData.flash9Renderer = new fFlash9SceneObjectSeqRenderer(this, spr, element as fObject);
					}
					//else if ((element as fObject).definition.objType == fEngineCValue.MdPicOne)
					//{
					//	element.customData.flash9Renderer = new fFlash9ObjectOneRenderer(this, spr, element as fObject);
					//}
					else if ((element as fObject).definition.objType == fEngineCValue.MdEffPicSeq)
					{
						element.customData.flash9Renderer = new fFlash9ObjectEffSeqRenderer(this, spr, element as fObject);
					}
					else
					{
						// KBEN: 日志输出
						throw new Error("createRendererFor type cannot distinguish");
					}
				}
				//else if (element is fBullet)
				//{
					// KBEN: 添加到自己层      
				//	m_SceneLayer[fScene.SLObject].addChild(spr);
				//	element.customData.flash9Renderer = new fFlash9BulletRenderer(this, spr, element as fBullet);
				//}
				else if (element is fFogPlane)
				{
					// KBEN: 添加到自己层      
					m_SceneLayer[EntityCValue.SLFog].addChild(spr);
					element.customData.flash9Renderer = new fFlash9FogRenderer(this, spr, element as fFogPlane);
				}
				else if (element is fEmptySprite)
				{
					m_SceneLayer[EntityCValue.SLObject].addChild(spr);
					element.customData.flash9Renderer = new fFlash9EmptySpriteRenderer(this, spr, element as fEmptySprite)
				}
				else
				{
					// KBEN: 添加到自己层      
					throw new Error("createRendererFor type cannot distinguish");
				}
				
				this.renderers[element.uniqueId] = element.customData.flash9Renderer;
			}
			
			// Return it
			return this.renderers[element.uniqueId];
		}
		
		// Recursively deletes all DisplayObjects in the container hierarchy
		public static function recursiveDelete(d:DisplayObjectContainer):void
		{
			if (!d)
				return;
			if (d.numChildren != 0)
				do
				{
					var c:DisplayObject = d.getChildAt(0);
					if (c != null)
					{
						c.cacheAsBitmap = false;
						if (c is Component)
						{
							
						}
						else if (c is DisplayObjectContainer)
							fFlash9RenderEngine.recursiveDelete(c as DisplayObjectContainer);
						if (c is MovieClip)
							(c as MovieClip).stop();
						if (c is Bitmap)
						{
							var b:Bitmap = c as Bitmap;
							// bug: BitmapData 大部分资源都是共用的，因此不能删除，需要自己手工清理   
							//if (b.bitmapData)
							//	b.bitmapData.dispose();
						} 
						if (c is Shape)
							(c as Shape).graphics.clear();
						d.removeChild(c);
					}
				} while (d.numChildren != 0 && c != null);
			
			if (d is Sprite)
				(d as Sprite).graphics.clear();
		}
		
		public function getSceneLayer(id:uint):Sprite
		{
			return m_SceneLayer[id];
		}
		
		public function set SceneLayer(value:Vector.<Sprite>):void 
		{
			m_SceneLayer = value;
		}
		
		public function get scrollRect():Rectangle
		{
			return m_scrollRect;
		}
	}
}