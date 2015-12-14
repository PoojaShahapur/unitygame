using System.Security;

namespace SDK.Lib
{
	public class fObject : fRenderableElement
	{
		// KBEN: 模板定义信息，内部数据最终会填写成实例化的数据使用  
		public fObjectDefinition definition;

		public float _orientation;

		private bool _hasUpdateTagBounds2d; //true 已经更新过m_tagBounds2d。角色建立后，只需更新m_tagBounds2d一次
		
		// KBEN: 这个是模板定义 ID  
		public string definitionID = "";
		// KBEN: 这个是实例化 ID 
		public string m_insID = "";

		public bool animated = false;
		
		public static string NEWORIENTATION = "objectNewOrientation";
		
		public static string GOTOANDPLAY = "objectGotoAndPlay";

		public static string GOTOANDSTOP = "objectGotoAndStop";
		
		// KBEN: 记录当前的状态，默认待机状态    
		protected uint m_state = EntityCValue.TStand;
		// KBEN: 帧率，这个定义在 fObjectDefinition 这里面，然后自己保存，以方便更改 
		protected float _leftInterval = 0; // KBEN: 动画播放剩余的时间
		public float _totalTime = 0; // KBEN: 动画播放的总的时间，插值计算帧

		// 是否配置文件已经初始化 
		protected bool m_binsXml = false;
		// 所有的偏移都在这里
		public fActDirOff m_ModelActDirOff = null;
		// 特效使用来偏移中心，人物调整中心位置，这个只是某一个方向时候为了不使用中间变量使用的值
		protected Point m_origLinkOff;		// 这个保存的是最初的偏移，因为编辑器中会修改这个值，因此保存一份最初的
		protected Point m_LinkOff = null;
		protected int m_preAct = -1;		// 为了模型偏移只计算一次，前一次动作
		protected int m_preDir = -1;		// 为了模型偏移只计算一次，前一次方向
		protected int m_preFrame = -1;		// 前一次帧

		public fObject(SecurityElement defObj)
            : base(defObj)
		{
			var str:String = defObj.@definition;
			
			// this.definitionID 组成是这样子的， "aaa-bbb" : aaa 是模板的 ID ， bbb 是实例 ID 
			var delimit:int = defObj.@definition.indexOf("_");
			if (delimit != -1)
			{
				this.definitionID = str.substring(0, delimit);
				this.m_insID = str.substring(delimit + 1, str.length);
			}
			else
			{
				this.definitionID = defObj.@definition;
				this.m_insID = "";
			}
			
			// 现在自己保存一份拷贝，这样方便修改
			var srcdef:fObjectDefinition = this.m_context.m_sceneResMgr.getObjectDefinition(this.definitionID);
			
			if (!srcdef)
			{
			}
			
			this.definition = new fObjectDefinition(srcdef.xmlData, srcdef.basepath);
			
			// Initialize rotation for this object
			this._orientation = 0;
			
			// Is it animated ?
			if (defObj.@animated.length() == 1)
				this.animated = (defObj.@animated.toString() == "true");
			
			this.top = this.z + this.height;
			this.x0 = this.x - this.radius;
			this.x1 = this.x + this.radius;
			this.y0 = this.y - this.radius;
			this.y1 = this.y + this.radius;
			
			// KBEN: 宽高需要自己从定义中指定 
			// 这些每一帧切换图片的时候都需要更新的，初始值设置为 0 就行了
			var w:Number = this.definition._width;
			var h:Number = this.definition._height;
			this.bounds2d = new Rectangle(-w / 2, -h, w, h);
			w = this.definition._width;
			h = this.definition.tagHeight;
			this.m_tagBounds2d = new Rectangle(-w / 2, -h, w, h);
			// KBEN: 宽高需要自己从定义中指定 
			
			// Screen area
			this.screenArea = this.bounds2d.clone();
			this.screenArea.offsetPoint(fScene.translateCoords(this.x, this.y, this.z));
			
			// Initial orientation
			if (defObj.@orientation.length() > 0)
				this.orientation = new Number(defObj.@orientation[0]);
			else
				this.orientation = 0;
			
			// 加载自己定义的内容 
			loadObjDefRes();
		}
		
		public override float distanceTo(float x, float y, float z)
		{
            float n1 = mathUtils.distance(x, y, this.x, this.y);
            float n2 = mathUtils.distance(x, y, this.x, this.y);
			
			return (n1 < n2) ? n1 : n2;
		}
		
