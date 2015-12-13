package org.ffilmation.engine.core
{
	// Imports
	//import com.bit101.components.Panel;
	//import com.gskinner.motion.easing.Back;
	import com.pblabs.engine.debug.Logger;
	import com.pblabs.engine.entity.BeingEntity;
	import com.pblabs.engine.entity.EffectEntity;
	import com.pblabs.engine.entity.EntityCValue;
	import org.ffilmation.engine.events.fNewCellEvent;
	//import com.pblabs.engine.resource.SWFResource;
	import com.util.DebugBox;
	import flash.display.BitmapData;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	//import flash.geom.Matrix;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	import flash.utils.Dictionary;
	import org.ffilmation.engine.core.sceneInitialization.fSceneInitializer;
	import org.ffilmation.engine.core.sceneLogic.fCharacterSceneLogic;
	import org.ffilmation.engine.core.sceneLogic.fEffectSceneLogic;
	import org.ffilmation.engine.core.sceneLogic.fEmptySpriteSceneLogic;
	import org.ffilmation.engine.core.sceneLogic.fFObjectSceneLogic;
	import org.ffilmation.engine.core.sceneLogic.fSceneRenderManager;
	import org.ffilmation.engine.datatypes.fCell;
	import org.ffilmation.engine.datatypes.stopPoint;
	import org.ffilmation.engine.elements.fCharacter;
	import org.ffilmation.engine.elements.fEmptySprite;
	import org.ffilmation.engine.elements.fFloor;
	import org.ffilmation.engine.elements.fFogPlane;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.elements.fSceneObject;
	import org.ffilmation.engine.events.fMoveEvent;
	import org.ffilmation.engine.helpers.fEngineCValue;
	import org.ffilmation.engine.helpers.fUtil;
	import org.ffilmation.engine.interfaces.fEngineRenderEngine;
	import org.ffilmation.engine.interfaces.fEngineSceneController;
	import org.ffilmation.engine.interfaces.fEngineSceneRetriever;
	import org.ffilmation.engine.logicSolvers.visibilitySolver.fVisibilitySolver;
	import org.ffilmation.engine.renderEngines.flash9RenderEngine.fFlash9ElementRenderer;
	import org.ffilmation.engine.renderEngines.flash9RenderEngine.fFlash9RenderEngine;
	
	//import org.ffilmation.profiler.*;
	
	/**
	 * <p>The fScene class provides control over a scene in your application. The API for this object is used to add and remove
	 * elements from the scene after it has been loaded, and managing cameras.</p>
	 *
	 * <p>Moreover, you can get information on topology, visibility from one point to another, search for paths and other useful
	 * methods that will help, for example, in programming AIs that move inside the scene.</p>
	 *
	 * <p>The data structures contained within this object are populated during initialization by the fSceneInitializer class (which you don't need to understand).</p>
	 *
	 */
	public class fScene extends EventDispatcher
	{
		// This counter is used to generate unique scene Ids
		private static var count:Number = 0;
		
		// This flag is set by the editor so every object is created as a character and can be moved ( so ugly I know but It does work )
		/** @private */
		public static var allCharacters:Boolean = false;
		
		// Private properties
		
		// 1. References
		private var _controller:fEngineSceneController = null;
		/** @private */
		//public var prof:fProfiler = null; // Profiler
		private var initializer:fSceneInitializer; // This scene's initializer
		/** @private */
		public var renderEngine:fEngineRenderEngine; // The render engine
		/** @private */
		public var renderManager:fSceneRenderManager; // The render manager
		/** @private */
		public var engine:fEngine;
		/** @private */
		public var _orig_container:Sprite; // Backup reference
		
		// 2. Geometry and sizes
		
		/** @private */
		public var viewWidth:Number; // Viewport size
		/** @private */
		public var viewHeight:Number; // Viewport size
		/** @private */
		public var top:Number; // Highest Z in the scene
		/** @private */
		public var gridWidth:int; // Grid size in cells
		/** @private */
		public var gridDepth:int;
		/** @private */
		public var gridHeight:int;
		/** @private */
		public var gridSize:int; // Grid size ( in pixels )
		public var gridSizeHalf:int; //half of grid size
		/** @private */
		public var levelSize:int; // Vertical grid size ( along Z axis, in pixels )
		/** @private */
		public var sortCubeSize:int = fEngine.SORTCUBESIZE; // Sorting cube size
		
		// 3. zSort
		
		/** @private */
		public var grid:Array; // The grid
		/** @private */
		public var allUsedCells:Array;
		/** @private */
		//public var sortAreas:Array; // zSorting generates this. This array points to contiguous spaces sharing the same zIndex
		// It is used to find the proper zIndex for a cell
		/** @private */
		//public var sortAreasRTree:fRTree; // This tree is used to search sortAreas efficiently
		
		/** @private */
		//public var allStatic2D:Array; // This provides fast 2D searches in the static elements
		/** @private */
		//public var allStatic2DRTree:fRTree;
		
		/** @private */
		//public var allStatic3D:Array; // This provides fast 3D searches in the static elements
		/** @private */
		//public var allStatic3DRTree:fRTree;
		
		// 4. Resources
		
		/** @private */
		//public var resourceManager:fSceneResourceManager; // The resource manager stores all definitions loaded for this scene
		
		// 5.Status			
		
		/** @private */
		public var IAmBeingRendered:Boolean = false; // If this scene is actually being rendered
		private var _enabled:Boolean; // Is the scene enabled ?
		
		// Public properties
		
		/**
		 * Every Scene is automatically assigned and ID
		 */
		public var id:String;
		
		/**
		 * The camera currently in use, if any
		 */
		public var currentCamera:fCamera;
		
		/**
		 * Were this scene is drawn
		 */
		public var container:Sprite;
		
		/**
		 * An string indicating the scene's current status
		 */
		public var stat:String;
		
		/**
		 * Indicates if the scene is loaded and ready
		 */
		public var ready:Boolean;
		
		/**
		 * Scene width in pixels.
		 */
		public var width:Number;
		
		/**
		 * Scene depth in pixels
		 */
		public var depth:Number;
		
		/**
		 * Scene height in pixels
		 */
		public var height:Number;
		
		/**
		 * An array of all floors for fast loop access. For "id" access use the .all array
		 */
		// KBEN: 这个是存放地形的，使用树进行裁剪    
		public var floors:Array;
		
		/**
		 * An array of all walls for fast loop access. For "id" access use the .all array
		 */
		// KBEN: 这个是存放墙的，暂时不用了  
		//public var walls:Array;
		
		/**
		 * An array of all objects for fast loop access. For "id" access use the .all array
		 */
		// KBEN: 这个里面存放的是场景中不能被移动和删除的实体，例如地物，使用树进行裁剪      
		public var objects:Array;
		
		// KBEN: 可移动或者可以动态删除的放在这里，特效，掉落物，npc ，直接使用中心点进行裁剪       
		public var m_dynamicObjects:Vector.<fObject>;
		
		/**
		 * An array of all characters for fast loop access. For "id" access use the .all array
		 */
		// KBEN: 这个是存放总是走动的，玩家，直接使用中心点进行裁剪     
		public var characters:Array;
		
		/**
		 * An array of all empty sprites for fast loop access. For "id" access use the .all array
		 */
		// KBEN: 空精灵，在使用       
		public var emptySprites:Array;
		
		/**
		 * An array of all lights for fast loop access. For "id" access use the .all array
		 */
		public var lights:Array;
		
		/**
		 * An array of all elements for fast loop access. For "id" access use the .all array. Bullets are not here
		 */
		public var everything:Array;
		
		/**
		 * The global light for this scene. Use this property to change light properties such as intensity and color
		 */
		public var environmentLight:fGlobalLight;
		
		/**
		 * An array of all elements in this scene, use it with ID Strings. Bullets are not here
		 */
		public var all:Array;
		
		/**
		 * 场景 UI bottom 层，显示在地形上，不需要排序和裁剪，自己控制显示隐藏，场景不管
		 * */
		public var m_sceneUIBtmEffVec:Vector.<fObject>;
		
		/**
		 * 场景 UI top 层，显示在玩家上，不需要排序和裁剪，自己控制显示隐藏，场景不管
		 * */
		public var m_sceneUITopEffVec:Vector.<fObject>;
		/**
		 * 场景锦囊特效持续层，显示在玩家下，地形的上面，不需要排序和裁剪，自己控制显示隐藏，场景不管
		 * */
		public var m_sceneJinNangEffVec:Vector.<fObject>;
		
		/**
		 * The AI-related method are grouped inside this object, for easier access
		 */
		public var AI:fAiContainer;
		
		/**
		 * All the bullets currently active in the scene are here
		 */
		//public var bullets:Array;
		
		// Bullets go here instead of being deleted, so they can be reused
		//private var bulletPool:Array;
		
		// Al events
		/** @private */
		public var events:Array;
		
		// KBEN: 场景配置 
		public var m_sceneConfig:fSceneConfig;
		
		// KBEN:
		public var m_floorWidth:Number; // 单个 Floor 区域宽度，单位像素  
		public var m_floorDepth:Number; // 单个 Floor 区域高度，单位像素  
		
		public var m_floorXCnt:uint; // X 方向面板个数 
		public var m_floorYCnt:uint; // Y 方向面板个数 
		
		public var m_scenePixelXOff:int = 0; // 场景 X 方向偏移，主要用在战斗场景，需要偏移一定距离
		public var m_scenePixelYOff:int = 0; // 场景 Y 方向偏移，主要用在战斗场景，需要偏移一定距离
		
		public var m_scenePixelWidth:int = 0; // 场景图像真实的像素宽度，可能比 scene 的格子的宽度要小
		public var m_scenePixelHeight:int = 0; // 场景图像真实的像素高度，可能比 scene 的格子的高度要小
		
		public var m_thumbnails:BitmapData; // 保存场景缩略图，因为缩略图资源加载完成的时候，场景西那是还没有完成，因此有的数据不能使用
		//public var m_thumbnailsExtend:BitmapData;
		
		public var m_depthDirty:Boolean = false; // 主要用来进行一帧中只调用一次 depthSort 这个函数
		public var m_depthDirtySingle:Boolean = false; // 每一个移动的时候需要深度排序
		public var m_singleDirtyArr:Vector.<fRenderableElement>; // 每一个深度改变的时候都存放在这个列表，一次更新
		
		// Events
		
		/**
		 * An string describing the process of loading and processing and scene XML definition file.
		 * Events dispatched by the scene while loading containg this String as a description of what is happening
		 */
		public static const LOADINGDESCRIPTION:String = "Creating scene";
		
		/**
		 * The fScene.LOADPROGRESS constant defines the value of the
		 * <code>type</code> property of the event object for a <code>scenecloadprogress</code> event.
		 * The event is dispatched when the status of the scene changes during scene loading and processing.
		 * A listener to this event can then check the scene's status property to update a progress dialog
		 *
		 */
		public static const LOADPROGRESS:String = "scenecloadprogress";
		
		/**
		 * The fScene.LOADCOMPLETE constant defines the value of the
		 * <code>type</code> property of the event object for a <code>sceneloadcomplete</code> event.
		 * The event is dispatched when the scene finishes loading and processing and is ready to be used
		 *
		 */
		public static const LOADCOMPLETE:String = "sceneloadcomplete";
		
		// KBEN: 渲染引擎层，这个仅仅是场景， UI 单独一层 
		public var m_SceneLayer:Vector.<Sprite>;
		//public static const SLTerrainThumbnails:uint = 0;		// 地形缩略图层，不参与排序的
		//public static const SLTerrain:uint = 0;		// 地形层，不参与排序的
		//public static const SLSceneUIBtm:uint = SLTerrain + 1;	// 1 场景 UI 底层，在地形上，不排序，不裁剪，场景不管
		//public static const SLBlur:uint = SLSceneUIBtm + 1;		// 2 运行拖尾，战斗场景中用
		//public static const SLShadow:uint = SLBlur + 1;		// 3 阴影层，场景中所有的不接收阴影，除了地形
		//public static const SLShadow1:uint = 4;		// 4 测试裁剪的时候添加的，测试的时候需要添加上
		//public static const SLBuild:uint= SLShadow + 1;		// 地物,这个不用排序了
		//public static const SLObject:uint = SLBuild + 1;		// 掉落物，NPC，怪物，人物，需要排序的  
		//public static const SLEffect:uint = SLObject + 1;		// 特效
		//public static const SLSceneUITop:uint = SLEffect + 1;	// 场景 UI 顶层，在人物上，不排序，不裁剪，场景不管
		//public static const SLFog:uint = SLSceneUITop + 1;			// 场景中雾效果的层    
		//public static const SLCnt:uint = SLFog + 1;			// 总共层的数量 
		// KBEN:阻挡点信息，每一个 fScene 存储对这个的引用，是一个二位数组，第一维是行，第二位是列，减小内存，使用   Dictionary 不使用 Vector      
		//public var m_stopPointList:Vector.<Vector.<stopPoint>>;
		public var m_stopPointList:Dictionary;
		public var m_defaultStopPoint:stopPoint; // 默认的阻挡点，为了统一处理，不用总是 if else 判断    
		
		private var m_fogPlane:fFogPlane; // 存储雾的信息
		public var m_sceneType:uint; // 场景类型，普通场景还是战斗场景，战斗场景震动使用
		public var m_serverSceneID:uint; // 服务器场景 id
		public var m_filename:uint; // 客户端地图文件名字，没有扩展名字
		
		public var m_sortByBeingMove:Boolean = true; // 由于 being 移动是否需要重新排序，战斗场景不需要了，太多移动了
		public var m_path:String;
		
		public var m_timesOfShow:int = 0;
		public var m_disposed:Boolean = false;
		public var m_dicDebugInfo:Dictionary;
		
		/**
		 * Constructor. Don't call directly, use fEngine.createScene() instead
		 * @private
		 */
		//function fScene(engine:fEngine, container:Sprite, retriever:fEngineSceneRetriever, width:Number, height:Number, renderer:fEngineRenderEngine = null, p:fProfiler = null):void
		function fScene(engine:fEngine, container:Sprite, retriever:fEngineSceneRetriever, width:Number, height:Number, serversceneid:uint, renderer:fEngineRenderEngine = null):void
		{
			// Properties
			this.id = "fScene_" + (fScene.count++);
			this.m_serverSceneID = serversceneid;
			this.engine = engine;
			this._orig_container = container;
			this.container = container;
			this.environmentLight = null;
			this.gridSize = 64;
			this.levelSize = 64;
			this.top = 0;
			this.stat = "Loading XML";
			this.ready = false;
			this.viewWidth = width;
			this.viewHeight = height;
			
			// Internal arrays
			this.floors = new Array;
			//this.walls = new Array;
			this.objects = new Array;
			this.characters = new Array;
			this.emptySprites = new Array;
			this.events = new Array;
			this.lights = new Array;
			this.everything = new Array;
			this.all = new Array;
			//this.bullets = new Array;
			//this.bulletPool = new Array;
			m_dicDebugInfo = new Dictionary();
			// AI
			this.AI = new fAiContainer(this);
			
			// KBEN: 场景层 
			m_SceneLayer = new Vector.<Sprite>(EntityCValue.SLCnt, true);
			// 这个在 startRender 的时候再创建    
			//var k:uint = 0;
			//while (k < fScene.SLCnt)
			//{
			//	m_SceneLayer[k] = new Sprite();
			//	container.addChild(m_SceneLayer[k]);
			//	++k;
			//}
			
			// Render engine
			//this.renderEngine = renderer || (new fFlash9RenderEngine(this, container, m_SceneLayer));
			if (renderer)
			{
				this.renderEngine = renderer;
			}
			else
			{
				this.renderEngine = new fFlash9RenderEngine(this, container, m_SceneLayer);
			}
			this.renderEngine.setViewportSize(width, height);
			
			// The render manager decides which elements are inside the viewport and which elements are not
			this.renderManager = new fSceneRenderManager(this);
			this.renderManager.setViewportSize(width, height);
			
			// Start xml retrieve process
			this.initializer = new fSceneInitializer(this, retriever);
			
			// Profiler ?
			//this.prof = p;
			
			// KBEN:
			m_sceneConfig = new fSceneConfig();
			m_dynamicObjects = new Vector.<fObject>();
			m_sceneUIBtmEffVec = new Vector.<fObject>();
			m_sceneUITopEffVec = new Vector.<fObject>();
			m_sceneJinNangEffVec = new Vector.<fObject>();
			m_stopPointList = new Dictionary();
			
			var defObj:XML =  <item x="-1" y="-1" type="0"/>;
			m_defaultStopPoint = new stopPoint(defObj, this);
			m_defaultStopPoint.isStop = false; // 默认的阻挡点设置成 false ，不要忘了
			m_sceneType = EntityCValue.SCComon;
			m_singleDirtyArr = new Vector.<fRenderableElement>();
		}
		
		/**
		 * Starts initialization process
		 * @private
		 */
		public function initialize():void
		{
			this.initializer.start();
		}
		
		// Public methods
		
		/**
		 * This method changes the viewport's size. It is useful, for example, to adapt your scene to liquid layouts
		 *
		 * @param width New width for the viewport
		 * @param height New height for the viewport
		 */
		// KBEN: 舞台大小改变处理函数 
		public function setViewportSize(width:int, height:int):void
		{
			// 相机保存视口数据
			if (this.currentCamera)
			{
				this.currentCamera.setViewportSize(width, height);
			}
			
			// 清理格子中保存的裁剪的数据
			var k:uint = 0;
			var m:uint = 0;
			var cell:fCell;
			while (k < this.gridDepth) // 行  
			{
				m = 0;
				while (m < this.gridWidth) // 列 
				{
					cell = this.getCellAt(m, k, 0);
					// bug: 如果地图还没加载完成，这个时候产生了窗口大小改变的事件，这个时候格子是空的，就会获取不到格子
					if (cell)
					{
						cell.updateScrollRect();
						cell.clearClip();
						cell = null; // KBEN:手工设置空，主要是为了释放
					}
					++m;
				}
				
				++k;
			}
			
			this.renderManager.setViewportSize(width, height);
			this.renderEngine.setViewportSize(width, height);
			if (this.IAmBeingRendered && this.currentCamera)
			{
				// 视口大小改变的时候，需要重置场景渲染单元格子
				this.renderManager.curCell = null;
				this.renderManager.preCell = null;
				this.renderManager.processNewCellCamera(this.currentCamera);
				this.renderEngine.setCameraPosition(this.currentCamera);
			}
		}
		
		/**
		 * This Method is called to enable the scene. It will enable all controllers associated to the scene and its
		 * elements. The engine no longer calls this method when the scene is shown. Do it manually when needed.
		 *
		 * A typical use of manual enabling/disabling of scenes is pausing the game or showing a dialog box of any type.
		 *
		 * @see org.ffilmation.engine.core.fEngine#showScene
		 */
		public function enable():void
		{
			// Enable scene controller
			this._enabled = true;
			if (this.controller)
				this.controller.enable();
			
			// Enable controllers for all elements in the scene
			for (var i:int = 0; i < this.everything.length; i++)
				if (this.everything[i].controller != null)
					this.everything[i].controller.enable();
			//for (i = 0; i < this.bullets.length; i++)
			//	this.bullets[i].enable();
		}
		
		/**
		 * This Method is called to disable the scene. It will disable all controllers associated to the scene and its
		 * elements. The engine no longer calls this method when the scene is hidden. Do it manually when needed.
		 *
		 * A typical use of manual enabling/disabling of scenes is pausing the game or showing a dialog box of any type.
		 *
		 * @see org.ffilmation.engine.core.fEngine#hideScene
		 */
		public function disable():void
		{
			// Disable scene controller
			this._enabled = false;
			if (this.controller)
				this.controller.disable();
			
			// Disable  controllers for all elements in the scene
			for (var i:int = 0; i < this.everything.length; i++)
				if (this.everything[i].controller != null)
					this.everything[i].controller.disable();
			//for (i = 0; i < this.bullets.length; i++)
			//	this.bullets[i].disable();
		}
		
		/**
		 * Assigns a controller to this scene
		 * @param controller: any controller class that implements the fEngineSceneController interface
		 */
		public function set controller(controller:fEngineSceneController):void
		{
			if (this._controller != null)
				this._controller.disable();
			this._controller = controller;
			this._controller.assignScene(this);
		}
		
		/**
		 * Retrieves controller from this scene
		 * @return controller: the class that is currently controlling the fScene
		 */
		public function get controller():fEngineSceneController
		{
			return this._controller;
		}
		
		public function get fogPlane():fFogPlane
		{
			return m_fogPlane;
		}
		
		public function set fogPlane(value:fFogPlane):void
		{
			m_fogPlane = value;
		}
		
		/**
		 * This method sets the active camera for this scene. The camera position determines the viewable area of the scene
		 *
		 * @param camera The camera you want to be active
		 *
		 */
		public function setCamera(camera:fCamera):void
		{
			// Stop following old camera
			if (this.currentCamera)
			{
				this.currentCamera.removeEventListener(fElement.MOVE, this.cameraMoveListener);
				this.currentCamera.removeEventListener(fElement.NEWCELL, this.cameraNewCellListener);
			}
			
			// Follow new camera
			this.currentCamera = camera;
			this.currentCamera.addEventListener(fElement.MOVE, this.cameraMoveListener, false, 0, true);
			this.currentCamera.addEventListener(fElement.NEWCELL, this.cameraNewCellListener, false, 0, true);
			if (this.IAmBeingRendered)
				this.renderManager.processNewCellCamera(camera);
			this.followCamera(this.currentCamera);
		}
		
		/**
		 * Creates a new camera associated to the scene
		 *
		 * @return an fCamera object ready to move or make active using the setCamera() method
		 *
		 */
		public function createCamera():fCamera
		{
			//Return
			return new fCamera(this);
		}
		
		/**
		 * Creates a new light and adds it to the scene. You won't see the light until you call its
		 * render() or moveTo() methods
		 *
		 * @param idlight: The unique id that will identify the light
		 *
		 * @param x: Initial x coordinate for the light
		 *
		 * @param y: Initial x coordinate for the light
		 *
		 * @param z: Initial x coordinate for the light
		 *
		 * @param size: Radius of the sphere that identifies the light
		 *
		 * @param color: An string specifying the color of the light in HTML format, example: #ffeedd
		 *
		 * @param intensity: Intensity of the light goes from 0 to 100
		 *
		 * @param decay: From 0 to 100 marks the distance along the lights's radius from where intensity starrts to fade fades. A 0 decay defines a solid light
		 *
		 * @param bumpMapped: Determines if this light will be rendered with bumpmapping. Please note that for the bumpMapping to work in a given surface,
		 * the surface will need a bumpMap definition and bumpMapping must be enabled in the engine's global parameters
		 *
		 */ /*
		   public function createOmniLight(idlight:String, x:Number, y:Number, z:Number, size:Number, color:String, intensity:Number, decay:Number, bumpMapped:Boolean = false):fOmniLight
		   {
		   // Create
		   var definitionObject:XML =  <light id={idlight} type="omni" size={size} x={x} y={y} z={z} color={color} intensity={intensity} decay={decay} bump={bumpMapped}/>;
		   var nfLight:fOmniLight = new fOmniLight(definitionObject, this);
		
		   // Events
		   nfLight.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
		   nfLight.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
		   nfLight.addEventListener(fLight.RENDER, this.processNewCell, false, 0, true);
		   nfLight.addEventListener(fLight.RENDER, this.renderElement, false, 0, true);
		   nfLight.addEventListener(fLight.SIZECHANGE, this.processNewLightDimensions, false, 0, true);
		
		   // Add to lists
		   this.lights.push(nfLight);
		   this.everything.push(nfLight);
		   this.all[nfLight.id] = nfLight;
		
		   //Return
		   if (this.IAmBeingRendered)
		   nfLight.render();
		   return nfLight;
		   }
		 */
		
		/**
		 * Removes an omni light from the scene. This is not the same as hiding the light, this removes the element completely from the scene
		 *
		 * @param light The light to be removed
		 */ /*
		   public function removeOmniLight(light:fOmniLight):void
		   {
		   // Remove from array
		   if (this.lights && this.lights.indexOf(light) >= 0)
		   {
		   this.lights.splice(this.lights.indexOf(light), 1);
		   this.everything.splice(this.everything.indexOf(light), 1);
		   }
		
		   // Hide light from elements
		   var cell:fCell = light.cell;
		   var nEl:Number = light.nElements;
		   for (var i2:Number = 0; i2 < nEl; i2++)
		   this.renderEngine.lightOut(light.elementsV[i2].obj, light);
		   light.scene = null;
		
		   nEl = this.characters.length;
		   for (i2 = 0; i2 < nEl; i2++)
		   this.renderEngine.lightOut(this.characters[i2], light);
		   this.all[light.id] = null;
		
		   // Events
		   light.removeEventListener(fElement.NEWCELL, this.processNewCell);
		   light.removeEventListener(fElement.MOVE, this.renderElement);
		   light.removeEventListener(fLight.RENDER, this.processNewCell);
		   light.removeEventListener(fLight.RENDER, this.renderElement);
		   light.removeEventListener(fLight.SIZECHANGE, this.processNewLightDimensions);
		
		   // This light may be in some character cache
		   light.removed = true;
		   }
		 */
		
		/**
		 *	Creates a new character an adds it to the scene
		 *
		 * @param idchar: The unique id that will identify the character
		 *
		 * @param def: Definition id. Must match a definition in some of the definition XMLs included in the scene
		 *
		 * @param x: Initial x coordinate for the character
		 *
		 * @param y: Initial x coordinate for the character
		 *
		 * @param z: Initial x coordinate for the character
		 *
		 * @param layer: 有些 npc 需要放在地物一层，不排序，永远在排序层下面
		 *
		 * @returns The newly created character, or null if the coordinates not allowed (outside bounds)
		 *
		 **/
		//public function createCharacter(idchar:String, def:String, x:Number, y:Number, z:Number, orientation:Number):fCharacter
		public function createCharacter(charType:uint, def:String, x:Number, y:Number, z:Number, orientation:Number, layer:uint = EntityCValue.SLObject):BeingEntity
		{
			// Ensure coordinates are inside the scene
			var c:fCell = this.translateToCell(x, y, z);
			if (c == null)
			{
				var str:String = "无效的坐标(" + x + "," + y + ")。实际地图(像素)大小是(" + this.widthpx() + "," + this.heightpx() + ")";
				DebugBox.info(str);
				return null;
			}
			var dist:fFloor = getFloorAtByPos(x, y);
			if (dist == null)
			{
				return null;
			}
			
			var idchar:String = fUtil.elementID(this.engine.m_context, charType);
			
			// Create
			var definitionObject:XML = <character id={idchar} definition={def} x={x} y={y} z={z} orientation={orientation}/>;
			//var nCharacter:fCharacter = new fCharacter(definitionObject, this);
			//var nCharacter:Player = new Player(definitionObject, this);
			var nCharacter:BeingEntity = new this.engine.m_context.m_typeReg.m_classes[charType](definitionObject, this);
			nCharacter.cell = c;
			nCharacter.m_district = dist;
			nCharacter.m_district.addCharacter(nCharacter.id);
			nCharacter.setDepth(c.zIndex);
			nCharacter.layer = layer;
			
			// Events
			nCharacter.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
			nCharacter.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
			
			// Add to lists
			this.characters.push(nCharacter);
			this.everything.push(nCharacter);
			this.all[nCharacter.id] = nCharacter;
			if (this.IAmBeingRendered)
			{
				this.addElementToRenderEngine(nCharacter);
				this.renderManager.processNewCellCharacter(nCharacter);
				this.render();
			}
			
			//Return
			return nCharacter;
		}
		
		/**
		 * Removes a character from the scene. This is not the same as hiding the character, this removes the element completely from the scene
		 *
		 * @param char The character to be removed
		 * bDispose - true 销毁char, false - char脱离场景
		 */
		public function removeCharacter(char:fCharacter, bDispose:Boolean = true):void
		{
			// Remove from array
			if (this.characters && this.characters.indexOf(char) >= 0)
			{
				this.characters.splice(this.characters.indexOf(char), 1);
				this.everything.splice(this.everything.indexOf(char), 1);
				this.all[char.id] = null;
			}
			
			// Hide
			if (bDispose)
			{
				char.hide();
			}
			
			// Events
			char.removeEventListener(fElement.NEWCELL, this.processNewCell);
			char.removeEventListener(fElement.MOVE, this.renderElement);
			
			// Remove from render engine
			this.removeElementFromRenderEngine(char);
			// bug: 有时候卡的时候,时间间隔会很大,导致计算的 x 值很大或者很小,已经移出地图了
			if (char.m_district)
			{
				char.m_district.clearCharacter(char.id);
				char.m_district = null;
			}
			if (bDispose)
			{
				char.dispose();
			}
			else
			{
				char.onRemoveFormScene();
				char.scene = null;
			}
		}
		
		public function addCharacter(nCharacter:fCharacter):void
		{
			var c:fCell = this.translateToCell(0, 0, 0);
			if (c == null)
			{
				return;
			}
			var dist:fFloor = getFloorAtByPos(0, 0);
			if (dist == null)
			{
				return;
			}
			
			nCharacter.scene = this;
			nCharacter.cell = c;
			nCharacter.m_district = dist;
			nCharacter.m_district.addCharacter(nCharacter.id);
			nCharacter.setDepth(c.zIndex);
			
			// Events
			nCharacter.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
			nCharacter.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
			
			// Add to lists
			this.characters.push(nCharacter);
			this.everything.push(nCharacter);
			this.all[nCharacter.id] = nCharacter;
			if (this.IAmBeingRendered)
			{
				this.addElementToRenderEngine(nCharacter);
				this.renderManager.processNewCellCharacter(nCharacter);
				this.render();
			}
		}
		
		// 掉落物  
		public function createFObject(fobjType:uint, def:String, x:Number, y:Number, z:Number, orientation:Number):fSceneObject
		{
			// Ensure coordinates are inside the scene
			var c:fCell = this.translateToCell(x, y, z);
			if (c == null)
			{
				return null;
			}
			var dist:fFloor = getFloorAtByPos(x, y);
			if (dist == null)
			{
				return null;
			}
			
			var idchar:String = fUtil.elementID(this.engine.m_context, fobjType);
			
			// Create
			var definitionObject:XML =  <fobject id={idchar} definition={def} x={x} y={y} z={z} orientation={orientation}/>;
			var fobject:fSceneObject = new this.engine.m_context.m_typeReg.m_classes[fobjType](definitionObject, this);
			fobject.cell = c;
			fobject.m_district = dist;
			fobject.m_district.addDynamic(fobject.id);
			fobject.setDepth(c.zIndex);
			
			// Events
			fobject.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
			fobject.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
			
			// Add to lists
			this.m_dynamicObjects.push(fobject);
			this.everything.push(fobject);
			this.all[fobject.id] = fobject;
			if (this.IAmBeingRendered)
			{
				this.addElementToRenderEngine(fobject);
				this.renderManager.processNewCellFObject(fobject);
				this.render();
			}
			
			//Return
			return fobject;
		}
		
		/**
		 * Removes a character from the scene. This is not the same as hiding the character, this removes the element completely from the scene
		 *
		 * @param char The character to be removed
		 * bDispose - true 销毁char, false - char脱离场景
		 */
		public function removeFObject(fobj:fSceneObject, bDispose:Boolean = true):void
		{
			// Remove from array
			if (this.m_dynamicObjects && this.m_dynamicObjects.indexOf(fobj) >= 0)
			{
				this.m_dynamicObjects.splice(this.m_dynamicObjects.indexOf(fobj), 1);
				this.everything.splice(this.everything.indexOf(fobj), 1);
				this.all[fobj.id] = null;
			}
			
			// Hide
			if (bDispose)
			{
				fobj.hide();
			}
			
			// Events
			fobj.removeEventListener(fElement.NEWCELL, this.processNewCell);
			fobj.removeEventListener(fElement.MOVE, this.renderElement);
			
			// Remove from render engine
			this.removeElementFromRenderEngine(fobj);
			if (fobj.m_district)
			{
				fobj.m_district.clearDynamic(fobj.id);
				fobj.m_district = null;
			}
			if (bDispose)
			{
				fobj.dispose();
			}
			else
			{
				fobj.scene = null;
			}
		}
		
		/**
		 *	Creates a new empty sprite an adds it to the scene
		 *
		 * @param idchar: The unique id that will identify the character
		 *
		 * @param x: Initial x coordinate for the character
		 *
		 * @param y: Initial x coordinate for the character
		 *
		 * @param z: Initial x coordinate for the character
		 *
		 * @returns The newly created empty Sprite
		 *
		 **/
		public function createEmptySprite(emptySpriteclass:Class, idspr:String, x:Number, y:Number, z:Number):fEmptySprite
		{
			var c:fCell = this.translateToCell(x, y, z);
			if (c == null)
			{
				return null;
			}
			var dist:fFloor = getFloorAtByPos(x, y);
			if (dist == null)
			{
				return null;
			}
			
			// Create
			var definitionObject:XML =  <emptySprite id={idspr} x={x} y={y} z={z}/>;
			var nEmptySprite:fEmptySprite = new emptySpriteclass(definitionObject, this);
			nEmptySprite.cell = this.translateToCell(x, y, z);
			nEmptySprite.m_district = dist;
			nEmptySprite.m_district.addEmptySprite(nEmptySprite.id);
			nEmptySprite.updateDepth();
			
			// Events
			nEmptySprite.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
			nEmptySprite.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
			
			// Add to lists
			this.emptySprites.push(nEmptySprite);
			this.everything.push(nEmptySprite);
			this.all[nEmptySprite.id] = nEmptySprite;
			if (this.IAmBeingRendered)
			{
				this.addElementToRenderEngine(nEmptySprite);
				this.renderManager.processNewCellEmptySprite(nEmptySprite);
			}
			
			//Return
			return nEmptySprite;
		}
		
		/**
		 * Removes an empty sprite from the scene. This is not the same as hiding it, this removes the element completely from the scene
		 *
		 * @param spr The emptySprite to be removed
		 */
		public function removeEmptySprite(spr:fEmptySprite, bDispose:Boolean = true):void
		{
			// Remove from arraya
			if (this.emptySprites && this.emptySprites.indexOf(spr) >= 0)
			{
				this.emptySprites.splice(this.emptySprites.indexOf(spr), 1);
				this.everything.splice(this.everything.indexOf(spr), 1);
				this.all[spr.id] = null;
			}
			
			// Hide
			spr.hide()
			
			// Events
			spr.removeEventListener(fElement.NEWCELL, this.processNewCell);
			spr.removeEventListener(fElement.MOVE, this.renderElement);
			
			// Remove from render engine
			this.removeElementFromRenderEngine(spr);
			if (spr.m_district)
			{
				spr.m_district.clearEmptySprite(spr.id);
				spr.m_district = null;
			}
			if (bDispose)
			{
				spr.dispose();
			}
			else
			{
				spr.scene = null;
			}
		}
		
		/**
		 * Creates a new bullet and adds it to the scene. Note that bullets use their own render system. The bulletRenderer interface allows
		 * you to have complex things such as trails. If it was integrated with the standard renderer, your bullets would have to be standard
		 * Sprites, and I dind't like that.
		 *
		 * <p><b>Note to developers:</b> bullets are reused. Creating new objects is slow, and depending on your game you could have a lot
		 * being created and destroyed. The engine uses an object pool to reuse "dead" bullets and minimize the amount of new() calls. This
		 * is transparent to you but I think this information can help tracking weird bugs</p>
		 *
		 * @param x Start position of the bullet
		 * @param y Start position of the bullet
		 * @param z Start position of the bullet
		 * @param speedx Speed of bullet
		 * @param speedy Speed of bullet
		 * @param speedz Speed of bullet
		 * @param renderer The renderer that will be drawing this bullet. In order to increase performace, you should't create a new
		 * renderer instance for each bullet: pass the same renderer to all bullets that look the same.
		 *
		 */ /*
		   public function createBullet(x:Number, y:Number, z:Number, speedx:Number, speedy:Number, speedz:Number, renderer:fEngineBulletRenderer):fBullet
		   {
		   // Is there an available bullet or a new one is needed ?
		   var b:fBullet;
		   if (this.bulletPool.length > 0)
		   {
		   b = this.bulletPool.pop();
		   b.moveTo(x, y, z);
		   b.show();
		   }
		   else
		   {
		   b = new fBullet(this);
		   b.moveTo(x, y, z);
		   if (this.IAmBeingRendered)
		   this.addElementToRenderEngine(b);
		   }
		
		   // Events
		   b.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
		   b.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
		   b.addEventListener(fBullet.SHOT, fBulletSceneLogic.processShot, false, 0, true);
		
		   // Properties
		   b.moveTo(x, y, z);
		   b.speedx = speedx;
		   b.speedy = speedy;
		   b.speedz = speedz;
		
		   // Init renderer
		   b.customData.bulletRenderer = renderer;
		   if (b.container)
		   b.customData.bulletRenderer.init(b);
		
		   // Add to lists
		   this.bullets.push(b);
		
		   // Enable
		   if (this._enabled)
		   b.enable();
		
		   // Return
		   return b;
		   }
		 */
		
		/**
		 * Removes a bullet from the scene. Bullets are automatically removed when they hit something,
		 * but you If you can't wait for them to be delete, you can do it manually.
		 * @param bullet The fBullet to be removed.
		 */ /*
		   public function removeBullet(bullet:fBullet):void
		   {
		   // Events
		   bullet.removeEventListener(fElement.NEWCELL, this.processNewCell);
		   bullet.removeEventListener(fElement.MOVE, this.renderElement);
		   bullet.removeEventListener(fBullet.SHOT, fBulletSceneLogic.processShot);
		
		   // Hide
		   bullet.disable();
		   bullet.customData.bulletRenderer.clear(bullet);
		   bullet.hide();
		
		   // Back to pool
		   //this.bullets.splice(this.bullets.indexOf(bullet), 1);
		   //this.bulletPool.push(bullet);
		   }
		 */
		// KBEN: 创建特效，这个是创建加入场景的特效，例如飞行特效      
		//public function createEffect(ideff:String, def:String, startx:Number, starty:Number, startz:Number, destx:Number, desty:Number, destz:Number, speedx:Number, speedy:Number, speedz:Number):EffectEntity
		// speed 速度大小 
		public function createEffect(ideff:String, def:String, startx:Number, starty:Number, startz:Number, destx:Number, desty:Number, destz:Number, speed:Number):EffectEntity
		{
			// Ensure coordinates are inside the scene
			var c:fCell = this.translateToCell(startx, starty, startz);
			if (c == null)
			{
				return null;
			}
			var dist:fFloor = getFloorAtByPos(startx, starty);
			if (dist == null)
			{
				return null;
			}
			
			// Create
			var definitionObject:XML =  <effect id={ideff} definition={def} x={startx} y={starty} z={startz}/>;
			// KBEN: 特效可能每一个的定义是不一样的，因此不用特效池了    
			var nEffect:EffectEntity = new EffectEntity(definitionObject, this);
			nEffect.cell = c;
			nEffect.m_district = dist;
			nEffect.m_district.addDynamic(nEffect.id);
			nEffect.setDepth(c.zIndex);
			
			// Events
			nEffect.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
			nEffect.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
			
			// KBEN: 属性设置 
			//nEffect.vx = speedx;
			//nEffect.vy = speedy;
			//nEffect.vz = speedz;
			nEffect.vel = speed;
			
			//nEffect.dx = destx;
			//nEffect.dy = desty;
			//nEffect.dz = destz;
			nEffect.startTof(startx, starty, startz, destx, desty, destz);
			
			//nEffect.addEffectPath(new fPoint3d(destx, desty, destz));
			// KBEN: bug 这个会更新一些列位置，但是这个时候有些变量还没有建立 
			//nEffect.moveTo(startx, starty, startz);
			
			// Add to lists
			this.m_dynamicObjects.push(nEffect);
			this.everything.push(nEffect);
			this.all[nEffect.id] = nEffect;
			if (this.IAmBeingRendered)
			{
				this.addElementToRenderEngine(nEffect);
				this.renderManager.processNewCellEffect(nEffect);
				this.render();
			}
			
			// KBEN: 这个尽量放到这   
			// nEffect.moveTo(startx, starty, startz);
			
			//Return
			return nEffect;
		}
		
		// KBEN: 创建不加入场景特效，例如链接特效，startx 坐标是相对坐标，相对于链接父对象，包括场景 UI 特效
		public function createEffectNIScene(ideff:String, def:String, startx:Number, starty:Number, startz:Number, efftype:uint = 2, bshow:Boolean = true):EffectEntity
		{
			// Create
			var definitionObject:XML =  <effect id={ideff} definition={def} x={startx} y={starty} z={startz}/>;
			// KBEN: 特效可能每一个的定义是不一样的，因此不用特效池了    
			var nEffect:EffectEntity = new EffectEntity(definitionObject, this);
			
			if (this.IAmBeingRendered)
			{
				// Init
				nEffect.container = this.renderEngine.initRenderFor(nEffect);
				
				// This happens only if the render Engine returns a container for every element. 
				if (nEffect.container)
				{
					nEffect.container.fElementId = nEffect.id;
					nEffect.container.fElement = nEffect;
				}
				
				// KBEN: 现在基本不存在 MovieClip ，仅仅是为了兼容之前的   
				// This can be null, depending on the render engine
				nEffect.flashClip = this.renderEngine.getAssetFor(nEffect);
				
				// Listen to show and hide events
				nEffect.addEventListener(fRenderableElement.ENABLE, this.enableListener, false, 0, true);
				nEffect.addEventListener(fRenderableElement.DISABLE, this.disableListener, false, 0, true);
				
				// Elements default to Mouse-disabled
				nEffect.disableMouseEvents();
				
				// 场景 UI 特效需要特殊处理
				if (efftype == EntityCValue.EFFSceneBtm || efftype == EntityCValue.EFFSceneTop)
				{
					// 移动需要就行处理
					nEffect.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
					this.everything.push(nEffect);
					this.all[nEffect.id] = nEffect;
					nEffect.type = efftype;
					
					if (efftype == EntityCValue.EFFSceneBtm)
					{
						m_sceneUIBtmEffVec.push(nEffect);
						nEffect.customData.flash9Renderer.changeContainerParent(m_SceneLayer[EntityCValue.SLSceneUIBtm]);
					}
					else
					{
						m_sceneUITopEffVec.push(nEffect);
						nEffect.customData.flash9Renderer.changeContainerParent(m_SceneLayer[EntityCValue.SLSceneUITop]);
					}
					
					if (bshow)
					{
						// 逻辑数据可见
						nEffect.show();
						nEffect.isVisibleNow = true;
						// 显示可见
						renderEngine.showElement(nEffect);
					}
				}
				else if (efftype == EntityCValue.EFFJinNang) // 锦囊场景持续特效
				{
					// 移动需要就行处理
					nEffect.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
					this.everything.push(nEffect);
					this.all[nEffect.id] = nEffect;
					nEffect.type = efftype;
					
					m_sceneJinNangEffVec.push(nEffect);
					// 更改父节点
					nEffect.customData.flash9Renderer.changeContainerParent(m_SceneLayer[EntityCValue.SLJinNang]);
					if (bshow)
					{
						// 逻辑数据可见
						nEffect.show();
						nEffect.isVisibleNow = true;
						// 显示可见
						renderEngine.showElement(nEffect);
					}
				}
			}
			
			return nEffect;
		}
		
		private function onremove(e:Event):void
		{
			trace("error");
		}
		
		// KBEN: 移除场景中特效，例如飞行特效      
		public function removeEffect(effect:EffectEntity):void
		{
			// Remove from array
			if (this.m_dynamicObjects && this.m_dynamicObjects.indexOf(effect) >= 0)
			{
				this.m_dynamicObjects.splice(this.m_dynamicObjects.indexOf(effect), 1);
				this.everything.splice(this.everything.indexOf(effect), 1);
				this.all[effect.id] = null;
			}
			
			// Hide
			effect.hide();
			
			// Events
			effect.removeEventListener(fElement.NEWCELL, this.processNewCell);
			effect.removeEventListener(fElement.MOVE, this.renderElement);
			
			// Remove from render engine
			this.removeElementFromRenderEngine(effect);
			effect.dispose();
			
			effect.scene = null;
			if (effect.m_district)
			{
				effect.m_district.clearDynamic(effect.id);
				effect.m_district = null;
			}
		}
		
		// KBEN: 移除非场景特效，例如链接特效    
		public function removeEffectNIScene(effect:EffectEntity):void
		{
			// Hide
			effect.hide();
			effect.isVisibleNow = false;
			
			// Remove from render engine
			// KBEN: 在这里面释放渲染资源    
			var r:fFlash9ElementRenderer = effect.customData.flash9Renderer;
			if (r == null)
			{				
				var str:String = "fScene::removeEffectNIScene dispose="+effect.isDisposed+fUtil.getStackInfo("");
				DebugBox.sendToDataBase(str);
			}
			// bug: 链接特效在 BeingEntity 隐藏的时候会调用 hideRender 从而隐藏 EffectEntity，而在移除的时候如果再次调用这个就会出现 this.containerParent.removeChild(this.container); 中 this.container 的 parent 为 null 的现象  
			if (r.screenVisible == true)
			{
				this.renderEngine.hideElement(effect);
			}
			this.renderEngine.stopRenderFor(effect);
			if (effect.container)
			{
				effect.container.fElementId = null;
				effect.container.fElement = null;
			}
			effect.container = null;
			effect.flashClip = null;
			
			// Stop listening to show and hide events
			effect.removeEventListener(fRenderableElement.ENABLE, this.enableListener);
			effect.removeEventListener(fRenderableElement.DISABLE, this.disableListener);
			
			// 场景 UI 特效需要特殊处理,玩家身上的链接特效是不需要加入到场景中的数据结构中的
			if (effect.type == EntityCValue.EFFSceneBtm || effect.type == EntityCValue.EFFSceneTop || effect.type == EntityCValue.EFFJinNang)
			{
				// 移动需要就行处理
				var idx:uint = 0;
				effect.removeEventListener(fElement.MOVE, this.renderElement);
				idx = this.everything.indexOf(effect);
				if (idx != -1)
				{
					this.everything.splice(idx, 1);
				}
				idx = this.all.indexOf(effect);
				if (idx != -1)
				{
					this.all.splice(idx, 1);
				}
				
				if (effect.type == EntityCValue.EFFSceneBtm)
				{
					idx = this.m_sceneUIBtmEffVec.indexOf(effect);
					if (idx != -1)
					{
						m_sceneUIBtmEffVec.splice(idx, 1);
					}
				}
				else if (effect.type == EntityCValue.EFFSceneTop)
				{
					idx = this.m_sceneUITopEffVec.indexOf(effect);
					if (idx != -1)
					{
						m_sceneUITopEffVec.splice(idx, 1);
					}
				}
				else if (effect.type == EntityCValue.EFFJinNang)
				{
					idx = this.m_sceneJinNangEffVec.indexOf(effect);
					if (idx != -1)
					{
						m_sceneJinNangEffVec.splice(idx, 1);
					}
				}
			}
			
			effect.dispose();
		}
		
		// 雾创建    
		public function createFog():fFogPlane
		{
			var def:XML =   <fog id="f1"/>;
			m_fogPlane = new fFogPlane(def, this);
			
			if (this.IAmBeingRendered)
			{
				// Init
				m_fogPlane.container = this.renderEngine.initRenderFor(m_fogPlane);
				
				// This happens only if the render Engine returns a container for every element. 
				if (m_fogPlane.container)
				{
					m_fogPlane.container.fElementId = m_fogPlane.id;
					m_fogPlane.container.fElement = m_fogPlane;
				}
				
				// Listen to show and hide events
				m_fogPlane.addEventListener(fRenderableElement.ENABLE, this.enableListener, false, 0, true);
				m_fogPlane.addEventListener(fRenderableElement.DISABLE, this.disableListener, false, 0, true);
				
				// Elements default to Mouse-disabled
				m_fogPlane.disableMouseEvents();
				
				// 直接显示出来    
				this.renderEngine.showElement(m_fogPlane);
			}
			
			return m_fogPlane;
		}
		
		// 雾销毁   
		public function removeFog(fogplane:fFogPlane):void
		{
			// Hide
			fogplane.hide();
			fogplane.isVisibleNow = false;
			
			// Remove from render engine
			// KBEN: 在这里面释放渲染资源    
			var r:fFlash9ElementRenderer = fogplane.customData.flash9Renderer;
			if (r)
			{
				if (r.screenVisible)
				{
					this.renderEngine.hideElement(fogplane);
				}
				
				this.renderEngine.stopRenderFor(fogplane);
			}
			if (fogplane.container)
			{
				fogplane.container.fElementId = null;
				fogplane.container.fElement = null;
			}
			fogplane.container = null;
			
			// Stop listening to show and hide events
			fogplane.removeEventListener(fRenderableElement.ENABLE, this.enableListener);
			fogplane.removeEventListener(fRenderableElement.DISABLE, this.disableListener);
			fogplane.dispose();
			fogplane = null;
		}
		
		/**
		 * This method translates scene 3D coordinates to 2D coordinates relative to the Sprite containing the scene
		 *
		 * @param x x-axis coordinate
		 * @param y y-axis coordinate
		 * @param z z-axis coordinate
		 *
		 * @return A Point in this scene's container Sprite
		 */
		public function translate3DCoordsTo2DCoords(x:Number, y:Number, z:Number):Point
		{
			return fScene.translateCoords(x, y, z);
		}
		
		/**
		 * This method translates scene 3D coordinates to 2D coordinates relative to the Stage
		 *
		 * @param x x-axis coordinate
		 * @param y y-axis coordinate
		 * @param z z-axis coordinate
		 *
		 * @return A Coordinate in the Stage
		 */
		public function translate3DCoordsToStageCoords(x:Number, y:Number, z:Number):Point
		{
			//Get offset of camera
			var rect:Rectangle = this.container.scrollRect;
			
			// Get point
			var r:Point = fScene.translateCoords(x, y, z);
			
			// Translate
			r.x -= rect.x;
			r.y -= rect.y;
			
			return r;
		}
		
		/**
		 * This method translates Stage coordinates to scene coordinates. Useful to map mouse events into game events
		 *
		 * @example You can call it like
		 *
		 * <listing version="3.0">
		 *  function mouseClick(evt:MouseEvent) {
		 *    var coords:Point = this.scene.translateStageCoordsTo3DCoords(evt.stageX, evt.stageY)
		 *    this.hero.character.teleportTo(coords.x,coords.y, this.hero.character.z)
		 *   }
		 * </listing>
		 *
		 * @param x x-axis coordinate
		 * @param y y-axis coordinate
		 *
		 * @return A Point in the scene's coordinate system. Please note that a Z value is not returned as It can't be calculated from a 2D input.
		 * The returned x and y correspond to z=0 in the game's coordinate system.
		 */
		public function translateStageCoordsTo3DCoords(x:Number, y:Number):Point
		{
			//get offset of camera
			var rect:Rectangle = this.container.scrollRect;
			var xx:Number = x + rect.x;
			var yy:Number = y + rect.y;
			
			return fScene.translateCoordsInverse(xx, yy);
		}
		
		/**
		 * This method returns the element under a Stage coordinate, and a 3D translation of the 2D coordinates passed as input.
		 * To achieve this it finds which visible elements are under the input pixel, ignoring the engine's internal coordinates.
		 * Now you can find out what did you click and which point of that element did you click.
		 *
		 * @param x Stage horizontal coordinate
		 * @param y Stage vertical coordinate
		 *
		 * @return An array of objects storing both the element under that point and a 3d coordinate corresponding to the 2d Point. This method returns null
		 * if the coordinate is not occupied by any element.
		 * Why an Array an not a single element ? Because you may want to search the Array for the element that better suits your intentions: for
		 * example if you use it to walk around the scene, you will want to ignore trees to reach the floor behind. If you are shooting
		 * people, you will want to ignore floors and look for objects and characters to target at.
		 *
		 * @see org.ffilmation.engine.datatypes.fCoordinateOccupant
		 */
		public function translateStageCoordsToElements(x:Number, y:Number):Array
		{
			// This must be passed to the renderer because we have no idea how things are drawn
			if (this.IAmBeingRendered)
				return this.renderEngine.translateStageCoordsToElements(x, y);
			else
				return null;
		}
		
		/**
		 * Use this method to completely rerender the scene. However, under normal circunstances there shouldn't be a need to call this manually
		 */
		public function render():void
		{
			// bug 不再有灯光
			return;
			// Render global light
			this.environmentLight.render();
			
			// Render dynamic lights
			var ll:int = this.lights.length;
			for (var i:int = 0; i < ll; i++)
				this.lights[i].render();
		}
		
		/**
		 * Normally you don't need to call this method manually. When an scene is shown, this method is called to initialize the render engine
		 * for this scene ( this involves creating all the Sprites ). This may take a couple of seconds.<br>
		 * Under special circunstances, however, you may want to call this method manually at some point before showing the scene. This is useful is you want
		 * the graphic assets to exist before the scene is shown ( to attach Mouse Events for example ).
		 */
		public function startRendering():void
		{
			if (this.IAmBeingRendered)
				return;
			
			/*if (this.currentCamera.m_bInit)
			{
				if (renderManager.curCell!=currentCamera.cell)
				{
					//this.renderManager.curCell = currentCamera.cell;
					renderManager.processNewCellCamera(currentCamera);
				}
			}*/
			// Set flag
			this.IAmBeingRendered = true;
			// 开始渲染的时候才创建内部场景图      
			var k:uint = 0;
			while (k < EntityCValue.SLCnt)
			{
				m_SceneLayer[k] = new fSceneChildLayer();
				(m_SceneLayer[k] as fSceneChildLayer).m_layNo = k;
				//m_SceneLayer[k].addEventListener(Event.REMOVED_FROM_STAGE, layremove);
				//m_SceneLayer[k].addEventListener(Event.ADDED_TO_STAGE, layadd);
				container.addChild(m_SceneLayer[k]);
				++k;
			}
			
			//m_SceneLayer[SLTerrain].visible = false;
			
			// 设置缩略图
			//if(m_thumbnails)
			//{
			//addTerThumbnails(m_thumbnails);
			//addTerThumbnails();
			//}
			
			// Init render engine
			this.renderEngine.initialize();
			this.renderEngine.SceneLayer = m_SceneLayer;
			
			// Init render manager
			this.renderManager.initialize();
			
			// Init render for all elements
			var jl:int = this.floors.length
			for (var j:int = 0; j < jl; j++)
				this.addElementToRenderEngine(this.floors[j]);
			//jl = this.walls.length;
			//for (j = 0; j < jl; j++)
			//this.addElementToRenderEngine(this.walls[j]);
			jl = this.objects.length
			for (j = 0; j < jl; j++)
				this.addElementToRenderEngine(this.objects[j]);
			jl = this.characters.length
			for (j = 0; j < jl; j++)
				this.addElementToRenderEngine(this.characters[j]);
			jl = this.emptySprites.length
			for (j = 0; j < jl; j++)
				this.addElementToRenderEngine(this.emptySprites[j]);
			//jl = this.bullets.length
			//for (j = 0; j < jl; j++)
			//{
			//	this.addElementToRenderEngine(this.bullets[j]);
			//	this.bullets[j].customData.bulletRenderer.init();
			//}
			jl = this.m_dynamicObjects.length
			for (j = 0; j < jl; j++)
				this.addElementToRenderEngine(this.m_dynamicObjects[j]);
			
			// KBEN: 添加雾到场景     
			if (this.m_sceneConfig.fogOpened)
			{
				//createFog();
				this.addElementToRenderEngineNoClip(m_fogPlane, false, true, true);
			}
			
			
			
			// Render scene
			this.render();
		
			// Update camera if any
			// KBEN: 上层逻辑会调用移动摄像机的，那个时候再更新
			//if (this.currentCamera)
			//	this.renderManager.processNewCellCamera(this.currentCamera);
		
			//drawFightGrid(new Point(10, 10), new Point(140, 140));
			//drawStopPt();
			//drawFloor();
		}
		
		//private function layadd(e:Event):void 
		//{
		//removeEventListener(Event.ADDED_TO_STAGE, layadd);
		//
		//}
		//
		//private function layremove(e:Event):void 
		//{
		//removeEventListener(Event.REMOVED_FROM_STAGE, layremove);
		//
		//}
		
		// KBEN: 添加可渲染元素，不进行裁剪 
		private function addElementToRenderEngineNoClip(element:fRenderableElement, bshowListen:Boolean = false, mouseListen:Boolean = true, bshow:Boolean = true):void
		{
			// Init
			element.container = this.renderEngine.initRenderFor(element);
			
			// This happens only if the render Engine returns a container for every element. 
			if (element.container)
			{
				element.container.fElementId = element.id;
				element.container.fElement = element;
			}
			
			// KBEN: 现在基本不存在 MovieClip ，仅仅是为了兼容之前的   
			// This can be null, depending on the render engine
			element.flashClip = this.renderEngine.getAssetFor(element);
			
			// Listen to show and hide events
			if (bshowListen)
			{
				element.addEventListener(fRenderableElement.SHOW, this.renderManager.showListener, false, 0, true);
				element.addEventListener(fRenderableElement.HIDE, this.renderManager.hideListener, false, 0, true);
			}
			if (mouseListen)
			{
				element.addEventListener(fRenderableElement.ENABLE, this.enableListener, false, 0, true);
				element.addEventListener(fRenderableElement.DISABLE, this.disableListener, false, 0, true);
			}
			
			// Add to render manager
			if (bshow)
			{
				this.renderEngine.showElement(element);
			}
			
			// Elements default to Mouse-disabled
			element.disableMouseEvents();
		}
		
		// KBEN: 移除非裁剪元素   
		private function removeElementFromRenderEngineNoClip(element:fRenderableElement, bshowListen:Boolean = false, mouseListen:Boolean = true, destroyingScene:Boolean = false):void
		{
			// bug: 如果渲染内容已经销毁，数据逻辑继续运行的时候，如果再次调用这个函数就会宕机，因此检查一下  
			if (!element.container)
			{
				return;
			}
			
			this.renderManager.removedItem(element, destroyingScene);
			this.renderEngine.stopRenderFor(element);
			if (element.container)
			{
				element.container.fElementId = null;
				element.container.fElement = null;
			}
			element.container = null;
			element.flashClip = null;
			
			// Stop listening to show and hide events
			if (bshowListen)
			{
				element.removeEventListener(fRenderableElement.SHOW, this.renderManager.showListener);
				element.removeEventListener(fRenderableElement.HIDE, this.renderManager.hideListener);
			}
			if (mouseListen)
			{
				element.removeEventListener(fRenderableElement.ENABLE, this.enableListener);
				element.removeEventListener(fRenderableElement.DISABLE, this.disableListener);
			}
		}
		
		// PRIVATE AND INTERNAL METHODS FOLLOW
		
		// INTERNAL METHODS RELATED TO RENDER
		
		/**
		 * This method adds an element to the renderEngine pool
		 */
		private function addElementToRenderEngine(element:fRenderableElement):void
		{
			// Init
			element.container = this.renderEngine.initRenderFor(element);
			
			// This happens only if the render Engine returns a container for every element. 
			if (element.container)
			{
				element.container.fElementId = element.id;
				element.container.fElement = element;
			}
			
			// KBEN: 现在基本不存在 MovieClip ，仅仅是为了兼容之前的。现在不获取了   
			// This can be null, depending on the render engine
			// element.flashClip = this.renderEngine.getAssetFor(element);
			
			// Listen to show and hide events
			element.addEventListener(fRenderableElement.SHOW, this.renderManager.showListener, false, 0, true);
			element.addEventListener(fRenderableElement.HIDE, this.renderManager.hideListener, false, 0, true);
			element.addEventListener(fRenderableElement.ENABLE, this.enableListener, false, 0, true);
			element.addEventListener(fRenderableElement.DISABLE, this.disableListener, false, 0, true);
			
			// Add to render manager
			this.renderManager.addedItem(element);
			
			// Elements default to Mouse-disabled
			element.disableMouseEvents();
		}
		
		/**
		 * This method removes an element from the renderEngine pool
		 */
		private function removeElementFromRenderEngine(element:fRenderableElement, destroyingScene:Boolean = false):void
		{
			// bug: 如果渲染内容已经销毁，数据逻辑继续运行的时候，如果再次调用这个函数就会宕机，因此检查一下  
			if (!element.container)
			{
				return;
			}
			
			this.renderManager.removedItem(element, destroyingScene);
			this.renderEngine.stopRenderFor(element);
			if (element.container)
			{
				element.container.fElementId = null;
				element.container.fElement = null;
			}
			element.container = null;
			element.flashClip = null;
			
			// Stop listening to show and hide events
			element.removeEventListener(fRenderableElement.SHOW, this.renderManager.showListener);
			element.removeEventListener(fRenderableElement.HIDE, this.renderManager.hideListener);
			element.removeEventListener(fRenderableElement.ENABLE, this.enableListener);
			element.removeEventListener(fRenderableElement.DISABLE, this.disableListener);
		}
		
		// Listens to elements made enabled
		private function enableListener(evt:Event):void
		{
			this.renderEngine.enableElement(evt.target as fRenderableElement);
		}
		
		// Listens to elements made disabled
		private function disableListener(evt:Event):void
		{
			this.renderEngine.disableElement(evt.target as fRenderableElement);
		}
		
		/**
		 * @private
		 * This method is called when the scene is no longer displayed.
		 */
		public function stopRendering():void
		{
			// Stop render for all elements
			// BUG:  
			//var jl:int = jl;
			var jl:int = this.floors.length;
			for (var j:int = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.floors[j], true);
			// KBEN: 没有了
			//jl = this.walls.length;
			//for (j = 0; j < jl; j++)
			//	this.removeElementFromRenderEngine(this.walls[j], true)
			jl = this.objects.length;
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.objects[j], true);
			jl = this.characters.length;
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.characters[j], true);
			jl = this.emptySprites.length;
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.emptySprites[j], true);
			// KBEN: 没有了
			//jl = this.bullets.length;
			//for (j = 0; j < jl; j++)
			//{
			//	this.bullets[j].customData.bulletRenderer.clear();
			//	this.removeElementFromRenderEngine(this.bullets[j], true);
			//}
			jl = this.m_dynamicObjects.length
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.m_dynamicObjects[j]);
			
			// Free bullet pool as the assets are no longer valid
			//jl = this.bulletPool.length;
			//for (j = 0; j < jl; j++)
			//{
			//	this.bulletPool[j].dispose();
			//	delete this.bulletPool[j];
			//}
			//this.bulletPool = new Array;
			
			// Stop render engine
			this.renderEngine.dispose();
			
			// Stop render manager
			this.renderManager.dispose();
			
			// 删除所有根节点下的资源
			var k:uint = 0;
			while (k < m_SceneLayer.length)
			{
				// 这里不用删除了，在 this.renderEngine.dispose(); 这个函数中处理了 
				//container.removeChild(m_SceneLayer[k]);
				m_SceneLayer[k] = null;
				++k;
			}
			
			// 这是固定大小，不清空   
			//m_SceneLayer.splice(0, m_SceneLayer.length);
			
			// Set flag
			this.IAmBeingRendered = false;
		}
		
		// A light changes its size
		/** @private */ /*
		   public function processNewLightDimensions(evt:Event):void
		   {
		   if (this.IAmBeingRendered)
		   {
		   var light:fOmniLight = evt.target as fOmniLight;
		
		   // Hide light from elements
		   var cell:fCell = light.cell;
		   var nEl:Number = light.nElements;
		   for (var i2:Number = 0; i2 < nEl; i2++)
		   this.renderEngine.lightReset(light.elementsV[i2].obj, light);
		
		   nEl = this.characters.length;
		   for (i2 = 0; i2 < nEl; i2++)
		   this.renderEngine.lightReset(this.characters[i2], light);
		
		   fLightSceneLogic.processNewLightDimensions(this, evt.target as fOmniLight);
		   }
		   }
		 */
		
		// Element enters new cell
		/** @private */
		public function processNewCell(evt:fNewCellEvent):void
		{
			if (this.engine.m_context.m_profiler)
				this.engine.m_context.m_profiler.enter("fScene.processNewCell");
			
			if (this.IAmBeingRendered)
			{
				// KBEN: 灯光全部去掉
				//if (evt.target is fOmniLight)
				//	fLightSceneLogic.processNewCellOmniLight(this, evt.target as fOmniLight);
				//else if (evt.target is fCharacter)
				if (evt.target is fCharacter)
				{
					var c:fCharacter = evt.target as fCharacter
					this.renderManager.processNewCellCharacter(c, evt.m_needDepthSort);
					fCharacterSceneLogic.processNewCellCharacter(this, c);
				}
				else if (evt.target is fEmptySprite)
				{
					var e:fEmptySprite = evt.target as fEmptySprite;
					this.renderManager.processNewCellEmptySprite(e);
					fEmptySpriteSceneLogic.processNewCellEmptySprite(this, e);
				}
				//else if (evt.target is fBullet)
				//{
				//	var b:fBullet = evt.target as fBullet;
				//	this.renderManager.processNewCellBullet(b);
				//}
				else if (evt.target is EffectEntity)
				{
					var eff:EffectEntity = evt.target as EffectEntity;
					this.renderManager.processNewCellEffect(eff);
					fEffectSceneLogic.processNewCellEffect(this, eff);
				}
				else if ((evt.target as fObject).m_resType == EntityCValue.PHFOBJ) // 掉落物 
				{
					var fobj:fSceneObject = evt.target as fSceneObject;
					this.renderManager.processNewCellFObject(fobj);
					fFObjectSceneLogic.processNewCellFObject(this, fobj);
				}
			}
			
			if (this.engine.m_context.m_profiler)
				this.engine.m_context.m_profiler.exit("fScene.processNewCell");
		}
		
		// LIstens to render events
		/** @private */
		public function renderElement(evt:Event):void
		{
			if (this.engine.m_context.m_profiler)
				this.engine.m_context.m_profiler.enter("fScene.renderElement");
			
			// If the scene is not being displayed, we don't update the render engine
			// However, the element's properties are modified. When the scene is shown the result is consistent
			// to what has changed while the render was not being updated
			if (this.IAmBeingRendered)
			{
				//if (evt.target is fOmniLight)
				//	fLightSceneLogic.renderOmniLight(this, evt.target as fOmniLight);
				if (evt.target is fCharacter)
					fCharacterSceneLogic.renderCharacter(this, evt.target as fCharacter);
				else if (evt.target is fEmptySprite)
					fEmptySpriteSceneLogic.renderEmptySprite(this, evt.target as fEmptySprite);
				//if (evt.target is fBullet)
				//	fBulletSceneLogic.renderBullet(this, evt.target as fBullet);
				else if (evt.target is EffectEntity)
					fEffectSceneLogic.renderEffect(this, evt.target as EffectEntity);
				else if (((evt.target) as fObject) && ((evt.target) as fObject).m_resType == EntityCValue.PHFOBJ) // 掉落物 
				{
					fFObjectSceneLogic.renderFObject(this, evt.target as fSceneObject);
				}
			}
			
			if (this.engine.m_context.m_profiler)
				this.engine.m_context.m_profiler.exit("fScene.renderElement");
		}
		
		// This method is called when the shadowQuality option changes
		/** @private */ /*
		   public function resetShadows():void
		   {
		   this.renderEngine.resetShadows();
		   var cl:int = this.characters.length;
		   for (i = 0; i < cl; i++)
		   fCharacterSceneLogic.processNewCellCharacter(this, this.characters[i], true);
		   cl = this.lights.length;
		   for (var i:int = 0; i < cl; i++)
		   fLightSceneLogic.processNewCellOmniLight(this, this.lights[i], true);
		   }
		 */
		
		// INTERNAL METHODS RELATED TO CAMERA MANAGEMENT
		
		// Listens cameras moving
		private function cameraMoveListener(evt:fMoveEvent):void
		{
			this.followCamera(evt.target as fCamera);
		}
		
		// Listens cameras changing cells.
		private function cameraNewCellListener(evt:Event):void
		{
			var camera:fCamera = evt.target as fCamera;
			this.renderEngine.setCameraPosition(camera);
			if (this.IAmBeingRendered)
				this.renderManager.processNewCellCamera(camera);
		}
		
		// Adjusts visualization to camera position
		private function followCamera(camera:fCamera):void
		{
			//if (this.prof)
			if (this.engine.m_context.m_profiler)
			{
				//this.prof.begin("Update camera");
				this.engine.m_context.m_profiler.enter("Update camera");
				this.renderEngine.setCameraPosition(camera);
				//this.prof.end("Update camera");
				this.engine.m_context.m_profiler.exit("Update camera");
			}
			else
			{
				this.renderEngine.setCameraPosition(camera);
			}
		}
		
		// INTERNAL METHODS RELATED TO DEPTHSORT
		
		// Returns a normalized zSort value for a cell in the grid. Bigger values display in front of lower values
		/** @private */
		public function computeZIndex(i:Number, j:Number, k:Number):Number
		{
			var ow:int = this.gridWidth;
			var od:int = this.gridDepth;
			// KBEN: 这个大小改成 1 就行了吧
			var oh:int = this.gridHeight;
			//return ((((((ow - i + 1) + (j * ow + 2))) * oh) + k)) / (ow * od * oh);
			return ((ow - i + 1) + (j * ow + 2)) / (ow * od);
		}
		
		// INTERNAL METHODS RELATED TO GRID MANAGEMENT	
		
		// Reset cell. This is called if the engine's quality options change to a better quality
		// as all cell info will have to be recalculated
		/** @private */
		public function resetGrid():void
		{
			var l:int = this.allUsedCells.length;
			for (var i:int = 0; i < l; i++)
			{
				//this.allUsedCells[i].characterShadowCache = new Array;
				delete this.allUsedCells[i].visibleObjs;
			}
		}
		
		// Returns the cell containing the given coordinates
		/** @private */
		public function translateToCell(x:Number, y:Number, z:Number):fCell
		{
			//if (x < 0 || y < 0 || z < 0)
			//return null;
			//return this.getCellAt(x / this.gridSize, y / this.gridSize, z / this.levelSize);
			if (x < 0 || y < 0)
				return null;
			return this.getCellAt(x / this.gridSize, y / this.gridSize);
		}
		
		// Returns the cell at specific grid coordinates. If cell does not exist, it is created.
		/** @private */
		public function getCellAt(i:int, j:int, k:int = 0):fCell
		{
			//if (i < 0 || j < 0 || k < 0)
			//return null;
			//if (i >= this.gridWidth || j >= this.gridDepth || k >= this.gridHeight)
			//return null;
			//
			// Create new if necessary
			//if (!this.grid[i] || !this.grid[i][j])
			//return null;
			//var arr:Array = this.grid[i][j];
			//if (!arr[k])
			//{
			//
			//var cell:fCell = new fCell();
			//
			// Z-Index
			//
			// Original call
			//cell.zIndex = this.computeZIndex(i,j,k)
			//
			// Inline for a bit of speedup
			//var ow:int = this.gridWidth;
			//var od:int = this.gridDepth;
			//var oh:int = this.gridHeight;
			//cell.zIndex = ((((((ow - i + 1) + (j * ow + 2))) * oh) + k)) / (ow * od * oh);
			
			//
			//var s:Array = this.sortAreas[i];
			//
			//var s:Array = this.sortAreasRTree.intersects(new fCube(i,j,k,i+1,j+1,k+1))
			//
			//var l:int = s.length;
			//
			//var found:Boolean = false;
			//for (var n:int = 0; !found && n < l; n++)
			//{
			//
			///* Original call
			//if(s[n].isPointInside(i,j,k)) {
			//found = true
			//cell.zIndex+=(s[n] as fSortArea).zValue
			//}*/
			//
			///* Inline for a bit of speedup */
			//var sA:fSortArea = s[n];
			//var sA:fSortArea = this.sortAreas[s[n]]
			//if ((i >= sA.i && i <= sA.i + sA.width) && (j >= sA.j && j <= sA.j + sA.depth) && (k >= sA.k && k <= sA.k + sA.height))
			//{
			//found = true;
			//cell.zIndex += sA.zValue;
			//}
			//
			//}
			//
			// Internal
			//cell.i = i;
			//cell.j = j;
			//cell.k = k;
			//cell.x = (this.gridSize >> 1) + (this.gridSize * i);
			//cell.y = (this.gridSize >> 1) + (this.gridSize * j);
			//cell.z = (this.levelSize >> 1) + (this.levelSize * k);
			//arr[k] = cell;
			//
			//this.allUsedCells[this.allUsedCells.length] = cell;
			//
			//}
			//
			// Return cell
			//return this.grid[i][j][k];
			
			if (i < 0 || j < 0)
				return null;
			if (i >= this.gridWidth || j >= this.gridDepth)
				return null;
			
			// Create new if necessary
			
			var arr:Array = this.grid[i];
			if (!arr)
				return null;
			var cell:fCell = arr[j];
			if (!cell)
			{
				cell = new fCell(this);
				
				// Z-Index				
				// Inline for a bit of speedup
				//var ow:int = this.gridWidth;
				//var od:int = this.gridDepth;
				// KBEN: gridHeight 永远为 1 
				//var oh:int = this.gridHeight;
				//cell.zIndex = ((ow - i + 1) + (j * ow + 2)) / (ow * od);
				
				//var s:Array = this.sortAreas[i];				
				//var l:int = s.length;
				
				//var found:Boolean = false;
				//for (var n:int = 0; !found && n < l; n++)
				//{
				//	var sA:fSortArea = s[n];
				//	if ((i >= sA.i && i <= sA.i + sA.width) && (j >= sA.j && j <= sA.j + sA.depth))
				//	{
				//		found = true;
				//		cell.zIndex += sA.zValue;
				//	}
				//}
				
				// Internal
				cell.i = i;
				cell.j = j;
				cell.k = k;
				cell.x = (this.gridSize >> 1) + (this.gridSize * i);
				cell.y = (this.gridSize >> 1) + (this.gridSize * j);
				cell.z = 0;
				cell.zIndex = cell.y;
				// 更新一下裁剪矩形信息
				cell.updateScrollRect();
				arr[j] = cell;
				
				// KBEN: 填写 fCell 阻挡点信息 
				var stoppoint:stopPoint = this.getStopPoint(cell.i, cell.j);
				cell.stoppoint = stoppoint;
				
				// KBEN: 填写查找数组    
				this.allUsedCells[this.allUsedCells.length] = cell;
			}
			
			// Return cell
			return cell;
		}
		
		/** @private */
		public static function translateCoords(x:Number, y:Number, z:Number):Point
		{
			/*
			   // KBEN: 位置变换
			   if (fSceneConfig.instance.mapType == fEngineCValue.Engine2d)
			   {
			   return new Point(x, y);
			   }
			   else
			   {
			   var xx:Number = x * fEngine.DEFORMATION;
			   var yy:Number = y * fEngine.DEFORMATION;
			   var zz:Number = z * fEngine.DEFORMATION;
			   var xCart:Number = (xx + yy) * 0.8944271909999159; //Math.cos(0.4636476090008061)
			   var yCart:Number = zz + (xx - yy) * 0.4472135954999579; //Math.sin(0.4636476090008061)
			
			   return new Point(xCart, -yCart);
			   }
			 */
			// bug: 性能
			return new Point(x, y);
		}
		
		/** @private */
		public static function translateCoordsInverse(x:Number, y:Number):Point
		{
			// KBEN: 
			if (fSceneConfig.instance.mapType == fEngineCValue.Engine2d)
			{
				return new Point(x, y);
			}
			else
			{
				//rotate the coordinates
				var yCart:Number = (x / 0.8944271909999159 + (y) / 0.4472135954999579) / 2;
				var xCart:Number = (-1 * (y) / 0.4472135954999579 + x / 0.8944271909999159) / 2;
				
				//scale the coordinates
				xCart = xCart / fEngine.DEFORMATION;
				yCart = yCart / fEngine.DEFORMATION;
				
				return new Point(xCart, yCart);
			}
		}
		
		// Get elements affected by lights from given cell, sorted by distance
		/** @private */ /*
		   public function getAffectedByLight(cell:fCell, range:Number = Infinity):void
		   {
		   var r:Array = fVisibilitySolver.calcAffectedByLight(this, cell.x, cell.y, cell.z, range);
		   cell.lightAffectedElements = r;
		   cell.lightRange = range;
		   }
		 */
		
		// Get elements visible from given cell, sorted by distance
		/** @private */
		public function getVisibles(cell:fCell, range:Number = Infinity):void		
		{
			var visibleFloor:Vector.<fFloor> = new Vector.<fFloor>();
			//var r:Array = fVisibilitySolver.calcVisibles(this, cell.x, cell.y, cell.z, range, visibleFloor);
			//var r:Array = fVisibilitySolver.calcVisibles(this, cam.x, cam.y, cam.z, range, visibleFloor);
			var r:Array = fVisibilitySolver.calcVisibles(this, cell, range, visibleFloor);
			cell.visibleElements = r;
			// 添加快速调用
			cell.m_visibleFloor = visibleFloor;
			cell.visibleRange = range;
		}
		
		/**
		 * @private
		 * This method frees all resources allocated by this scene. Always dispose unused scene objects:
		 * scenes generate lots of internal Arrays and BitmapDatas that will eat your RAM fast if they are not properly deleted
		 */
		public function dispose():void
		{
			// Free properties
			// bug: 这个放在最后，因为销毁的函数要用到这个变量 
			//this.engine = null;
			//for (var i:int = 0; i < this.sortAreas.length; i++)
			//	delete this.sortAreas[i];
			//this.sortAreas = null;
			//this.sortAreasRTree = null;
			
			//this.allStatic2D = null;
			//this.allStatic2DRTree = null;
			
			//this.allStatic3D = null;
			//this.allStatic3DRTree = null;
			
			// 一定放在 this.renderEngine = null; 之前
			m_disposed = true;
			removeFog(m_fogPlane);
			m_fogPlane = null;
			
			// bug: 内存泄露，事件没有移除
			if (this.currentCamera)
			{
				this.currentCamera.removeEventListener(fElement.MOVE, this.cameraMoveListener);
				this.currentCamera.removeEventListener(fElement.NEWCELL, this.cameraNewCellListener);
				this.currentCamera.dispose();
			}
			this.currentCamera = null;
			this._controller = null;
			
			// Stop current initialization, if any
			if (this.initializer)
				this.initializer.dispose();
			this.initializer = null;
			//this.resourceManager = null;
			
			// Free render engine
			this.renderEngine.dispose();
			this.renderEngine = null;
			
			// Free render manager
			this.renderManager.dispose();
			this.renderManager = null;
			
			if (this._orig_container.parent)
				this._orig_container.parent.removeChild(this._orig_container);
			this._orig_container = null;
			this.container = null;
			
			// Free elements
			var il:int;
			var ele:Object;
			for each(ele in floors)
			{
				ele.dispose();
			}			
			for each(ele in objects)
			{
				ele.dispose();
			}
			
			for each(ele in characters)
			{
				ele.dispose();
			}
			
			for each(ele in emptySprites)
			{
				ele.dispose();
			}	
			
			this.floors = null;
			//this.walls = null;
			this.objects = null;
			this.characters = null;
			this.emptySprites = null;
			this.events = null;
			this.lights = null;
			this.everything = null;
			this.all = null;
			//this.bullets = null;
			//this.bulletPool = null;
			
			// Free grid
			this.freeGrid();
			
			// Free materials
			fMaterial.disposeMaterials(this);
			this.engine = null;
			
			// 释放阻挡点资源
			m_defaultStopPoint = null;			
			m_stopPointList = null;
			
			// 释放 ai
			this.AI = null;
			
			// 清除显示根节点   
			//var k:uint = 0;
			//while (k < m_SceneLayer.length)
			//{
			// 这个在 this.renderEngine.dispose(); 这个函数中已经移除了  
			//container.removeChild(m_SceneLayer[k]);
			//	m_SceneLayer[k] = null;
			//	++k;
			//}
			
			//m_SceneLayer.splice(0, m_SceneLayer.length);
			m_SceneLayer = null;
			
			if (m_thumbnails)
			{
				//m_thumbnails.dispose();
				m_thumbnails = null;
			}
			//if(m_thumbnailsExtend)
			//{
			//	m_thumbnailsExtend.dispose();
			//	m_thumbnailsExtend = null;
			///}
			
			// 环境灯光释放
			this.environmentLight.dispose();
			this.environmentLight = null;
		}
		
		/**
		 * This method frees memory used by the grid in this scene
		 */
		private function freeGrid():void
		{
			var l:int = this.allUsedCells.length;
			for (var i:int = 0; i < l; i++)
				this.allUsedCells[i].dispose();
			this.grid = null;
			this.allUsedCells = null;
		}
		
		// KBEN: 阻挡点测试，初始化几个阻挡点  
		public function testStopPoint():void
		{
			var k:uint = 0;
			var m:uint = 0;
			var cell:fCell;
			while (k < this.gridDepth) // 行  
			{
				while (m < this.gridWidth) // 列 
				{
					cell = this.getCellAt(m, k, 0);
					
					if (k == 3 && m == 5)
					{
						cell.stoppoint.isStop = true;
						
						// 绘制阻挡点 
						var floor:fFloor = getFloorByGridPos(m, k);
							//(floor.customData.flash9Renderer as fFlash9FloorRenderer).drawGrid(m - floor.i, k - floor.j);
					}
					
					++m;
				}
				
				m = 0;
				++k;
			}
		}
		
		// KBEN: 根据格子找对应的区块，就是 floor
		public function getFloorByGridPos(ix:uint, iy:uint):fFloor
		{
			var floor:fFloor;
			for (var key:String in this.floors)
			{
				floor = this.floors[key];
				if (floor.i <= ix && ix < floor.i + floor.gWidth && floor.j <= iy && iy < floor.j + floor.gDepth)
				{
					return floor;
				}
			}
			
			return null;
		}
		
		/*
		   // KBEN: 雾绘制 pt: 全局坐标
		   public function clearFog(ptx:int, pty:int, ptz:int):void
		   {
		   if (this.m_sceneConfig.fogOpened)
		   {
		   var floor:fFloor;
		   var localx:int;
		   var localy:int;
		
		   var theCell:fCell = this.translateToCell(ptx, pty, ptz);
		   floor = this.getFloorByGridPos(theCell.i, theCell.j);
		   if (floor)
		   {
		   // KBEN: 转换成 fPlane 局部坐标
		   localx = ptx - floor.x0;
		   localy = pty - floor.y0;
		
		   var render:fFlash9PlaneRenderer = floor.customData.flash9Renderer;
		   if (render)
		   {
		   render.clearFog(localx, localy);
		   }
		   }
		   }
		   }
		 */
		
		// 区域获取    
		public function translateToFloor(x:Number, y:Number):fFloor
		{
			// bug: 经常有些值返回 null ，这里直接跑出异常    
			if (x < 0 || y < 0)
			{
				throw new Error("cell is null");
				return null;
			}
			return this.getFloorAt(x / this.m_floorWidth, y / this.m_floorDepth);
		}
		
		public function getFloorAt(i:int, j:int):fFloor
		{
			// bug: 经常有些值返回 null ，这里直接跑出异常    
			if (i < 0 || j < 0)
			{
				throw new Error("cell is null");
				return null;
			}
			// bug: 经常有些值返回 null ，这里直接跑出异常    
			if (i >= this.m_floorXCnt || j >= this.m_floorYCnt)
			{
				throw new Error("cell is null");
				return null;
			}
			
			return floors[j * this.m_floorXCnt + i];
		}
		
		// 根据世界空间中的位置获取区域, 这个坐标是场景中的坐标,不是 stage 坐标
		public function getFloorAtByPos(x:Number, y:Number):fFloor
		{
			if (x < 0 || y < 0)
				return null;
			var i:int = x / this.m_floorWidth;
			var j:int = y / this.m_floorDepth;
			
			if (i < 0 || j < 0)
				return null;
			if (i >= this.m_floorXCnt || j >= this.m_floorYCnt)
				return null;
			
			return floors[j * this.m_floorXCnt + i];
		}
		
		// KBEN: 如果没有找到直接返回 -1 ，返回所在 floor 索引   
		public function translateToFloorIdx(x:Number, y:Number):int
		{
			if (x < 0 || y < 0)
				return -1;
			var i:int = x / this.m_floorWidth;
			var j:int = y / this.m_floorDepth;
			
			if (i < 0 || j < 0)
				return -1;
			if (i >= this.m_floorXCnt || j >= this.m_floorYCnt)
				return -1;
			
			return (j * this.m_floorXCnt + i);
		}
		
		// KBEN: 获取并且改变 floor 中的动态对象 
		//public function translateToFloorAndIdx(x:Number, y:Number, idx:int, uniqueId:int, id:String, type:uint):int
		public function translateToFloorAndIdx(x:Number, y:Number, idx:int, id:String, type:uint):int
		{
			var srci:int;
			var srcj:int;
			
			if (idx != -1)
			{
				srcj = idx / this.m_floorXCnt;
				srci = idx % this.m_floorYCnt;
			}
			
			if (x < 0 || y < 0)
				return -1;
			var i:int = x / this.m_floorWidth;
			var j:int = y / this.m_floorDepth;
			
			if (i < 0 || j < 0)
				return -1;
			if (i >= this.m_floorXCnt || j >= this.m_floorYCnt)
				return -1;
			
			if ((j * this.m_floorXCnt + i) == idx) // 如果一样就不更改了  
				return (j * this.m_floorXCnt + i);
			
			if (type == EntityCValue.TEfffect)
			{
				//floors[j * this.m_floorXCnt + i].addDynamic(uniqueId, id)
				floors[j * this.m_floorXCnt + i].addDynamic(id)
				if (idx != -1)
				{
					//floors[j * this.m_floorXCnt + i].clearDynamic(uniqueId)
					floors[j * this.m_floorXCnt + i].clearDynamic(id)
				}
			}
			else if (type == EntityCValue.TPlayer || type == EntityCValue.TVistNpc || type == EntityCValue.TBattleNpc || type == EntityCValue.TNpcPlayerFake)
			{
				//floors[j * this.m_floorXCnt + i].addCharacter(uniqueId, id)
				floors[j * this.m_floorXCnt + i].addCharacter(id)
				if (idx != -1)
				{
					//floors[j * this.m_floorXCnt + i].clearCharacter(uniqueId)
					floors[j * this.m_floorXCnt + i].clearCharacter(id)
				}
			}
			
			return (j * this.m_floorXCnt + i);
		}
		
		// KBEN: 直接获取阻挡点信息   
		public function getStopPoint(xpos:int, ypos:int):stopPoint
		{
			if (m_stopPointList[ypos] && m_stopPointList[ypos][xpos])
			{
				return m_stopPointList[ypos][xpos];
			}
			return m_defaultStopPoint;
		}
		
		// KBEN: 添加阻挡点  xpos : 列数  ypos : 行数    
		public function addStopPoint(xpos:int, ypos:int, stoppoint:stopPoint):void
		{
			m_stopPointList[ypos] ||= new Dictionary();
			m_stopPointList[ypos][xpos] = stoppoint;
		}
		
		public function correctX(x:Number):Number
		{
			if (x < 0)
			{
				Logger.info(null, null, "x 错误，原始坐标 " + x + " 修正坐标 " + " 0");
				return 0;
			}
			else if (x >= width)
			{
				Logger.info(null, null, "x 错误，原始坐标 " + x + " 修正坐标 " + (width - 1));
				return width - 1;
			}
			else
			{
				return x;
			}
		}
		
		public function correctY(y:Number):Number
		{
			if (y < 0)
			{
				Logger.info(null, null, "y 错误，原始坐标 " + y + " 修正坐标 " + "0");
				return 0;
			}
			else if (y >= this.depth)
			{
				Logger.info(null, null, "y 错误，原始坐标 " + y + " 修正坐标 " + (this.depth - 1));
				return this.depth - 1;
			}
			else
			{
				return y;
			}
		}
		
		public function ServerPointToClientPoint(ptServer:Point):Point
		{
			var ptClient:Point = new Point();
			ptClient.x = ptServer.x * this.gridSize;
			ptClient.y = ptServer.y * this.gridSize;
			
			ptClient.x = correctX(ptClient.x);
			ptClient.y = correctY(ptClient.y);
			return ptClient;
		}
		
		public function ServerPointToClientPoint2(x:int, y:int):Point
		{
			var ptClient:Point = new Point();
			ptClient.x = x * this.gridSize;
			ptClient.y = y * this.gridSize;
			
			ptClient.x = correctX(ptClient.x);
			ptClient.y = correctY(ptClient.y);
			return ptClient;
		}
		
		// 场景宽度和高度，像素为单位，这个是真实的图片的宽度和高度
		public function widthpx():int
		{			
			return m_scenePixelWidth;
		}
		
		public function heightpx():int
		{			
			return m_scenePixelHeight;
		}
		
		// 这个是整个场景的像素宽度和高度，由于以格子为单位，所以可能这个像素大小要比真正的图片像素大小要大，并且战斗地图的左右两边都添加了很多的空格子
		public function sceneWidthpx():int
		{
			return gridWidth * gridSize;
		}
		
		public function sceneHeightpx():int
		{
			return gridDepth * gridSize;
		}
		
		public function isCoordinateLegal(_x:Number, _y:Number):Boolean
		{
			return _x < widthpx() && _y < heightpx();
		}
		
		// 绘制战斗格子 startPt : 起始点    gridsize : 格子大小
		public function drawFightGrid(startPt:Point, gridsize:Point):void
		{
			var xcnt:uint = 7;
			var ycnt:uint = 3;
			var screenpos:Point;
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.clear();
			m_SceneLayer[EntityCValue.SLShadow].graphics.lineStyle(1, 0x000000);
			//m_SceneLayer[SLShadow].graphics.beginFill(0xFFFFFF);
			
			// 绘制格子   
			var k:int = 0;
			while (k < xcnt + 1)
			{
				screenpos = translateCoords(startPt.x + k * gridsize.x, startPt.y, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
				
				screenpos = fScene.translateCoords(startPt.x + k * gridsize.x, startPt.y + ycnt * gridsize.y, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
				
				++k;
			}
			
			k = 0;
			while (k < ycnt + 1)
			{
				screenpos = fScene.translateCoords(startPt.x, startPt.y + k * gridsize.y, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
				
				screenpos = fScene.translateCoords(startPt.x + xcnt * gridsize.x, startPt.y + k * gridsize.y, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
				
				++k;
			}
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.endFill();
		}
		
		public function clearFightGrid():void
		{
			m_SceneLayer[EntityCValue.SLShadow].graphics.clear();
		}
		
		// 调试的时候绘制阻挡点
		public function drawStopPt():void
		{
			var screenpos:Point;
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.clear();
			m_SceneLayer[EntityCValue.SLShadow].graphics.lineStyle(1, 0x000000);
			//m_SceneLayer[SLShadow].graphics.beginFill(0xFF0000);
			
			// 绘制格子
			// 绘制竖线
			var k:int = 0;
			while (k < this.gridWidth + 1)
			{
				screenpos = fScene.translateCoords(k * this.gridSize, 0, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
				
				screenpos = fScene.translateCoords(k * this.gridSize, this.gridDepth * this.gridSize, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
				
				++k;
			}
			
			// 绘制横线
			k = 0;
			while (k < this.gridDepth + 1)
			{
				screenpos = fScene.translateCoords(0, k * this.gridSize, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
				
				screenpos = fScene.translateCoords(this.gridWidth * this.gridSize, k * this.gridSize, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
				
				++k;
			}
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.endFill();
			
			// 阻挡点使用红色
			m_SceneLayer[EntityCValue.SLShadow].graphics.lineStyle(1, 0xFF0000);
			var m:uint = 0;
			k = 0;
			while (k < this.gridDepth) // 行  
			{
				m = 0;
				while (m < this.gridWidth) // 列 
				{
					var c:fCell = getCellAt(m, k, 0);
					if (c.stoppoint.isStop) // 绘制阻挡点  
					{
						drawGrid(m, k);
					}
					
					++m;
				}
				
				++k;
			}
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.endFill();
		}
		
		// 绘制 fFloor
		public function drawFloor():void
		{
			var screenpos:Point;
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.clear();
			m_SceneLayer[EntityCValue.SLShadow].graphics.lineStyle(1, 0x000000);
			//m_SceneLayer[SLShadow].graphics.beginFill(0xFF0000);
			
			// 绘制格子
			// 绘制竖线
			var k:int = 0;
			while (k < this.m_floorXCnt + 1)
			{
				screenpos = fScene.translateCoords(k * this.m_floorWidth, 0, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
				
				screenpos = fScene.translateCoords(k * this.m_floorWidth, this.m_floorYCnt * this.m_floorDepth, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
				
				++k;
			}
			
			// 绘制横线
			k = 0;
			while (k < this.m_floorYCnt + 1)
			{
				screenpos = fScene.translateCoords(0, k * this.m_floorDepth, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
				
				screenpos = fScene.translateCoords(this.m_floorWidth * this.m_floorXCnt, k * this.m_floorDepth, 0);
				m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
				
				++k;
			}
			
			m_SceneLayer[EntityCValue.SLShadow].graphics.endFill();
		}
		
		// 绘制滚动矩形
		public function drawScrollRect(cell:fCell):void
		{
			var screenpos:Point;
			
			m_SceneLayer[EntityCValue.SLShadow1].graphics.clear();
			m_SceneLayer[EntityCValue.SLShadow1].graphics.lineStyle(1, 0xFF0000);
			
			// 第一行
			screenpos = fScene.translateCoords(cell.m_scrollRect.x, cell.m_scrollRect.y, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.moveTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords(cell.m_scrollRect.x + cell.m_scrollRect.width, cell.m_scrollRect.y, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.lineTo(screenpos.x, screenpos.y);
			
			// 第二行
			screenpos = fScene.translateCoords(cell.m_scrollRect.x, cell.m_scrollRect.y + cell.m_scrollRect.height, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.moveTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords(cell.m_scrollRect.x + cell.m_scrollRect.width, cell.m_scrollRect.y + cell.m_scrollRect.height, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.lineTo(screenpos.x, screenpos.y);
			
			// 第一列
			screenpos = fScene.translateCoords(cell.m_scrollRect.x, cell.m_scrollRect.y, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.moveTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords(cell.m_scrollRect.x, cell.m_scrollRect.y + cell.m_scrollRect.height, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.lineTo(screenpos.x, screenpos.y);
			
			// 第二列
			screenpos = fScene.translateCoords(cell.m_scrollRect.x + cell.m_scrollRect.width, cell.m_scrollRect.y, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.moveTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords(cell.m_scrollRect.x + cell.m_scrollRect.width, cell.m_scrollRect.y + cell.m_scrollRect.height, 0);
			m_SceneLayer[EntityCValue.SLShadow1].graphics.lineTo(screenpos.x, screenpos.y);
			
			m_SceneLayer[EntityCValue.SLShadow1].graphics.endFill();
		}
		
		public function drawGrid(ix:uint, iy:uint):void
		{
			var screenpos:Point;
			
			screenpos = fScene.translateCoords(ix * this.gridSize, iy * this.gridSize, 0);
			m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords((ix + 1) * this.gridSize, (iy + 1) * this.gridSize, 0);
			m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords((ix + 1) * this.gridSize, iy * this.gridSize, 0);
			m_SceneLayer[EntityCValue.SLShadow].graphics.moveTo(screenpos.x, screenpos.y);
			
			screenpos = fScene.translateCoords(ix * this.gridSize, (iy + 1) * this.gridSize, 0);
			m_SceneLayer[EntityCValue.SLShadow].graphics.lineTo(screenpos.x, screenpos.y);
		}
		
		public function clearStopPt():void
		{
			m_SceneLayer[EntityCValue.SLShadow].graphics.clear();
		}
		
		public function getPosInMap():Point
		{
			return new Point(this.container.mouseX, container.mouseY);
		}
		
		public function sceneLayer(id:uint):Sprite
		{
			return m_SceneLayer[id];
		}
		
		public function convertToUIPos(x:Number, y:Number):Point
		{
			var ret:Point = new Point();
			var rect:Rectangle = container.scrollRect;
			if (rect != null)
			{
				ret.x = x - rect.x;
				ret.y = y - rect.y;
			}
			return ret;
		}
		
		// 转换 stage 到 scene 坐标
		public function convertG2S(x:Number, y:Number):Point
		{
			var ret:Point = new Point();
			var rect:Rectangle = container.scrollRect;
			if (rect != null)
			{
				ret.x = x + rect.x;
				ret.y = y + rect.y;
			}
			return ret;
		}
		
		// 处理地形缩略图文件
		//public function addTerThumbnails(bmd:BitmapData):void
		//public function addTerThumbnails():void
		//{
		//if (m_SceneLayer[SLTerrainThumbnails].numChildren)
		//{
		//throw new Event("terrrain already add Thumbnails");
		//}
		
		// 退出场景的时候释放资源
		//var bm:Bitmap = new Bitmap(bmd);
		//bm.width = m_scenePixelWidth;
		//bm.height = m_scenePixelHeight;
		//m_SceneLayer[SLTerrainThumbnails].addChild(bm);
		
		//copy 如果不存在就拷贝，如果已经存在就补考呗了
		/*
		   var mat:Matrix = new Matrix();
		   mat.scale(m_scenePixelWidth/bmd.width, m_scenePixelHeight/bmd.height);
		   m_thumbnailsExtend = new BitmapData(m_scenePixelWidth, m_scenePixelHeight);
		   m_thumbnailsExtend.draw(bmd, mat);
		   bmd.dispose();
		   bmd = null;
		 */
		
		//if(!m_thumbnailsExtend && m_thumbnails)
		//{
		//	var mat:Matrix = new Matrix();
		//	mat.scale(m_scenePixelWidth/m_thumbnails.width, m_scenePixelHeight/m_thumbnails.height);
		//	m_thumbnailsExtend = new BitmapData(m_scenePixelWidth, m_scenePixelHeight);
		//	m_thumbnailsExtend.draw(m_thumbnails, mat);
		//m_thumbnails.dispose();
		//m_thumbnails = null;
		//}
		//}
		
		// 逻辑上一帧结束，有些一帧中更新很一次的数据在帧结束更新就放在这里，否则深度更新会更新很多次
		public function onFrameEnd():void
		{
			// 每一帧结束更新一次深度排序
			if (true == this.m_depthDirty)
			{
				this.renderManager.depthSort();
				this.m_depthDirty = false;
				
				this.m_depthDirtySingle = false;
				this.m_singleDirtyArr.length = 0;
			}
			else if (true == this.m_depthDirtySingle)
			{
				this.renderManager.depthSortSingle();
				this.m_depthDirtySingle = false;
				
				this.m_singleDirtyArr.length = 0;
			}
		}
	}
}