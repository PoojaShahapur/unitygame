// Basic renderable element class
package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	// Imports
	//import com.pblabs.engine.debug.Logger;
	import com.pblabs.engine.resource.SWFResource;
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.geom.ColorTransform;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	import flash.utils.Dictionary;
	import org.ffilmation.engine.core.fElement;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fEngine;
	import org.ffilmation.engine.core.fGlobalLight;
	import org.ffilmation.engine.core.fRenderableElement;
	import org.ffilmation.engine.core.fShadowQuality;
	import org.ffilmation.engine.elements.fCharacter;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.helpers.fActDefinition;
	import org.ffilmation.engine.helpers.fActDirectDefinition;
	import org.ffilmation.utils.movieClipUtils;
	import org.ffilmation.utils.objectPool;
	
	/**
	 * This class renders an fObject
	 * @private
	 * @brief 每一个动作在一张单独的图片上，不同的动作在不同的图片上         
	 */
	public class fFlash9ObjectOneRenderer extends fFlash9ElementRenderer
	{
		// Private properties
		private var baseObj:Sprite;
		//private var lights:Array;
		private var glight:fGlobalLight;
		//private var allShadows:Array;
		//private var currentSprite:Sprite;			// 这个此时是 Sprite 
		private var currentSpriteIndex:Number = -1;	// 这个是方向 
		private var _currentFrame:int = 0;			// 这个相当于 MovieClip 中的索引  
		//private var occlusionCount:Number = 0;
		private var _currentBitMap:Bitmap;			// 这个是序列帧动画    
		
		// These reflects how shadows are rendered
		//public var simpleShadows:Boolean = false;
		//public var eraseShadows:Boolean = true;
		
		// Protected properties
		//protected var projectionCache:fObjectProjectionCache;
		
		/** @private */
		//public var shadowObj:Class
		// KBEN: 简单阴影直接挂在人物身上显示 
		public var shadowSp:Sprite;
		// KBEN: 每一个 Key 对应一个动作，然后里面有8个方向的动作    
        protected var _framesDic:Dictionary = null;  
		// KBEN: 存储当前的动作    
		protected var _frames:Vector.<BitmapData> = null;  
		
		// Constructor
		/** @private */
		function fFlash9ObjectOneRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fObject):void
		{
			// Previous
			super(rEngine, element, null, container);
			
			// Angle
			var correctedAngle:Number = element._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			//this.currentSpriteIndex = int(correctedAngle * element.sprites.length);
			this.currentSpriteIndex = int(correctedAngle * element.definition.yCount);
			
			// KBEN: 
			_framesDic = new Dictionary();
			
			// Shadows
			//this.allShadows = new Array;
			//this.resetShadows();
			
			// Light control
			//this.lights = new Array();
			
			// Projection cache
			//this.projectionCache = new fObjectProjectionCache;
		}
		
		/**
		 * This method creates the assets for this plane. It is only called when the element in shown and the assets don't exist
		 */
		public override function createAssets():void
		{
			// Attach base clip
			this.baseObj = objectPool.getInstanceOf(Sprite) as Sprite;
			container.addChild(this.baseObj);
			this.baseObj.mouseEnabled = false;
			
			// Cache as bitmap non-animated objects
			var element:fObject = this.element as fObject;
			this.container.cacheAsBitmap = !(element is fCharacter) && element.animated != true;
			
			// Show and hide listeners, to redraw shadows
			element.addEventListener(fRenderableElement.SHOW, this.showListener, false, 0, true);
			element.addEventListener(fRenderableElement.HIDE, this.hideListener, false, 0, true);
			element.addEventListener(fObject.NEWORIENTATION, this.rotationListener, false, 0, true);
			element.addEventListener(fObject.GOTOANDPLAY, this.gotoAndPlayListener, false, 0, true);
			element.addEventListener(fObject.GOTOANDSTOP, this.gotoAndStopListener, false, 0, true);
			if (element is fCharacter)
				element.addEventListener(fElement.MOVE, this.moveListener, false, 0, true);
			
			//this.occlusionCount = 0;
			
			// Draw initial sprite
			this.currentSpriteIndex = -1;
			// KBEN: _currentBitMap 这个在初始化的时候创建    
			this._currentBitMap = objectPool.getInstanceOf(Bitmap) as Bitmap;
			this.baseObj.addChild(this._currentBitMap);
			_currentBitMap.x = element.bounds2d.x;
			_currentBitMap.y = element.bounds2d.y;
			// KBEN: 加载图片资源在显示的时候加载     
			element.loadRes(0, 0);
			
			// KBEN: 在自己脚底下添加简单的阴影    
			if (fEngine.shadowQuality == fShadowQuality.NORMAL)
			{
				addSimpleShadow();
			}
			
			// 这个放在这吧，不放在 init 函数里面了，因为如果创建 character 后直接 hideElement ，这个 xml 资源可能加载了，但是图片资源可能没加载，就不能设置 this.assetsCreated = true; 导致再次 showElement 的时候又会嗲用这个函数一次
			this.assetsCreated = true;
		}
		
		/**
		 * This method destroys the assets for this element. It is only called when the element in hidden and fEngine.conserveMemory is set to true
		 */
		public override function destroyAssets():void
		{
			// Current
			//objectPool.returnInstance(this.currentSprite);
			//if (this.baseObj && this.currentSprite)
			//	this.baseObj.removeChild(this.currentSprite);
			//this.currentSprite = null;
			
			// References
			this.flashClip = this.element.flashClip = null;
			
			// Events
			this.element.removeEventListener(fRenderableElement.SHOW, this.showListener);
			this.element.removeEventListener(fRenderableElement.HIDE, this.hideListener);
			this.element.removeEventListener(fObject.NEWORIENTATION, this.rotationListener);
			this.element.removeEventListener(fObject.GOTOANDPLAY, this.gotoAndPlayListener);
			this.element.removeEventListener(fObject.GOTOANDSTOP, this.gotoAndStopListener);
			if (this.element is fCharacter)
				this.element.removeEventListener(fElement.MOVE, this.moveListener);
			
			// Gfx
			if (this.baseObj)
			{
				container.removeChild(this.baseObj);
				fFlash9RenderEngine.recursiveDelete(this.baseObj);
				objectPool.returnInstance(this.baseObj);
				this.baseObj = null;
			}
			
			this.currentSpriteIndex = -1;
		}
		
		/**
		 * Listens to an object changing rotation and updates all sprites
		 */
		private function rotationListener(evt:Event = null):void
		{
			var el:fObject = this.element as fObject;
			var correctedAngle:Number = el._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			//var newSprite:int = int(correctedAngle * el.sprites.length);
			var newSprite:int = int(correctedAngle * el.definition.yCount);
			
			if (this.currentSpriteIndex != newSprite)
			{				
				// KBEN:支持在一张图上的内容   
				_currentBitMap.bitmapData = getFrame(_currentFrame, newSprite);
				
				// Update shadow model
				//if (!this.simpleShadows)
				//{
					//
					//var l:int = this.allShadows.length;
					//var shadowClase:Class = el.sprites[newSprite].shadow;
					//for (var i:int = 0; i < l; i++)
					//{
						//
						//var info:fObjectShadow = this.allShadows[i];
						//var n:MovieClip = objectPool.getInstanceOf(shadowClase) as MovieClip;
						//if (info.clip.parent)
							//info.clip.parent.removeChild(info.clip);
						//objectPool.returnInstance(info.clip);
						//info.shadow.addChild(n);
						//info.clip = n;
						//n.gotoAndPlay(lastFrame);
						//
					//}
					//
				//}
				
				this.currentSpriteIndex = newSprite;
				
				//if (this.eraseShadows)
				//{
					//
					// Update shadows
					//l = this.allShadows.length;
					//for (i = 0; i < l; i++)
					//{
						//
						//var p:fRenderableElement = this.allShadows[i].request;
						//if (p.container.stage && p is fPlane)
						//{
							//try
							//{
								//p.customData.flash9Renderer.undoCache(true);
							//}
							//catch (e:Error)
							//{
								//trace(e);
							//}
						//}
					//}
				//}
			}
		}
		
		/**
		 * When a character moves, the cache needs to be reset
		 */
		private function moveListener(evt:Event):void
		{
			// Delete projection cache
			//this.projectionCache = new fObjectProjectionCache;
		}
		
		/**
		 * This method syncs shadows to the base movieClip
		 */
		// KBEN: 这个函数就是说回到待机状态  
		private function gotoAndStopListener(evt:Event):void
		{
			// No animated shadows in this mode
			//if (this.simpleShadows)
			//	return;
			
			//var l:Number = this.allShadows.length;
			//for (var i:Number = 0; i < l; i++)
			//	this.allShadows[i].clip.gotoAndStop(this.flashClip.currentFrame);
			
			// KBEB: 切换状态 
			var element:fObject = this.element as fObject;
			
			// KBEN: 如果资源加载完成  
			if (isLoaded)
			{
				// KBEN: 如果资源初始化 
				if (!isActInit(element.getAction(), 0))
				{
					buildFrames(element.getAction(), 0);
				}
				_frames = _framesDic[element.getAction()];
			}
		}
		
		/**
		 * This method syncs shadows to the base movieClip
		 */
		// KBEN: 切换状态需要在这里进行处理   
		private function gotoAndPlayListener(evt:Event):void
		{
			// No animated shadows in this mode
			//if (this.simpleShadows)
			//	return;
			
			//var l:Number = this.allShadows.length;
			//for (var i:Number = 0; i < l; i++)
			//	this.allShadows[i].clip.gotoAndPlay(this.flashClip.currentFrame);
			
			// KBEB: 切换状态 
			var element:fObject = this.element as fObject;
			
			// KBEN: 如果资源加载完成  
			if (isLoaded)
			{
				// KBEN: 如果资源初始化 
				if (!isActInit(element.getAction(), this.currentSpriteIndex))
				{
					buildFrames(element.getAction(), this.currentSpriteIndex);
				}
				_frames = _framesDic[element.getAction()][this.currentSpriteIndex];
			}
		}
		
		/**
		 * This method will redraw this object's shadows when it is shown
		 */
		private function showListener(evt:Event):void
		{
			//var l:int = this.allShadows.length;
			//for (var i:int = 0; i < l; i++)
			//{
				//this.allShadows[i].clip.visible = true;
				//if (this.eraseShadows)
				//{
					//var p:fRenderableElement = this.allShadows[i].request;
					//if (p.container.stage && p is fPlane)
					//{
						//try
						//{
							//p.customData.flash9Renderer.undoCache(true);
						//}
						//catch (e:Error)
						//{
						//}
					//}
				//}
			//}
		}
		
		/**
		 * This method will erase this object's shadows when it is hidden
		 */
		private function hideListener(evt:Event):void
		{
			//var l:int = this.allShadows.length;
			//for (var i:int = 0; i < l; i++)
			//{
				//this.allShadows[i].clip.visible = false;
				//if (this.eraseShadows)
				//{
					//var p:fRenderableElement = this.allShadows[i].request;
					//if (p.container.stage && p is fPlane)
					//{
						//try
						//{
							//p.customData.flash9Renderer.undoCache(true);
						//}
						//catch (e:Error)
						//{
						//}
					//}
				//}
			//}
		}
		
		/*
		 * Returns a Shadow representation of this object, so
		 * the other elements can draw this shadow on themselves
		 *
		 * @param request The renderableElement requesting the shadow
		 *
		 * @return A movieClip instance ready to attach to the element that has to show the shadow of this object
		 */
		//public function getShadow(request:fRenderableElement):fObjectShadow
		//{
			//
			//var shadow:Sprite = objectPool.getInstanceOf(Sprite) as Sprite;
			//var par:Sprite = objectPool.getInstanceOf(Sprite) as Sprite;
			//var clip:MovieClip;
			//par.addChild(shadow);
			//var el:fObject = this.element as fObject;
			//
			// Return either the proper shadow or a simple spot depending on quality settings
			//if (!this.simpleShadows)
			//{
				//
				//var clase:Class = el.sprites[this.currentSpriteIndex].shadow as Class;
				//clip = objectPool.getInstanceOf(clase) as MovieClip;
				//if (this.currentSprite)
					//clip.gotoAndPlay(this.currentSprite.currentFrame);
				//
			//}
			//else
			//{
				//clip = objectPool.getInstanceOf(MovieClip) as MovieClip;
				//movieClipUtils.circle(clip.graphics, 0, 0, 1.5 * el.radius, 20, 0x000000, 100 - this.glight.intensity);
			//}
			//
			//shadow.addChild(clip);
			//
			//var ret:fObjectShadow = objectPool.getInstanceOf(fObjectShadow) as fObjectShadow;
			//ret.shadow = shadow;
			//ret.clip = clip;
			//ret.request = request;
			//ret.object = this.element as fObject;
			//
			//this.allShadows[this.allShadows.length] = ret;
			//return ret;
		//
		//}
		
		/*
		 * Deletes a shadow representation. It is called when this shadow is no longer needed
		 *
		 * @param sh The shadow we return
		 *
		 */
		//public function returnShadow(sh:fObjectShadow):void
		//{
			//
			// Return library instances to pool so they can be reused.
			//if (sh.shadow.parent)
				//sh.shadow.parent.removeChild(sh.shadow);
			//sh.shadow.removeChild(sh.clip);
			//objectPool.returnInstance(sh.clip);
			//objectPool.returnInstance(sh.shadow);
			//objectPool.returnInstance(sh.shadow.parent);
			//objectPool.returnInstance(sh);
			//
			//var pos:Number = this.allShadows.indexOf(sh);
			//this.allShadows.splice(pos, 1);
			//sh.dispose();
		//
		//}
		
		/**
		 * Resets shadows. This is called when the fEngine.shadowQuality value is changed
		 */
		//public override function resetShadows():void
		//{
			//
			//this.simpleShadows = false;
			//if (fEngine.shadowQuality == fShadowQuality.BASIC || (this.element is fCharacter && fEngine.shadowQuality == fShadowQuality.NORMAL))
				//this.simpleShadows = true;
			//
			//this.eraseShadows = false;
			//if (fEngine.shadowQuality == fShadowQuality.BEST || (!(this.element is fCharacter) && fEngine.shadowQuality == fShadowQuality.GOOD))
				//this.eraseShadows = true;
		//
		///*if(this.allShadows) for(var i:Number=0;i<this.allShadows.length;i++) {
		   //this.allShadows[i].dispose()
		   //delete this.allShadows[i]
		   //}
		 //this.allShadows = new Array*/
		//
		//}
		
		/*
		 * Calculates the projection of this object to a given floor Z
		 */
		//public function getSpriteProjection(floorz:Number, x:Number, y:Number, z:Number):fObjectProjection
		//{
			//
			// Test cache
			//if (this.projectionCache.test(floorz, x, y, z))
			//{
				//
					//trace("Read cache")
				//
			//}
			//else
			//{
				//
				//trace("Write cache")
				//if (this.element.z > floorz && z < this.element.z)
				//{
					//
					// No projection
					//this.projectionCache.update(floorz, x, y, z, null);
					//return this.projectionCache.projection;
					//
				//}
				//
				// Create new value 
				//var ret:fObjectProjection = new fObjectProjection();
				//ret.polygon = fProjectionSolver.calculateProjection(x, y, z, this.element, floorz).contours[0];
				//ret.origin = new Point((ret.polygon[0].x + ret.polygon[1].x) / 2, (ret.polygon[0].y + ret.polygon[1].y) / 2);
				//ret.end = new Point((ret.polygon[2].x + ret.polygon[3].x) / 2, (ret.polygon[2].y + ret.polygon[3].y) / 2)
				//ret.size = Point.distance(ret.origin, ret.end);
				//this.projectionCache.update(floorz, x, y, z, ret);
				//
			//}
			//
			//return this.projectionCache.projection;
		//
		//}
		
		/**
		 * Redraws lights in this Object
		 */
		private function paintLights():void
		{
			var res:ColorTransform = new ColorTransform;
			
			res.concat(this.glight.color);
			
			// KBEN: 只有 GI ，没有 LocalLight 
			//for (var i:String in this.lights)
			//{
				//
				//if (this.lights[i].light.scene != null)
				//{
					//var n:ColorTransform = this.lights[i].getTransform();
					//res.redMultiplier += n.redMultiplier;
					//res.blueMultiplier += n.blueMultiplier;
					//res.greenMultiplier += n.greenMultiplier;
					//res.redOffset += n.redOffset;
					//res.blueOffset += n.blueOffset;
					//res.greenOffset += n.greenOffset;
				//}
			//}
			
			// Clamp
			res.redMultiplier = Math.min(1, res.redMultiplier);
			res.blueMultiplier = Math.min(1, res.blueMultiplier);
			res.greenMultiplier = Math.min(1, res.greenMultiplier);
			res.redOffset = Math.min(128, res.redOffset);
			res.blueOffset = Math.min(128, res.blueOffset);
			res.greenOffset = Math.min(128, res.greenOffset);
			
			this.baseObj.transform.colorTransform = res;
		}
		
		/**
		 * Sets global light
		 */
		public override function renderGlobalLight(light:fGlobalLight):void
		{
			this.glight = light;
			this.paintLights();
		}
		
		/**
		 * Global light changes intensity
		 */
		public override function processGlobalIntensityChange(light:fGlobalLight):void
		{
			this.paintLights();
		}
		
		/**
		 * Global light changes color
		 */
		public override function processGlobalColorChange(light:fGlobalLight):void
		{
			this.paintLights();
		}
		
		/**
		 *	Light reaches element
		 */
		//public override function lightIn(light:fLight):void
		//{
			//
			// Already there ?
			//if (this.lights && !this.lights[light.uniqueId])
			//{
				//this.lights[light.uniqueId] = new fLightWeight(this.element as fObject, light);
				//light.addEventListener(fLight.COLORCHANGE, this.processLightIntensityChange, false, 0, true);
				//light.addEventListener(fLight.INTENSITYCHANGE, this.processLightIntensityChange, false, 0, true);
			//}
		//
		//}
		
		/**
		 *	Light leaves element
		 */
		//public override function lightOut(light:fLight):void
		//{
			//
			//if (this.lights && this.lights[light.uniqueId])
			//{
				//delete this.lights[light.uniqueId];
				//this.paintLights();
				//light.removeEventListener(fLight.COLORCHANGE, this.processLightIntensityChange);
				//light.removeEventListener(fLight.INTENSITYCHANGE, this.processLightIntensityChange);
			//}
		//
		//}
		
		//private function processLightIntensityChange(e:Event):void
		//{
			//this.paintLights();
		//}
		
		/**
		 * Render start
		 */
		//public override function renderStart(light:fLight):void
		//{
			//
			// Already there ?	
			//if (!this.lights[light.uniqueId])
				//this.lights[light.uniqueId] = new fLightWeight(this.element as fObject, light);
		//
		//}
		
		/**
		 * Render ( draw ) light
		 */
		//public override function renderLight(light:fLight):void
		//{
			//
			//this.lights[light.uniqueId].updateWeight();
		//
		//}
		
		/**
		 * Renders shadows of other elements upon this element
		 */
		//public override function renderShadow(light:fLight, other:fRenderableElement):void
		//{
		//
		//}
		
		/**
		 * Ends render
		 */
		//public override function renderFinish(light:fLight):void
		//{
			//this.paintLights();
		//}
		
		/**
		 * Starts acclusion related to one character
		 */
		//public override function startOcclusion(character:fCharacter):void
		//{
			//this.occlusionCount++;
			//this.container.alpha = character.occlusion / 100;
		//}
		
		/**
		 * Updates acclusion related to one character
		 */
		//public override function updateOcclusion(character:fCharacter):void
		//{
		//}
		
		/**
		 * Stops acclusion related to one character
		 */
		//public override function stopOcclusion(character:fCharacter):void
		//{
			//this.occlusionCount--;
			//if (this.occlusionCount == 0)
				//this.container.alpha = 1;
		//}
		
		/** @private */
		public function disposeObjectRenderer():void
		{
			// Lights
			//for (var i:Number = 0; i < this.lights.length; i++)
				//delete this.lights[i];
			//this.lights = null;
			this.glight = null;
			//
			// Shadows
			//if (this.projectionCache)
				//this.projectionCache.dispose();
			//this.projectionCache = null;
			//this.shadowObj = null;
			//if (this.allShadows)
				//for (i = 0; i < this.allShadows.length; i++)
				//{
					//this.allShadows[i].dispose();
					//delete this.allShadows[i];
				//}
			//this.allShadows = null;
			
			// Gfx
			this.destroyAssets();
			
			this.disposeRenderer();
		
		}
		
		/** @private */
		public override function dispose():void
		{
			this.disposeObjectRenderer();
		}
		
		// KBEN:添加一个简单阴影 
		private function addSimpleShadow():void
		{
			shadowSp ||= new Sprite();
			var clip:MovieClip = new MovieClip();
			var el:fObject = this.element as fObject;
			if (!shadowSp.numChildren)
			{
				clip = objectPool.getInstanceOf(MovieClip) as MovieClip;
				movieClipUtils.circle(clip.graphics, 0, 0, 1.5 * el.radius, 20, 0x000000, 100);
				shadowSp.addChild(clip);
				container.addChild(this.shadowSp);
			}
		}
		
		// KBEN:
		// 每一帧更新     
		override public function onTick(deltaTime:Number):void
		{
			var el:fObject = this.element as fObject;			
			//var correctedAngle:Number = el._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			// var newSprite:int = int(correctedAngle * el.sprites.length);
			//var newSprite:int = int(correctedAngle * el.definition.yCount);
			
			// KBEN: 添加帧播放速率控制
			//var misperframe:Number = el.framerateInvDic[el.getAction()];
			var misperframe:Number = el.definition.dicAction[el.getAction()].framerateInv;
			if (el.leftInterval + deltaTime >= misperframe)
			{
				_currentFrame = (++_currentFrame) % el.definition.dicAction[el.getAction()].xCount;
				el.leftInterval = el.leftInterval + deltaTime - misperframe;
			}
			else
			{
				el.leftInterval += deltaTime;
			}
			
			// KBEN: 资源建立才更新    
			if (isLoaded)
			{
				_currentBitMap.bitmapData = getFrame(_currentFrame, currentSpriteIndex);
			}
		}
		
		/**
         * Deletes the frames so this class can be re-used with a new set of frames.
         */
        protected function deleteFrames():void
        {
            _frames = null;
        }
        
        /**
         * True if the frames associated with this sprite container have been loaded.
         */
		// KBEN: 资源是否加载  
        public function get isLoaded():Boolean
        {
            //return _frames != null;
			var el:fObject = this.element as fObject;
			var act:uint = el.getAction();
			return el.actLoaded(act, 0);
        }
		
		// KBEN: 某一个动作是否初始化
		public function isActInit(act:uint, direction:uint):Boolean
		{
			return _framesDic[act] != null;
		}
		
		// KBEN: xindex 是 x 方向索引
        public function getFrame(xindex:int, yindex:int):BitmapData
        {
			var el:fObject = this.element as fObject;
            if(!isLoaded)
                return null;
            
			return _frames[yindex * el.definition.dicAction[el.getAction()].xCount + xindex];
        }
		
		// 这个就是直接生成帧序列， act: 生成的动作的索引    
		protected function buildFrames(act:uint, direction:uint) : void
        {
			// 这个是图片在一张图片上     			
			var el:fObject = this.element as fObject;
			
			// KBEN:
			if(!isLoaded)
				return;
			// KBEN: 动作只初始化一边    
			if (isActInit(act, direction))
				return;
			
			// KBEN: 取出帧数量
			_framesDic[act] = new Vector.<BitmapData>(el.definition.dicAction[act].frameCount);

			_frames = _framesDic[act];

			// KBEN: 一个动作在一张图上    
			var idx:int = 0;	// 动作索引 
			var angleIdx:uint = 0;	// 方向索引  
			var picIdx:uint = 0; 	// 图片索引  
			var actDirect:fActDirectDefinition;
			var bitmapdata:BitmapData;
			for (var keyaction:String in el.definition.dicAction)	// 每一个动作 
			{
				var action:fActDefinition;
				action = el.definition.dicAction[keyaction];
				angleIdx = 0;
				picIdx = 0;
				//actDirect = action.directArr[0];
				actDirect = action.directDic[0];
				bitmapdata = objectPool.getInstanceOf(actDirect.spriteVec[0].sprite) as BitmapData;
				// KBEN: 所有的动作资源一次全部构造出来，这个以后可以改   
				while(angleIdx < el.definition.yCount)
				{
					while (picIdx < angleIdx * action.xCount)
					{
						_frames[picIdx] = new BitmapData(el.definition._width, el.definition._height, true);
						var area:Rectangle = el.definition.dicAction[el.getAction()].getFrameArea(picIdx);	
						_frames[picIdx].copyPixels(bitmapdata, area, new Point(0, 0));
						++picIdx;
					}
					
					++angleIdx;
					++idx;
				}
				
				objectPool.returnInstance(bitmapdata);
			}
        }
		
		// KBEN:主要是图片类初始化类常量  
		override public function init(res:SWFResource, act:uint, direction:uint):void
		{
			// KBEN: 初始化后设置加载完成 
			//this.assetsCreated = true;
			
			var element:fObject = this.element as fObject;
			element.definition.init(res, act);
			buildFrames(act, direction);
			rotationListener();
		}
	}
}