		/**
		 *		 positive Z
		 *		         |
		 *		         |      / positive X
		 *		         |    /
		 *		         |  /
		 *	   (0,0,0) X/
		 *		          \
		 *		            \
		 *		              \
		 *		                \ positive Y
		 */
		public void setAttrOrientation(float angle)
		{
			angle += 45;
			setOrientation(angle);
		}
		
		virtual public void setOrientation(float angle)
		{
			float correctedAngle = angle % 360;
			if (correctedAngle < 0)
				correctedAngle += 360;
			this._orientation = correctedAngle;
			correctedAngle /= 360;
			if (isNaN(correctedAngle))
				return;

            int newSprite = int(correctedAngle * this.definition.yCount);
			this.collisionModel.orientation = this.definition.angle(newSprite);
			
			this.dispatchEvent(new Event(fObject.NEWORIENTATION));
		}
		
		public float getOrientation()
		{
			return this._orientation;
		}
		
		public void setHeight(h)
		{
			this.collisionModel.height = h;
			this.top = this.z + h;
		}
		
		public float getHeight()
		{
			return this.collisionModel.height;
		}

		public float getRadius()
		{
			return this.collisionModel.getRadius();
		}
		
		public uint getState()
		{
			return m_state;
		}
		
		public void setState(uint value)
		{
			if (m_state == value)
			{
				return;
			}
			onStateChange(m_state, value);
			m_state = value;
			this.gotoAndPlay(state2StateStr(m_state));
		}
		
		public void onStateChange(uint oldState, uint newState)
		{
			
		}
		
		public float getLeftInterval()
		{
			return _leftInterval;
		}
		
		public void setLeftInterval(float value)
		{
			_leftInterval = value;
		}
		
		// KBEN: 这个就是切换状态的函数    
		public override void gotoAndPlay(string where)
		{
			this.dispatchEvent(new Event(fObject.GOTOANDPLAY));
		}

		// KBEN: 这个也是切换状态，有点多余，切换到站立状态的时候就是这个函数   
		public override void gotoAndStop(string where)
		{
			// KBEN: 记录人物状态 
			m_state = convStateStr2ID(where);
			// Dispatch event so the render engine updates the screen
			this.dispatchEvent(new Event(fObject.GOTOANDSTOP));
		}
		
		public override void moveTo(float x, float y, float z)
		{
			
		}
		
		/** @private */
		public void disposeObject()
		{
			// KBEN: 移除，否则会宕机
			// bug: 这个地方会遍历 7 * 8 这么多次数
			string dir;
			for (String key in _resDic)
			{
				for(dir in _resDic[key])
				{
					if (_resDic[key][dir])
					{
						_resDic[key][dir].removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
						_resDic[key][dir].removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);

						if (this.m_context.m_resMgr.getResource(_resDic[key][dir].filename, SWFResource))
						{
							this.m_context.m_resMgr.unload(_resDic[key][dir].filename, SWFResource);
						}
						_resDic[key][dir] = null;
					}
				}
				
				_resDic[key] = null;
			}
			_resDic = null;
			
			// 释放加载的定义文件，否则特效释放后资源才加载进来，就会宕机   
			if (this.m_ObjDefRes)
			{
				this.m_ObjDefRes.removeEventListener(ResourceEvent.LOADED_EVENT, onObjDefResLoaded);
				this.m_ObjDefRes.removeEventListener(ResourceEvent.FAILED_EVENT, onObjDefResFailed);
				this.m_context.m_resMgr.unload(this.m_ObjDefRes.filename, SWFResource);
				this.m_ObjDefRes = null;
			}
			
			this.definition = null;
		}

		public override void dispose()
		{
			this.disposeObject();
			base.dispose();
		}
		
		public string state2StateStr(uint state)
		{
			switch (state)
			{
				case EntityCValue.TStand: 
					return EntityCValue.TSStand;
				case EntityCValue.TRun: 
					return EntityCValue.TSRun;
				case EntityCValue.TJump: 
					return EntityCValue.TSJump;
				case EntityCValue.TAttack: 
					return EntityCValue.TSAttack;
				case EntityCValue.THurt: 
					return EntityCValue.TSHurt;
				case EntityCValue.TDie: 
					return EntityCValue.TSDie;
				case EntityCValue.TDaZuo: 
					return EntityCValue.TSDaZuo;
				default: 
					return EntityCValue.TSStand;
			}
		}
		
