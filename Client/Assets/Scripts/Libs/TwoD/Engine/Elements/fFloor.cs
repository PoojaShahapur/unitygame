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
		{
			this.gWidth = int((defObj.@width / scene.gridSize) + 0.5);
			this.gDepth = int((defObj.@height / scene.gridSize) + 0.5);
			this.width = scene.gridSize * this.gWidth;
			this.depth = scene.gridSize * this.gDepth;
			
			base(defObj, scene, this.width, this.depth);
			m_resType = EntityCValue.PHTER;
			
			this.i = int((defObj.@x / scene.gridSize) + 0.5);
			this.j = int((defObj.@y / scene.gridSize) + 0.5);
			this.k = int((defObj.@z / scene.levelSize) + 0.5);
			this.x0 = this.x = this.i * scene.gridSize;
			this.y0 = this.y = this.j * scene.gridSize;
			this.top = this.z = this.k * scene.levelSize;
			this.x1 = this.x0 + this.width;
			this.y1 = this.y0 + this.depth;
			
			m_dynamicElementDic = new Dictionary();
			m_characterDic = new Dictionary();
			m_emptySpriteDic = new Dictionary();
			//m_dynamicElement2Dic = new Dictionary();
			//m_character2Dic = new Dictionary();
			//m_emptySprite2Dic = new Dictionary();
			
			m_dynamicElementVec = new Dictionary<string, int>();
			m_characterVec = new Dictionary<string, int>();
			
			this.xmlObj = null;	// bug: 释放这个资源,总是释放不了这个 xml
		}
		
		// Is this floor in front of other plane ? Note that a false return value does not imply the opposite: None of the planes
		// may be in front of each other
		/** @private */
		public override function inFrontOf(p:fPlane):Boolean
		{
			var floor:fFloor = p as fFloor
			if ((this.i < (floor.i + floor.gWidth) && (this.j + this.gDepth) > floor.j && this.k > floor.k) || ((this.j + this.gDepth) > floor.j && (this.i + this.gWidth) <= floor.i) || (this.i >= floor.i && this.i < (floor.i + floor.gWidth) && this.j >= (floor.j + floor.gDepth)))
				return true;
			return false;
		}
		
		/** @private */
		public override function distanceTo(x:Number, y:Number, z:Number):Number
		{
			// Easy case
			if (x >= this.x && x <= this.x + this.width && y >= this.y && y <= this.y + this.depth)
				return ((this.z - z) > 0) ? (this.z - z) : -(this.z - z);
			
			var d2d:Number;
			if (y < this.y)
			{
				d2d = mathUtils.distancePointToSegment(new Point(this.x, this.y), new Point(this.x + width, this.y), new Point(x, y));
			}
			else if (y > (this.y + this.depth))
			{
				d2d = mathUtils.distancePointToSegment(new Point(this.x, this.y + this.depth), new Point(this.x + width, this.y + this.depth), new Point(x, y));
			}
			else
			{
				if (x < this.x)
					d2d = mathUtils.distancePointToSegment(new Point(this.x, this.y), new Point(this.x, this.y + this.depth), new Point(x, y));
				else if (x > this.x + this.width)
					d2d = mathUtils.distancePointToSegment(new Point(this.x + this.width, this.y), new Point(this.x + this.width, this.y + this.depth), new Point(x, y));
				else
					d2d = 0;
			}
			
			var dz:Number = z - this.z;
			return Math.sqrt(dz * dz + d2d * d2d);
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
			if(!m_dynamicElementDic[id])
			{
				m_dynamicElementDic[id] = 1;
				m_dynamicElementVec.push(id);
			}
			else
			{
				throw new Event("dynamic already in current floor");
			}
		}
		
		public void clearDynamic(string id)
		{
			if(m_dynamicElementDic[id])
			{
				m_dynamicElementDic[id] = null;
				delete m_dynamicElementDic[id];
				
				var idx:int = m_dynamicElementVec.indexOf(id);
				m_dynamicElementVec.splice(idx, 1);
			}
			else
			{
				throw new Event("dynamic not in current floor");
			}
		}
		
		public function addCharacter(id:String):void
		{
			if(!m_characterDic[id])
			{
				m_characterDic[id] = 1;
				m_characterVec.push(id);
			}
			else
			{
				throw new Event("dynamic already in current floor");
			}
		}
		
		public void clearCharacter(string id)
		{	
			if(m_characterDic[id])
			{
				m_characterDic[id] = null;
				delete m_characterDic[id];
				
				var idx:int = m_characterVec.indexOf(id);
				m_characterVec.splice(idx, 1);
			}
			else
			{
				throw new Event("character not in current floor");
			}
		}
		
		// 显示这个区域中的各种可显示内容
		public void showObject(fCell cell)
		{
            // 计算所有可视化显示的内容
            // KBEN: 除了地形和人物，其它可视化都放在这里，存放需要深度排序的
            uint dynamicLength;
			fObject dynObject;
			
			uint i2 = 0;
            int chLength = 0;
            fCharacter character;

            int esLength = 0;

            int distidx = -1;
			
			dynamicLength = m_dynamicElementVec.length;

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
						this.scene.renderManager.elementsV[distidx = this.scene.renderManager.elementsV.length] = dynObject;
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
						distidx = this.scene.renderManager.elementsV.indexOf(dynObject);
						if(distidx != -1)
						{
							this.scene.renderManager.elementsV.splice(distidx, 1);
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
			chLength = m_characterVec.length;
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
						distidx = this.scene.renderManager.charactersV.indexOf(character);
						if(distidx != -1)
						{
							this.scene.renderManager.charactersV.splice(distidx, 1);
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
            uint dynamicLength;
            fObject dynObject;

            uint i2 = 0;
            int chLength = 0;
            fCharacter character;

            int esLength = 0;
            fEmptySprite spr;

            int distidx = -1;
			
			dynamicLength = m_dynamicElementVec.length;

			for (i2 = 0; i2 < dynamicLength; i2++)
			{
				// 动态对象设置可视化状态
				dynObject = scene.all[m_dynamicElementVec[i2]];
				if (dynObject.isVisibleNow && dynObject._visible)
				{
					// 从显示列表中去掉
					distidx = this.scene.renderManager.elementsV.indexOf(dynObject);
					if(distidx != -1)
					{
						this.scene.renderManager.elementsV.splice(distidx, 1);
					}
					
					// Remove asset
					this.scene.renderManager.renderEngine.hideElement(dynObject);
					this.scene.renderManager.removeFromDepthSort(dynObject);
					//anyChanges = true;
					dynObject.isVisibleNow = false;	
				}
			}
			
			// KBEN: 存放人物，需要深度排序
			chLength = m_characterVec.length;
			for (i2 = 0; i2 < chLength; i2++)
			{
				// Is character within range ?
				character = scene.all[m_characterVec[i2]];
				if (character.isVisibleNow && character._visible)
				{
					// 从显示列表中去掉
					distidx = this.scene.renderManager.charactersV.indexOf(character);
					if(distidx != -1)
					{
						this.scene.renderManager.charactersV.splice(distidx, 1);
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
			
			// KBEN: 地上物，不会改变的地上建筑
		}
	}
}