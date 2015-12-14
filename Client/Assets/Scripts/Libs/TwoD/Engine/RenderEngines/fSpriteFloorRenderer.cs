namespace SDK.Lib
{
	public class fSpriteFloorRenderer : fSpriteElementRenderer
    {
		private float origWidth;
		private float origHeight;
		
		// public Rectangle scrollR;
		private float occlusionCount = 0;
		
		public fSpriteFloorRenderer(fSpriteRenderEngine rEngine, fPlane element, float width, float height)
            : base(rEngine, element)
		{
			this.scene = element.scene;
			
			this.origWidth = width;
			this.origHeight = height;

			//this.element.addEventListener(fPlane.NEWMATERIAL, this.newMaterial);
		}
 
		public void createAssets()
		{
            fPlane element = this.element as fPlane;
			element.loadRes(0, 0);
			this.assetsCreated = true;
		}

		public void destroyAssets()
		{
			//this.element.removeEventListener(fPlane.NEWMATERIAL, this.newMaterial);

            fPlane element = this.element as fPlane;
		}

		public override void show()
		{
			//this.containerParent.addChild(this.container);
		}

		public override void hide()
		{
			//try
			//{
			//	this.containerParent.removeChild(this.container);
			//}
			//catch (e:Error)
			//{
			//}
		}

		private void newMaterial(fNewMaterialEvent evt)
		{
			element.loadRes(0, 0);
		}
		
		public void disposePlaneRenderer()
		{
			this.destroyAssets();
			this.disposeRenderer();
		}
		
		public override void dispose()
		{
			this.disposePlaneRenderer();
		}

		// KBEN: 初始化所有的数据        
		//override public void init(SWFResource res, uint act, uint direction)
		//{
		//	m_imgInit = true;
  //          fPlane element = this.element as fPlane;
		//	element.material.cls.init(res);
		//}
	}
}