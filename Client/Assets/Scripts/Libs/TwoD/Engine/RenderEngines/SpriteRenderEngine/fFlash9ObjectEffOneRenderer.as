package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	// Imports
	//import com.pblabs.engine.debug.Logger;
	import com.pblabs.engine.resource.SWFResource;
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.geom.ColorTransform;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	import org.ffilmation.engine.core.fElement;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fGlobalLight;
	import org.ffilmation.engine.core.fRenderableElement;
	import org.ffilmation.engine.elements.fCharacter;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.helpers.fActDefinition;
	import org.ffilmation.engine.helpers.fActDirectDefinition;
	import org.ffilmation.utils.objectPool;
	
	/**
	 * This class renders an fObject
	 * @private
	 * @brief 这个是特效渲染器，所有的图在一张图片上     
	 */
	public class fFlash9ObjectEffOneRenderer extends fFlash9ElementRenderer
	{
		// Private properties
		private var baseObj:Sprite;
		private var glight:fGlobalLight;
		private var _currentFrame:int = 0;			// 这个相当于 MovieClip 中的索引，当前的帧      
		//private var occlusionCount:Number = 0;
		private var _currentBitMap:Bitmap;			// 这个是序列帧动画    
		
		// KBEN: 存储当前的动作    
		protected var _frames:Vector.<BitmapData> = null;  
		
		// Constructor
		/** @private */
		function fFlash9ObjectEffOneRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fObject):void
		{
			// Previous
			super(rEngine, element, null, container);
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
			element.addEventListener(fObject.GOTOANDPLAY, this.gotoAndPlayListener, false, 0, true);
			element.addEventListener(fObject.GOTOANDSTOP, this.gotoAndStopListener, false, 0, true);
			if (element is fCharacter)
				element.addEventListener(fElement.MOVE, this.moveListener, false, 0, true);
			
			//this.occlusionCount = 0;
			// KBEN: _currentBitMap 这个在初始化的时候创建    
			this._currentBitMap = objectPool.getInstanceOf(Bitmap) as Bitmap;
			this.baseObj.addChild(this._currentBitMap);
			_currentBitMap.x = element.bounds2d.x;
			_currentBitMap.y = element.bounds2d.y;
			// KBEN: 加载图片资源在显示的时候加载     
			element.loadRes(0, 0);
			
			// 这个放在这吧，不放在 init 函数里面了，因为如果创建 character 后直接 hideElement ，这个 xml 资源可能加载了，但是图片资源可能没加载，就不能设置 this.assetsCreated = true; 导致再次 showElement 的时候又会嗲用这个函数一次
			this.assetsCreated = true;
		}
		
		/**
		 * This method destroys the assets for this element. It is only called when the element in hidden and fEngine.conserveMemory is set to true
		 */
		public override function destroyAssets():void
		{
			// References
			this.flashClip = this.element.flashClip = null;
			
			// Events
			this.element.removeEventListener(fRenderableElement.SHOW, this.showListener);
			this.element.removeEventListener(fRenderableElement.HIDE, this.hideListener);
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
		}
		
		/**
		 * When a character moves, the cache needs to be reset
		 */
		private function moveListener(evt:Event):void
		{
			
		}
		
		/**
		 * This method syncs shadows to the base movieClip
		 */
		// KBEN: 这个函数就是说回到待机状态  
		private function gotoAndStopListener(evt:Event):void
		{
			
		}
		
		/**
		 * This method syncs shadows to the base movieClip
		 */
		// KBEN: 切换状态需要在这里进行处理   
		private function gotoAndPlayListener(evt:Event):void
		{
			
		}
		
		/**
		 * This method will redraw this object's shadows when it is shown
		 */
		private function showListener(evt:Event):void
		{
			
		}
		
		/**
		 * This method will erase this object's shadows when it is hidden
		 */
		private function hideListener(evt:Event):void
		{
			
		}
		
		/**
		 * Redraws lights in this Object
		 */
		private function paintLights():void
		{
			var res:ColorTransform = new ColorTransform;
			
			res.concat(this.glight.color);
			
			// KBEN: 只有 GI ，没有 LocalLight 			
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
		
		/** @private */
		public function disposeObjectRenderer():void
		{
			this.glight = null;			
			// Gfx
			this.destroyAssets();
			
			this.disposeRenderer();
		
		}
		
		/** @private */
		public override function dispose():void
		{
			this.disposeObjectRenderer();
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
				_currentBitMap.bitmapData = getFrame(_currentFrame, 0);
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
		public function isActInit(act:uint):Boolean
		{
			return _frames != null;
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
		protected function buildFrames(act:uint) : void
        {
			// 这个是图片在一张图片上     			
			var el:fObject = this.element as fObject;
			
			// KBEN:
			if(!isLoaded)
				return;
			// KBEN: 动作只初始化一边    
			if (isActInit(act))
				return;
			
			// KBEN: 取出帧数量
			_frames = new Vector.<BitmapData>(el.definition.dicAction[act].frameCount);
			// KBEN: 一个动作在一张图上    
			var idx:int = 0;	// 动作索引 
			var angleIdx:uint = 0;	// 方向索引  
			var picIdx:uint = 0; 	// 图片索引  
			var bitmapdata:BitmapData;
			// 特效只有一个动作   

			var action:fActDefinition;
			var actDirect:fActDirectDefinition;
			action = el.definition.dicAction[0];
			angleIdx = 0;
			picIdx = 0;
			actDirect = action.directDic[0];
			bitmapdata = objectPool.getInstanceOf(actDirect.spriteVec[0].sprite) as BitmapData;

			// KBEN: 所有的动作资源一次全部构造出来，这个以后可以改   
			while(angleIdx < el.definition.yCount)
			{
				while (picIdx < angleIdx * action.xCount)
				{
					if (angleIdx * action.xCount + picIdx > action.total)
					{
						break;
					}
					
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
		
		// KBEN:主要是图片类初始化类常量，粒子只有一个动作，就是序列帧     
		override public function init(res:SWFResource, act:uint, direction:uint):void
		{
			// KBEN: 初始化后设置加载完成 
			//this.assetsCreated = true;
			
			var element:fObject = this.element as fObject;
			element.definition.init(res, act);
			buildFrames(act);
		}
	}
}