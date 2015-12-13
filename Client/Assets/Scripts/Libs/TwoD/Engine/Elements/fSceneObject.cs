namespace SDK.Lib
{
	public class fSceneObject : fObject 
	{
		public var scene:fScene;
		public function fSceneObject(defObj:XML, scene:fScene):void
		{
			this.scene = scene;
			super(defObj, scene.engine.m_context);
			
		}
		override public function moveTo(x:Number, y:Number, z:Number):void
		{			
			// Last position
			var dx:Number = this.x;
			var dy:Number = this.y;
			var dz:Number = this.z;
			
			// Set new coordinates			   
			this.x = x;
			this.y = y;
			this.z = z;
			
			// Check if element moved into a different cell
			var cell:fCell = this.scene.translateToCell(x, y, z);
			if (this.cell == null || cell == null || cell != this.cell)
			{
				this.cell = cell;
				dispatchEvent(new fNewCellEvent(fElement.NEWCELL));
			}
						
			// Dispatch event			
			this.dispatchEvent(new fMoveEvent(fElement.MOVE, this.x - dx, this.y - dy, this.z - dz));
		}
		// KBEN: 清除标签    
		public function clearFloorInfo(type:uint):void
		{
			if (this.scene.m_sceneConfig.optimizeCutting)
			{
				var floor:fFloor = this.scene.translateToFloor(this.x, this.y);
				if (type == EntityCValue.TEfffect)
				{
					//floor.clearDynamic(uniqueId);
					floor.clearDynamic(this.id);
				}
				else if (type == EntityCValue.TPlayer || type == EntityCValue.TVistNpc || type == EntityCValue.TBattleNpc || type == EntityCValue.TNpcPlayerFake)
				{
					//floor.clearCharacter(uniqueId);
					floor.clearCharacter(this.id);
				}
			}
		}

		public function updateDepth():void
		{			
			var c:fCell = (this.cell == null) ? (this.scene.translateToCell(this.x, this.y, this.z)) : (this.cell);
			var nz:Number = c.zIndex;
			this.setDepth(nz);
		}

		override public function set orientation(angle:Number):void
		{
			// KBEN: 选择对应的动画 			
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.Engine2d)
			{
				angle += 45;
			}			
			setOrientation(angle);
		}
		
		public function toUIPos():Point
		{
			return scene.convertToUIPos(this.x, this.y);
		}		
	}
}