		public uint getAction()
		{
			switch (m_state)
			{
				case EntityCValue.TStand: 
					return EntityCValue.TActStand;
				case EntityCValue.TRun: 
					return EntityCValue.TActRun;
				case EntityCValue.TJump: 
					return EntityCValue.TActJump;
				case EntityCValue.TAttack: 
					return EntityCValue.TActAttack;
				case EntityCValue.THurt: 
					return EntityCValue.TActHurt;
				case EntityCValue.TDie: 
					return EntityCValue.TActDie;
				case EntityCValue.TDaZuo: 
					return EntityCValue.TActDaZuo;
				default: 
					return EntityCValue.TActStand;
			}
		}
		
		// KBEN:转换状态字符串到对应的状态数字   
		public static uint convStateStr2ID(string str)
		{
			if (EntityCValue.TSStand == str)
			{
				return EntityCValue.TStand;
			}
			else if (EntityCValue.TSRun == str)
			{
				return EntityCValue.TRun;
			}
			else if (EntityCValue.TSJump == str)
			{
				return EntityCValue.TJump;
			}
			else if (EntityCValue.TSAttack == str)
			{
				return EntityCValue.TAttack;
			}
			else if (EntityCValue.TSHurt == str)
			{
				return EntityCValue.THurt;
			}
			else if (EntityCValue.TSDie == str)
			{
				return EntityCValue.TDie;
			}
			else if (EntityCValue.TSDaZuo == str)
			{
				return EntityCValue.TDaZuo;
			}
			
			return EntityCValue.TStand;
		}
		
		// KBEN: 主要用来加载图片资源   
		override public void loadRes(uint act, uint direction)
		{
			// bug: 如果一个 fObject 正在加载配置文件,在没有加载完成的时候如果在此调用这个函数,就会导致资源的引用计数增加,但是监听器只有一个,导致资源卸载不了
			if(this._resDic[act] && this._resDic[act][direction])	// 说明资源正在加载,但是没有加载完成,如果在此加载只能增加引用计数,但是监听器没有增加
			{
				return;
			}

            // KBEN: 这个就是图片加载，配置文件需要兼容两者，渲染文件单独写就行了      
            // 图片需要自己手工创建资源，启动解析配置文件的时候不再加载
            // 注意 load 中如果直接调用 onResLoaded ，可能这个时候 _resDic 中对应 key 的内容还没有放到 _resDic 中 
            string path;
			// 有时候如果没有资源这个值就是 null
			if (this.definition.dicAction[act].directDic[direction].mediaPath)
			{
				path = this.m_context.m_path.getPathByName(this.definition.dicAction[act].directDic[direction].mediaPath, m_resType);
			}
			
			// 路径存在才加载资源
			if (path)
			{
				//_resDic[act] ||= new Vector.<SWFResource>(8, true);
				_resDic[act] ||= new Dictionary();
				
				var mirrordir:uint = 0; // 映射的方向
				mirrordir = fUtil.getMirror(direction);
				
				var res:SWFResource = this.m_context.m_resMgr.getResource(path, SWFResource) as SWFResource;
				if (!res)
				{
					_resDic[act][direction] = this.m_context.m_resMgr.load(path, SWFResource, onResLoaded, onResFailed);
					if (mirrordir != direction)
					{
						_resDic[act][mirrordir] = _resDic[act][direction];
						_resDic[act][mirrordir].incrementReferenceCount();		// 添加引用计数，释放的时候，这两个方向都会释放一次的
					}
				}
				else if (!res.isLoaded)
				{
					_resDic[act][direction] = res;
					res.incrementReferenceCount();
					res.addEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
					res.addEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
					
					if (mirrordir != direction)
					{
						_resDic[act][mirrordir] = _resDic[act][direction];
						_resDic[act][mirrordir].incrementReferenceCount();		// 添加引用计数，释放的时候，这两个方向都会释放一次的
					}
				}
				else if (!res.didFail) // bug: 加载成功才能设置 
				{
					_resDic[act][direction] = res;
					res.incrementReferenceCount();
					onResLoaded(new ResourceEvent(ResourceEvent.LOADED_EVENT, res));
					
					if (mirrordir != direction)
					{
						_resDic[act][mirrordir] = _resDic[act][direction];
						_resDic[act][mirrordir].incrementReferenceCount();		// 添加引用计数，释放的时候，这两个方向都会释放一次的
					}
				}
			}
		}
		
