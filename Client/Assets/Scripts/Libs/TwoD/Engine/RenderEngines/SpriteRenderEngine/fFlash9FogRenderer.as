package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	// Imports
	//import com.adobe.images.JPGEncoder;
	import com.pblabs.engine.entity.EntityCValue;
	//import com.pblabs.engine.resource.SWFResource;
	import com.util.PBUtil;
	import flash.display.Bitmap;
	import flash.display.BitmapData;
	import flash.display.BlendMode;
	import flash.display.DisplayObjectContainer;
	import flash.display.Shape;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.TimerEvent;
	import flash.filters.BitmapFilterQuality;
	import flash.filters.BlurFilter;
	import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	//import flash.net.FileReference;
	//import flash.utils.ByteArray;
	import flash.utils.Timer;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.core.fEngine;
	import org.ffilmation.engine.core.fPlane;
	import org.ffilmation.engine.core.fScene;
	import org.ffilmation.engine.elements.fFogPlane;
	import org.ffilmation.engine.events.fNewMaterialEvent;
	import org.ffilmation.engine.helpers.fEngineCValue;
	import org.ffilmation.utils.objectPool;
	import org.ffilmation.utils.polygons.fPolygon;
	import org.ffilmation.utils.vport;
	
	/**
	 * ...
	 * @author 
	 * @brief 黑色雾渲染器     
	 */
	/**
	 * This class renders fPlanes
	 * @private
	 */
	public class fFlash9FogRenderer extends fFlash9ElementRenderer
	{
		// Private properties
		private var origWidth:Number;
		private var origHeight:Number;
		private var cacheTimer:Timer;
		
		public var scrollR:Rectangle; // Scroll Rectangle for this plane, to optimize viewing areas.
		public var planeDeform:Matrix; // Transformation matrix for this plane that sets the proper perspective
		public var clipPolygon:fPolygon; // This is the shape polygon with perspective applied
		
		// Cache for this plane, to bake a bitmap of it when it doesn't change
		public var finalBitmapMask:Shape;
		public var finalBitmap:Bitmap;
		private var finalBitmapData:BitmapData;
		
		private var spriteToDraw:Sprite;
		public var baseContainer:DisplayObjectContainer;

		public var zIndex:Number = 0; // zIndex
		
		// KBEN: 黑色雾
		protected var m_fogSprite:Sprite;
		public var deformedSimpleShadowsLayer:Sprite;
		public var vp:vport;
		
		// 雾过滤器  
		public var m_blurFilter:BlurFilter;
		//protected var m_showFogSprite:Sprite;	// 挂在到显示列表上的      
		protected var m_bufferData:BitmapData;	// 临时的缓冲区数据   
		
		// Constructor
		function fFlash9FogRenderer(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fFogPlane):void
		{
			this.scene = element.scene;
			var destination:Sprite = objectPool.getInstanceOf(Sprite) as Sprite;
			container.addChild(destination);
			
			this.scrollR = new Rectangle(0, 0, element.width, element.depth);
			this.planeDeform = new Matrix();
			
			this.origWidth = element.width;
			this.origHeight = element.depth;
			this.spriteToDraw = destination;
			
			// Previous
			super(rEngine, element, null, container);
			
			// KBEN: 这个要放在父构造函数之后，这样 scene 才不会为空 
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO || 
			    this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				this.planeDeform.rotate(-45 * Math.PI / 180);
				this.planeDeform.scale(1.0015, 0.501);
			}
			
			// Listen to changes in material
			element.addEventListener(fPlane.NEWMATERIAL, this.newMaterial, false, 0, true);
			element.addEventListener(fFogPlane.FOGREGION, this.newFogRegion, false, 0, true);
			
			// This is the polygon that is drawn to represent this plane, with perspective applied
			this.clipPolygon = new fPolygon();
			var contours:Array = element.shapePolygon.contours;
			
			// Process shape vertexes
			// KBEN: 这个是裁剪矩形填充的地方，一定要填充，否则显示有问题  
			if (element is fFogPlane)
			{
				var cl:int = contours.length;
				for (var k:int = 0; k < cl; k++)
				{
					var c:Array = contours[k];
					var projectedShape:Array = new Array;
					var cl2:int = c.length;
					for (var k2:int = 0; k2 < cl2; k2++)
						projectedShape[k2] = fScene.translateCoords(fEngine.RENDER_FINETUNE_3 + c[k2].x * fEngine.RENDER_FINETUNE_1 + c[k2].y * fEngine.RENDER_FINETUNE_2, fEngine.RENDER_FINETUNE_3 + c[k2].y * fEngine.RENDER_FINETUNE_1 + c[k2].x * fEngine.RENDER_FINETUNE_2, 0);
					this.clipPolygon.contours[this.clipPolygon.contours.length] = projectedShape;
				}
			}
			
			// Clipping viewport
			this.vp = new vport();
			this.vp.x_min = element.x;
			this.vp.x_max = element.x + element.width;
			this.vp.y_min = element.y;
			this.vp.y_max = element.y + element.depth;
		}
		
		/**
		 * This method creates the assets for this plane. It is only called the first time the element scrolls into view
		 */
		public override function createAssets():void
		{
			var element:fFogPlane = this.element as fFogPlane;
			
			// This is the Sprite where all light layers are generated.
			// This Sprite is attached to the sprite that is visible onscreen
			this.baseContainer = objectPool.getInstanceOf(Sprite) as Sprite;
			// KBEN: 掩码层    
			this.finalBitmap = new Bitmap(null, "never", true);
			//this.finalBitmap = new Bitmap();
			this.finalBitmapMask = new Shape();
			this.finalBitmapMask.graphics.clear();
			this.finalBitmapMask.graphics.beginFill(0xFF0000, 1);
			this.clipPolygon.draw(this.finalBitmapMask.graphics);
			this.spriteToDraw.addChild(this.finalBitmapMask);
			this.finalBitmapMask.graphics.endFill();
			this.finalBitmap.mask = this.finalBitmapMask;
			
			// KBEN: 在这里分配资源 
			this.finalBitmapData = new BitmapData(this.scene.engine.m_context.m_config.m_curWidth + 2 * element.m_border, this.scene.engine.m_context.m_config.m_curHeight + 2 * element.m_border, true, 0);
			//this.finalBitmapData = new BitmapData(this.scene.engine.m_context.m_config.m_stageWidth, this.scene.engine.m_context.m_config.m_stageHeight);

			// KBEN: 直接赋值    
			this.finalBitmap.bitmapData = finalBitmapData;
			//this.m_showFogSprite = new Sprite();
			
			//this.spriteToDraw.addChild(this.m_showFogSprite);
			//this.m_showFogSprite.blendMode = BlendMode.MULTIPLY;
			//this.m_showFogSprite.addChild(this.finalBitmap);
			//this.m_showFogSprite.cacheAsBitmap = true;
			//this.spriteToDraw.blendMode = BlendMode.MULTIPLY;
			
			this.spriteToDraw.addChild(this.finalBitmap);
			this.finalBitmap.blendMode = BlendMode.MULTIPLY;
			
			// KBEN: 这个要放在父构造函数之后，这样 scene 才不会为空 
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO || 
			    this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				this.finalBitmap.y = Math.round(element.bounds2d.y);
			}
			
			this.baseContainer.mouseEnabled = false;
			
			// Object shadows with qualities other than fShadowQuality.BEST will be drawn here instead of into each lights's ERASE layer
			this.deformedSimpleShadowsLayer = objectPool.getInstanceOf(Sprite) as Sprite;
			this.deformedSimpleShadowsLayer.mouseEnabled = false;
			this.deformedSimpleShadowsLayer.mouseChildren = false;
			this.deformedSimpleShadowsLayer.transform.matrix = this.planeDeform;
			this.spriteToDraw.addChild(this.deformedSimpleShadowsLayer);
			
			// KBEN: 雾颜色 
			this.m_fogSprite = new Sprite();
			var m:Matrix = m_fogSprite.transform.matrix;
			m = planeDeform;
			m_fogSprite.transform.matrix = m;
			
			// test:测试雾图像   
			//this.spriteToDraw.addChild(this.baseContainer);
			
			this.baseContainer.addChild(this.m_fogSprite);
			// KBEN: 与其它内容进行加成    
			//this.m_fogSprite.blendMode = BlendMode.MULTIPLY;
			this.m_fogSprite.mouseEnabled = false;
			this.m_fogSprite.mouseChildren = false;
			// 初始化绘制黑色 
			var fogplane:fFogPlane = this.element as fFogPlane;
			this.m_fogSprite.graphics.beginFill(0x000000);
			this.m_fogSprite.graphics.drawRect(0, 0, fogplane.width, fogplane.depth);
			//this.m_fogSprite.graphics.drawRect(0, 0, 500, 500);
			this.m_fogSprite.graphics.endFill();
			
			// Cache as Bitmap with Timer cache
			// The cache is disabled while the Plane is being modified and a timer is set to re-enable it
			// if the plane doesn't change in a while
			// KBEN: 这个地方为什么要定时器呢     
			//this.undoCache();
			//this.cacheTimer = new Timer(100, 1);
			//this.cacheTimer.addEventListener(TimerEvent.TIMER, this.cacheTimerListener, false, 0, true);
			//this.cacheTimer.start();
			
			// 使用模糊过滤器    
			m_blurFilter = new BlurFilter();
			m_blurFilter.blurX = element.m_blurX;
			m_blurFilter.blurY = element.m_blurY;
			m_blurFilter.quality = BitmapFilterQuality.MEDIUM;
			//m_fogSprite.filters = [m_blurFilter];
			//this.finalBitmap.filters = [m_blurFilter];
			//this.finalBitmap.cacheAsBitmap = true;
			
			// 雾就不缓存了     
			//undoCache();
		}
		
		/**
		 * This method destroys the assets for this element. It is only called when the element in hidden and fEngine.conserveMemory is set to true
		 */
		public override function destroyAssets():void
		{
			// Cache
			this.undoCache();
			if (this.cacheTimer)
			{
				this.cacheTimer.removeEventListener(TimerEvent.TIMER, this.cacheTimerListener);
				this.cacheTimer.stop();
				this.cacheTimer = null;
			}
			
			var element:fPlane = this.element as fPlane;
			// Return to object pool
			fFlash9RenderEngine.recursiveDelete(this.baseContainer);
			objectPool.returnInstance(this.baseContainer);
			
			if (this.finalBitmap)
				this.finalBitmap.mask = null;
			if (this.finalBitmapMask)
				this.finalBitmapMask.graphics.clear();
			this.finalBitmapMask = null;
			
			this.finalBitmap = null;
			if (this.finalBitmapData)
				this.finalBitmapData.dispose();
			this.finalBitmapData = null;
			this.baseContainer = null;
			
			element.removeEventListener(fPlane.NEWMATERIAL, this.newMaterial);
			element.removeEventListener(fFogPlane.FOGREGION, this.newFogRegion);
		}
		
		// PLANE CACHE
		//////////////
		
		/**
		 * Cache on
		 */
		public function doCache():void
		{
			// Already cached
			if (this.finalBitmap.parent)
				return;
			
			// New cache
			if (this.finalBitmapData)
				this.finalBitmapData.dispose();
			// KBEN: 资源分配在开始的时候就分配好了，除非分辨率改变    
			//this.finalBitmapData = new BitmapData(this.element.bounds2d.width, this.element.bounds2d.height, true, 0);
			
			// Draw
			var oMatrix:Matrix = new Matrix();
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO || 
				this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				oMatrix.translate(0, Math.round(element.bounds2d.y));
			}
			//this.finalBitmapData.draw(this.baseContainer, oMatrix, null, null, new Rectangle(0, 0, 1000, 1000));
			this.finalBitmapData.draw(this.baseContainer, null, null, null, new Rectangle(0, 0, this.scene.engine.m_context.m_config.m_curWidth, this.scene.engine.m_context.m_config.m_curHeight));
			
			// Display
			this.finalBitmap.bitmapData = this.finalBitmapData;
			// KBEN: 都赋值一下， this.diffuse.y 这个值本来就是对的
			this.finalBitmap.y = Math.round(element.bounds2d.y);
			// KBEN: 将整个面板的内容显示到屏幕上    
			this.spriteToDraw.addChildAt(this.finalBitmap, 0);
			
			try
			{
				// KBEN: 缓存开启的时候要把 baseContainer 从场景图上移除   
				this.spriteToDraw.removeChild(this.baseContainer)
			}
			catch (e:Error)
			{
			}
			
			this.container.cacheAsBitmap = true;
		}
		
		/**
		 * Cache off
		 */
		// KBEN: 这个函数是使用缓存  
		public function undoCache(autoStart:Boolean = false):void
		{			
			var p:fPlane = this.element as fPlane;
			if (this.finalBitmapData)
				this.finalBitmapData.dispose();
			// KBEN: 缓存关闭的时候要把 baseContainer 添加到场景图上  
			this.spriteToDraw.addChildAt(this.baseContainer, 0);
			
			try
			{
				this.spriteToDraw.removeChild(this.finalBitmap);
			}
			catch (e:Error)
			{
			}
			
			this.container.cacheAsBitmap = false;
			
			if (autoStart)
				this.cacheTimer.start();
		}
		
		// KBEN: 更新内容 
		public function updateFog():void
		{			
			// Draw
			var oMatrix:Matrix = new Matrix();
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO || 
				this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				oMatrix.translate(0, Math.round(element.bounds2d.y));
			}

			this.finalBitmapData.draw(this.baseContainer, null, null, BlendMode.NORMAL, new Rectangle(0, 0, this.scene.engine.m_context.m_config.m_curWidth, this.scene.engine.m_context.m_config.m_curHeight));
			//this.finalBitmapData.draw(this.m_fogSprite, null, null, null, new Rectangle(0, 0, this.scene.engine.m_context.m_config.m_stageWidth, this.scene.engine.m_context.m_config.m_stageHeight));
			
			// KBEN: 都赋值一下， this.diffuse.y 这个值本来就是对的
			this.finalBitmap.y = Math.round(element.bounds2d.y);
			// bug: 缓存后，将不能与背景进行 multiply 进行混合了，为什么啊  
			//this.container.cacheAsBitmap = true;
		}
		
		/**
		 * This listener sets the cache of a Plane back to true when it doesn't change for a while
		 */
		public function cacheTimerListener(event:TimerEvent):void
		{
			this.doCache();
		}
		
		// REACT TO CHANGES IN SCENE
		////////////////////////////
		
		/**
		 * Renders element visible
		 */
		public override function show():void
		{
			this.containerParent.addChild(this.container);
		}
		
		/**
		 * Renders element invisible
		 */
		public override function hide():void
		{
			try
			{
				this.containerParent.removeChild(this.container);
			}
			catch (e:Error)
			{
			}
		}
		
		// OTHER
		////////
		
		/**
		 * Mouse management
		 */
		public override function disableMouseEvents():void
		{
			this.container.mouseEnabled = false;
			this.spriteToDraw.mouseEnabled = false;
		}
		
		/**
		 * Mouse management
		 */
		public override function enableMouseEvents():void
		{
			this.container.mouseEnabled = true;
			this.spriteToDraw.mouseEnabled = true;
		}
		
		// 雾改变需要绘制的时候调用这个函数    
		private function newMaterial(evt:fNewMaterialEvent):void
		{
			// 绘制整个雾   
			var fogplane:fFogPlane = this.element as fFogPlane;
			var fillColor:uint=0xFFFFFF;
			var lineColor:uint = 0xFFFFFF;
			if(EntityCValue.CenterMain == evt.id)	// 更新主角位置
			{
				if (fogplane.m_drawMethod == EntityCValue.MTReg)
				{
					PBUtil.drawRegPolygonX(m_fogSprite.graphics, fogplane.m_radius, fogplane.m_segments, fogplane.m_centerX, fogplane.m_centerY);
				}
				else if(fogplane.m_drawMethod == EntityCValue.MTNoReg)
				{
					PBUtil.drawNoRegPolygon(fogplane.m_ptList, m_fogSprite.graphics);
				}
				else if(fogplane.m_drawMethod == EntityCValue.MTCircle)
				{
					m_fogSprite.graphics.beginFill(fillColor);
					m_fogSprite.graphics.drawCircle(fogplane.m_centerX, fogplane.m_centerY, fogplane.m_radius);
					m_fogSprite.graphics.endFill();
				}
			}
			else if(EntityCValue.CenterAny == evt.id)		// 更新任意区域
			{
				m_fogSprite.graphics.beginFill(fillColor);
				m_fogSprite.graphics.drawCircle(fogplane.m_anycenterX, fogplane.m_anycenterY, fogplane.m_anyradius);
				m_fogSprite.graphics.endFill();
			}
		}
		
		private function newFogRegion(evt:Event):void
		{
			var fogplane:fFogPlane = this.element as fFogPlane;
			
			if (this.finalBitmapData)
				this.finalBitmapData.dispose();			
			try
			{
				this.spriteToDraw.removeChild(this.finalBitmap);
			}
			catch (e:Error)
			{
			}
			
			this.container.cacheAsBitmap = false;
			
			this.finalBitmapData = new BitmapData(this.scene.engine.m_context.m_config.m_curWidth + 2 * fogplane.m_border, this.scene.engine.m_context.m_config.m_curHeight + 2 * fogplane.m_border, true, 0);
			
			
			
			
			var startx:Number = 0;
			var starty:Number = 0;
			
			var endx:Number = 0;
			var endy:Number = 0;
			
			var width:Number = 0
			var height:Number = 0;
			
			startx = fogplane.x - this.scene.engine.m_context.m_config.m_curWidth / 2 - fogplane.m_border;
			starty = fogplane.y - this.scene.engine.m_context.m_config.m_curHeight / 2 - fogplane.m_border;
			
			if (startx < 0)
			{
				startx = 0;
			}
			if (starty < 0)
			{
				starty = 0;
			}
			
			endx = fogplane.x + this.scene.engine.m_context.m_config.m_curWidth / 2 + fogplane.m_border;
			endy = fogplane.y + this.scene.engine.m_context.m_config.m_curHeight / 2 + fogplane.m_border;
			
			if (endx > this.scene.width)
			{
				endx = this.scene.width;
			}
			if (endy > this.scene.depth)
			{
				endy = this.scene.depth;
			}
			
			width = endx - startx;
			height = endy - starty;
			
			// 更位位图   
			//var bitmap:BitmapData = new BitmapData(m_fogSprite.width, m_fogSprite.height);
			//bitmap.draw(m_fogSprite);
			this.container.cacheAsBitmap = false;
			
			var oMatrix:Matrix = new Matrix();
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO || 
				this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				oMatrix.translate(0, Math.round(element.bounds2d.y));
			}

			//this.finalBitmapData.draw(this.baseContainer, null, null, null, new Rectangle(startx, starty, width, height));
			//this.finalBitmapData = new BitmapData(width, height, true, 0);
			oMatrix = new Matrix();
			oMatrix.translate(-startx, -starty);
			//this.finalBitmapData.draw(this.m_fogSprite, null, null, null, new Rectangle(startx, starty, width, height));
			this.finalBitmapData.draw(this.m_fogSprite, oMatrix, null, null, new Rectangle(0, 0, width, height));
			
			//this.finalBitmap.x = Math.round(startx - fogplane.x);
			//this.finalBitmap.y = Math.round(starty - fogplane.y);
			
			this.finalBitmap.x = Math.round(startx);
			this.finalBitmap.y = Math.round(starty);
			
			//this.finalBitmap.x = 100;
			//this.finalBitmap.y = 100;
			
			// bug: 缓存后，将不能与背景进行 multiply 进行混合了，为什么啊  
			//this.container.cacheAsBitmap = true;
			
			// 应用滤镜
			this.finalBitmapData.lock();
			this.finalBitmapData.applyFilter(this.finalBitmapData, new Rectangle(fogplane.m_border, fogplane.m_border, this.scene.engine.m_context.m_config.m_curWidth, this.scene.engine.m_context.m_config.m_curHeight), new Point(fogplane.m_border, fogplane.m_border),  m_blurFilter);
			this.finalBitmapData.unlock();	
			
			
			
			
			// Display
			this.finalBitmap.bitmapData = this.finalBitmapData;

			// KBEN: 将整个面板的内容显示到屏幕上    
			this.spriteToDraw.addChild(this.finalBitmap);			
			//this.container.cacheAsBitmap = true;
		}

		/*
		// test: 测试保存图片 
		public function savePic():void
		{
			// KBEN: 打印测试     
			var encode:JPGEncoder = new JPGEncoder(90);
			var ba:ByteArray = encode.encode(finalBitmapData);
			var file:FileReference = new FileReference();
			file.save(ba, "aaa.jpg");
		}
		*/
		
		/** @private */
		public function disposePlaneRenderer():void
		{
			// Assets
			this.destroyAssets();
			
			this.clipPolygon = null
			this.spriteToDraw = null;
			
			this.disposeRenderer();
		}
		
		/** @private */
		public override function dispose():void
		{
			this.disposePlaneRenderer();
		}
		
		// 服务器刷新两边进场景消息，会调用父类的这个函数，从而把 元素的 x y 坐标同步到 render 中了，导致坐标错误
		override public function place():void
		{
			
		}
	}
}