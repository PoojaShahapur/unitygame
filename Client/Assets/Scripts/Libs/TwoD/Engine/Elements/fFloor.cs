namespace SDK.Lib
{
	public class fFloor : fPlane
	{
		public var gWidth:int;
		/** @private */
		public var gDepth:int;
		/** @private */
		public var i:int;
		/** @private */
		public var j:int;
		/** @private */
		public var k:int;
		
		// Public properties
		
		/**
		 * Floor width in pixels. Size along x-axis
		 */
		public var width:Number;
		
		/**
		 * Floor depth in pixels. Size along y-axis
		 */
		public var depth:Number;
		
		/** @private */
		public var bounds:fPlaneBounds;
		
		// KBEN: 存放这个区域中的动态改变的实体，查找使用 Dictionary ，遍历使用 Vector ，只有参与深度排序的内容才需要加入这里，方便快速剔除
		// 某个元素的uniqueid 放在数组的下标
		public var m_dynamicElementDic:Dictionary;
		public var m_characterDic:Dictionary;
		public var m_emptySpriteDic:Dictionary;
		// 某个元素的数组中的下标对应元素的uniqueid
		//public var m_dynamicElement2Dic:Dictionary;
		//public var m_character2Dic:Dictionary;
		//public var m_emptySprite2Dic:Dictionary;
		
		// KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
		public var m_dynamicElementVec:Vector.<String>;
		// KBEN: 存放人物，需要深度排序
		public var m_characterVec:Vector.<String>;	
		// KBEN: 地上物，不会改变的地上建筑
		public var m_staticVec:Vector.<String>;
		// KBEN: 空精灵存放
		public var m_emptyVec:Vector.<String>;
		//public var m_fullVisible:Boolean = false;	// 这个格子中的内容是否全部可见
		
		// Constructor
		/** @private */
		function fFloor(defObj:XML, scene:fScene):void
		{
			// Dimensions, parse size and snap to gride
			this.gWidth = int((defObj.@width / scene.gridSize) + 0.5);
			this.gDepth = int((defObj.@height / scene.gridSize) + 0.5);
			this.width = scene.gridSize * this.gWidth;
			this.depth = scene.gridSize * this.gDepth;
			
			// Previous
			super(defObj, scene, this.width, this.depth);
			m_resType = EntityCValue.PHTER;
			
			// Specific coordinates
			this.i = int((defObj.@x / scene.gridSize) + 0.5);
			this.j = int((defObj.@y / scene.gridSize) + 0.5);
			this.k = int((defObj.@z / scene.levelSize) + 0.5);
			this.x0 = this.x = this.i * scene.gridSize;
			this.y0 = this.y = this.j * scene.gridSize;
			this.top = this.z = this.k * scene.levelSize;
			this.x1 = this.x0 + this.width;
			this.y1 = this.y0 + this.depth;
			
			// KBEN: 面板显示包围盒 
			// Bounds
			this.bounds = new fPlaneBounds(this);
			var c1:Point = fScene.translateCoords(this.width, 0, 0);
			var c2:Point = fScene.translateCoords(this.width, this.depth, 0);
			var c3:Point = fScene.translateCoords(0, this.depth, 0);
			this.bounds2d = new Rectangle(0, c1.y, c2.x, c3.y - c1.y);
			
			// Screen area
			this.screenArea = this.bounds2d.clone();
			this.screenArea.offsetPoint(fScene.translateCoords(this.x, this.y, this.z));
		
			m_dynamicElementDic = new Dictionary();
			m_characterDic = new Dictionary();
			m_emptySpriteDic = new Dictionary();
			//m_dynamicElement2Dic = new Dictionary();
			//m_character2Dic = new Dictionary();
			//m_emptySprite2Dic = new Dictionary();
			
			m_dynamicElementVec = new Vector.<String>();
			m_characterVec = new Vector.<String>();
			m_emptyVec = new Vector.<String>();
			
			this.xmlObj = null;	// bug: 释放这个资源,总是释放不了这个 xml
		}
		
		// Is this floor in front of other plane ? Note that a false return value does not imply the opposite: None of the planes
		// may be in front of each other
		/** @private */
		public override function inFrontOf(p:fPlane):Boolean
		{
			var floor:fFloor = p as fFloor
			if ((this.i < (floor.i + floor.gWidth) && (this.j + this.gDepth) > floor.j && this.k > floor.k) || ((this.j + this.gDepth) > floor.j && (this.i + this.gWidth) <= floor.i) || (this.i >= floor.i && this.i < (floor.i + floor.gWidth) && this.j >= (floor.j + floor.gDepth)))
				return true;
			return false;
		}
		
		/** @private */
		public override function distanceTo(x:Number, y:Number, z:Number):Number
		{
			// Easy case
			if (x >= this.x && x <= this.x + this.width && y >= this.y && y <= this.y + this.depth)
				return ((this.z - z) > 0) ? (this.z - z) : -(this.z - z);
			
			var d2d:Number;
			if (y < this.y)
			{
				d2d = mathUtils.distancePointToSegment(new Point(this.x, this.y), new Point(this.x + width, this.y), new Point(x, y));
			}
			else if (y > (this.y + this.depth))
			{
				d2d = mathUtils.distancePointToSegment(new Point(this.x, this.y + this.depth), new Point(this.x + width, this.y + this.depth), new Point(x, y));
			}
			else
			{
				if (x < this.x)
					d2d = mathUtils.distancePointToSegment(new Point(this.x, this.y), new Point(this.x, this.y + this.depth), new Point(x, y));
				else if (x > this.x + this.width)
					d2d = mathUtils.distancePointToSegment(new Point(this.x + this.width, this.y), new Point(this.x + this.width, this.y + this.depth), new Point(x, y));
				else
					d2d = 0;
			}
			
			var dz:Number = z - this.z;
			return Math.sqrt(dz * dz + d2d * d2d);
		}
		
		/** @private */
		public function disposeFloor():void
		{
			this.bounds = null;
			this.disposePlane();
		}
		
		/** @private */
		public override function dispose():void
		{
			this.disposeFloor();
		}
		
		// KBEN: id 是 fElement.uniqueId 和 fElement.id     
		public function addDynamic(id:String):void
		{
			if(!m_dynamicElementDic[id])
			{
				m_dynamicElementDic[id] = 1;
				m_dynamicElementVec.push(id);
			}
			else
			{
				throw new Event("dynamic already in current floor");
			}
		}
		
		public function clearDynamic(id:String):void
		{
			if(m_dynamicElementDic[id])
			{
				m_dynamicElementDic[id] = null;
				delete m_dynamicElementDic[id];
				
				var idx:int = m_dynamicElementVec.indexOf(id);
				m_dynamicElementVec.splice(idx, 1);
			}
			else
			{
				throw new Event("dynamic not in current floor");
			}
		}
		
		public function addCharacter(id:String):void
		{
			if(!m_characterDic[id])
			{
				m_characterDic[id] = 1;
				m_characterVec.push(id);
			}
			else
			{
				throw new Event("dynamic already in current floor");
			}
		}
		
		public function clearCharacter(id:String):void
		{	
			if(m_characterDic[id])
			{
				m_characterDic[id] = null;
				delete m_characterDic[id];
				
				var idx:int = m_characterVec.indexOf(id);
				m_characterVec.splice(idx, 1);
			}
			else
			{
				throw new Event("character not in current floor");
			}
		}
		
		// 显示这个区域中的各种可显示内容
		public function showObject(cell:fCell):void
		{
			// 计算所有可视化显示的内容
			// KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
			var dynamicLength:uint;
			var dynObject:fObject;
			
			var i2:uint = 0;
			var chLength:int = 0;
			var character:fCharacter;
			
			var esLength:int = 0;
			var spr:fEmptySprite;
			
			//var anyChanges:Boolean = false;
			var distidx:int = -1;
			
			dynamicLength = m_dynamicElementVec.length;
			
			//m_fullVisible = true;		// 区域裁剪之前设置全部可见标志
			
			for (i2 = 0; i2 < dynamicLength; i2++)
			{
				// 动态对象设置可视化状态
				dynObject = scene.all[m_dynamicElementVec[i2]];
				// 距离裁剪改成矩形裁剪, bug: 这样判断这个区域上面的所有内容必然可见， (x, y, z) 是区域的位置
				if (cell.m_scrollRect.contains(dynObject.x, dynObject.y))
				{
					dynObject.willBeVisible = true;
					
					// 显示可视化对象
					dynObject.willBeVisible = false;
					if (!dynObject.isVisibleNow && dynObject._visible)
					{
						this.scene.renderManager.elementsV[distidx = this.scene.renderManager.elementsV.length] = dynObject;
						this.scene.renderManager.renderEngine.showElement(dynObject);
						this.scene.renderManager.addToDepthSort(dynObject);
						dynObject.isVisibleNow = true;
					}
				}
				else	// 如果不可见
				{
					if (dynObject.isVisibleNow && dynObject._visible)
					{
						// 从显示列表中去掉
						distidx = this.scene.renderManager.elementsV.indexOf(dynObject);
						if(distidx != -1)
						{
							this.scene.renderManager.elementsV.splice(distidx, 1);
						}
						// Remove asset
						this.scene.renderManager.renderEngine.hideElement(dynObject);
						this.scene.renderManager.removeFromDepthSort(dynObject);
						//anyChanges = true;
						dynObject.isVisibleNow = false;	
					}
				}
			}
			
			// KBEN: 存放人物，需要深度排序
			chLength = m_characterVec.length;
			for (i2 = 0; i2 < chLength; i2++)
			{
				// Is character within range ?
				character = scene.all[m_characterVec[i2]];
				if (cell.m_scrollRect.contains(character.x, character.y))
				{
					character.willBeVisible = true;

					character.willBeVisible = false;
					if (!character.isVisibleNow && character._visible)
					{
						this.scene.renderManager.charactersV[this.scene.renderManager.charactersV.length] = character; 
						// Add asset
						this.scene.renderManager.renderEngine.showElement(character);
						if(EntityCValue.SLBuild != character.layer)		// 如果不是地物层
						{
							this.scene.renderManager.addToDepthSort(character);
						}
						character.isVisibleNow = true;
						//anyChanges = true;
					}
				}
				else	// 如果不可见
				{
					//m_fullVisible = false;
					if (character.isVisibleNow && character._visible)
					{
						// 从显示列表中去掉
						distidx = this.scene.renderManager.charactersV.indexOf(character);
						if(distidx != -1)
						{
							this.scene.renderManager.charactersV.splice(distidx, 1);
						}
						// Remove asset
						this.scene.renderManager.renderEngine.hideElement(character);
						if(EntityCValue.SLBuild != character.layer)		// 如果不是地物层
						{
							this.scene.renderManager.removeFromDepthSort(character);
						}
						//anyChanges = true;
						character.isVisibleNow = false;	
					}
				}
			}
			
			// KBEN: 地上物，不会改变的地上建筑
		}
		
		public function hideObject():void
		{
			// 计算所有可视化显示的内容
			// KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
			var dynamicLength:uint;
			var dynObject:fObject;
			
			var i2:uint = 0;
			var chLength:int = 0;
			var character:fCharacter;
			
			var esLength:int = 0;
			var spr:fEmptySprite;
			
			//var anyChanges:Boolean = false;
			var distidx:int = -1;
			
			dynamicLength = m_dynamicElementVec.length;
			
			//m_fullVisible = false;		// 区域裁剪之前设置全部可见标志
			
			for (i2 = 0; i2 < dynamicLength; i2++)
			{
				// 动态对象设置可视化状态
				dynObject = scene.all[m_dynamicElementVec[i2]];
				if (dynObject.isVisibleNow && dynObject._visible)
				{
					// 从显示列表中去掉
					distidx = this.scene.renderManager.elementsV.indexOf(dynObject);
					if(distidx != -1)
					{
						this.scene.renderManager.elementsV.splice(distidx, 1);
					}
					
					// Remove asset
					this.scene.renderManager.renderEngine.hideElement(dynObject);
					this.scene.renderManager.removeFromDepthSort(dynObject);
					//anyChanges = true;
					dynObject.isVisibleNow = false;	
				}
			}
			
			// KBEN: 存放人物，需要深度排序
			chLength = m_characterVec.length;
			for (i2 = 0; i2 < chLength; i2++)
			{
				// Is character within range ?
				character = scene.all[m_characterVec[i2]];
				if (character.isVisibleNow && character._visible)
				{
					// 从显示列表中去掉
					distidx = this.scene.renderManager.charactersV.indexOf(character);
					if(distidx != -1)
					{
						this.scene.renderManager.charactersV.splice(distidx, 1);
					}
					// Remove asset
					this.scene.renderManager.renderEngine.hideElement(character);
					if(EntityCValue.SLBuild != character.layer)		// 如果不是地物层
					{
						this.scene.renderManager.removeFromDepthSort(character);
					}
					//anyChanges = true;
					character.isVisibleNow = false;	
				}
			}
			
			// KBEN: 地上物，不会改变的地上建筑
		}
	}
}