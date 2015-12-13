// Basic renderable element class
package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	// Imports
	//import com.adobe.images.JPGEncoder;
	//import com.pblabs.engine.debug.Logger;
	import com.pblabs.engine.entity.BeingEntity;
	import com.pblabs.engine.entity.EntityCValue;
	import com.pblabs.engine.resource.SWFResource;
	import com.util.DebugBox;
	import com.util.PBUtil;
	import flash.filters.GradientGlowFilter;
	import flash.geom.Point;
	
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.DisplayObjectContainer;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.filters.GlowFilter;
	import flash.geom.ColorTransform;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.utils.Dictionary;
	
	import org.ffilmation.engine.core.fElement;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fGlobalLight;
	import org.ffilmation.engine.core.fRenderableElement;
	import org.ffilmation.engine.elements.fCharacter;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.helpers.fActDefinition;
	import org.ffilmation.engine.helpers.fActDirectDefinition;
	import org.ffilmation.engine.helpers.fUtil;
	import org.ffilmation.engine.model.fUIObject;
	import org.ffilmation.utils.movieClipUtils;
	import org.ffilmation.utils.objectPool;
	/**
	 * This class renders an fObject
	 * @private
	 * @brief 这个是以图片为渲染器的，在不同的图片序列上，需要自己记录帧号
	 */
	public class fFlash9ObjectSeqRenderer extends fFlash9ElementRenderer
	{
		// Private properties
		protected var _baseObj:Sprite;	// 人物模型层

		protected var _horseNode:Sprite;	// 马匹
		
		//private var lights:Array;
		private var _glight:fGlobalLight;
		//private var allShadows:Array;
		//private var currentSprite:Sprite;			// 这个此时是 Sprite 
		protected var _currentSpriteIndex:Number = 0; // 这个是方向，-1 总是宕机，尤其是没有调用 rotationListener 直接进入场景的时候。改成 0， 防止宕机
		private var _currentFrame:int = 0; // 这个相当于 MovieClip 中的索引  
		private var _occlusionCount:Number = 0;
		protected var _currentBitMap:Bitmap; // 这个是序列帧动画
		protected var _currentHorseBitMap:Bitmap; // 这个是骑乘物的动画帧序列
		
		protected var _actForchangeInfoByActDir:uint=uint.MAX_VALUE;
		protected var _dirForchangeInfoByActDir:uint=uint.MAX_VALUE;
		protected var _textField:TextField;
		protected var _updateCurrentFrameMode:int;
		// These reflects how shadows are rendered
		//public var simpleShadows:Boolean = false;
		//public var eraseShadows:Boolean = true;
		
		// Protected properties
		//protected var projectionCache:fObjectProjectionCache;
		
		/** @private */
		//public var shadowObj:Class
		// KBEN: 简单阴影直接挂在人物身上显示 
		public var m_shadowSp:Sprite;
		public var m_shadowImageSp:Sprite;
		public var m_shadowBM:Bitmap;	// 影子

		// KBEN: 每一个 Key 对应一个动作，然后里面有8个方向的动作    
		protected var _framesDic:Dictionary = null;	//[act, Dictionary]集合。其中的Dictionary是[direction, Vector.<BitmapData>]
		// KBEN: 二维数组，一维是方向，二维才是存储当前方向的动作序列，    
		protected var _frames:Vector.<BitmapData> = null;
		
		protected var m_behind:DisplayObjectContainer; // 在这个元素后面的层    
		protected var m_infront:DisplayObjectContainer; // 在这个元素前面的层
		protected var m_uiLay:DisplayObjectContainer; 	  // UI 挂到场景中的层
		
		protected var m_picFilter:GradientGlowFilter; // 显示的图片的滤镜		
		
		// Constructor
		/** @private */
		function fFlash9ObjectSeqRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fObject):void
		{
			// Previous
			super(rEngine, element, null, container);
			
			// Angle
			//var correctedAngle:Number = element._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			//this.currentSpriteIndex = int(correctedAngle * element.sprites.length);
			this._currentSpriteIndex = this.computeSpriteIndex();
			
			// KBEN: 
			_framesDic = new Dictionary();
			
			this.m_behind = new Sprite();
			this.m_infront = new Sprite();
			this.m_uiLay = new Sprite();
			this.m_sceneContainer = new Sprite();
			
			_updateCurrentFrameMode = EntityCValue.UpdateCurrentFrameMode_Increase;
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
			// Cache as bitmap non-animated objects
			var element:fObject = this.element as fObject;
			
			// 生成下面的层     
			//this.m_behind = new Sprite();
			//this.container.addChild(this.m_behind);
			if (!needHideSceneShow())
			{
				this.container.addChild(this.m_sceneContainer);
			}
			
			// 影子
			//m_shadowSp = objectPool.getInstanceOf(Sprite) as Sprite;
			//m_shadowSp.mouseEnabled = false;
			//this.container.addChild(this.m_shadowSp);
			
			//m_shadowImageSp = objectPool.getInstanceOf(Sprite) as Sprite;
			//m_shadowImageSp.mouseEnabled = false;
			//this.container.addChild(this.m_shadowImageSp);			
			
			// 坐骑的模型
			this._horseNode = objectPool.getInstanceOf(Sprite) as Sprite;
			//this.container.addChild(this._horseNode);
			this.m_sceneContainer.addChild(this._horseNode);
			this._horseNode.mouseEnabled = false;
			
			this._currentHorseBitMap = objectPool.getInstanceOf(Bitmap) as Bitmap;
			
			// Attach base clip,人物模型
			this._baseObj = objectPool.getInstanceOf(Sprite) as Sprite;
			//this.container.addChild(this._baseObj);
			this.m_sceneContainer.addChild(this._baseObj);
			this._baseObj.mouseEnabled = false;
			
			// 生成上面的层    
			//this.m_infront = new Sprite();
			//this.container.addChild(this.m_infront);
			this.m_sceneContainer.addChild(this.m_infront);
			this.container.addChild(this.m_uiLay);
			this.container.cacheAsBitmap = !(element is fCharacter) && element.animated != true;
			
			// Show and hide listeners, to redraw shadows
			this.element.addEventListener(fRenderableElement.SHOW, this.showListener, false, 0, true);
			this.element.addEventListener(fRenderableElement.HIDE, this.hideListener, false, 0, true);
			this.element.addEventListener(fObject.NEWORIENTATION, this.rotationListener, false, 0, true);
			this.element.addEventListener(fObject.GOTOANDPLAY, this.gotoAndPlayListener, false, 0, true);
			this.element.addEventListener(fObject.GOTOANDSTOP, this.gotoAndStopListener, false, 0, true);
			if (element is fCharacter)
				this.element.addEventListener(fElement.MOVE, this.moveListener, false, 0, true);
			
			this._occlusionCount = 0;
			
			// bug 如果 xml 已经初始化，直接加载资源 swf ，会导致朝向不对的问题，这里根据朝向调整角度
			// Draw initial sprite
			// this.currentSpriteIndex = 0;
			this._currentSpriteIndex = this.computeSpriteIndex();
			
			// KBEN: _currentBitMap 这个在初始化的时候创建    
			this._currentBitMap = objectPool.getInstanceOf(Bitmap) as Bitmap;
			this._baseObj.addChild(this._currentBitMap);
			_currentBitMap.x = element.bounds2d.x;
			_currentBitMap.y = element.bounds2d.y;
			
			// bug: 这个地方必须调用，第一次资源加载完成后要设置一下渲染器的方向，rotationListener 这个事件监听器注册的时候，fObject 构造函数发送 NEWORIENTATION 这个事件的时候 rotationListener 还没有注册，因此资源加载完成后要初始化一下方向
			// 第一次加载资源的时候强制纠正一下方向
			//rotationListener();
			
			// KBEN: 加载图片资源在显示的时候加载，切换动作的时候加载对应的资源，这里直接加载，测试使用，以后更改         
			// BUG: 静态 Npc 可能不会调用 gotoAndPlayListener ，仅仅是站在那里，因此显示的时候需要调用  
			if (element.binitXmlDef())
			{
				element.loadRes(element.getAction(), this._currentSpriteIndex);
			}
			else
			{
				element.loadObjDefRes();
			}
			
			// 这个放在这吧，不放在 init 函数里面了，因为如果创建 character 后直接 hideElement ，这个 xml 资源可能加载了，但是图片资源可能没加载，就不能设置 this.assetsCreated = true; 导致再次 showElement 的时候又会嗲用这个函数一次
			this.assetsCreated = true;
			
			// KBEN: 在自己脚底下添加简单的阴影    
			//if (fEngine.shadowQuality == fShadowQuality.NORMAL)
			//{
			//	addSimpleShadow();
			//}
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
			if (this._baseObj)
			{
				//this.container.removeChild(this._baseObj);
				this.m_sceneContainer.removeChild(this._baseObj);
				fFlash9RenderEngine.recursiveDelete(this._baseObj);
				objectPool.returnInstance(this._baseObj);
				this._baseObj = null;
			}
			
			m_shadowSp = null;
			m_shadowImageSp = null;
			
			this._currentSpriteIndex = 0;
			
			// 释放图片资源 
			var el:fObject = this.element as fObject;
			var key:String = "";
			var action:fActDefinition;
			//var dir:int = 0;
			var dir:String = "";
			var framecnt:uint = 0;
			for (key in _framesDic)
			{
				action = el.definition.dicAction[key];
				//_frames = _framesDic[key];
				//while (idx < action.frameCount)
				//{
				//objectPool.returnInstance(_frames[idx]);
				//_frames[idx] = null;
				//++idx;
				//}
				//dir = 0;
				//while (dir < el.definition.yCount)
				for(dir in _framesDic[key])
				{
					_frames = _framesDic[key][dir];
					framecnt = 0;
					while (framecnt < action.xCount)
					{
						_frames[framecnt] = null;
						++framecnt;
					}
					
					_framesDic[key][dir] = null;
					//++dir;
				}
				_framesDic[key] = null;
			}
			
			_frames = null;
			// notice: 最好把 _framesDic 字典全部释放了   
			_framesDic = null;
			
		}

		public static const spriteIndexCoefficient:Number = 8 / 360;
		private function computeSpriteIndex():int
		{
			//这里假设el.definition.yCount==8
			var ret:int;
			var v:Number = (this.element as fObject)._orientation * spriteIndexCoefficient;
			if (v >= 7.5)
			{
				ret = 0;
			}
			else
			{
				ret = Math.round(v);
			}
			return ret;
		}

		/**
		 * Listens to an object changing rotation and updates all sprites
		 */
		// 这个函数改变方向 bforce : 是否强制更新，资源加载完成后必然强制更新     
		private function rotationListener(evt:Event = null, bforce:Boolean = false):void
		{
			var el:fObject = this.element as fObject;
			//var correctedAngle:Number = el._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			//var newSprite:int = int(correctedAngle * el.sprites.length);
			var newSprite:int = computeSpriteIndex();
			var act:uint = el.getAction();
			
			// bug改变方向的时候就不用从 0 开始了，改变动作的时候再从0 开始吧
			//_currentFrame = 0; // 从开始播放
			
			//  bug: 如果还没加载进来，就会使用上一个动作的图片信息，但是偏移是这个动作的偏移，就会很奇怪  
			// 如果 xml 定义加载进来，就更新位置信息   
			//if (el.binitXmlDef())
			//{
			//changeInfoByActDir(act, newSprite);
			//}
			
			if (this._currentSpriteIndex != newSprite || bforce)
			{
				// KBEN:支持在一张图上的内容，在 onTick 中更新，
				// BUG:可能 _currentBitMap 还没有创建就调用这个函数      
				//if (this.screenVisible)
				//{
				//	_currentBitMap.bitmapData = getFrame(_currentFrame, newSprite, act);
				//}
				
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
				
				this._currentSpriteIndex = newSprite;
				
				// 方向改变也需要替换动作 
				// KBEN: 如果资源加载完成  
				if (isLoaded(act, this._currentSpriteIndex))
				{
					// KBEN: 如果资源初始化 
					if (!isActInit(act, this._currentSpriteIndex))
					{
						buildFrames(act, this._currentSpriteIndex);
					}
					_frames = _framesDic[act][this._currentSpriteIndex];
					
					// 如果 xml 定义加载进来，就更新位置信息   
					changeInfoByActDir(act, this._currentSpriteIndex);
				}
				else
				{
					if (el.binitXmlDef())
					{
						element.loadRes(act, this._currentSpriteIndex);
					}
				}
				
				// 检查骑乘物资源
				if (el.canUpdataRide(el.subState, act))	// 如果骑乘
				{
					act = fUtil.getMountActByPlayerAct(act);
					
					if ((el.curHorseRenderData as fFlash9ObjectSeqRenderer).isLoaded(act, this._currentSpriteIndex))
					{
						// KBEN: 如果资源初始化 
						if (!(el.curHorseRenderData as fFlash9ObjectSeqRenderer).isActInit(act, this._currentSpriteIndex))
						{
							(el.curHorseRenderData as fFlash9ObjectSeqRenderer).buildFrames(act, this._currentSpriteIndex);
						}
						(el.curHorseRenderData as fFlash9ObjectSeqRenderer).frames = (el.curHorseRenderData as fFlash9ObjectSeqRenderer).framesDic[act][this._currentSpriteIndex];
						
						// 如果 xml 定义加载进来，就更新位置信息   
						(el.curHorseRenderData as fFlash9ObjectSeqRenderer).changeInfoByActDir(act, this._currentSpriteIndex);
					}
					else
					{
						if (el.curHorseData.binitXmlDef())
						{
							el.curHorseData.loadRes(act, this._currentSpriteIndex);
						}
					}
				}
				
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
			_currentFrame = 0; // 从开始播放    
			var element:fObject = this.element as fObject;
			var act:uint = element.getAction();
			
			// KBEN: 如果资源加载完成  
			if (isLoaded(act, this._currentSpriteIndex))
			{
				// KBEN: 如果资源初始化 
				if (!isActInit(act, this._currentSpriteIndex))
				{
					buildFrames(act, this._currentSpriteIndex);
				}
				_frames = _framesDic[act][this._currentSpriteIndex];
				
				// 如果 xml 定义加载进来，就更新位置信息   
				changeInfoByActDir(act, this._currentSpriteIndex);
			}
			else
			{
				if (element.binitXmlDef())
				{
					element.loadRes(act, this._currentSpriteIndex);
				}
			}
			
			// 检查骑乘物资源
			if (element.canUpdataRide(element.subState, act))	// 如果骑乘
			{
				act = fUtil.getMountActByPlayerAct(act);
				
				if ((element.curHorseRenderData as fFlash9ObjectSeqRenderer).isLoaded(act, this._currentSpriteIndex))
				{
					// KBEN: 如果资源初始化 
					if (!(element.curHorseRenderData as fFlash9ObjectSeqRenderer).isActInit(act, this._currentSpriteIndex))
					{
						(element.curHorseRenderData as fFlash9ObjectSeqRenderer).buildFrames(act, this._currentSpriteIndex);
					}
					(element.curHorseRenderData as fFlash9ObjectSeqRenderer).frames = (element.curHorseRenderData as fFlash9ObjectSeqRenderer).framesDic[act][this._currentSpriteIndex];
					
					// 如果 xml 定义加载进来，就更新位置信息   
					(element.curHorseRenderData as fFlash9ObjectSeqRenderer).changeInfoByActDir(act, this._currentSpriteIndex);
				}
				else
				{
					if (element.curHorseData.binitXmlDef())
					{
						element.curHorseData.loadRes(act, this._currentSpriteIndex);
					}
				}
			}
		}
		
		/**
		 * This method syncs shadows to the base movieClip
		 */
		// KBEN: 切换状态需要在这里进行处理，这个函数切换动作     
		private function gotoAndPlayListener(evt:Event):void
		{
			// No animated shadows in this mode
			//if (this.simpleShadows)
			//	return;
			
			//var l:Number = this.allShadows.length;
			//for (var i:Number = 0; i < l; i++)
			//	this.allShadows[i].clip.gotoAndPlay(this.flashClip.currentFrame);
			
			// KBEB: 切换状态 
			_currentFrame = 0; // 从开始播放    
			var element:fObject = this.element as fObject;
			var act:uint = element.getAction();
			
			// KBEN: 如果资源加载完成  
			if (isLoaded(act, this._currentSpriteIndex))
			{
				// KBEN: 如果资源初始化 
				if (!isActInit(act, this._currentSpriteIndex))
				{
					buildFrames(act, this._currentSpriteIndex);
				}
				_frames = _framesDic[act][this._currentSpriteIndex];
				
				// 如果 xml 定义加载进来，就更新位置信息   
				changeInfoByActDir(act, this._currentSpriteIndex);
			}
			else
			{
				if (element.binitXmlDef())
				{
					element.loadRes(act, this._currentSpriteIndex);
				}
			}
			
			// 检查骑乘物资源
			if (element.canUpdataRide(element.subState, act))	// 如果骑乘
			{
				act = fUtil.getMountActByPlayerAct(act);
				
				if ((element.curHorseRenderData as fFlash9ObjectSeqRenderer).isLoaded(act, this._currentSpriteIndex))
				{
					// KBEN: 如果资源初始化 
					if (!(element.curHorseRenderData as fFlash9ObjectSeqRenderer).isActInit(act, this._currentSpriteIndex))
					{
						(element.curHorseRenderData as fFlash9ObjectSeqRenderer).buildFrames(act, this._currentSpriteIndex);
					}
					(element.curHorseRenderData as fFlash9ObjectSeqRenderer).frames = (element.curHorseRenderData as fFlash9ObjectSeqRenderer).framesDic[act][this._currentSpriteIndex];
					
					// 如果 xml 定义加载进来，就更新位置信息   
					(element.curHorseRenderData as fFlash9ObjectSeqRenderer).changeInfoByActDir(act, this._currentSpriteIndex);
				}
				else
				{
					if (element.curHorseData.binitXmlDef())
					{
						element.curHorseData.loadRes(act, this._currentSpriteIndex);
					}
				}
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
			var fobj:fObject = this.element as fObject;
			if (!fobj.m_affectByGI) // 没有 GI 影响
			{
				return;
			}
			var res:ColorTransform = new ColorTransform;
			
			res.concat(this._glight.color);
			
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
			
			this._baseObj.transform.colorTransform = res;
		}
		
		/**
		 * Sets global light
		 */
		public override function renderGlobalLight(light:fGlobalLight):void
		{
			this._glight = light;
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
		public override function startOcclusion(character:fCharacter):void
		{
			this._occlusionCount++;
			//this.container.alpha = character.occlusion / 100;
		}
		
		/**
		 * Updates acclusion related to one character
		 */
		public override function updateOcclusion(character:fCharacter):void
		{
		}
		
		/**
		 * Stops acclusion related to one character
		 */
		public override function stopOcclusion(character:fCharacter):void
		{
			this._occlusionCount--;
			if (this._occlusionCount == 0)
				this.container.alpha = 1;
		}
		
		/** @private */
		public function disposeObjectRenderer():void
		{
			// Lights
			//for (var i:Number = 0; i < this.lights.length; i++)
			//delete this.lights[i];
			//this.lights = null;
			this._glight = null;
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
		protected function addSimpleShadow():void
		{
			m_shadowSp ||= new Sprite();
			m_shadowSp.mouseEnabled = false;
			var clip:Sprite = new Sprite();
			var el:fObject = this.element as fObject;
			if (!m_shadowSp.numChildren)
			{
				clip = objectPool.getInstanceOf(Sprite) as Sprite;
				movieClipUtils.circle(clip.graphics, 0, 0, 1.5 * el.radius, 20, 0x000000, 100);
				m_shadowSp.addChild(clip);
				//this.container.addChild(this.m_shadowSp);
				this.m_sceneContainer.addChild(this.m_shadowSp);
			}
		}
		
		// 影子是一个贴图
		protected function addImageShadow():void
		{
			//m_shadowSp ||= new Sprite();
			//m_shadowSp.mouseEnabled = false;
			
			m_shadowBM ||= new Bitmap();
			m_shadowBM.bitmapData = this.element.m_context.m_replaceResSys.shadowPlace;
			
			this.m_shadowImageSp.addChild(m_shadowBM);
			//this.container.addChild(this.m_shadowSp);
			
			// 调整影子的位置
			if(m_shadowBM.bitmapData)
			{
				this.m_shadowImageSp.x = -(int)(m_shadowBM.bitmapData.width/2);
				this.m_shadowImageSp.y = -(int)(m_shadowBM.bitmapData.height/2);
			}
		}
		
		private function increaseCurrentFrame(act:uint, deltaTime:Number):void
		{
			var el:fObject = this.element as fObject;
			var actDef:fActDefinition = el.definition.dicAction[act];
			//var misperframe:Number = el.framerateInvDic[act];
			var misperframe:Number = actDef.framerateInv;
			//var repeat:Boolean = el.repeatDic[act];
			var repeat:Boolean = actDef.repeat;	
			
			// 合并上面的代码后的结果 
			if (repeat || _currentFrame != actDef.xCount - 1)
			{
				// 这个是逐帧播放
				if (el.leftInterval + deltaTime >= misperframe)
				{
					_currentFrame = (++_currentFrame) % actDef.xCount;
					el.leftInterval = el.leftInterval + deltaTime - misperframe;
				}
				else
				{
					el.leftInterval += deltaTime;
				}
				
				el.leftInterval = PBUtil.clamp(el.leftInterval, 0, misperframe);		// 最大就累积一帧
			}
		}
		
		private function decreaseCurrentFrame(act:uint, deltaTime:Number):void
		{
			if (_currentFrame > 0)
			{
				_currentFrame--;
			}
		}

		// KBEN:
		// 每一帧更新     
		override public function onTick(deltaTime:Number):void
		{
			// 限制大小，根据每秒至少 1 帧限制的
			//deltaTime = PBUtil.clamp(deltaTime, 0, 0.04);
			//deltaTime = PBUtil.clamp(deltaTime, 0.04, 1);
			// 更新动作必要的信息   
			var el:fObject = this.element as fObject;
			//var correctedAngle:Number = el._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			// var newSprite:int = int(correctedAngle * el.sprites.length);
			//var newSprite:int = int(correctedAngle * el.definition.yCount);
			
			// KBEN: 添加帧播放速率控制
			var act:uint = el.getAction();
			
			if (_updateCurrentFrameMode == EntityCValue.UpdateCurrentFrameMode_Increase)
			{
				increaseCurrentFrame(act, deltaTime);
			}
			else if (_updateCurrentFrameMode == EntityCValue.UpdateCurrentFrameMode_Decrease)
			{
				decreaseCurrentFrame(act, deltaTime);
			}

			// KBEN: 资源建立才更新    
			// 这一段只有显示的时候才计算   
			if (this.screenVisible)
			{
				if (isLoaded(act, this._currentSpriteIndex) && isActInit(act, this._currentSpriteIndex))
				{
					// 每一帧都改变
					//if (_actForchangeInfoByActDir != act || _dirForchangeInfoByActDir != this.currentSpriteIndex)
					//{
						changeInfoByActDir(act, this._currentSpriteIndex);
						_actForchangeInfoByActDir = act;
						_dirForchangeInfoByActDir = this._currentSpriteIndex;
					//}
					_currentBitMap.bitmapData = getFrame(_currentFrame, _currentSpriteIndex, act);
					// test: 测试 BitmapData 是否被 dispose 了 
					//if (_currentBitMap.bitmapData)
					//{
					//Logger.info(null, null, "bitmapdate width: " + _currentBitMap.bitmapData.width);
					//_currentBitMap.bitmapData.width;
					//}
				}
				// 这些动作和方向的改变放到 gotoAndPlayListener rotationListener 这两个函数中去了  
				//else if (!isLoaded(act))	// 资源没有加载    
				//{
				//	element.loadRes(act);
				//}
				//else if (!isActInit(act))	// 资源还没有初始化    
				//{
				//	buildFrames(act);
				//	rotationListener();
				//}
				else if (isLoaded(act, this._currentSpriteIndex) && !isActInit(act, this._currentSpriteIndex)) // 如果资源已经加载但是没有初始化,就初始化     
				{
					buildFrames(act, this._currentSpriteIndex);
				}
				
				// 更新马匹显示
				if (el.canUpdataRide(el.subState, act))	// 如果骑乘
				{
					act = fUtil.getMountActByPlayerAct(act);

					if (!this._horseNode.numChildren)
					{
						this._horseNode.addChild(this._currentHorseBitMap);
						_currentHorseBitMap.x = el.curHorseData.bounds2d.x;
						_currentHorseBitMap.y = el.curHorseData.bounds2d.y;
					}

					if ((el.curHorseRenderData as fFlash9ObjectSeqRenderer).isLoaded(act, this._currentSpriteIndex) && (el.curHorseRenderData as fFlash9ObjectSeqRenderer).isActInit(act, this._currentSpriteIndex))
					{
						// 每一帧都改变
						(el.curHorseRenderData as fFlash9ObjectSeqRenderer).changeInfoByActDir(act, this._currentSpriteIndex);
						_currentHorseBitMap.bitmapData = (el.curHorseRenderData as fFlash9ObjectSeqRenderer).getFrame(_currentFrame, _currentSpriteIndex, act);
					}
					else if ((el.curHorseRenderData as fFlash9ObjectSeqRenderer).isLoaded(act, this._currentSpriteIndex) && !(el.curHorseRenderData  as fFlash9ObjectSeqRenderer).isActInit(act, this._currentSpriteIndex)) // 如果资源已经加载但是没有初始化,就初始化     
					{
						(el.curHorseRenderData as fFlash9ObjectSeqRenderer).buildFrames(act, this._currentSpriteIndex);
					}
					
					//if (!this.m_shadowImageSp.numChildren)
					//{
					//	addImageShadow();
					//}
				}
				else
				{
					if (this._horseNode.numChildren)
					{
						this._horseNode.removeChild(this._currentHorseBitMap);
						this._currentHorseBitMap.bitmapData = null;
					}
					
					//if (this.m_shadowImageSp.numChildren)
					//{
					//	this.m_shadowImageSp.removeChild(this.m_shadowBM);
					//	this.m_shadowBM.bitmapData = null;
					//}
				}

				if (_needDrawTextField && _baseObj)
				{
					_needDrawTextField = false;
					var tWidth:uint;
					
					var strDesc:String = "";
					if (element is BeingEntity)
					{
						strDesc = (element as BeingEntity).nameDisc;
					}
					else if (element is fUIObject)
					{
						strDesc = (element as fUIObject).nameDisc;
					}
					
					if (_textField == null)
					{
						_textField = new TextField();
						_textField.autoSize = TextFieldAutoSize.CENTER;
						_textField.multiline = true;
						_baseObj.addChild(_textField);
						_textField.mouseEnabled = false;
						
						var filter:GlowFilter = new GlowFilter(0, 1, 2, 2, 16);
						_textField.filters = [filter];
					}
					
					_textField.htmlText = strDesc;
					tWidth = _textField.width;
					_textField.x = element.m_tagBounds2d.x + (element.m_tagBounds2d.width - tWidth) / 2;
					_textField.y = element.m_tagBounds2d.y - 20;
				}
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
		public function isLoaded(act:uint, direction:uint):Boolean
		{
			//return _frames != null;
			var el:fObject = this.element as fObject;
			return el.actLoaded(act, direction);
		}
		
		// KBEN: 某一个动作是否初始化
		public function isActInit(act:uint, direction:uint):Boolean
		{
			if (_framesDic[act] && _framesDic[act][direction] && _framesDic[act][direction][0])
			{
				return true;
			}
			
			return false;
			
			// bug: 这么判断有问题 可能  _frameInitDic[act][dir] = true; 设置后，才判断一些初始化，但此时 _framesDic 还没有初始化   
			//var el:fObject = this.element as fObject;
			//if (el.frameInitDic[act] && el.frameInitDic[act][direction])
			//{
			//	return true;
			//}
			
			//return false;
		}
		
		// KBEN: xindex 是 x 方向索引, yindex 就是 0 - 7  8 个方向
		public function getFrame(xindex:int, yindex:int, act:uint):BitmapData
		{
			var el:fObject = this.element as fObject;
			if (!isLoaded(act, yindex))
				return null;
			//return _frames[yindex * el.definition.dicAction[el.getAction()].xCount + xindex];
			return _frames[xindex];
		}
		
		// 这个就是直接生成帧序列， act: 生成的动作的索引 direction : 0 - 7    
		protected function buildFrames(act:uint, direction:uint):void
		{
			// 这个是图片在一张图片上     
			//var el:fObject = this.element as fObject;
			//if (!imageData)
			//    return;
			
			//_frames = new Vector.<BitmapData>(el.definition.frameCount);
			
			//for (var i:int = 0; i < el.definition.frameCount; i++)
			//{
			//	var area:Rectangle = el.definition.getFrameArea(i);										
			//	_frames[i] = new BitmapData(area.width, area.height, true);
			//	_frames[i].copyPixels(imageData, area, new Point(0, 0));
			//}
			
			var el:fObject = this.element as fObject;
			//if (!imageData)
			//    return;
			
			//_frames = new Vector.<BitmapData>(el.definition.frameCount);
			
			//for (var i:int = 0; i < el.definition.frameCount; i++)
			//{										
			//	_frames[i] = new BitmapData(el.definition._width, el.definition._height, true);
			//	_frames[i].copyPixels(imageData, area, new Point(0, 0));
			//}
			
			// KBEN:
			if (!isLoaded(act, direction))
				return;
			// KBEN: 动作只初始化一边    
			if (isActInit(act, direction))
				return;
			
			// KBEN: 取出帧数量
			// 二维数组，一维是8个方向，二维才是图片序列   
			//_framesDic[act] = new Vector.<BitmapData>(el.definition.dicAction[act].frameCount);
			if (!_framesDic[act])	// 如果动作字典不存在
			{
				//_framesDic[act] = new Vector.<Vector.<BitmapData>>(el.definition.yCount, true);
				//var dirlistidx:int = 0;
				//while (dirlistidx < el.definition.yCount)
				//{
				//	_framesDic[act][dirlistidx] = new Vector.<BitmapData>(el.definition.dicAction[act].xCount, true);
				//	++dirlistidx;
				//}
				
				_framesDic[act] = new Dictionary();
			}
			
			// 如果方向序列不存在
			if(!_framesDic[act][direction])
			{
				_framesDic[act][direction] = new Vector.<BitmapData>();
			}
			
			//_frames = new Vector.<BitmapData>(el.definition.totalFrameCount);
			//var frame:Vector.<BitmapData>;
			//var frame:Vector.<Vector.<BitmapData>>;
			var frame:Dictionary;
			frame = _framesDic[act];
			
			var action:fActDefinition;
			var actDirect:fActDirectDefinition;
			var globalIdx:int = 0;
			var idx:int = 0;
			//for (var keyaction:String in el.definition.dicAction)	// 每一个动作 
			//{
			action = el.definition.dicAction[act];
			
			// KBEN: 所有的动作资源一次全部构造出来，这个以后可以改   
			//for (var keydegree:String in action.directDic)
			//for each(var spriteDef:fSpriteDefinition in action.directArr)
			//for each(actDirect in action.directArr)
			//{
			//actDirect = action.directArr[direction];
			actDirect = action.directDic[direction];
			//var bitmap:Bitmap;
			var bitmapdata:BitmapData;
			//_frames[i] = new BitmapData(el.definition._width, el.definition._height, true);
			//bitmapdata = objectPool.getInstanceOf(action.directDic[keydegree].sprite) as BitmapData;
			//bitmapdata = objectPool.getInstanceOf(spriteDef.sprite) as BitmapData;
			idx = 0;
			while (idx < action.xCount)
			{
				//bitmap = objectPool.getInstanceOf(action.directDic[keydegree].sprite) as Bitmap;
				//_frames[idx] = bitmap.bitmapData;
				//bitmapdata = objectPool.getInstanceOf(actDirect.spriteVec[idx].sprite) as BitmapData;
				
				// KBEN: 图像序列优化，所有的公用一个图像
				if (actDirect.flipMode == EntityCValue.FLPX)
				{
					bitmapdata = el._resDic[act][direction].getExportedAsset(actDirect.spriteVec[idx].startName, true, actDirect.flipMode);
				}
				else
				{
					bitmapdata = el._resDic[act][direction].getExportedAsset(actDirect.spriteVec[idx].startName, true);
				}
				
				//frame[globalIdx] = bitmapdata;
				frame[direction][globalIdx] = bitmapdata;
				if (bitmapdata == null)
				{
					var strInfo:String = "name";
					if (el is fObject)
					{
						strInfo = (el as fObject).definitionID + "_" + (el as fObject).m_insID;
						strInfo += "_" + act + "_" + direction + ".swf\n";
						strInfo += actDirect.spriteVec[idx].startName;
					}
					strInfo += "\n(bitmapdata == null)"
					DebugBox.info(strInfo);
				}
				
				++idx;
				++globalIdx;
			}
			//}
			//}
			
			// bug 需要动作方向都一致才行，开始只有动作方向相同，没有判断方向，尤其是 direction 这个方向资源加载进来的时候， direction 已经改变成其它方向了，这个时候就会有问题
			if (act == el.getAction() && direction == this._currentSpriteIndex)
			{
				_frames = _framesDic[act][direction];
			}
		}
		
		//public function get imageData():BitmapData
		//{
		//if (!_image)
		//return null;
		//
		//return _image.bitmapData;
		//}
		
		// KBEN:主要是图片类初始化类常量  
		override public function init(res:SWFResource, act:uint, direction:uint):void
		{
			// KBEN: 初始化后设置加载完成，这个还是放在 createAssets 这个函数设置，具体原因看 createAssets 这个函数注释 
			// this.assetsCreated = true;
			var element:fObject = this.element as fObject;
			//element.definition.init(res, act);
			buildFrames(act, direction);
			
			// bug: 这个地方必须调用，第一次资源加载完成后要设置一下渲染器的方向，rotationListener 这个事件监听器注册的时候，fObject 构造函数发送 NEWORIENTATION 这个事件的时候 rotationListener 还没有注册，因此资源加载完成后要初始化一下方向
			// 这个在 createAssets 最后调用一次就算了，在这里调用会把 _currentFrame 设置为 0 ，相当于帧倒退回去了
			//rotationListener(null, true);
		}
		
		// KBEN: 某一个不能重复的动作是否播放完   
		override public function aniOver():Boolean
		{
			var element:fObject = this.element as fObject;
			if(_currentFrame == element.definition.dicAction[element.getAction()].xCount - 1)
			{
				if(!element.definition.dicAction[element.getAction()].repeat)
				{
					return true;
				}
			}
			
			return false;
		}
		
		/*
		// test: 经常出现有几帧是空的图片 
		public function savePic():void
		{
			// KBEN: 打印测试     
			var encode:JPGEncoder = new JPGEncoder(90);
			var file:FileReference = new FileReference();
			
			var currentFrame:uint = 0;
			var finalBitmapData:BitmapData;
			var filename:String;
			
			var el:fObject = this.element as fObject;
			var act:uint = el.getAction();
			if (!isLoaded(act, this.currentSpriteIndex) || !isActInit(act, this.currentSpriteIndex))
			{
				return;
			}
			
			//while (currentFrame < el.definition.dicAction[act].xCount)
			//{
			//finalBitmapData = getFrame(currentFrame, currentSpriteIndex, act);
			//var ba:ByteArray = encode.encode(finalBitmapData);
			//filename = "aaa" + currentFrame + ".jpg";
			//file.save(ba, filename);
			//++currentFrame;
			//}
			
			//var ba:ByteArray = encode.encode(_currentBitMap.bitmapData);
			var ba:ByteArray = encode.encode(_frames[0]);
			filename = "aaa" + currentFrame + ".jpg";
			file.save(ba, filename);
		}
		*/
		
		// 只是将图片数据清除    
		override public function disposeShow():void
		{
			var el:fObject = this.element as fObject;
			var key:String = "";
			var action:fActDefinition;
			//var dir:int = 0;
			var dir:String = "";
			var framecnt:uint = 0;
			for (key in _framesDic)
			{
				action = el.definition.dicAction[key];
				if (_framesDic[key])
				{
					//dir = 0;
					//while (dir < el.definition.yCount)
					for(dir in _framesDic[key])
					{
						_frames = _framesDic[key][dir];
						framecnt = 0;
						while (framecnt < action.xCount)
						{
							_frames[framecnt] = null;
							++framecnt;
						}
						
						//_framesDic[key][dir] = null;
						//_framesDic[key][dir].splice(0, _framesDic[key][dir].length);
						_framesDic[key][dir].length = 0;
						//++dir;
					}
					//_framesDic[key].splice(0, _framesDic[key].length);
					_framesDic[key] = null;
				}
			}
			
			// 把动作全部释放    
			var keylist:Vector.<String> = new Vector.<String>();
			for (key in _framesDic)
			{
				keylist.push(key);
			}
			
			var idx:uint = 0;
			while (idx < keylist.length)
			{
				_framesDic[keylist[idx]] = null;
				delete _framesDic[keylist[idx]];
				//_framesDic[keylist[idx]] = null;
				
				++idx;
			}
			
			//keylist.splice(0, keylist.length);
			keylist.length = 0;
			keylist = null;
			
			_currentFrame = 0;		// 设置显示的图片帧为 0
		}
		
		override public function get actDir():Number
		{
			// 一定要更新方向，第一次加载完成后，可能 currentSpriteIndex 还没有更新 
			//var el:fObject = this.element as fObject;
			//var correctedAngle:Number = el._orientation / 360;
			this._currentSpriteIndex = this.computeSpriteIndex();
			
			return this._currentSpriteIndex;
		}
		
		override public function get currentFrame():int
		{
			return _currentFrame;
		}
		
		override public function set currentFrame(value:int):void
		{
			_currentFrame = value;
		}
		
		public function changeInfoByActDir(act:uint, dir:uint):void
		{
			/*if (_actForchangeInfoByActDir == act && _dirForchangeInfoByActDir == this.currentSpriteIndex)
			{
				return;
			}*/
					
			var el:fObject = this.element as fObject;
			el.changeInfoByActDir(act, dir);
			
			if (_currentBitMap)
			{
				_currentBitMap.x = element.bounds2d.x;
				_currentBitMap.y = element.bounds2d.y;
			}
			if (_textField)
			{
				//var tWidth:uint = 150;
				var tWidth:uint = _textField.width;
				//_textField.x = element.bounds2d.x + (element.bounds2d.width - tWidth) / 2;
				//_textField.y = element.bounds2d.y - 40;
				_textField.x = element.m_tagBounds2d.x + (element.m_tagBounds2d.width - tWidth) / 2;
				_textField.y = element.m_tagBounds2d.y - 20;
				if (tx != _textField.x)
				{
					tx = _textField.x;
				}
			}
		}
		public var tx:Number;
		public override function layerContainer(layer:uint):DisplayObjectContainer
		{
			if (0 == layer)
			{
				return m_infront;
			}
			
			return m_behind;
		}
		
		override public function get curImage():BitmapData
		{
			return _currentBitMap.bitmapData;
		}
		
		public function get curBitmap():Bitmap
		{
			return _currentBitMap;
		}
		
		public function getBaseObj():Sprite
		{
			return _baseObj;
		}
		
		public function getUILayObj():Sprite
		{
			return m_uiLay as Sprite;
		}
		
		public function setScale(s:Number):void
		{
			_baseObj.scaleX = s;
			_baseObj.scaleY = s;
		}
		
		// 补偿缩放，这个主要是设置缩放后，名字也会缩放，因此设置一下弥补，是名字缩放到原始大小
		public function compensateScale(s:Number):void
		{
			if (_textField == null)
			{
				_textField = new TextField();
				_textField.autoSize = TextFieldAutoSize.CENTER;
				_textField.multiline = true;
				_baseObj.addChild(_textField);
				_textField.mouseEnabled = false;
				
				var filter:GlowFilter = new GlowFilter(0, 1, 2, 2, 16);
				_textField.filters = [filter];
			}

			//if (_textField)
			//{
				_textField.scaleX = s;
				_textField.scaleY = s;
			//}
		}
		
		public function get framesDic():Dictionary
		{
			return _framesDic;
		}
		
		public function set frames(value:Vector.<BitmapData>):void
		{
			_frames = value;
		}
		
		public function get currentHorseBitMap():Bitmap
		{
			return _currentHorseBitMap;
		}
		
		public function setUpdateCurrentFrameMode(mode:int):void
		{
			_updateCurrentFrameMode = mode;
		}
		
		public function get hasMountsRenderData():Boolean
		{
			if (_currentHorseBitMap&&this._currentHorseBitMap.bitmapData)
			{
				return true;
			}
			
			return false;
		}
		
		protected function needHideSceneShow():Boolean
		{
			if (this.element is this.element.m_context.m_typeReg.m_classes[EntityCValue.TPlayer])		// 只有非主角玩家角色才需要隐藏
			{
				if (this.element.m_context.isSetLocalFlags(14))		// 如果没有设置就是显示
				{
					return true;
				}
				
				return false;
			}
			
			return false;
		}
		
		override public function hitTest(globalx:Number, globaly:Number):Boolean
		{	
			if (_currentBitMap && _currentBitMap.bitmapData)
			{
				try
				{
					if (_currentBitMap.bitmapData.hitTest(new Point(0, 0), 100, _currentBitMap.globalToLocal(new Point(globalx, globaly))))
					{
						return true;
					}
				}
				catch (e:Error)
				{
					return false;
				}
			}
			
			return false;
		}
		
				
		override public function onMouseEnter():void
		{
			if (!m_picFilter)
			{
				m_picFilter = PBUtil.buildGradientGlowFilter();
			}
			
			m_picFilter.colors = [0x000000, 0x00FF00];
			this._currentBitMap.filters = [m_picFilter];
		}
		
		override public function onMouseLeave():void
		{
			this._currentBitMap.filters = null;
		}
	}
}