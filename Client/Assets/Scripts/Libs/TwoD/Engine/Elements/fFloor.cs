using System.Collections.Generic;
using System.Security;

namespace SDK.Lib
{
	public class fFloor : fPlane
	{
		public int gWidth;
		public int gDepth;
		public int i;
		public int j;
		public int k;
   
		public float width;

		public float depth;

		// KBEN: 存放这个区域中的动态改变的实体，查找使用 Dictionary ，遍历使用 Vector ，只有参与深度排序的内容才需要加入这里，方便快速剔除
		// 某个元素的uniqueid 放在数组的下标
		public Dictionary<string, int> m_dynamicElementDic;
		public Dictionary<string, int> m_characterDic;
		public Dictionary<string, int> m_emptySpriteDic;

		// KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
		public MList<string> m_dynamicElementVec;
		// KBEN: 存放人物，需要深度排序
		public MList<string> m_characterVec;	
		// KBEN: 地上物，不会改变的地上建筑
		public MList<string> m_staticVec;

		public fFloor(SecurityElement defObj, fScene scene)
            : base(defObj, scene, 0, 0)
		{
            int defWidth = 0;
            int defHeight = 0;
            UtilXml.getXmlAttrInt(defObj, "width", ref defWidth);
            UtilXml.getXmlAttrInt(defObj, "height", ref defHeight);
            this.gWidth = (int)((defWidth / scene.gridSize) + 0.5);
			this.gDepth = (int)((defHeight / scene.gridSize) + 0.5);
			this.width = scene.gridSize * this.gWidth;
			this.depth = scene.gridSize * this.gDepth;

            //m_resType = EntityCValue.PHTER;
            int defX = UtilXml.getXmlAttrInt(defObj, "x", ref defWidth);
            int defY = UtilXml.getXmlAttrInt(defObj, "y", ref defWidth);
            int defZ = UtilXml.getXmlAttrInt(defObj, "z", ref defWidth);

            this.i = (int)((defX / scene.gridSize) + 0.5);
			this.j = (int)((defY / scene.gridSize) + 0.5);
			this.k = (int)((defZ / scene.levelSize) + 0.5);
			this.x0 = this.x = this.i * scene.gridSize;
			this.y0 = this.y = this.j * scene.gridSize;
			this.top = this.z = this.k * scene.levelSize;
			this.x1 = this.x0 + this.width;
			this.y1 = this.y0 + this.depth;
			
			m_dynamicElementDic = new Dictionary<string, int>();
			m_characterDic = new Dictionary<string, int>();

			m_dynamicElementVec = new MList<string>();
			m_characterVec = new MList<string>();
			
			this.xmlObj = null;	// bug: 释放这个资源,总是释放不了这个 xml
		}

		public void disposeFloor()
		{
			this.disposePlane();
		}
		
		public override void dispose()
		{
			this.disposeFloor();
		}

        // KBEN: id 是 fElement.uniqueId 和 fElement.id     
        public void addDynamic(string id)
		{
			if(m_dynamicElementDic.ContainsKey(id))
			{
				m_dynamicElementDic[id] = 1;
				m_dynamicElementVec.push(id);
			}
			else
			{
				//throw new Event("dynamic already in current floor");
			}
		}
		
		public void clearDynamic(string id)
		{
			if(m_dynamicElementDic.ContainsKey(id))
			{
				m_dynamicElementDic[id] = 0;
				m_dynamicElementDic.Remove(id);

                int idx = m_dynamicElementVec.IndexOf(id);
				m_dynamicElementVec.RemoveAt(idx);
			}
			else
			{
				//throw new Event("dynamic not in current floor");
			}
		}
		
		public void addCharacter(string id)
		{
			if(!m_characterDic.ContainsKey(id))
			{
				m_characterDic[id] = 1;
				m_characterVec.push(id);
			}
			else
			{
				//throw new Event("dynamic already in current floor");
			}
		}
		
		public void clearCharacter(string id)
		{	
			if(m_characterDic.ContainsKey(id))
			{
				m_characterDic[id] = 0;
				m_characterDic.Remove(id);

                int idx = m_characterVec.IndexOf(id);
				m_characterVec.RemoveAt(idx);
			}
			else
			{
				//throw new Event("character not in current floor");
			}
		}
		
