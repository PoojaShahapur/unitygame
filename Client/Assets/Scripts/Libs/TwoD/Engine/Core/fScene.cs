using System.Collections;
using System.Collections.Generic;

namespace SDK.Lib
{
	public class fScene : EventDispatch
	{
		private static float count = 0;

		public static bool allCharacters = false;
		
		private fEngineSceneController _controller = null;
		private fSceneInitializer initializer;
		public fEngineRenderEngine renderEngine;
		public fSceneRenderManager renderManager;
		public fEngine engine;
        public AuxComponent _orig_container;

		public float viewWidth;

		public float viewHeight;

		public float top;

        public int gridWidth;

		public int gridDepth;

		public int gridHeight;

		public int gridSize;
        public int gridSizeHalf;

        public int levelSize;

		public int sortCubeSize = fEngine.SORTCUBESIZE;
		
		public ArrayList grid;
		public ArrayList allUsedCells;
	
		public bool IAmBeingRendered = false;
		private bool _enabled;

		public string id;

		public fCamera currentCamera;
		
		public AuxComponent container;

		public string stat;
		
		public bool ready;
		
		public float width;

		public float depth;
		
		public float height;
		
		// KBEN: 这个是存放地形的，使用树进行裁剪    
		public ArrayList floors;
	
		// KBEN: 这个里面存放的是场景中不能被移动和删除的实体，例如地物，使用树进行裁剪      
		public ArrayList objects;
		
		// KBEN: 可移动或者可以动态删除的放在这里，特效，掉落物，npc ，直接使用中心点进行裁剪       
		public MList<fObject> m_dynamicObjects;

		// KBEN: 这个是存放总是走动的，玩家，直接使用中心点进行裁剪     
		public ArrayList characters;

		/**
		 * 所有元素
		 */
		public ArrayList everything;

		/**
		 * 所有元素
		 */
		public ArrayList all;
		
		/**
		 * AI 内容
		 */
		public fAiContainer AI;

		public ArrayList events;
		
		// KBEN: 场景配置 
		public fSceneConfig m_sceneConfig;
		
		// KBEN:
		public float m_floorWidth; // 单个 Floor 区域宽度，单位像素  
		public float m_floorDepth; // 单个 Floor 区域高度，单位像素  
		
		public uint m_floorXCnt; // X 方向面板个数 
		public uint m_floorYCnt; // Y 方向面板个数 
		
		public int m_scenePixelXOff = 0; // 场景 X 方向偏移，主要用在战斗场景，需要偏移一定距离
		public int m_scenePixelYOff = 0; // 场景 Y 方向偏移，主要用在战斗场景，需要偏移一定距离
		
		public int m_scenePixelWidth = 0; // 场景图像真实的像素宽度，可能比 scene 的格子的宽度要小
		public int m_scenePixelHeight = 0; // 场景图像真实的像素高度，可能比 scene 的格子的高度要小

		public bool m_depthDirty = false; // 主要用来进行一帧中只调用一次 depthSort 这个函数
		public bool m_depthDirtySingle = false; // 每一个移动的时候需要深度排序
		public MList<fRenderableElement> m_singleDirtyArr; // 每一个深度改变的时候都存放在这个列表，一次更新
		
		public static string LOADINGDESCRIPTION = "Creating scene";
		
		public static string LOADPROGRESS = "scenecloadprogress";

		public static string LOADCOMPLETE = "sceneloadcomplete";
		
		// KBEN: 渲染引擎层，这个仅仅是场景， UI 单独一层 
		public MList<SpriteLayer> m_SceneLayer;

		public Dictionary<int, Dictionary<int, StopPoint>> m_stopPointList;
		public StopPoint m_defaultStopPoint; // 默认的阻挡点，为了统一处理，不用总是 if else 判断    
		
		public uint m_serverSceneID;    // 服务器场景 id
		public uint m_filename;         // 客户端地图文件名字，没有扩展名字
		
		public bool m_sortByBeingMove = true;
		public string m_path;
		
		public int m_timesOfShow = 0;
		public bool m_disposed = false;
		public Dictionary<int, int> m_dicDebugInfo;