		// 资源加载成功     
		public void onResLoaded(ResourceEvent event)
		{
			event.resourceObject.removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
			event.resourceObject.removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
			
			//var act:uint = actByRes(event.resourceObject as SWFResource);
			var act:int = 0;
			var dir:int = 0;
			var mirrordir:uint = 0; // 映射的方向
			var curdir:uint = 0; // 模型当前方向
			
			// 资源加载成功
			act = int(fUtil.getActByPath(event.resourceObject.filename));
			dir = int(fUtil.getDirByPath(event.resourceObject.filename));
			mirrordir = fUtil.getMirror(dir);
			// 确定最终的方向可能是镜像
			// 也有可能是两个映射方向同时加载，这个时候优先初始化当前模型动作方向
			if (getAction() == act)
			{
				var render:fFlash9ElementRenderer = customData.flash9Renderer;
				// bug : 可能渲染器被卸载了资源才被加载进来，结果就宕机了，原来是主角过场景的时候从场景移除，结果 flash9Renderer 就为空了，结果这个时候资源加载进来了 
				if (render)
				{
					curdir = render.actDir;
					if (curdir == dir || curdir == mirrordir)
					{
						dir = curdir;
						render.init(event.resourceObject as SWFResource, act, dir);
					}
				}
			}
			
			Logger.info(null, null, event.resourceObject.filename + " loaded");
		}
		
		// 资源加载失败    
		public void onResFailed(ResourceEvent event)
		{
			event.resourceObject.removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
			event.resourceObject.removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
			
			Logger.error(null, null, event.resourceObject.filename + " failed");
			
			var act:int = 0;
			var dir:int = 0;
			var mirrordir:uint = 0; // 映射的方向
			
			// 资源加载成功
			act = int(fUtil.getActByPath(event.resourceObject.filename));
			dir = int(fUtil.getDirByPath(event.resourceObject.filename));
			
			mirrordir = fUtil.getMirror(dir);
			_resDic[act][dir] = null;
			delete _resDic[act][dir];
			
			if (_resDic[act][mirrordir])
			{
				_resDic[act][mirrordir] = null;
				delete _resDic[act][mirrordir];
			}
			
			this.m_context.m_resMgr.unload(event.resourceObject.filename, SWFResource);
			
			// 如果其他方向都没有
			var hasRes:Boolean = false;
			for(dir in _resDic[act])
			{
				if (_resDic[act][dir] != null)
				{
					hasRes = true;
					break;
				}
			}
			
			if (!hasRes)
			{
				_resDic[act] = null;
				delete _resDic[act];
			}
		}
		
		// 加载对象定义 xml 配置文件   
		override public void loadObjDefRes()
		{
			// bug: 如果一个 fObject 正在加载配置文件,在没有加载完成的时候如果在此调用这个函数,就会导致资源的引用计数增加,但是监听器只有一个,导致资源卸载不了
			if(this.m_ObjDefRes || m_binsXml)	// this.m_ObjDefRes 存在说明正在加载， m_binsXml 存在说明配置文件已经初始化完成
			{
				return;
			}
			var insdef:fObjectDefinition = this.m_context.m_sceneResMgr.getInsDefinition(this.m_insID);
			if (insdef)
			{
				initObjDef();
				
				// 如果可视，加载资源    
				if (this.customData.flash9Renderer && (this.customData.flash9Renderer as fFlash9ElementRenderer).screenVisible)
				{
					this.loadRes(this.getAction(), (this.customData.flash9Renderer as fFlash9ElementRenderer).actDir);
				}
			}
			else
			{
				var filename:String = "x" + this.m_insID;
				var type:int = fUtil.xmlResType(filename);
				filename = this.m_context.m_path.getPathByName(filename + ".swf", type);

				var res:SWFResource = this.m_context.m_resMgr.getResource(filename, SWFResource) as SWFResource;
				if (!res)
				{
					this.m_ObjDefRes = this.m_context.m_resMgr.load(filename, SWFResource, this.onObjDefResLoaded, this.onObjDefResFailed) as SWFResource;
				}
				else if (!res.isLoaded)
				{
					this.m_ObjDefRes = res;
					res.incrementReferenceCount();
					
					res.addEventListener(ResourceEvent.LOADED_EVENT, onObjDefResLoaded);
					res.addEventListener(ResourceEvent.FAILED_EVENT, onObjDefResFailed);
				}
				else if (!res.didFail) // bug: 加载成功才能设置 
				{
					this.m_ObjDefRes = res;
					res.incrementReferenceCount();
					onObjDefResLoaded(new ResourceEvent(ResourceEvent.LOADED_EVENT, res));
				}
			}
		}
		
