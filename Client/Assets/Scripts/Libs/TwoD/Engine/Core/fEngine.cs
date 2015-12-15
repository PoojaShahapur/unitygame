namespace SDK.Lib
{
	// KBEN: 引擎内部由于视口改变需要处理的都在这里 
	public class fEngine : EventDispatch
	{
		public SpriteContainer container;
		private MList<fScene> scenes;

		public fScene current;
		
		public fEngine(SpriteContainer container)
		{
			this.container = container;
			this.scenes = new MList<fScene>();
			this.current = null;
		}

		public fScene createScene(float width, float height, fEngineRenderEngine renderer = null)
		{
            fScene nfScene = new fScene(this, width, height, renderer);
			nfScene.initialize();

			this.scenes.Add(nfScene);
			return nfScene;
		}
		
		public void destroyScene(fScene sc)
		{
			this.hideScene(sc);
			sc.dispose();
			this.scenes.RemoveAt(this.scenes.IndexOf(sc));
			//objectPool.flush();
		}
		
		public void showScene(fScene sc)
		{	
			if (this.current == sc)
				return;
			
			if (this.current != null)
			{
				this.current.stopRendering();
				//this.container.removeChild(this.current.container);
			}
			
			this.current = sc;
			this.current.startRendering();
			this.current.enable();
			//this.container.addChild(sc.container);
		}
	
		public void hideScene(fScene sc, bool destroyRender = true)
		{
			if (this.current == sc)
			{
				if (destroyRender)
					this.current.stopRendering();
				//this.container.removeChild(this.current.container);
				this.current = null;
			}
		}

		// KBEN: 最后一帧调用
		public void onFrameEnd()
		{
			//if (current)
			//{
			//	current.onFrameEnd();
			//}

			//foreach(fScene iscene in scenes.list())
			//{
			//	if (iscene && iscene != current)
			//	{
			//		iscene.onFrameEnd();
			//	}
			//}
		}
	}
}