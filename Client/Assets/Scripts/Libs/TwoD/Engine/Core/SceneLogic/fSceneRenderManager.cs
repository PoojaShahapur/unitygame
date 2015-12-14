using System.Collections;

namespace SDK.Lib
{
	public class fSceneRenderManager
	{
		private fScene scene;   // 对被管理场景的引用
		public float range;     // 当前 ViewPort 可视化元素的范围
		private ArrayList depthSortArr; // 深度排序的元素
		
		// KBEN: 地形，以及一些静态地物可视化元素放在这里，不需要深度排序，可视化都是内部访问的   
		// KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的 
		public ArrayList elementsV;
		// KBEN: 存放人物，需要深度排序        
		public ArrayList charactersV; // 当前可视的 characters 数组
		private fCell cell; // 摄像机所在的 Cell
		protected fCell m_preCell; // 这个是前一个摄像机所在的格子
		public fEngineRenderEngine renderEngine; // 渲染引擎
		
		public fSceneRenderManager(fScene scene)
		{
			this.scene = scene;
			this.renderEngine = this.scene.renderEngine;
		}
		
		// 设置 Scene 的 ViewPort 大小
		public void setViewportSize(float width, float height)
		{
			this.range = (float)(UtilApi.Sqrt(width * width + height * height) * 0.5);
			if (this.range <= 0)
				this.range = 0;
			else
				this.range += 2 * this.scene.gridSize;
		}

		public void initialize()
		{
			this.depthSortArr = new ArrayList();
			this.elementsV = new ArrayList();
			this.charactersV = new ArrayList();
		}
		
		// KBEN: 优化裁剪
		public void processNewCellCamera(fCamera cam)
		{
			// 如果摄像机没有初始化，就不处理
			if (!cam.m_bInit)
			{
				return;
			}

			this.m_preCell = this.cell;
			this.cell = cam.cell;
			float x = 0, y = 0, z = 0;
            MList<fFloor> tempElements;
			
			// 摄像机进入新的单元
			if (this.cell.visibleElements || this.cell.visibleRange < this.range)
			{
				this.scene.getVisibles(this.cell, this.range);
			}
			
			// 更新之前格子进入当前格子需要更新的区域
			if (!this.cell.m_updateDistrict[this.m_preCell])
			{
				this.cell.updateByPreInCur(this.m_preCell);
			}
			
			// 这个现在存放必须更新的区域，如果这个区域在上一个格子也是完全可见的，那么这个格子就不用更新了
			//tempElements = this.cell.m_visibleFloor;
			tempElements = this.cell.m_updateDistrict[this.m_preCell];
			
			int nEl;
            int nElements = tempElements.Count;
            int i2 = 0;

            int distidx = 0;
            fFloor floor;
            fRenderableElement ele;
			
			// tempElements 这里面的地形区域必然是可见的
			for (i2 = 0; i2 < nElements; i2++)
			{
				floor = tempElements[i2];				
				floor.willBeVisible = false;
				if (!floor.isVisibleNow && floor._visible)
				{
					floor.isVisibleNow = true;
					this.renderEngine.showElement(floor);
					
				}
				floor.showObject(this.cell);
			}

            // 处理隐藏区域
            MList<fFloor> hideDistList = this.cell.m_hideDistrict[this.m_preCell];
			nEl = hideDistList.Count;
			for (i2 = 0; i2 < nEl; i2++)
			{
				floor = hideDistList[i2];				
				if (floor.isVisibleNow && floor._visible)
				{
					// 隐藏掉区域中的数据
					floor.hideObject();
					this.renderEngine.hideElement(floor);
					floor.isVisibleNow = false;
				}
			}
			
			this.scene.m_depthDirty = true; // KBEN: 必须重新排序，至于是否真的需要排序，在排序的时候检查
		}
		
