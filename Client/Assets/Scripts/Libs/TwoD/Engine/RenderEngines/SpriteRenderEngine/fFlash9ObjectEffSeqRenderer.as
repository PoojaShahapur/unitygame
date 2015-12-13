package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	//import com.pblabs.engine.debug.Logger;
	import com.pblabs.engine.entity.EntityCValue;
	import com.pblabs.engine.resource.SWFResource;
	import com.util.PBUtil;
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.geom.ColorTransform;
	import flash.geom.Matrix;
	import org.ffilmation.engine.core.fElement;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fGlobalLight;
	import org.ffilmation.engine.core.fRenderableElement;
	import org.ffilmation.engine.elements.fCharacter;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.elements.fSceneObject;
	import org.ffilmation.engine.helpers.fActDefinition;
	import org.ffilmation.engine.helpers.fActDirectDefinition;
	import org.ffilmation.utils.movieClipUtils;
	import org.ffilmation.utils.objectPool;
	
	/**
	 * This class renders an fObject
	 * @private
	 * @brief 特效渲染器，所有的资源在一张图上          
	 */
	public class fFlash9ObjectEffSeqRenderer extends fFlash9ElementRenderer
	{
		// Private properties
		private var _baseObj:Sprite;
		private var _glight:fGlobalLight;
		private var _currentSpriteIndex:Number = -1;	// 这个是方向
		private var _currentFrame:int = 0;			// 这个相当于 MovieClip 中的索引
		private var _currentBitMap:Bitmap;			// 这个是序列帧动画
		// KBEN: 存储当前的动作
		protected var _frames:Vector.<BitmapData> = null;  
		// KBEN: 简单阴影直接挂在人物身上显示
		public var _shadowSp:Sprite;
		// KBEN: 调试信息输出的地方 
		public var _debugSprite:Sprite;
		
		// Constructor
		/** @private */
		function fFlash9ObjectEffSeqRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fObject):void
		{
			// Previous
			super(rEngine, element, null, container);
			
			// Angle
			var correctedAngle:Number = element._orientation / 360;
			// KBEN: 角度获取都要改成这个 
			//this.currentSpriteIndex = int(correctedAngle * element.sprites.length);
			// KBEN: 特效只有一个动作 
			this._currentSpriteIndex = 0;
			
			// KBEN: 注意重新初始化的时候也会导致 _frames.length = 0 
			_frames = new Vector.<BitmapData>();
		}
		
		/**
		 * This method creates the assets for this plane. It is only called when the element in shown and the assets don't exist
		 */
		public override function createAssets():void
		{
			// test创建调试信息
			//_debugSprite ||= new Sprite();
			//_debugSprite.mouseEnabled = false;
			//container.addChild(this._debugSprite);
			
			// Attach base clip
			this._baseObj = objectPool.getInstanceOf(Sprite) as Sprite;
			container.addChild(this._baseObj);
			this._baseObj.mouseEnabled = false;
			
			// Cache as bitmap non-animated objects
			//var element:fObject = this.element as fObject;
			var element:fSceneObject = this.element as fSceneObject;
			this.container.cacheAsBitmap = !(element is fCharacter) && element.animated != true;
			
			// Show and hide listeners, to redraw shadows
			this.element.addEventListener(fRenderableElement.SHOW, this.showListener, false, 0, true);
			this.element.addEventListener(fRenderableElement.HIDE, this.hideListener, false, 0, true);
			this.element.addEventListener(fObject.GOTOANDPLAY, this.gotoAndPlayListener, false, 0, true);
			this.element.addEventListener(fObject.GOTOANDSTOP, this.gotoAndStopListener, false, 0, true);
			// KBEN: 特效也添加移动监听   
			this.element.addEventListener(fElement.MOVE, this.moveListener, false, 0, true);
			
			//this.occlusionCount = 0;
			
			// Draw initial sprite
			// KBEN: 特效只有一个动作      
			this._currentSpriteIndex = 0;
			// KBEN: _currentBitMap 这个在初始化的时候创建    
			this._currentBitMap = objectPool.getInstanceOf(Bitmap) as Bitmap;
			this._baseObj.addChild(this._currentBitMap);
			_currentBitMap.x = element.bounds2d.x;
			_currentBitMap.y = element.bounds2d.y;
			// KBEN: 加载图片资源在显示的时候加载 
			// 如果 xml 配置文件已经初始化了，说明这个对象构造的时候资源已经加载完成了，这个时候需要自己初始化 
			if (element.binitXmlDef())
			{
				// 初始化渲染一些数据    
				init(null, 0, 0);
				element.loadRes(0, 0);
			}
			
			// KBEN: 在自己脚底下添加简单的阴影    
			//addSimpleShadow();
			if(element.scene.m_sceneConfig.bshowCharCenter)
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
			// References
			this.flashClip = this.element.flashClip = null;
			
			// Events
			this.element.removeEventListener(fRenderableElement.SHOW, this.showListener);
			this.element.removeEventListener(fRenderableElement.HIDE, this.hideListener);
			this.element.removeEventListener(fObject.GOTOANDPLAY, this.gotoAndPlayListener);
			this.element.removeEventListener(fObject.GOTOANDSTOP, this.gotoAndStopListener);
			// bug: 除了玩家，特效也需要释放
			//if (this.element is fCharacter)
			this.element.removeEventListener(fElement.MOVE, this.moveListener);
			
			// Gfx
			if (this._baseObj)
			{
				container.removeChild(this._baseObj);
				fFlash9RenderEngine.recursiveDelete(this._baseObj);
				objectPool.returnInstance(this._baseObj);
				this._baseObj = null;
			}
			
			this._currentSpriteIndex = -1;
			
			var el:fObject = this.element as fObject;	
			var key:String;
			var action:fActDefinition;
			var idx:uint = 0;

			action = el.definition.dicAction[0];
			while (idx < action.frameCount)
			{
				//objectPool.returnInstance(_frames[idx]);
				_frames[idx] = null;
				++idx;
			}
			
			_frames = null;
		}

		/**
		 * When a character moves, the cache needs to be reset
		 */
		private function moveListener(evt:Event):void
		{
			// KBEN: 移动位置     
			place();
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
			var fobj:fObject = this.element as fObject;
			if (!fobj.m_affectByGI) // 没有 GI 影响
			{
				return;
			}
			
			var res:ColorTransform = new ColorTransform;
			
			res.concat(this._glight.color);
			
			// KBEN: 只有 GI ，没有 LocalLight 		
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
		
		/** @private */
		public function disposeObjectRenderer():void
		{
			this._glight = null;			
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
		// 每一帧更新图片，位置不在这里更新         
		override public function onTick(deltaTime:Number):void
		{
			// 限制大小，根据每秒 30 帧限制的
			//deltaTime = PBUtil.clamp(deltaTime, 0, 0.04);
			//deltaTime = PBUtil.clamp(deltaTime, 0.04, 1);
			
			var el:fObject = this.element as fObject;						
			// KBEN: 添加帧播放速率控制
			var act:uint = el.getAction();
			var misperframe:Number = el.definition.dicAction[act].framerateInv;
			var repeat:Boolean = el.definition.dicAction[act].repeat;
			
			// 特效配置文件加载后可能帧数已经超过最大的帧数了，先校正一边
			if (_currentFrame > el.definition.dicAction[act].xCount - 1)
			{
				_currentFrame = el.definition.dicAction[act].xCount - 1
			}
			
			if (!repeat)
			{
				if (_currentFrame == el.definition.dicAction[act].xCount - 1)
					return;
			}
			
			// 这个是逐帧播放
			if (el.leftInterval + deltaTime >= misperframe)
			{
				_currentFrame = (++_currentFrame) % el.definition.dicAction[el.getAction()].xCount;
				el.leftInterval = el.leftInterval + deltaTime - misperframe;
			}
			else
			{
				el.leftInterval += deltaTime;
			}
			
			el.leftInterval = PBUtil.clamp(el.leftInterval, 0, misperframe);		// 最大就累积一帧
			
			/*
			// 这个是插值计算
			el._totalTime += deltaTime;
			_currentFrame = el._totalTime%(misperframe * el.definition.dicAction[act].xCount)/misperframe;
			*/
			
			// KBEN: 资源建立才更新    
			// 只有 xml 初始化后才行   
			if (el.binitXmlDef())
			{
				if (isLoaded(act, 0) && isActInit(act, 0))
				{
					changeInfoByActDir(act, 0);
					_currentBitMap.bitmapData = getFrame(_currentFrame, _currentSpriteIndex, act);
					if (el.definition.bscale)	// 水平缩放
					{
						_currentBitMap.width = el.scaleWidth2Left;
						_currentBitMap.height = el.scaleHeight2Orig;
					}
					else if(el.definition.bscaleV)	// 垂直缩放
					{
						_currentBitMap.width = el.scaleWidth2Orig;
						_currentBitMap.height = el.scaleHeight2Top;
					}
				}
				else if (!isLoaded(act, 0))	// 资源没有加载    
				{
					element.loadRes(act, 0);
				}
				else if (!isActInit(act, 0))	// 资源还没有初始化    
				{
					changeInfoByActDir(act, 0);
					buildFrames(act, 0);
					_currentBitMap.bitmapData = getFrame(_currentFrame, _currentSpriteIndex, act);
					if (el.definition.bscale)
					{
						_currentBitMap.width = el.scaleWidth2Left;
						_currentBitMap.height = el.scaleHeight2Orig;
					}
					else if(el.definition.bscaleV)	// 垂直缩放
					{
						_currentBitMap.width = el.scaleWidth2Orig;
						_currentBitMap.height = el.scaleHeight2Top;
					}
				}
				//else if (isLoaded(act, 0) && !isActInit(act, 0))	// 如果资源已经加载但是没有初始化,就初始化     
				//{
					//buildFrames(act, this.currentSpriteIndex);
				//}
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
			return (_frames.length > 0 && _frames[_currentFrame] != null);
		}
		
		// KBEN: xindex 是 x 方向索引
        public function getFrame(xindex:int, yindex:int, act:uint):BitmapData
        {
			var el:fObject = this.element as fObject;
            if(!isLoaded(act, 0))
                return null;
			return _frames[yindex * el.definition.dicAction[el.getAction()].xCount + xindex];
        }
		
		// 这个就是直接生成帧序列， act: 生成的动作的索引    
		// 特效仅仅初始化 _frames 这个就行了  
		protected function buildFrames(act:uint, direction:uint) : void
        {			
			var el:fObject = this.element as fObject;
			// KBEN:
			if(!isLoaded(act, 0))
				return;
			// KBEN: 动作只初始化一边    
			if (isActInit(act, 0))
				return;
			
			// KBEN: 取出帧数量
			if (!_frames)
			{
				_frames = new Vector.<BitmapData>(el.definition.dicAction[act].frameCount);
			}
			
			var idx:int = 0;

			var action:fActDefinition;
			var actDirect:fActDirectDefinition;
			var bitmapdata:BitmapData;
			action = el.definition.dicAction[0];
			
			// KBEN: 所有的动作资源一次全部构造出来，这个以后可以改   
			var path:String;
			path = el.m_context.m_path.getPathByName(action.directDic[0].spriteVec[_currentFrame].mediaPath, EntityCValue.PHEFF);

			bitmapdata = el.resDic[path].getExportedAsset(action.directDic[0].spriteVec[_currentFrame].startName, true, el.bFlip);
			_frames[_currentFrame] = bitmapdata;
        }
		
		// KBEN:主要是图片类初始化类常量  
		override public function init(res:SWFResource, act:uint, direction:uint):void
		{
			// KBEN: 初始化后设置加载完成，一定要设置，否则人物身上的特效在离开屏幕的时候会被隐藏掉,在显示的时候会调用 public function showElement(element:fRenderableElement):void ，里面会有判断 if (!r.assetsCreated) 会再次创建一边信息
			//this.assetsCreated = true;
			
			//var element:fObject = this.element as fObject;
			//element.definition.init(res, act);
			//buildFrames(act, 0);
			
			var el:fObject = this.element as fObject;
			
			// KBEN: 取出帧数量
			_frames = new Vector.<BitmapData>(el.definition.dicAction[act].xCount);
		}
		
		// KBEN: 某一个不能重复的动作是否播放完   
		override public function aniOver():Boolean 
		{
			var element:fObject = this.element as fObject;
			return (_currentFrame == element.definition.dicAction[element.getAction()].xCount - 1);
		}
		
		// 获取当前帧，现在特效最差情况就是每一个特效都在一个文件中      
		override public function get currentFrame():int 
		{
			return _currentFrame;
		}
		
		// 每一帧切换的时候，如果资源和上一阵不一样，就需要调用这个    
		protected function changeInfoByActDir(act:uint, dir:uint):void
		{
			var el:fObject = this.element as fObject;
			el.changeInfoByActDir(act, dir);

			if (_currentBitMap)
			{
				if (el.hasScale())
				{
					el.m_context.m_SObjectMgr.m_scaleMatrix = _currentBitMap.transform.matrix;
					el.m_context.m_SObjectMgr.m_scaleMatrix.identity();
					el.m_context.m_SObjectMgr.m_scaleMatrix.translate(element.bounds2d.x, element.bounds2d.y);
					el.m_context.m_SObjectMgr.m_scaleMatrix.scale(el.scaleFactor, el.scaleFactor);
					_currentBitMap.transform.matrix = el.m_context.m_SObjectMgr.m_scaleMatrix;
				}
				else
				{
					_currentBitMap.x = element.bounds2d.x;
					_currentBitMap.y = element.bounds2d.y;
				}
			}
			
			// 添加调试
			//if(_debugSprite)
			//{
				//_debugSprite.x = element.bounds2d.x;
				//_debugSprite.y = element.bounds2d.y;
				
				//_debugSprite.graphics.clear();
				//_debugSprite.graphics.beginFill(0x00FF00);
				//_debugSprite.graphics.drawRect(0, 0, element.bounds2d.width, element.bounds2d.height);
				//_debugSprite.graphics.endFill();
			//}
		}
		
		// KBEN:添加一个简单阴影 
		private function addSimpleShadow():void
		{			
			// 创建阴影
			_shadowSp ||= new Sprite();
			_shadowSp.mouseEnabled = false;
			var clip:Sprite = new Sprite();
			var el:fObject = this.element as fObject;
			if (!_shadowSp.numChildren)
			{
				clip = objectPool.getInstanceOf(Sprite) as Sprite;
				movieClipUtils.circle(clip.graphics, 0, 0, 1.5 * el.radius, 20, 0xFF0000, 100);
				_shadowSp.addChild(clip);
				container.addChild(this._shadowSp);
			}
		}
		
		override public function changeAngle(value:Number):void
		{
			var m:Matrix = _baseObj.transform.matrix;
			m.rotate(value);
			_baseObj.transform.matrix = m;
		}
		
		override public function getCurFrame(deltaTime:Number):int
		{
			var curframe:int = 0;
			var el:fObject = this.element as fObject;						
			// KBEN: 添加帧播放速率控制
			var act:uint = el.getAction();
			var misperframe:Number = el.definition.dicAction[act].framerateInv;
			var repeat:Boolean = el.definition.dicAction[act].repeat;
			curframe = _currentFrame;
			
			// 特效配置文件加载后可能帧数已经超过最大的帧数了，先校正一边
			if (curframe > el.definition.dicAction[act].xCount - 1)
			{
				curframe = el.definition.dicAction[act].xCount - 1
			}
			
			if (!repeat)
			{
				if (curframe == el.definition.dicAction[act].xCount - 1)
				{
					return curframe;
				}
			}
			
			if (el.leftInterval + deltaTime >= misperframe)
			{
				curframe = (++curframe) % el.definition.dicAction[el.getAction()].xCount;
			}
			
			return curframe;
		}
	}
}