		public void onObjDefResLoaded(ResourceEvent event)
		{
			event.resourceObject.removeEventListener(ResourceEvent.LOADED_EVENT, onObjDefResLoaded);
			event.resourceObject.removeEventListener(ResourceEvent.FAILED_EVENT, onObjDefResFailed);
			
			Logger.info(null, null, event.resourceObject.filename + " loaded");
			
			initObjDef();
			// 是不是初始化完成资源，就可以移除这个资源啊
			//this.m_context.m_resMgrNoProg.unload(event.resourceObject.filename, SWFResource);
			this.m_context.m_resMgr.unload(event.resourceObject.filename, SWFResource);
			this.m_ObjDefRes = null;
			
			// 如果可视，加载资源
			if (this.customData.flash9Renderer && (this.customData.flash9Renderer as fFlash9ElementRenderer).screenVisible)
			{
				this.loadRes(this.getAction(), (this.customData.flash9Renderer as fFlash9ElementRenderer).actDir);
			}
		}
		
		public void onObjDefResFailed(ResourceEvent event)
		{
			event.resourceObject.removeEventListener(ResourceEvent.LOADED_EVENT, onResLoaded);
			event.resourceObject.removeEventListener(ResourceEvent.FAILED_EVENT, onResFailed);
			
			Logger.error(null, null, event.resourceObject.filename + " failed");
			
			this.m_ObjDefRes = null;
			this.m_context.m_resMgr.unload(event.resourceObject.filename, SWFResource);
		}
		
		public bool binitXmlDef()
		{
			return m_binsXml;
		}
		
		// 将 from 中的属性覆盖到 to 中，实现重载的机制，只覆盖 from 中有的属性，如果 from 中没有属性就不覆盖，使用 to 中自己的属性         
		public void overwriteAtt(fObjectDefinition to, fObjectDefinition from)
		{
			if (from.overwrite)
			{
				to.overwriteAtt(from, m_insID);
			}
		}
		
		// 调整默认的属性处理
		public void adjustAtt(fObjectDefinition objdef, string insID)
		{
			objdef.adjustAtt(insID);
		}
		
		// 这个函数调用后，对象定义才算初始化完毕，人物模型默认处理方式
		public void initObjDef()
		{
			m_binsXml = true;
			
			var key:String;
			
			var insdef:fObjectDefinition = this.m_context.m_sceneResMgr.getInsDefinition(this.m_insID);
			if (!insdef && this.m_ObjDefRes)
			{
				var bytes:ByteArray;
				var clase:String = fUtil.xmlResClase(this.m_ObjDefRes.filename);
				bytes = this.m_ObjDefRes.getExportedAsset(clase) as ByteArray;
				
				var xml:XML;
				xml = new XML(bytes.readUTFBytes(bytes.length));
				
				insdef = new fObjectDefinition(xml, this.m_ObjDefRes.filename);
				xml = null;
				this.m_context.m_sceneResMgr.addInsDefinition(insdef);
				
				// 更改帧率
				var actfrate:Dictionary = this.m_context.modelFrameRate(fUtil.modelInsNum(this.m_insID));
				if (actfrate != null)
				{
					for (key in insdef.dicAction)
					{
						if (actfrate[key])
						{
							insdef.dicAction[key].framerate = actfrate[key];
						}
					}
				}
			}
			if (insdef)
			{
				// 重载属性     
				overwriteAtt(this.definition, insdef);
			}
			updateFrameRate();
		}
		