		public void processNewCellCharacter(fCharacter character, bool needDepthsort = true)
		{
			// 如果摄像机不可见就返回吧
			if (!this.scene.currentCamera.m_bInit)
			{
				return;
			}
			
			if (character._visible)
			{
				if (this.cell.m_scrollRect.contains(character.x, character.y))
				{
					if (!character.isVisibleNow)
					{
						this.charactersV[this.charactersV.length] = character;
						this.renderEngine.showElement(character);
						if (EntityCValue.SLBuild != character.layer) // 如果不是地物层
						{
							this.addToDepthSort(character);
						}
						character.isVisibleNow = true;
					}
				}
				else
				{
					if (character.isVisibleNow)
					{
                        int pos = this.charactersV.indexOf(character);
						this.charactersV.splice(pos, 1);
						this.renderEngine.hideElement(character);
						if (EntityCValue.SLBuild != character.layer) // 如果不是地物层
						{
							this.removeFromDepthSort(character);
						}
						character.isVisibleNow = false;
					}
				}
			}
			
			if (character.cell != null)
			{
				// 只需要更新深度值，主角更新相机的时候会更新显示的
				// bug: 但是如果 processNewCellCamera 没有 m_depthDirty = true ，就会导致不排序，就会有点问题，但是可以减少一次排序
				if (EntityCValue.SLBuild != character.layer) // 如果不是地物层
				{
					character.setDepth(character.cell.zIndex, needDepthsort);
				}
			}
		}
		
		// KBEN: 特效处理 
		public void processNewCellEffect(EffectEntity effect)
		{
			// 如果摄像机没有初始化，就不处理
			if (!this.scene.currentCamera.m_bInit)
			{
				return;
			}
			
			if (effect.cell == null)
			{
				this.scene.removeEffect(effect);
				return;
			}
			
			if (effect._visible)
			{
				if (this.cell.m_scrollRect.contains(effect.x, effect.y))
				{
					if (!effect.isVisibleNow)
					{
						this.elementsV[this.elementsV.length] = effect;
						this.renderEngine.showElement(effect);
						this.addToDepthSort(effect);
						effect.isVisibleNow = true;
					}
				}
				else
				{
					if (effect.isVisibleNow)
					{
						var pos:int = this.elementsV.indexOf(effect);
						this.elementsV.splice(pos, 1);
						this.renderEngine.hideElement(effect);
						this.removeFromDepthSort(effect);
						effect.isVisibleNow = false;
					}
				}
			}
			
			effect.setDepth(effect.cell.zIndex);
		}

		public void showListener(Event evt)
		{
			this.addedItem(evt.target as fRenderableElement);
		}
		
		public void addedItem(fRenderableElement ele)
		{
			try
			{
				// 如果相机还没有放到正确的位置，就不添加显示内容了，很可能要移动相机，结果内容全部清理一遍
				if (!this.scene.currentCamera.m_bInit)
				{
					return;
				}
				
				if (!ele.isVisibleNow && ele._visible && this.cell.m_scrollRect.contains(ele.x, ele.y))
				{
					ele.isVisibleNow = true;
					this.renderEngine.showElement(ele);
					// KBEN: fFloor 不参与深度排序 
					if (!(ele is fFloor))
					{
						// 地物层也不需要排序
						if (EntityCValue.SLBuild != ele.layer) // 如果不是地物层
						{
							this.addToDepthSort(ele);
						}
					}
					
					// KBEN: 这个地方需要修改   
					// KBEN: 玩家 npc 全部放在这里   
					//if (ele is fCharacter)
					if (ele is BeingEntity)
					{
						this.charactersV[this.charactersV.length] = ele as fCharacter;
					}
					else if (ele is EffectEntity)
					{
						this.elementsV[this.elementsV.length] = ele;
					}

					// KBEN: 地形不排序，只可视化剔除   
					if (!(ele is fFloor))
					{
						//this.depthSort();
						// KBEN: 需要重新排序，增加的时候需要深度排序
						if (EntityCValue.SLBuild != ele.layer) // 如果不是地物层
						{
							this.scene.m_depthDirty = true;
						}
					}
				}
			}
			catch (Error e)
			{
				string strLog = "";
				if (scene)
				{
					strLog += "scene " + scene.m_serverSceneID + " ";
					if (scene.currentCamera)
					{
						strLog += "scene.currentCamera ";
					}
				}
				if (cell)
				{
					strLog += "cell ";
				}
				if (renderEngine)
				{
					strLog += "renderEngine ";
				}
				
				if (ele)
				{
					strLog += "ele ";
				}
			}
		}

		public void hideListener(Event evt)
		{
			this.removedItem(evt.target as fRenderableElement);
		}
		
