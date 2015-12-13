package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	// Imports
	import com.pblabs.engine.core.ITickedObject;
	import com.pblabs.engine.resource.SWFResource;
	import flash.display.BitmapData;
	import flash.display.DisplayObject;
	import flash.display.DisplayObjectContainer;
	import flash.display.MovieClip;
	import flash.geom.Point;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fGlobalLight;
	import org.ffilmation.engine.core.fLight;
	import org.ffilmation.engine.core.fRenderableElement;
	import org.ffilmation.engine.core.fScene;
	import org.ffilmation.engine.elements.fCharacter;
	
	/**
	 * This is the basic flash 9 element renderer. All renderes inherit from this
	 * @private
	 */
	public class fFlash9ElementRenderer implements ITickedObject
	{
		/** The element this class renders */
		public var element:fRenderableElement;
		
		/** The engine for this renderer */
		public var rEngine:fFlash9RenderEngine;
		
		/** The scene where the rendering occurs */
		public var scene:fScene;
		
		/** The container for the element */
		public var container:fElementContainer;

		// 场景层
		public var m_sceneContainer:DisplayObjectContainer;
		
		/** The graphic asset for this element */
		public var flashClip:MovieClip;
		
		/** The graphic asset that is displayed for this element */
		public var containerToPaint:DisplayObject;
		
		/** Storing this allows us to show/hide the element */
		public var containerParent:DisplayObjectContainer;
		
		/** Are the assets for this plane already created ? */
		// 这个不再判断资源是否加载完成，现在不同的动作在不同资源里   
		public var assetsCreated:Boolean = false;
		
		/** This stores render messages that reached the element before it was created, so when it is created it can be synched to the current state */
		// KBEN: 去掉 rendermessage
		//public var renderMessages:fRenderMessageQueue;
		
		/** Is the element visible ( not logically but in terms of render visibility */
		public var screenVisible:Boolean = false;
		
		// KBEN:资源，暂时都是将图片打包成 SWC 资源，资源放在 fRenderableElement 类中了   
		// protected var _res:SWFResource = null;	
		protected var _needDrawTextField:Boolean = false;
		
		// Constructor
		/** @private */
		function fFlash9ElementRenderer(rEngine:fFlash9RenderEngine, element:fRenderableElement, libraryMovieClip:DisplayObject, spriteToShowHide:fElementContainer):void
		{
			// Pointer to element
			this.element = element;			
			this.rEngine = rEngine;
			
			// Main container
			this.containerToPaint = libraryMovieClip;
			if (libraryMovieClip is MovieClip)
				this.flashClip = (libraryMovieClip as MovieClip);
			this.container = spriteToShowHide;
			
			// The container comes attached from the engine only so we can store the reference to the parent
			this.containerParent = this.container.parent;
			if (this.containerParent)
			{
				this.containerParent.removeChild(this.container);
			}
			
			// Move asset to appropiate position
			this.place();
			
			// Create message queue
			// KBEN: 去掉 rendermessage
			//this.renderMessages = new fRenderMessageQueue();
		}
		
		/**
		 * This method creates the assets for this element. It is only called when the element in shown and the assets don't exists
		 */
		public function createAssets():void
		{
		}
		
		/**
		 * This method destroys the assets for this element. It is only called when the element in hidden and fEngine.conserveMemory is set to true
		 */
		public function destroyAssets():void
		{
		}
		
		/**
		 * Place asset its proper position
		 */
		public function place():void
		{
			// Place in position
			var coords:Point = fScene.translateCoords(this.element.x, this.element.y, this.element.z);
			this.container.x = Math.floor(coords.x);
			this.container.y = Math.floor(coords.y);
		}
		
		/**
		 * Mouse management
		 */
		public function disableMouseEvents():void
		{
			this.container.mouseEnabled = false;
		}
		
		/**
		 * Mouse management
		 */
		public function enableMouseEvents():void
		{
			this.container.mouseEnabled = true;
		}
		
		/**
		 * Renders element visible
		 */
		public function show():void
		{
			if (containerParent)
			{
				this.containerParent.addChild(this.container);
			}
			
			// KBEN: 更新链接元素显示   
			element.showRender();
		}
		
		/**
		 * Renders element invisible
		 */
		public function hide():void
		{
			// 防止调用多次隐藏挂掉
			if (containerParent && container.parent == containerParent)
			{
				this.containerParent.removeChild(this.container);
			}
			
			// KBEN: 更新链接元素显示    
			element.hideRender();
		}
		
		/**
		 * Sets global light
		 */
		public function renderGlobalLight(light:fGlobalLight):void
		{
		}
		
		/**
		 * Global light changes intensity
		 */
		public function processGlobalIntensityChange(light:fGlobalLight):void
		{
		}
		
		/**
		 * Global light changes color
		 */
		public function processGlobalColorChange(light:fGlobalLight):void
		{
		}
		
		/**
		 *	Light reaches element
		 */
		public function lightIn(light:fLight):void
		{
		}
		
		/**
		 * Light leaves element
		 */
		public function lightOut(light:fLight):void
		{
		}
		
		/**
		 * Light is to be reset
		 */
		public function lightReset(light:fLight):void
		{
			this.lightOut(light);
		}
		
		/**
		 * Render start
		 */
		public function renderStart(light:fLight):void
		{
		
		}
		
		/**
		 * Render ( draw ) light
		 */
		public function renderLight(light:fLight):void
		{
		
		}
		
		/**
		 * Renders shadows of other elements upon this element
		 */
		public function renderShadow(light:fLight, other:fRenderableElement):void
		{
		
		}
		
		/**
		 * Updates shadow of a moving element into this element
		 */
		public function updateShadow(light:fLight, other:fRenderableElement):void
		{
		
		}
		
		/**
		 * Removes shadow from another element
		 */
		public function removeShadow(light:fLight, other:fRenderableElement):void
		{
		
		}
		
		/**
		 * Ends render
		 */
		public function renderFinish(light:fLight):void
		{
		
		}
		
		/**
		 * Starts acclusion related to one character
		 */
		public function startOcclusion(character:fCharacter):void
		{
		}
		
		/**
		 * Updates acclusion related to one character
		 */
		public function updateOcclusion(character:fCharacter):void
		{
		}
		
		/**
		 * Stops acclusion related to one character
		 */
		public function stopOcclusion(character:fCharacter):void
		{
		}
		
		/**
		 * Resets shadows. This is called when the fEngine.shadowQuality value is changed
		 */
		public function resetShadows():void
		{
		}
		
		/**
		 * Frees resources
		 */
		public function disposeRenderer():void
		{
			// Remove dependencies
			this.containerToPaint = null;
			fFlash9RenderEngine.recursiveDelete(this.container);
			this.container = null;
			this.containerParent = null;
			this.element = null;	
			this.rEngine = null;
			this.flashClip = null;
			//this.renderMessages.dispose();
			//this.renderMessages = null;
		}
		
		public function dispose():void
		{
			this.disposeRenderer();
		}
		
		// KBEN:
		// 每一帧更新 
		public function onTick(deltaTime:Number):void
		{
			
		}
		
		// KBEN:主要是资源类初始化类常量  
		public function init(res:SWFResource, act:uint, direction:uint):void
		{
			
		}
		
		// KBEN: 切换渲染显示容器，一般是把特效关联到一个实体上面  
		public function changeContainerParent(pnt:DisplayObjectContainer):void
		{
			if (this.container.parent)
			{
				this.containerParent.removeChild(this.container)
			}
			
			this.containerParent = pnt;
			//this.containerParent.addChild(this.container);
		}
		
		// KBEN: 动作是否播放完，重复动作总是返回 false ，不重复的动作播放完了返回 true 
		public function aniOver():Boolean 
		{
			return false;
		}
		
		// 在换外观的时候使用，将外观的属性释放了，目前只实现人物   
		public function disposeShow():void
		{
			
		}
		
		// 获取当前动作的方向， 0 - 7 
		public function get actDir():Number
		{
			return 0;
		}
		
		// 检测不规则图像，有透明区域的图像的鼠标点选 
		public function hitTest(globalx:Number, globaly:Number):Boolean
		{
			return false;
		}
		
		public function get currentFrame():int 
		{
			return 0;
		}
		
		public function set currentFrame(value:int):void
		{
			
		}
		
		public function set rawTextField(b:Boolean):void
		{
			_needDrawTextField = b;
		}
		
		public function layerContainer(layer:uint):DisplayObjectContainer
		{
			return null;
			// 或者抛出异常    
			//throw new Event("error");
		}
		
		public function onMouseEnter():void
		{
			
		}
		
		public function onMouseLeave():void
		{
			
		}
		
		public function changeAngle(value:Number):void
		{
			
		}
		
		public function get curImage():BitmapData
		{
			return null;
		}
		
		public function getCurFrame(deltaTime:Number):int
		{
			return 0;
		}
		
		public function clearName():void
		{
			
		}
		
		public function grayChange(bgrey:Boolean):void
		{
			
		}
		
		public function toggleSceneShow(bshow:Boolean):void
		{
			if (bshow)
			{
				if (!this.container.contains(this.m_sceneContainer))
				{
					this.container.addChild(this.m_sceneContainer);
				}
			}
			else
			{
				if (this.container.contains(this.m_sceneContainer))
				{
					this.container.removeChild(this.m_sceneContainer);
				}
			}
		}
	}
}