		// 显示这个区域中的各种可显示内容
		public void showObject(fCell cell)
		{
            // 计算所有可视化显示的内容
            // KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
            int dynamicLength;
			fObject dynObject;
			
			int i2 = 0;
            int chLength = 0;
            fCharacter character;

            int esLength = 0;

            int distidx = -1;
			
			dynamicLength = m_dynamicElementVec.Count();

			for (i2 = 0; i2 < dynamicLength; i2++)
			{
				// 动态对象设置可视化状态
				dynObject = scene.all[m_dynamicElementVec[i2]];
				// 距离裁剪改成矩形裁剪, bug: 这样判断这个区域上面的所有内容必然可见， (x, y, z) 是区域的位置
				if (cell.m_scrollRect.contains(dynObject.x, dynObject.y))
				{
					dynObject.willBeVisible = true;
					
					// 显示可视化对象
					dynObject.willBeVisible = false;
					if (!dynObject.isVisibleNow && dynObject._visible)
					{
						this.scene.renderManager.elementsV[distidx = this.scene.renderManager.elementsV.Count] = dynObject;
						this.scene.renderManager.renderEngine.showElement(dynObject);
						this.scene.renderManager.addToDepthSort(dynObject);
						dynObject.isVisibleNow = true;
					}
				}
				else	// 如果不可见
				{
					if (dynObject.isVisibleNow && dynObject._visible)
					{
						// 从显示列表中去掉
						distidx = this.scene.renderManager.elementsV.IndexOf(dynObject);
						if(distidx != -1)
						{
							this.scene.renderManager.elementsV.RemoveAt(distidx);
						}
						// Remove asset
						this.scene.renderManager.renderEngine.hideElement(dynObject);
						this.scene.renderManager.removeFromDepthSort(dynObject);
						//anyChanges = true;
						dynObject.isVisibleNow = false;	
					}
				}
			}
			
			// KBEN: 存放人物，需要深度排序
			chLength = m_characterVec.Count();
			for (i2 = 0; i2 < chLength; i2++)
			{
				// Is character within range ?
				character = scene.all[m_characterVec[i2]];
				if (cell.m_scrollRect.contains(character.x, character.y))
				{
					character.willBeVisible = true;

					character.willBeVisible = false;
					if (!character.isVisibleNow && character._visible)
					{
						this.scene.renderManager.charactersV[this.scene.renderManager.charactersV.length] = character; 
						// Add asset
						this.scene.renderManager.renderEngine.showElement(character);
						if(EntityCValue.SLBuild != character.layer)		// 如果不是地物层
						{
							this.scene.renderManager.addToDepthSort(character);
						}
						character.isVisibleNow = true;
						//anyChanges = true;
					}
				}
				else	// 如果不可见
				{
					//m_fullVisible = false;
					if (character.isVisibleNow && character._visible)
					{
						// 从显示列表中去掉
						distidx = this.scene.renderManager.charactersV.IndexOf(character);
						if(distidx != -1)
						{
							this.scene.renderManager.charactersV.RemoveAt(distidx);
						}
						// Remove asset
						this.scene.renderManager.renderEngine.hideElement(character);
						if(EntityCValue.SLBuild != character.layer)		// 如果不是地物层
						{
							this.scene.renderManager.removeFromDepthSort(character);
						}
						//anyChanges = true;
						character.isVisibleNow = false;	
					}
				}
			}
			
			// KBEN: 地上物，不会改变的地上建筑
		}
		
		public void hideObject()
		{
            // 计算所有可视化显示的内容
            // KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
            int dynamicLength;
            fObject dynObject;

            int i2 = 0;
            int chLength = 0;
            fCharacter character;

            int esLength = 0;

            int distidx = -1;
			
			dynamicLength = m_dynamicElementVec.Count();

			for (i2 = 0; i2 < dynamicLength; i2++)
			{
				// 动态对象设置可视化状态
				dynObject = scene.all[m_dynamicElementVec[i2]];
				if (dynObject.isVisibleNow && dynObject._visible)
				{
					// 从显示列表中去掉
					distidx = this.scene.renderManager.elementsV.IndexOf(dynObject);
					if(distidx != -1)
					{
						this.scene.renderManager.elementsV.RemoveAt(distidx);
					}
					
					// Remove asset
					this.scene.renderManager.renderEngine.hideElement(dynObject);
					this.scene.renderManager.removeFromDepthSort(dynObject);
					//anyChanges = true;
					dynObject.isVisibleNow = false;	
				}
			}
			
			// KBEN: 存放人物，需要深度排序
			chLength = m_characterVec.Count();
			for (i2 = 0; i2 < chLength; i2++)
			{
				character = scene.all[m_characterVec[i2]];
				if (character.isVisibleNow && character._visible)
				{
					// 从显示列表中去掉
					distidx = this.scene.renderManager.charactersV.IndexOf(character);
					if(distidx != -1)
					{
						this.scene.renderManager.charactersV.RemoveAt(distidx);
					}
					// Remove asset
					this.scene.renderManager.renderEngine.hideElement(character);
					if(EntityCValue.SLBuild != character.layer)		// 如果不是地物层
					{
						this.scene.renderManager.removeFromDepthSort(character);
					}
					character.isVisibleNow = false;	
				}
			}
		}
	}
}