		public void removedItem(fRenderableElement ele, bool destroyingScene = false)
		{
            BeingEntity ch;
            int pos;
			if (ele.isVisibleNow)
			{
				ele.isVisibleNow = false;
				// KBEN:    
				if (ele is fFloor)
				{
					this.renderEngine.hideElement(ele);
					// KBEN: fFloor 不参与深度排序   
					//this.removeFromDepthSort(ele);
					//}
				}
				else if (ele is BeingEntity)
				{
					// KBEN:   
					ch = ele as BeingEntity;
					pos = this.charactersV.indexOf(ch);
					if (pos >= 0)
					{
						this.charactersV.splice(pos, 1);
						this.renderEngine.hideElement(ele);
						this.removeFromDepthSort(ele);
					}
				}
				else if (ele is EffectEntity)
				{
					pos = this.elementsV.indexOf(ele);
					if (pos >= 0)
					{
						this.elementsV.splice(pos, 1);
						this.renderEngine.hideElement(ele);
						this.removeFromDepthSort(ele);
					}
				}
				else
				{
					this.renderEngine.hideElement(ele);
					this.removeFromDepthSort(ele);
				}
			}
		}
		
		// KBEN: 不需要深度排序的不会调用这个函数   
		public void addToDepthSort(fRenderableElement item)
		{
			if (this.depthSortArr.indexOf(item) < 0)
			{
				this.depthSortArr.push(item);
				item.addEventListener(fRenderableElement.DEPTHCHANGE, this.depthChangeListener, false, 0, true);
			}
		}
		
		public void removeFromDepthSort(fRenderableElement item)
		{
			this.depthSortArr.splice(this.depthSortArr.indexOf(item), 1);
			item.removeEventListener(fRenderableElement.DEPTHCHANGE, this.depthChangeListener);
		}

		public void depthChangeListener(Event evt)
		{
			// 如果深度排序应经需要重新排序了，就没有必然在单独排序自己了
			if (this.scene.m_depthDirty || !this.scene.m_sortByBeingMove)
			{
				return;
			}
			
			// 设置标示
			this.scene.m_depthDirtySingle = true;
			this.scene.m_singleDirtyArr.push(evt.target as fRenderableElement);
		}
		
		// 某一些单个改变的内容
		public void depthSortSingle()
		{
            ArrayList ar = this.depthSortArr;
			// KBEN: 深度排序
			fUtil.insortSort(this.depthSortArr);
            int i = ar.length;
			if (i == 0)
				return;
            // KBEN: 除了地形都排序
            Sprite p = this.scene.m_SceneLayer[EntityCValue.SLObject];
			for each (fRenderableElement el in this.scene.m_singleDirtyArr)
			{
                int oldD = el.depthOrder;
                // KBEN: 插入排序
                int newD = this.depthSortArr.indexOf(el);
				if (newD != oldD)
				{
					el.depthOrder = newD;
					// KBEN: 地形不排序，阴影需要排序
					// KBEN: 不需要深度排序的不会调用 addToDepthSort 这个函数，因此这里不用调用这个函数
					try
					{
						// 如果由于调整其它的位置，导致这个位置可能已经放在正确的位置了，就不再调整位置了
						if (p.getChildIndex(el.container) != newD)
						{
							p.setChildIndex(el.container, newD);
						}
					}
					catch (e:Error)
					{
						Logger.info(null, null, "depthSortSingle error");
					}
				}
			}
		}

		public void depthSort()
		{
            ArrayList ar = this.depthSortArr;
			// KBEN: 深度排序
			fUtil.insortSort(this.depthSortArr);
            int i = ar.length;
			if (i == 0)
				return;
            // KBEN: 除了地形都排序
            Sprite p = this.scene.m_SceneLayer[EntityCValue.SLObject];
			
			try
			{
				while (i--)
				{
					if (p.getChildAt(i) != ar[i].container)
					{
						p.setChildIndex(ar[i].container, i);
						ar[i].depthOrder = i;
					}
				}
			}
			catch (Error e) // KBEN: 有时候竟然会莫名其妙的减少一个  
			{
				
			}
		}

		public void dispose()
		{
			if (this.depthSortArr)
			{
                int il = this.depthSortArr.Count;
				for (int i = 0; i < il; i++)
					delete this.depthSortArr[i];
			}
			this.depthSortArr = null;
			if (this.elementsV)
			{
				il = this.elementsV.length;
				for (i = 0; i < il; i++)
					delete this.elementsV[i];
			}
			this.elementsV = null;
			if (this.charactersV)
			{
				il = this.charactersV.length;
				for (i = 0; i < il; i++)
					delete this.charactersV[i];
			}
			this.charactersV = null;

			this.cell = null;
			this.m_preCell = null;
		}
		
		public void setCurCell(fCell value)
        {
			this.cell = value;
		}
		
		public fCell getCurCell()
		{
			return this.cell;
		}
		
		public void setPreCell(fCell value)
        {
			this.m_preCell = value;
		}
	}
}