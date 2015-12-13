namespace SDK.Lib
{
	public class fCharacter : fSceneObject, fMovingElement
	{
		public static const COLLIDE:String = "charactercollide";
		
		/**
		 * The fCharacter.WALKOVER constant defines the value of the
		 * <code>type</code> property of the event object for a <code>characterwalkover</code> event.
		 * The event is dispatched when the character walks over a non-solid object of the scene
		 *
		 * @eventType characterwalkover
		 */
		public static const WALKOVER:String = "characterwalkover";
		
		/**
		 * The fCharacter.EVENT_IN constant defines the value of the
		 * <code>type</code> property of the event object for a <code>charactereventin</code> event.
		 * The event is dispatched when the character enters a cell where an event was defined
		 *
		 * @eventType charactereventin
		 */
		public static const EVENT_IN:String = "charactereventin";
		
		/**
		 * The fCharacter.EVENT_OUT constant defines the value of the
		 * <code>type</code> property of the event object for a <code>charactereventout</code> event.
		 * The event is dispatched when the character leaves a cell where an event was defined
		 *
		 * @eventType charactereventout
		 */
		public static const EVENT_OUT:String = "charactereventout";
		
		// Public properties
		
		/**
		 * This value goes from 0 to 100 and indicates the alpha strenght of the "hole" that is opened in planes that cover this character
		 * "Cover" means literally, onscreen. This allows you to see what you are doing behind a wall. The default "100" value disables this effect
		 */
		//public var occlusion:Number = 100;
		
		// Private properties
		
		/**
		 * Elements currently being occluded by this character
		 * @private
		 */
		//public var currentOccluding:Array;
		
		/**
		 * Numeric counter for fast Array lookups
		 * @private
		 */
		//public var counter:int;
		
		/**
		 * Array of render cache. For each light in the scene, a list of elements that are shadowed by this character at its current position
		 * @private
		 */
		//public var vLights:Array;
		
		/**
		 * Array of cells the character occupies
		 * @private
		 */
		//public var occupiedCells:Array;
		
		// Constructor
		/** @private */
		function fCharacter(defObj:XML, scene:fScene):void
		{
			// Previous
			super(defObj, scene);
			
			// Characters are animated always
			this.animated = true;
			
			// Lights
			//this.vLights = new Array;
			
			// Occlusion
			//this.currentOccluding = new Array;
			
			// Counter
			//this.counter = this.scene.characters.length;
			
			// Occupied cells
			//this.occupiedCells = new Array;
			if (!this.scene.ready)
				this.scene.addEventListener(fScene.LOADCOMPLETE, this.onSceneLoaded, false, 0, true);
			//else	// KBEN: 这个暂时去掉
			//	this.updateOccupiedCells();
		}
		
		/**
		 * Moves a character into a new position, ignoring collisions
		 *
		 * @param x: New x coordinate
		 *
		 * @param y: New y coordinate
		 *
		 * @param z: New z coordinate
		 *
		 */
		public function teleportTo(x:Number, y:Number, z:Number):void
		{
			//var s:Boolean = this.solid;
			//this.solid = false;
			this.moveTo(x, y, z);
			//this.solid = s;
		}
		
		/*
		 * Characters can be moved
		 *
		 * @param x: New x coordinate
		 *
		 * @param y: New y coordinate
		 *
		 * @param z: New z coordinate
		 *
		 */ /** @private */
		// 从一个点直接跳到另外一个点   
		public override function moveTo(x:Number, y:Number, z:Number):void
		{			
			// KBEN: 防止 z 小于 0 
			if (z < 0)
			{
				z = 0;
			}
			// Last position
			var lx:Number = this.x;
			var ly:Number = this.y;
			var lz:Number = this.z;
			
			// Movement
			var dx:Number = x - lx;
			var dy:Number = y - ly;
			var dz:Number = z - lz;
			
			// bug: 这个地方如果返回，没有 exit 
			if (dx == 0 && dy == 0 && dz == 0)
			//if (dx == 0 && dy == 0)
			{
				return;
			}
			
			try
			{
				// Set new coordinates			   
				this.x = x;
				this.y = y;
				this.z = z;
				
				var radius:Number = this.radius;
				var height:Number = this.height;
				
				this.top = this.z + height;
				
				// Check for collisions against other fRenderableElements.
				// collisionSolver.solveCharacterCollisions() tests a character's collisions at its current position, generates collision events (if any)
				// and moves the character into a valid position if necessary.
				// KBEN: 去掉阻挡     
				//if (this.solid)
				//	fCollisionSolver.solveCharacterCollisions(this, dx, dy, dz);
				
				// Check if element moved into a different cell
				var cell:fCell = this.scene.translateToCell(this.x, this.y, this.z);
				
				if (cell != this.cell || this.cell == null)
				{
					// Check for XML events in cell we leave
					// KBEN: 暂时不用
					//if (this.cell != null)
					//{
					//	var k:Number = this.cell.events.length;
					//	for (var i:Number = 0; i < k; i++)
					//	{
					//		var evt:fCellEventInfo = this.cell.events[i];
					//		if (cell.events.indexOf(evt) < 0)
					//			dispatchEvent(new fEventOut(fCharacter.EVENT_OUT, evt.name, evt.xml));
					//	}
					//}
					
					var lastCell:fCell = this.cell;
					this.cell = cell;
					//this.updateOccupiedCells();
					var newCell:fNewCellEvent = new fNewCellEvent(fElement.NEWCELL);
					if (lastCell && cell.y == lastCell.y)
					{						
						newCell.m_needDepthSort = false;
					}				
					dispatchEvent(newCell);
					
					// 继续判断是否是新的 district
					var dist:fFloor = this.scene.getFloorAtByPos(this.x, this.y);

					if(m_district == null || dist == null || m_district != dist)
					{
						// 到达新的区域没有什么好做的，仅仅是将自己的信息移动到新的区域
						if (m_district)
						{
							m_district.clearCharacter(this.id);
						}
						m_district = dist;
						if (m_district)
						{
							m_district.addCharacter(this.id);
						}
					}
					
					// Check for XML events in new cell
					// KBEN: 暂时不用了
					//if (this.cell != null && lastCell != null)
					//{
					//	k = this.cell.events.length;
					//	for (i = 0; i < k; i++)
					//	{
					//		evt = this.cell.events[i];
					//		if (lastCell.events.indexOf(evt) < 0)
					//			dispatchEvent(new fEventIn(fCharacter.EVENT_IN, evt.name, evt.xml));
					//	}
					//}
				}
				 
				// Dispatch move event
				if (this.x != lx || this.y != ly || this.z != lz)
					dispatchEvent(new fMoveEvent(fElement.MOVE, this.x - lx, this.y - ly, this.z - lz));
					
				// KBEN: 绘制雾    
				//this.scene.clearFog(this.x, this.y, this.z);
			}
			catch (e:Error)
			{
				// This means we tried to move outside scene limits
				this.x = lx;
				this.y = ly;
				this.z = lz;
				dispatchEvent(new fCollideEvent(fCharacter.COLLIDE, null));
			}
		}
		
		/** @private */
		public function disposeCharacter():void
		{
			var ll:int = 0;
			//var ll:int = this.scene.lights.length;
			//for (var j:int = 0; j < ll; j++)
			//{
			//	var light:fLight = this.scene.lights[j];
			//	if (light)
			//		light.vCharacters[this.counter] = null;
			//}
			
			/*for (var i:String in this.vLights)
				delete this.vLights[i];*/
			//ll = this.currentOccluding.length;
			//for (var k:uint = 0; k < ll; k++)
			//	delete this.currentOccluding[k];
			//this.currentOccluding = null;
			//this.vLights = null;
			
			// Clear out the old cells
			//this.clearOccupiedCells();
			//this.clearFloorInfo(EntityCValue.TPlayer);
			
			// KBEN: 这个一定要放在最后，他会销毁自身的很多变量，这些变量在销毁之前很可能会用到   
			this.disposeObject();
		}
		
		//在从场景对象(fScene)上移除时，调用此函数
		public function onRemoveFormScene():void
		{
			//this.clearOccupiedCells();
			//this.currentOccluding = new Array;
		}
		
		/** @private */
		public override function dispose():void
		{
			this.disposeCharacter();
			super.dispose();
		}
		
		private function onSceneLoaded(evt:fProcessEvent):void
		{
			//this.updateOccupiedCells();
		}
		
		/*
		// KBEN: 占据的单元格不再更新
		// Assigns a new list of occupied cells to this character. Thnx to Alex Stone
		private function updateOccupiedCells():void
		{
			// Retrieve new list of occupied cells
			var theCell:fCell = this.scene.translateToCell(this.x, this.y, this.z);
			var cells:Array = new Array;
			var cellRadius:int = int((this.radius / this.scene.gridSize) + 0.5);
			
			// Loop ranges
			var i1:int = theCell.i - cellRadius;
			if (i1 < 0)
				i1 = 0;
			var i2:int = theCell.i + cellRadius;
			var j1:int = theCell.j - cellRadius;
			if (j1 < 0)
				i1 = 0;
			var j2:int = theCell.j + cellRadius;
			//var k2:Number = (this.top / this.scene.levelSize);
			
			for (var i:int = i1; i <= i2; i++)
			{
				for (var j:int = j1; j <= j2; j++)
				{
					// bug: 由于没有第三维，因此这里每循环一次就加 1 个相同的    
					//for (var k:int = theCell.k; k <= k2; k++)
					//{
						//var newCell:fCell = this.scene.getCellAt(i, j, k);
						var newCell:fCell = this.scene.getCellAt(i, j, 0);
						if (newCell)
							cells[cells.length] = newCell;
					//}
				}
			}
			
			// Clear out the old cells
			this.clearOccupiedCells();
			
			// Update new cells
			this.occupiedCells = cells;
			// KBEN: 更新所在单元格中的人物
			var forEach:Function = function(item:*, index:int, array:Array):void
			{
				item.charactersOccupying[item.charactersOccupying.length] = this;
			}
			this.occupiedCells.forEach(forEach, this);
		}
		
		private function clearOccupiedCells():void
		{			
			if (occupiedCells == null)
			{
				return;
			}
			var filter:Function = function(item:*, index:int, array:Array):Boolean
			{
				if (item == this)
				{
					return false;
				}
				return true;
			}
			var forEach:Function = function(item:*, index:int, array:Array):void
			{
				item.charactersOccupying.filter(filter, this);
			}
			// bug: 这里面竟然有32 个 fCell，怎么一个玩家占据这么多单元格子啊  
			this.occupiedCells.forEach(forEach, this);
			this.occupiedCells = null;
		}
		*/
	}
}