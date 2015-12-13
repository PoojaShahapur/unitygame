namespace SDK.Lib
{
	public class fSceneRenderManager
	{
		// Properties
		private var scene:fScene; // Reference to the scene being managed
		public var range:Number; // The range of visible elements for the current viewport size
		private var depthSortArr:Array; // Array of elements for depth sorting
		
		// KBEN: 地形，以及一些静态地物可视化元素放在这里，不需要深度排序，可视化都是内部访问的   
		//public var _staticElementV:Array;
		// KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的 
		public var elementsV:Array; // An array of the elements currently visible
		// KBEN: 存放人物，需要深度排序        
		public var charactersV:Array; // An array of the characters currently visible,继续需要,点选使用
		//public var emptySpritesV:Array; // An array of emptySprites currently visible
		private var cell:fCell; // The cell where the camera is
		protected var m_preCell:fCell; // 这个是前一个摄像机所在的格子
		public var renderEngine:fEngineRenderEngine; // A reference to the render engine
		
		// Constructor
		public function fSceneRenderManager(scene:fScene):void
		{
			this.scene = scene;
			this.renderEngine = this.scene.renderEngine;
		}
		
		// Receives the viewport size for this scene
		public function setViewportSize(width:Number, height:Number):void
		{
			this.range = Math.sqrt(width * width + height * height) * 0.5; //(2*fEngine.DEFORMATION)
			if (this.range <= 0)
				this.range = Infinity;
			else
				this.range += 2 * this.scene.gridSize;
		}
		
		// This method is called when the scene is to be rendered and its render engine is ready
		public function initialize():void
		{
			this.depthSortArr = new Array;
			this.elementsV = new Array;
			this.charactersV = new Array;
			//this.emptySpritesV = new Array;
		
			// KBEN: 地面可视化   
			//_staticElementV = new Array();
		}
		
		// KBEN: 优化裁剪
		public function processNewCellCamera(cam:fCamera):void
		{
			// 如果摄像机没有初始化，就不处理
			if (!cam.m_bInit)
			{
				return;
			}
			// Init
			this.m_preCell = this.cell;
			this.cell = cam.cell;
			var x:Number, y:Number, z:Number;
			var tempElements:Vector.<fFloor>;
			
			// Camera enters new cell
			if (!this.cell.visibleElements || this.cell.visibleRange < this.range)
			{
				//this.scene.getVisibles(this.cell, this.range);
				this.scene.getVisibles(this.cell, this.range);
					//this.scene.getVisibles(this.cell, cam, this.range);
			}
			
			// 更新之前格子进入当前格子需要更新的区域
			if (!this.cell.m_updateDistrict[this.m_preCell])
			{
				this.cell.updateByPreInCur(this.m_preCell);
			}
			
			// 这个现在存放必须更新的区域，如果这个区域在上一个格子也是完全可见的，那么这个格子就不用更新了
			//tempElements = this.cell.m_visibleFloor;
			tempElements = this.cell.m_updateDistrict[this.m_preCell];
			
			var nEl:int;
			var nElements:int = tempElements.length;
			var i2:int = 0;
			
			var distidx:int = 0;
			var floor:fFloor;
			var ele:fRenderableElement;
			
			// tempElements 这里面的地形区域必然是可见的
			for (i2 = 0; i2 < nElements; i2++)
			{
				floor = tempElements[i2];				
				//newElementsV.push(ele);		// 一定要重新放进另外一个列表中
				floor.willBeVisible = false;
				if (!floor.isVisibleNow && floor._visible)
				{
					// Add asset
					floor.isVisibleNow = true;
					this.renderEngine.showElement(floor);
					
				}
				floor.showObject(this.cell);
			}
			
			// 处理隐藏区域
			var hideDistList:Vector.<fFloor> = this.cell.m_hideDistrict[this.m_preCell];
			nEl = hideDistList.length;
			for (i2 = 0; i2 < nEl; i2++)
			{
				floor = hideDistList[i2];				
				if (floor.isVisibleNow && floor._visible)
				{
					// 隐藏掉区域中的数据
					floor.hideObject();
					// Remove asset
					this.renderEngine.hideElement(floor);
					// KBEN: _floorV 不需要深度排序     
					//this.removeFromDepthSort(ele);
					// KBEN: 隐藏区域不需要深度排序
					// anyChanges = true;
					floor.isVisibleNow = false;
				}
			}
			
			this.scene.m_depthDirty = true; // KBEN: 必须重新排序，至于是否真的需要排序，在排序的时候检查
		}
		
		// Process
		public function processNewCellCharacter(character:fCharacter, needDepthsort:Boolean = true):void
		{
			if (this.scene.engine.m_context.m_profiler)
				this.scene.engine.m_context.m_profiler.enter("fScene.processNewCellCharacter");
			
			// 如果摄像机不可见就返回吧
			if (!this.scene.currentCamera.m_bInit)
			{
				return;
			}
			
			// If visible, we place it
			if (character._visible)
			{
				if (this.cell.m_scrollRect.contains(character.x, character.y))
				{
					// Create if it enters the screen
					if (!character.isVisibleNow)
					{
						this.charactersV[this.charactersV.length] = character;
						this.renderEngine.showElement(character);
						if (EntityCValue.SLBuild != character.layer) // 如果不是地物层
						{
							this.addToDepthSort(character);
						}
						character.isVisibleNow = true;
					}
				}
				else
				{
					// Destroy if it leaves the screen
					if (character.isVisibleNow)
					{
						var pos:int = this.charactersV.indexOf(character);
						this.charactersV.splice(pos, 1);
						this.renderEngine.hideElement(character);
						if (EntityCValue.SLBuild != character.layer) // 如果不是地物层
						{
							this.removeFromDepthSort(character);
						}
						character.isVisibleNow = false;
					}
				}
			}
			
			// Change depth of object
			if (character.cell != null)
			{
				// 只需要更新深度值，主角更新相机的时候会更新显示的
				// bug: 但是如果 processNewCellCamera 没有 m_depthDirty = true ，就会导致不排序，就会有点问题，但是可以减少一次排序
				if (EntityCValue.SLBuild != character.layer) // 如果不是地物层
				{
					character.setDepth(character.cell.zIndex, needDepthsort);
				}
			}
			
			if (this.scene.engine.m_context.m_profiler)
				this.scene.engine.m_context.m_profiler.exit("fScene.processNewCellCharacter");
		}
		
		// Process new cells for empty sprites
		public function processNewCellEmptySprite(spr:fEmptySprite, needDepthsort:Boolean = true):void
		{
			// If visible, we place it
			if (spr._visible)
			{						
				if (this.cell.m_scrollRect.contains(spr.x, spr.y))
				{
					// Create if it enters the screen
					if (!spr.isVisibleNow)
					{
						//this.emptySpritesV[this.emptySpritesV.length] = spr;
						this.renderEngine.showElement(spr);
						this.addToDepthSort(spr);
						spr.isVisibleNow = true;
					}
				}
				else
				{
					// Destroy if it leaves the screen
					if (spr.isVisibleNow)
					{
						//var pos:int = this.emptySpritesV.indexOf(spr);
						//this.emptySpritesV.splice(pos, 1);
						this.renderEngine.hideElement(spr);
						this.removeFromDepthSort(spr);
						spr.isVisibleNow = false;
					}
				}
			}
			
			// Change depth of object
			if (needDepthsort)
			{
				spr.updateDepth();
			}
		}
		
		// KBEN: 特效处理 
		public function processNewCellEffect(effect:EffectEntity):void
		{
			// 如果摄像机没有初始化，就不处理
			if (!this.scene.currentCamera.m_bInit)
			{
				return;
			}
			
			// If it goes outside the scene, destroy it
			if (effect.cell == null)
			{
				this.scene.removeEffect(effect);
				return;
			}
			
			// If visible, we place it
			if (effect._visible)
			{								
				// Inside range ?
				//if (effect.distance2d(x, y, z) < this.range)
				//if (this.scene.currentCamera.m_scrollRect.contains(effect.x, effect.y))
				if (this.cell.m_scrollRect.contains(effect.x, effect.y))
				{
					// Create if it enters the screen
					if (!effect.isVisibleNow)
					{
						this.elementsV[this.elementsV.length] = effect;
						this.renderEngine.showElement(effect);
						this.addToDepthSort(effect);
						effect.isVisibleNow = true;
					}
				}
				else
				{
					// Destroy if it leaves the screen
					if (effect.isVisibleNow)
					{
						var pos:int = this.elementsV.indexOf(effect);
						this.elementsV.splice(pos, 1);
						this.renderEngine.hideElement(effect);
						this.removeFromDepthSort(effect);
						effect.isVisibleNow = false;
					}
				}
			}
			
			effect.setDepth(effect.cell.zIndex);
		}

		// Listens to elements made visible and adds assets to display list if they are within display range
		public function showListener(evt:Event):void
		{
			this.addedItem(evt.target as fRenderableElement);
		}
		
		// Adds an element to the render logic
		public function addedItem(ele:fRenderableElement):void
		{
			try
			{
				// 如果相机还没有放到正确的位置，就不添加显示内容了，很可能要移动相机，结果内容全部清理一遍
				if (!this.scene.currentCamera.m_bInit)
				{
					return;
				}
				
				if (!ele.isVisibleNow && ele._visible && this.cell.m_scrollRect.contains(ele.x, ele.y))
				{
					ele.isVisibleNow = true;
					this.renderEngine.showElement(ele);
					// KBEN: fFloor 不参与深度排序 
					if (!(ele is fFloor))
					{
						// 地物层也不需要排序
						if (EntityCValue.SLBuild != ele.layer) // 如果不是地物层
						{
							this.addToDepthSort(ele);
						}
					}
					
					// KBEN: 这个地方需要修改   
					// KBEN: 玩家 npc 全部放在这里   
					//if (ele is fCharacter)
					if (ele is BeingEntity)
					{
						this.charactersV[this.charactersV.length] = ele as fCharacter;
					}
					else if (ele is EffectEntity)
					{
						this.elementsV[this.elementsV.length] = ele;
					}
					else if (ele is fObject)
					{
						// 掉落物    
						if ((ele as fSceneObject).m_resType == EntityCValue.PHFOBJ)
						{
							this.elementsV[this.elementsV.length] = ele;
						}
					}

					// Redo depth sort
					// KBEN: 地形不排序，只可视化剔除   
					if (!(ele is fFloor))
					{
						//this.depthSort();
						// KBEN: 需要重新排序，增加的时候需要深度排序
						if (EntityCValue.SLBuild != ele.layer) // 如果不是地物层
						{
							this.scene.m_depthDirty = true;
						}
					}
				}
			}
			catch (e:Error)
			{
				var strLog:String = "";
				if (scene)
				{
					strLog += "scene " + scene.m_serverSceneID + " ";
					if (scene.currentCamera)
					{
						strLog += "scene.currentCamera ";
					}
				}
				if (cell)
				{
					strLog += "cell ";
				}
				if (renderEngine)
				{
					strLog += "renderEngine ";
				}
				
				if (ele)
				{
					strLog += "ele ";
				}
				DebugBox.sendToDataBase(strLog + e.getStackTrace());
				
			}
		}
		
		// Listens to elements made invisible and removes assets to display list if they were within display range
		public function hideListener(evt:Event):void
		{
			this.removedItem(evt.target as fRenderableElement);
		}
		
		// Removes an element from the render logic
		public function removedItem(ele:fRenderableElement, destroyingScene:Boolean = false):void
		{
			var ch:BeingEntity;
			var pos:int;
			if (ele.isVisibleNow)
			{
				ele.isVisibleNow = false;
				// KBEN:    
				if (ele is fFloor)
				{
					this.renderEngine.hideElement(ele);
					// KBEN: fFloor 不参与深度排序   
					//this.removeFromDepthSort(ele);
					//}
				}
				else if (ele is BeingEntity)
				{
					// KBEN:   
					ch = ele as BeingEntity;
					pos = this.charactersV.indexOf(ch);
					if (pos >= 0)
					{
						this.charactersV.splice(pos, 1);
						this.renderEngine.hideElement(ele);
						this.removeFromDepthSort(ele);
					}
				}
				else if (ele is EffectEntity)
				{
					pos = this.elementsV.indexOf(ele);
					if (pos >= 0)
					{
						this.elementsV.splice(pos, 1);
						this.renderEngine.hideElement(ele);
						this.removeFromDepthSort(ele);
					}
				}
				else if (ele is this.scene.engine.m_context.m_typeReg.m_classes[EntityCValue.TFallObject])
				{
					pos = this.elementsV.indexOf(ele);
					if (pos >= 0)
					{
						this.elementsV.splice(pos, 1);
						this.renderEngine.hideElement(ele);
						this.removeFromDepthSort(ele);
					}
				}
				else
				{
					//throw new Error("[removedItem] type is not distinguish");
					this.renderEngine.hideElement(ele);
					this.removeFromDepthSort(ele);
					
					// Redo depth sort
					// KBEN: 需要深度排序,移除的时候不需要深度排序吧
					//if (!destroyingScene)
					//{
					//this.depthSort();
					//	this.scene.m_depthDirty = true;
					//}
				}
			}
		}
		
		// Adds an element to the depth sort array
		// KBEN: 不需要深度排序的不会调用这个函数   
		public function addToDepthSort(item:fRenderableElement):void
		{
			if (this.depthSortArr.indexOf(item) < 0)
			{
				this.depthSortArr.push(item);
				item.addEventListener(fRenderableElement.DEPTHCHANGE, this.depthChangeListener, false, 0, true);
			}
		}
		
		// Removes an element from the depth sort array
		public function removeFromDepthSort(item:fRenderableElement):void
		{
			this.depthSortArr.splice(this.depthSortArr.indexOf(item), 1);
			item.removeEventListener(fRenderableElement.DEPTHCHANGE, this.depthChangeListener);
		}

		public function depthChangeListener(evt:Event):void
		{
			// 如果深度排序应经需要重新排序了，就没有必然在单独排序自己了
			if (this.scene.m_depthDirty || !this.scene.m_sortByBeingMove)
			{
				return;
			}
			
			// 设置标示
			this.scene.m_depthDirtySingle = true;
			this.scene.m_singleDirtyArr.push(evt.target as fRenderableElement);
		}
		
		// 某一些单个改变的内容
		public function depthSortSingle():void
		{
			var ar:Array = this.depthSortArr;
			// KBEN: 深度排序
			fUtil.insortSort(this.depthSortArr);
			var i:int = ar.length;
			if (i == 0)
				return;
			// KBEN: 除了地形都排序
			var p:Sprite = this.scene.m_SceneLayer[EntityCValue.SLObject];
			for each (var el:fRenderableElement in this.scene.m_singleDirtyArr)
			{
				var oldD:int = el.depthOrder;
				// KBEN: 插入排序
				var newD:int = this.depthSortArr.indexOf(el);
				if (newD != oldD)
				{
					el.depthOrder = newD;
					// KBEN: 地形不排序，阴影需要排序
					// KBEN: 不需要深度排序的不会调用 addToDepthSort 这个函数，因此这里不用调用这个函数
					try
					{
						// 如果由于调整其它的位置，导致这个位置可能已经放在正确的位置了，就不再调整位置了
						if (p.getChildIndex(el.container) != newD)
						{
							p.setChildIndex(el.container, newD);
						}
					}
					catch (e:Error)
					{
						Logger.info(null, null, "depthSortSingle error");
					}
				}
			}
		}
		
		// Depth sorts all elements currently displayed
		public function depthSort():void
		{
			var ar:Array = this.depthSortArr;
			// KBEN: 深度排序
			//ar.sortOn("_depth", Array.NUMERIC);
			fUtil.insortSort(this.depthSortArr);
			var i:int = ar.length;
			if (i == 0)
				return;
			//var p:Sprite = this.scene.container;
			// KBEN: 除了地形都排序
			var p:Sprite = this.scene.m_SceneLayer[EntityCValue.SLObject];
			
			try
			{
				while (i--)
				{
					if (p.getChildAt(i) != ar[i].container)
					{
						p.setChildIndex(ar[i].container, i);
						ar[i].depthOrder = i;
					}
				}
			}
			catch (e:Error) // KBEN: 有时候竟然会莫名其妙的减少一个  
			{
				Logger.info(null, null, "depthSort error");
			}
		}
		
		// Frees resources
		public function dispose():void
		{
			if (this.depthSortArr)
			{
				var il:int = this.depthSortArr.length;
				for (var i:int = 0; i < il; i++)
					delete this.depthSortArr[i];
			}
			this.depthSortArr = null;
			if (this.elementsV)
			{
				il = this.elementsV.length;
				for (i = 0; i < il; i++)
					delete this.elementsV[i];
			}
			this.elementsV = null;
			if (this.charactersV)
			{
				il = this.charactersV.length;
				for (i = 0; i < il; i++)
					delete this.charactersV[i];
			}
			this.charactersV = null;

			this.cell = null;
			this.m_preCell = null;
		}
		
		public function set curCell(value:fCell):void
		{
			this.cell = value;
		}
		
		public function get curCell():fCell
		{
			return this.cell;
		}
		
		public function set preCell(value:fCell):void
		{
			this.m_preCell = value;
		}
	}
}