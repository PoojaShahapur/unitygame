namespace SDK.Lib
{
	public class fFloor extends fPlane
	{
		// Private properties
		
		/** @private */
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
//			if (p is fWall)
//			{
//				var wall:fWall = p as fWall;
//				if (wall.vertical)
//				{
//					if ((this.i < wall.i && (this.j + this.gDepth) > wall.j && this.k > wall.k)
//						//|| ((this.j+this.gDepth)>wall.j && (this.i+this.gWidth)<=wall.i)
//						//|| (this.i<=wall.i && (this.j+this.gDepth)>wall.j && this.k>=(wall.k+wall.gHeight)) 
//						)
//						return true;
//					return false;
//				}
//				else
//				{
//					if ((this.i < (wall.i + wall.size) && (this.j + this.gDepth) > wall.j && this.k > wall.k)
//						//|| (this.i<(wall.i+wall.size) && this.j>=wall.j)
//						//|| (this.i<(wall.i+wall.size) && (this.j+this.gDepth)>wall.j && this.k>=(wall.k+wall.gHeight))
//						)
//						return true;
//					return false;
//				}
//			}
//			else
//			{
				var floor:fFloor = p as fFloor
				if ((this.i < (floor.i + floor.gWidth) && (this.j + this.gDepth) > floor.j && this.k > floor.k) || ((this.j + this.gDepth) > floor.j && (this.i + this.gWidth) <= floor.i) || (this.i >= floor.i && this.i < (floor.i + floor.gWidth) && this.j >= (floor.j + floor.gDepth)))
					return true;
				return false;
//			}
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
		//public function addDynamic(uniqueId:int, id:String):void
		public function addDynamic(id:String):void
		{
			//m_dynamicElementDic[uniqueId] = m_dynamicElementVec.length;
			//m_dynamicElement2Dic[m_dynamicElementVec.length] = uniqueId;
			//m_dynamicElementVec.push(id);
			
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
		
		//public function clearDynamic(uniqueId:int):void
		public function clearDynamic(id:String):void
		{
			//var arridx:int = m_dynamicElementDic[uniqueId];
			//m_dynamicElementDic[uniqueId] = null;
			//delete m_dynamicElementDic[uniqueId];
			
			//m_dynamicElement2Dic[arridx] = null;
			//delete m_dynamicElement2Dic[arridx];
			
			//m_dynamicElementVec.splice(arridx, 1);
			
			//var cnt:int = m_dynamicElementVec.length;
			//++arridx;
			//while (arridx < cnt)
			//{
			//	m_dynamicElementDic[m_dynamicElement2Dic[arridx]] = arridx - 1;
			//	m_dynamicElement2Dic[arridx - 1] = m_dynamicElement2Dic[arridx];
			//	m_dynamicElement2Dic[arridx] = null;
			//	delete m_dynamicElement2Dic[arridx];
				
			//	++arridx;
			//}
			
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
		
		//public function addCharacter(uniqueId:int, id:String):void
		public function addCharacter(id:String):void
		{
			//m_characterDic[uniqueId] = m_characterVec.length;
			//m_character2Dic[m_characterVec.length] = uniqueId;
			//m_characterVec.push(id);
			
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
		
		//public function clearCharacter(uniqueId:int):void
		public function clearCharacter(id:String):void
		{
			//var arridx:int = m_characterDic[uniqueId];
			//m_characterDic[uniqueId] = null;
			//delete m_characterDic[uniqueId];
			
			//m_character2Dic[arridx] = null;
			//delete m_character2Dic[arridx];
			
			//m_characterVec.splice(arridx, 1);
			
			//var cnt:int = m_characterVec.length;
			//++arridx;
			//while (arridx < cnt)
			//{
				//m_characterDic[m_character2Dic[arridx]] = arridx - 1;
				//m_character2Dic[arridx - 1] = m_character2Dic[arridx];
				//m_character2Dic[arridx] = null;
				//delete m_character2Dic[arridx];
				
				//++arridx;
			//}
			
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
		
		// public function addEmptySprite(uniqueId:int, id:String):void
		public function addEmptySprite(id:String):void
		{
			//m_emptySpriteDic[uniqueId] = m_emptyVec.length;
			//m_emptySprite2Dic[m_emptyVec.length] = uniqueId;
			//m_emptyVec.push(id);
			
			if(!m_emptySpriteDic[id])
			{
				m_emptySpriteDic[id] = 1;
				m_emptyVec.push(id);
			}
			else
			{
				throw new Event("emptySprite already in current floor");
			}
		}
		
		//public function clearEmptySprite(uniqueId:int):void
		public function clearEmptySprite(id:String):void
		{
			//var arridx:int = m_emptySpriteDic[uniqueId];
			//m_emptySpriteDic[uniqueId] = null;
			//delete m_emptySpriteDic[uniqueId];
			
			//m_emptySprite2Dic[arridx] = null;
			//delete m_emptySprite2Dic[arridx];
			
			//m_emptyVec.splice(arridx, 1);
			
			//var cnt:int = m_emptyVec.length;
			//++arridx;
			//while (arridx < cnt)
			//{
				//m_emptySpriteDic[m_emptySpriteDic[arridx]] = arridx - 1;
				//m_emptySprite2Dic[arridx - 1] = m_emptySprite2Dic[arridx];
				//m_emptySprite2Dic[arridx] = null;
				//delete m_emptySprite2Dic[arridx];
				
				//++arridx;
			//}
			
			if(m_emptySpriteDic[id])
			{
				m_emptySpriteDic[id] = null;
				delete m_emptySpriteDic[id];
				
				var idx:int = m_emptyVec.indexOf(id);
				m_emptyVec.splice(idx, 1);
			}
			else
			{
				throw new Event("emptySprite not in current floor");
			}
		}
		
		// 显示这个区域中的各种可显示内容
		//public function showObject(elementsV:Array, newDynObjectV:Array, charactersV:Array, newV:Array, emptySpritesV:Array, newEmptySpriteV:Array, range:Number):void
		//public function showObject(rendermgr:fSceneRenderManager, newDynObjectV:Array, newV:Array, newEmptySpriteV:Array):Boolean
		//public function showObject(newDynObjectV:Array, newV:Array, newEmptySpriteV:Array):Boolean
		//public function showObject(cell:fCell):Boolean
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
				//if (dynObject.distance2d(x, y, z) < this.scene.renderManager.range)
				//if (this.scene.currentCamera.m_scrollRect.contains(dynObject.x, dynObject.y))
				if (cell.m_scrollRect.contains(dynObject.x, dynObject.y))
				{
					//newDynObjectV[newDynObjectV.length] = dynObject;
					dynObject.willBeVisible = true;
					
					// 显示可视化对象
					dynObject.willBeVisible = false;
					if (!dynObject.isVisibleNow && dynObject._visible)
					{
						// Add asset
						this.scene.renderManager.elementsV[distidx = this.scene.renderManager.elementsV.length] = dynObject;
						this.scene.renderManager.renderEngine.showElement(dynObject);
						this.scene.renderManager.addToDepthSort(dynObject);
						dynObject.isVisibleNow = true;
						//anyChanges = true;
					}
				}
				else	// 如果不可见
				{
					//m_fullVisible = false;
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
				//if (character.distance2d(x, y, z) < this.scene.renderManager.range)
				//if (this.scene.currentCamera.m_scrollRect.contains(character.x, character.y))
				if (cell.m_scrollRect.contains(character.x, character.y))
				{
					//newV[newV.length] = character;
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
			
			// KBEN: 空精灵存放
			esLength = m_emptyVec.length;
			for (i2 = 0; i2 < esLength; i2++)
			{
				// Is emptysprite within range ?
				spr = scene.all[m_emptyVec[i2]];
				//if (spr.distance2d(x, y, z) < this.scene.renderManager.range)
				//if (this.scene.currentCamera.m_scrollRect.contains(spr.x, spr.y))
				if (cell.m_scrollRect.contains(spr.x, spr.y))
				{
					//newEmptySpriteV[newEmptySpriteV.length] = spr;
					spr.willBeVisible = true;
					
					spr.willBeVisible = false;
					if (!spr.isVisibleNow && spr._visible)
					{
						// Add asset
						this.scene.renderManager.renderEngine.showElement(spr);
						this.scene.renderManager.addToDepthSort(spr);
						spr.isVisibleNow = true;
						//anyChanges = true;
					}
					
					// 从显示列表中去掉
					//distidx = this.scene.renderManager.emptySpritesV.indexOf(spr);
					//if(distidx != -1)
					//{
					//	this.scene.renderManager.emptySpritesV.splice(distidx, 1);
					//}
				}
				else	// 如果不可见
				{
					//m_fullVisible = false;
					if (spr.isVisibleNow && spr._visible)
					{
						// Remove asset
						this.scene.renderManager.renderEngine.hideElement(spr);
						this.scene.renderManager.removeFromDepthSort(spr);
						//anyChanges = true;
						spr.isVisibleNow = false;	
					}
				}
			}
			
			//return anyChanges;
		}
		
		//public function hideObject():Boolean
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
			
			// KBEN: 空精灵存放
			esLength = m_emptyVec.length;
			for (i2 = 0; i2 < esLength; i2++)
			{
				// Is emptysprite within range ?
				spr = scene.all[m_emptyVec[i2]];
				if (spr.isVisibleNow && spr._visible)
				{
					// Remove asset
					this.scene.renderManager.renderEngine.hideElement(spr);
					this.scene.renderManager.removeFromDepthSort(spr);
					//anyChanges = true;
					spr.isVisibleNow = false;	
				}
			}

			//anyChanges = false;
			//return anyChanges;
		}
	}
}