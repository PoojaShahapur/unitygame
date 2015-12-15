using System.Security;

namespace SDK.Lib
{
	public class fSceneObject : fObject 
	{
		public fScene scene;

		public fSceneObject(SecurityElement defObj, fScene scene)
            : base(defObj)
		{
			this.scene = scene;
		}

		override public void moveTo(float x, float y, float z)
		{			
			float dx = this.x;
            float dy = this.y;
            float dz = this.z;
			
			this.x = x;
			this.y = y;
			this.z = z;
			
			fCell cell = this.scene.translateToCell(x, y, z);
			if (this.cell == null || cell == null || cell != this.cell)
			{
				this.cell = cell;
				dispatchEvent(fElement.NEWCELL, new fNewCellEvent());
			}

			this.dispatchEvent(fElement.MOVE, new fMoveEvent(this.x - dx, this.y - dy, this.z - dz));
		}

		// KBEN: 清除标签
		public void clearFloorInfo(uint type)
		{
            //if (this.scene.m_sceneConfig.optimizeCutting)
            //{
            //    fFloor floor = this.scene.translateToFloor(this.x, this.y);
            //    floor.clearDynamic(this.id);
            //    floor.clearCharacter(this.id);
            //}
        }

        public void updateDepth()
		{
            fCell c = (this.cell == null) ? (this.scene.translateToCell(this.x, this.y, this.z)) : (this.cell);
			float nz = c.zIndex;
			this.setDepth(nz);
		}

		public void setOrientation(float angle)
		{
			// KBEN: 选择对应的动画 			
			angle += 45;
			setOrientation(angle);
		}
	}
}