namespace SDK.Lib
{
	public class fFlash9FloorRenderer : fFlash9PlaneRenderer
	{	
		public fFlash9FloorRenderer(fFlash9RenderEngine rEngine, fElementContainer container, fFloor element)
		{
            Sprite destination = objectPool.getInstanceOf(Sprite) as Sprite;
			container.addChild(destination);
			
			this.scrollR = new Rectangle(0, 0, element.width, element.depth);
			this.planeDeform = new Matrix(); //fFlash9FloorRenderer.matrix

			super(rEngine, element, element.width, element.depth, destination, container);
			
			// KBEN: 这个要放在父构造函数之后，这样 scene 才不会为空 
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO || 
			    this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				this.planeDeform.rotate(-45 * Math.PI / 180);
				this.planeDeform.scale(1.0015, 0.501);
			}
			
			// Clipping viewport
			this.vp = new vport();
			this.vp.x_min = element.x;
			this.vp.x_max = element.x + element.width;
			this.vp.y_min = element.y;
			this.vp.y_max = element.y + element.depth;
		}

		public void disposeFloorRenderer()
		{
			this.disposePlaneRenderer();
		}
		
		public override void dispose()
		{
			this.disposeFloorRenderer();
		}
	}
}