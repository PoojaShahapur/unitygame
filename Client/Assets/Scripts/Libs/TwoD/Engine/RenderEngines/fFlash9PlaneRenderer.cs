namespace SDK.Lib
{
	public class fFlash9PlaneRenderer : fFlash9ElementRenderer
	{
		private float origWidth;
		private float origHeight;
		
		// public Rectangle scrollR;
		private float occlusionCount = 0;
		
		public fFlash9PlaneRenderer(fFlash9RenderEngine rEngine, fPlane element, float width, float height)
            : base(rEngine, element)
		{
			this.scene = element.scene;
			
			this.origWidth = width;
			this.origHeight = height;

			this.element.addEventListener(fPlane.NEWMATERIAL, this.newMaterial, false, 0, true);
		}
 
		public override void createAssets()
		{
			// Retrieve diffuse map
			var element:fPlane = this.element as fPlane;
			
			this.diffuse = new Bitmap(null, "auto", true);
			if (this.scene.m_sceneConfig.mapType == fEngineCValue.EngineISO ||
				this.scene.m_sceneConfig.mapType == fEngineCValue.Engine25d)
			{
				this.diffuse.y = int(Math.round(element.bounds2d.y));
			}

			this.baseContainer = objectPool.getInstanceOf(Sprite) as Sprite;
			// KBEN: 
			this.baseContainer.addChild(this.diffuse);
			
			// KBEN: 掩码层    
			this.finalBitmap = new Bitmap(null, "auto", true);

			this.baseContainer.mouseEnabled = false;
			
			// KBEN: 这个地方为什么要定时器呢，这个地方不处理了，等资源加载进来统一处理
			//this.undoCache();
			this.cacheTimer = new Timer(100, 1);
			this.cacheTimer.addEventListener(TimerEvent.TIMER, this.cacheTimerListener, false, 0, true);
			//this.cacheTimer.start();
			
			// KBEN: 加载资源就放在这里吧，这个地方需要判断一下，如果资源加载了，直接初始化，不要再走一遍加载了 
			element.loadRes(0, 0);
			// 如果有资源，element.loadRes(0, 0); 就会设置
			if(!this.diffuse.bitmapData)
			{
				initThumbnails();
			}
			
			// 这个放在这吧，不放在 init 函数里面了，因为如果创建 character 后直接 hideElement ，这个 xml 资源可能加载了，但是图片资源可能没加载，就不能设置 this.assetsCreated = true; 导致再次 showElement 的时候又会嗲用这个函数一次
			this.assetsCreated = true;
		}

		public override void destroyAssets()
		{
			// Cache
			this.undoCache();
			if (this.cacheTimer)
			{
				this.cacheTimer.stop();
				this.cacheTimer.removeEventListener(TimerEvent.TIMER, this.cacheTimerListener);
				this.cacheTimer = null;
			}
			
			// bug 没有释放资源
			this.element.removeEventListener(fPlane.NEWMATERIAL, this.newMaterial);

			var element:fPlane = this.element as fPlane;

			this.diffuse = null;
			// 这个就不释放了,这个 BitmapData 就是 fTileMaterial 这个材质里面释放的
			if(!m_imgInit)
			{
				if (this.diffuseData)
					this.diffuseData.dispose();
			}
			this.diffuseData = null;
			// Shadows
			this.resetShadows();
		
			// Return to object pool
			fFlash9RenderEngine.recursiveDelete(this.baseContainer);
			objectPool.returnInstance(this.baseContainer);
			if (lightC)
			{
				objectPool.returnInstance(this.lightC);
			}
			objectPool.returnInstance(this.occlusionLayer);
			
			if (this.finalBitmap)
				this.finalBitmap.mask = null;

			this.finalBitmap = null;
			if (this.finalBitmapData)
			{
				this.finalBitmapData.dispose();
				this.finalBitmapData = null;
			}
			this.lightC = null;
			//this.simpleHolesC = null;
			this.black = null;
			this.environmentC = null;
			this.baseContainer = null;
		}

		public void cacheTimerListener(TimerEvent evt)
		{
			this.doCache();
		}

		public override void show()
		{
			this.containerParent.addChild(this.container);
		}

		public override void hide()
		{
			try
			{
				this.containerParent.removeChild(this.container);
			}
			catch (e:Error)
			{
			}
		}

		private void newMaterial(fNewMaterialEvent evt)
		{
			element.loadRes(0, 0);
		}

		public override void startOcclusion(fCharacter character)
		{
			if (this.occlusionCount == 0)
			{
				this.container.addChild(this.occlusionLayer);
				this.disableMouseEvents();
			}
			this.occlusionCount++;
			
			// Create spot if needed
			if (!this.occlusionSpots[character.uniqueId])
			{
				var spr:Sprite = objectPool.getInstanceOf(Sprite) as Sprite;
				spr.mouseEnabled = false;
				spr.mouseChildren = false;
				
				var size:Number = (character.radius > character.height) ? character.radius : character.height;
				size *= 1.5;
				//movieClipUtils.circle(spr.graphics, 0, 0, size, 50, 0xFFFFFF, character.occlusion);
				this.occlusionSpots[character.uniqueId] = spr;
			}
			
			this.occlusionLayer.addChild(this.occlusionSpots[character.uniqueId]);
		}

		public override void updateOcclusion(fCharacter character)
		{
			var spr:Sprite = this.occlusionSpots[character.uniqueId];
			if (!spr)
				return;
			var p:Point = new Point(0, -character.height / 2);
			p = character.container.localToGlobal(p);
			p = this.occlusionLayer.globalToLocal(p);
			spr.x = p.x;
			spr.y = p.y;
		}

		public override void stopOcclusion(fCharacter character)
		{
			if (!this.occlusionSpots[character.uniqueId])
				return ;
			this.occlusionLayer.removeChild(this.occlusionSpots[character.uniqueId]);
			this.occlusionCount--;
			if (this.occlusionCount == 0)
			{
				this.enableMouseEvents();
				this.container.removeChild(this.occlusionLayer);
			}
		}

		public override void disableMouseEvents()
		{
			this.container.mouseEnabled = false;
			this.spriteToDraw.mouseEnabled = false;
		}

		public override void enableMouseEvents()
		{
			this.container.mouseEnabled = true;
			this.spriteToDraw.mouseEnabled = true;
		}
		
		public void disposePlaneRenderer()
		{
			// Assets
			this.destroyAssets();
			
			this.clipPolygon = null
			this.spriteToDraw = null;
			
			this.disposeRenderer();
		}
		
		public override void dispose()
		{
			this.disposePlaneRenderer();
		}

		// KBEN: 初始化所有的数据        
		override public void init(SWFResource res, uint act, uint direction)
		{
			// 释放之前的缩略图
			if(!m_imgInit)
			{
				if (diffuseData)
				{
					this.diffuseData.dispose();
					this.diffuseData = null;
				}
			}

			m_imgInit = true;
			var element:fPlane = this.element as fPlane;
			element.material.cls.init(res);			

			// 初始化自己的数据    
			var d:BitmapData = element.material.getDiffuse(element,this.origWidth,this.origHeight,true)
			if (d)
			{
				this.diffuseData = d; 	// 赋值资源
				if (!this.diffuse)
				{
					this.diffuse = new Bitmap(diffuseData, "auto", true);
				}
				else
				{
					this.diffuse.bitmapData = diffuseData;
				}

				this.container.visible = true;
			}
			else
			{
				this.diffuse.bitmapData = new BitmapData(1, 1, false, 0);
				this.diffuseData = null;
				this.container.visible = false;
			}
			
			// KBEN: 初始化完成后，缓存一下数据 
			this.undoCache(true);
		}
	}
}