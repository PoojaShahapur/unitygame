namespace SDK.Lib
{
	public class fPlane : fRenderableElement
	{
		public static string NEWMATERIAL = "planenewmaterial";

		public fMaterial material;
		
		public float zIndex;
		
		private float planeWidth;
		private float planeHeight;
		public fScene scene;
		
		public fPlane(XmlNode defObj, fScene scene, float width, float  height)
		{
			this.scene = scene;
			super(defObj, scene.engine.m_context, defObj.@src.length() != 1);
			
			// 2 维
			this.planeWidth = width;
			this.planeHeight = height;

			if (defObj.@src.length() == 1 && defObj.@src.toString().length)
				this.assignMaterial(defObj.@src);
				
			_resDic[0] = new Vector.<SWFResource>(1, true);
		}
		
		public void assignMaterial(string id)
		{
			this.material = fMaterial.getMaterial(id, this.scene);
			this.dispatchEvent(new fNewMaterialEvent(fPlane.NEWMATERIAL, id, this.planeWidth, this.planeHeight));
		}
		
		public override void moveTo(float x, float y, float z)
		{
			throw new Exception("Exception: You can't move a fPlane. (" + this.id + ")");
		}

		public bool inFrontOf(fPlane p)
		{
			return false;
		}
		
		public void setZ(float zIndex)
		{
			this.zIndex = zIndex;
			this.setDepth(zIndex);
		}
		
		public void disposePlane()
		{
			// KBEN: 移除，否则会宕机
			if (_resDic[0] && _resDic[0][0])
			{
				_resDic[0][0].removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
				_resDic[0][0].removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
				
				//_resDic[0][0].decrementReferenceCount();
				//this.scene.engine.m_context.m_resMgrNoProg.unload(_resDic[0][0].filename, SWFResource);
				this.m_context.m_resMgr.unload(_resDic[0][0].filename, SWFResource);
				_resDic[0][0] = null;
			}
		
			_resDic[0] = null;
			delete _resDic[0];
				
			_resDic = null;
			
			this.material = null;
			this.disposeRenderable();
		}
		
		/** @private */
		public override void dispose()
		{
			this.disposePlane();
		}
		
		// KBEN: 主要用来加载图片资源   
		override public void loadRes(uint act, uint direction)
		{
			// 有些是没有材质的，就不加载了
			if(this.material == null)
				return;
			// KBEN: 这个就是图片加载，配置文件需要兼容两者，渲染文件单独写就行了 
			
			// 图片需要自己手工创建资源，启动解析配置文件的时候不再加载  
			var path:String;
			path = this.m_context.m_path.getPathByName(material.definition.mediaPath, m_resType);
			
			var res:SWFResource = this.m_context.m_resMgr.getResource(path, SWFResource) as SWFResource;
			if (!res)
			{
				_resDic[act][direction] = this.m_context.m_resMgr.load(path, SWFResource, onResLoaded, onResFailed);
			}
			else if(!res.isLoaded)
			{
				_resDic[act][direction] = res;
				res.incrementReferenceCount();
				res.addEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
				res.addEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
			}
			else
			{
				_resDic[act][direction] = res;
				res.incrementReferenceCount();
				onResLoaded(new ResourceEvent(ResourceEvent.LOADED_EVENT, res));
			}
		}
		
		// 资源加载成功     
		public void onResLoaded(ResourceEvent event)
		{
			event.resourceObject.removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
			event.resourceObject.removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
			
			var res:SWFResource = event.resourceObject as SWFResource;
			var r:fFlash9ElementRenderer = customData.flash9Renderer;
			// bug : 可能渲染器被卸载了资源才被加载进来，结果就宕机了 
			if (r != null)
			{
				r.init(res, 0, 0);
			}
			Logger.info(null, null, res.filename + "load loaded");
		}
		
		// 资源加载失败    
		public void onResFailed(ResourceEvent event)
		{
			event.resourceObject.removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
			event.resourceObject.removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
			
			//_resDic[0][0].decrementReferenceCount();
			_resDic[0][0] = null;
	
			//this.scene.engine.m_context.m_resMgrNoProg.unload(event.resourceObject.filename, SWFResource);
			this.m_context.m_resMgr.unload(event.resourceObject.filename, SWFResource);
	
			var res:SWFResource = event.resourceObject as SWFResource;
			Logger.error(null, null, res.filename + "load failed");
		}
	}
}