		public fScene(fEngine engine, AuxComponent container, fEngineSceneRetriever retriever, float width, float height, uint serversceneid, fEngineRenderEngine renderer = null)
		{
			this.id = "fScene_" + (fScene.count++);
			this.m_serverSceneID = serversceneid;
			this.engine = engine;
			this._orig_container = container;
			this.container = container;
			this.gridSize = 64;
			this.levelSize = 64;
			this.top = 0;
			this.stat = "Loading XML";
			this.ready = false;
			this.viewWidth = width;
			this.viewHeight = height;
			
			this.floors = new ArrayList();
			this.objects = new ArrayList();
			this.characters = new ArrayList();
			this.events = new ArrayList();
			this.everything = new ArrayList();
			this.all = new ArrayList();
			// AI
			this.AI = new fAiContainer(this);
			
			// KBEN: 场景层 
			m_SceneLayer = new MList<SpriteLayer>(EntityCValue.SLCnt);

			if (renderer != null)
			{
				this.renderEngine = renderer;
			}
			else
			{
				this.renderEngine = new fFlash9RenderEngine(this, container, m_SceneLayer);
			}
			this.renderEngine.setViewportSize(width, height);

			this.renderManager = new fSceneRenderManager(this);
			this.renderManager.setViewportSize(width, height);
			
			this.initializer = new fSceneInitializer(this, retriever);

			// KBEN:
			m_sceneConfig = new fSceneConfig();
			m_dynamicObjects = new MList<fObject>();
			m_sceneUIBtmEffVec = new MList<fObject>();
			m_sceneUITopEffVec = new MList<fObject>();
			m_sceneJinNangEffVec = new MList<fObject>();
			m_stopPointList = new Dictionary<int, Dictionary<int, StopPoint>>();
			
			var defObj:XML =  <item x="-1" y="-1" type="0"/>;
			m_defaultStopPoint = new StopPoint(defObj, this);
			m_defaultStopPoint.isStop = false; // 默认的阻挡点设置成 false ，不要忘了
			m_singleDirtyArr = new MList<fRenderableElement>();
		}
		
		public void initialize()
		{
			this.initializer.start();
		}
		