		public void changeInfoByActDir(uint act, uint dir)
		{
			var action:fActDefinition;
			var actdir:fActDirectDefinition;
			var curFrame:int;
			
			var render:fFlash9ElementRenderer = this.customData.flash9Renderer as fFlash9ElementRenderer;
			curFrame = render.currentFrame;
			
			if (m_preAct == act && m_preDir == dir && m_preFrame == curFrame)
			{
				return;
			}
			action = this.definition.dicAction[act];
			if (action)
			{
				//actdir = action.directArr[dir];
				actdir = action.directDic[dir];
				// 初始化一下方向信息    
				if (actdir)
				{
					if(m_preAct != act || m_preDir != dir)		// 只要动作或者方向有一个改变的就需要重新计算，现在由于有骑马，骑马的动作和其它动作分别单独使用一个偏移
					{
						updateModelOff(act, dir, 0, actdir.flipMode);
					}

					this.bounds2d.x = actdir.spriteVec[curFrame].origin.x + m_LinkOff.x;
					if (!canUpdataRide(subState, act))	// 如果不是骑乘状态
					{
						this.bounds2d.y = actdir.spriteVec[curFrame].origin.y + m_LinkOff.y;
					}
					else
					{
						updateMountserOff(act, dir, curFrame, actdir.flipMode);
						//if (!m_MountserLinkOff.x && !m_MountserLinkOff.y)		// 没有定义过这个骑乘者的偏移
						if(!m_context.m_SObjectMgr.m_hasOffInCurFrame)
						{
							this.bounds2d.y = actdir.spriteVec[curFrame].origin.y + m_LinkOff.y + curHorseData.definition.link1fHeight;
						}
						else
						{
							this.bounds2d.x = actdir.spriteVec[curFrame].origin.x + m_LinkOff.x + m_MountserLinkOff.x;
							this.bounds2d.y = actdir.spriteVec[curFrame].origin.y + m_LinkOff.y + m_MountserLinkOff.y;
						}
					}
					
					//this.bounds2d.width = action.width;
					//this.bounds2d.height = action.height;
					this.bounds2d.width = actdir.spriteVec[curFrame].picWidth;
					this.bounds2d.height = actdir.spriteVec[curFrame].picHeight;
					
					// Screen area
					this.screenArea = this.bounds2d.clone();
					this.screenArea.offsetPoint(fScene.translateCoords(this.x, this.y, this.z));
					
					if (_hasUpdateTagBounds2d == false)
					{
						var tagx:Number;
						var tagy:Number;
						var tagw:Number;
						var tagh:Number;
						
						// 中心点左边比较小 
						if (2 * Math.abs(actdir.spriteVec[curFrame].origin.x) <= this.bounds2d.width)
						{
							tagx = actdir.spriteVec[curFrame].origin.x;
							tagw = 2 * Math.abs(actdir.spriteVec[curFrame].origin.x);
						}
						else
						{
							tagx = -(this.bounds2d.width + actdir.spriteVec[curFrame].origin.x);
							tagw = 2 * (this.bounds2d.width + actdir.spriteVec[curFrame].origin.x);
						}
						
						if (this.definition.tagHeight == 0)
						{
							//this.definition.tagHeight = getTagHeight();
							this.definition.tagHeight = this.m_context.getTagHeight(fUtil.modelInsNum(this.m_insID));
						}
						tagy = this.definition.tagHeight;
						tagh = -this.definition.tagHeight;
						
						if (!canUpdataRide(subState, act))	// 如果不是骑乘状态
						{
							setTagBounds2d(tagx, tagy, tagw, tagh);
						}
						else
						{
							setTagBounds2d(tagx, tagy + curHorseData.definition.link1fHeight, tagw, tagh);
						}
						_hasUpdateTagBounds2d = true;
					}
				}
			}
			
			// 保存之前的数据
			m_preAct = act;
			m_preDir = dir;
			m_preFrame = curFrame;
		}
		
		public fPoint3d getOrigin(int act, int dir)
		{
			return definition.getOrigin(act, dir);
		}
		
		// 根据设置更新帧率    
		protected void updateFrameRate()
		{
		
		}
		
		// 获取动作的帧数     
		public uint getActFrameCnt(uint act)
		{
			var action:fActDefinition;
			action = this.definition.dicAction[act];
			if (action)
			{
				return action.xCount;
			}
			
			return 1;
		}
		
		// 获取动作帧率     
		public uint getActFrameRate(uint act)
		{
			var action:fActDefinition;
			action = this.definition.dicAction[act];
			if (action)
			{
				return action.framerate;
			}
			
			return 1;
		}
		
		// 获取动作播放时间   
		public float getActLength(uint act)
		{
			var action:fActDefinition;
			action = this.definition.dicAction[act];
			if (action)
			{
				return action.xCount / action.framerate;
			}
			
			return 1;
		}
		
		public uint getSubState()
		{
			return EntityCValue.STNone;
		}
		
		public void setSubState(uint value)
		{
			
		}
		
		// 特效是否需要翻转
		public uint getFlip()
		{
			return 0;
		}
		
		// 获取特效的类型
		public int getType()
		{
			return 0;
		}
		
		public float getAngle()
		{
			return 0;
		}
		
		public void setPreAct(int value)
		{
			m_preAct = value;
		}
		
		public void setPreDir(int value)
		{
			m_preDir = value;
		}
		
		public void setPreFrame(int value)
		{
			m_preFrame = value;
		}
	}
}