		// KBEN: 舞台大小改变处理函数 
		public void setViewportSize(int width, int height)
		{
			// 相机保存视口数据
			if (this.currentCamera != null)
			{
				this.currentCamera.setViewportSize(width, height);
			}

            // 清理格子中保存的裁剪的数据
            uint k = 0;
            uint m = 0;
            fCell cell;
			while (k < this.gridDepth) // 行  
			{
				m = 0;
				while (m < this.gridWidth) // 列 
				{
					cell = this.getCellAt(m, k, 0);
					// bug: 如果地图还没加载完成，这个时候产生了窗口大小改变的事件，这个时候格子是空的，就会获取不到格子
					if (cell != null)
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
		
		public void enable()
		{
			this._enabled = true;
			if (this.controller != null)
				this.controller.enable();
			
			for (int i = 0; i < this.everything.Count; i++)
				if (this.everything[i].controller != null)
					this.everything[i].controller.enable();
		}
		
		public void disable()
		{
			this._enabled = false;
			if (this.controller != null)
				this.controller.disable();
			
			for (int i = 0; i < this.everything.Count; i++)
				if (this.everything[i].controller != null)
					this.everything[i].controller.disable();
		}
		
		public void setController(fEngineSceneController controller)
		{
			if (this._controller != null)
				this._controller.disable();
			this._controller = controller;
			this._controller.assignScene(this);
		}
		
		public fEngineSceneController getController()
		{
			return this._controller;
		}

		public void setCamera(fCamera camera)
		{
			if (this.currentCamera != null)
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
		
		public fCamera createCamera()
		{
			return new fCamera(this);
		}

		public BeingEntity createCharacter(uint charType, string def, float x, float y, float z, float orientation, uint layer = EntityCValue.SLObject)
		{
            fCell c = this.translateToCell(x, y, z);
			if (c == null)
			{
				string str = "无效的坐标(" + x + "," + y + ")。实际地图(像素)大小是(" + this.widthpx() + "," + this.heightpx() + ")";
				return null;
			}
            fFloor dist = getFloorAtByPos(x, y);
			if (dist == null)
			{
				return null;
			}

            String idchar = fUtil.elementID(this.engine.m_context, charType);

            XML definitionObject = <character id={idchar} definition={def} x={x} y={y} z={z} orientation={orientation}/>;

            BeingEntity nCharacter = new this.engine.m_context.m_typeReg.m_classes[charType](definitionObject, this);
			nCharacter.cell = c;
			nCharacter.m_district = dist;
			nCharacter.m_district.addCharacter(nCharacter.id);
			nCharacter.setDepth(c.zIndex);
			nCharacter.layer = layer;
			
			nCharacter.addEventListener(fElement.NEWCELL, this.processNewCell, false, 0, true);
			nCharacter.addEventListener(fElement.MOVE, this.renderElement, false, 0, true);
			
			this.characters.push(nCharacter);
			this.everything.push(nCharacter);
			this.all[nCharacter.id] = nCharacter;
			if (this.IAmBeingRendered)
			{
				this.addElementToRenderEngine(nCharacter);
				this.renderManager.processNewCellCharacter(nCharacter);
				this.render();
			}
			
			return nCharacter;
		}
		
		/**
		 * bDispose - true 销毁char, false - char脱离场景
		 */
		public void removeCharacter(fCharacter character, bool bDispose = true)
		{
			if (this.characters && this.characters.indexOf(character) >= 0)
			{
				this.characters.splice(this.characters.indexOf(character), 1);
				this.everything.splice(this.everything.indexOf(character), 1);
				this.all[character.id] = null;
			}
			
			if (bDispose)
			{
                character.hide();
			}

            character.removeEventListener(fElement.NEWCELL, this.processNewCell);
            character.removeEventListener(fElement.MOVE, this.renderElement);
			
			// Remove from render engine
			this.removeElementFromRenderEngine(character);
			// bug: 有时候卡的时候,时间间隔会很大,导致计算的 x 值很大或者很小,已经移出地图了
			if (character.m_district)
			{
                character.m_district.clearCharacter(character.id);
                character.m_district = null;
			}
			if (bDispose)
			{
                character.dispose();
			}
			else
			{
                character.onRemoveFormScene();
                character.scene = null;
			}
		}
		
		public void addCharacter(fCharacter nCharacter)
		{
            fCell c = this.translateToCell(0, 0, 0);
			if (c == null)
			{
				return;
			}
            fFloor dist = getFloorAtByPos(0, 0);
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

        // KBEN: 创建特效，这个是创建加入场景的特效，例如飞行特效，speed 速度大小 
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
		
		public function render():void
		{
			
		}
		
		public function startRendering():void
		{
			if (this.IAmBeingRendered)
				return;

			// Set flag
			this.IAmBeingRendered = true;
			// 开始渲染的时候才创建内部场景图      
			var k:uint = 0;
			while (k < EntityCValue.SLCnt)
			{
				m_SceneLayer[k] = new fSceneChildLayer();
				(m_SceneLayer[k] as fSceneChildLayer).m_layNo = k;
				container.addChild(m_SceneLayer[k]);
				++k;
			}
			
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

			jl = this.m_dynamicObjects.length
			for (j = 0; j < jl; j++)
				this.addElementToRenderEngine(this.m_dynamicObjects[j]);
			
			// KBEN: 添加雾到场景     
			if (this.m_sceneConfig.fogOpened)
			{
				this.addElementToRenderEngineNoClip(m_fogPlane, false, true, true);
			}

			this.render();

		}

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
			var jl:int = this.floors.length;
			for (var j:int = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.floors[j], true);

			jl = this.objects.length;
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.objects[j], true);
			jl = this.characters.length;
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.characters[j], true);
			jl = this.emptySprites.length;
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.emptySprites[j], true);

			jl = this.m_dynamicObjects.length
			for (j = 0; j < jl; j++)
				this.removeElementFromRenderEngine(this.m_dynamicObjects[j]);
			
			// Stop render engine
			this.renderEngine.dispose();
			
			// Stop render manager
			this.renderManager.dispose();
			
			// 删除所有根节点下的资源
			var k:uint = 0;
			while (k < m_SceneLayer.length)
			{
				// 这里不用删除了，在 this.renderEngine.dispose(); 这个函数中处理了 
				m_SceneLayer[k] = null;
				++k;
			}

			// Set flag
			this.IAmBeingRendered = false;
		}
		
		// Element enters new cell
		/** @private */
		public function processNewCell(evt:fNewCellEvent):void
		{
			if (this.engine.m_context.m_profiler)
				this.engine.m_context.m_profiler.enter("fScene.processNewCell");
			
			if (this.IAmBeingRendered)
			{
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
			
			return cell;
		}
		
		/** @private */
		public static function translateCoords(x:Number, y:Number, z:Number):Point
		{
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
		
		// Get elements visible from given cell, sorted by distance
		/** @private */
		public function getVisibles(cell:fCell, range:Number = Infinity):void		
		{
			var visibleFloor:Vector.<fFloor> = new Vector.<fFloor>();

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
			// 一定放在 this.renderEngine = null; 之前
			m_disposed = true;
			
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
			
			m_SceneLayer = null;

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