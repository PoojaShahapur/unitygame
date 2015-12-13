package org.ffilmation.engine.renderEngines.flash9RenderEngine
{
	import com.pblabs.engine.entity.EntityCValue;
	import com.util.DebugBox;
	import flash.display.BitmapData;
	import flash.utils.Dictionary;
	import org.ffilmation.engine.core.fElementContainer;
	import org.ffilmation.engine.elements.fObject;
	import org.ffilmation.engine.helpers.fActDefinition;
	import org.ffilmation.engine.helpers.fActDirectDefinition;

	/**
	 * @author 马匹显示数据
	 */
	public class fFlash9HorseSeqRender extends fFlash9ObjectSeqRenderer
	{
		public function fFlash9HorseSeqRender(rEngine:fFlash9RenderEngine, container:fElementContainer, element:fObject) 
		{
			super(rEngine, container, element);
		}
		
		override public function changeInfoByActDir(act:uint, dir:uint):void
		{
			var el:fObject = this.element as fObject;
			el.changeInfoByActDir(act, dir);
			
			var hostrender:fFlash9ObjectSeqRenderer = el.horseHost.customData.flash9Renderer as fFlash9ObjectSeqRenderer;
			//if (hostrender.currentHorseBitMap)	// 需要更新host中的这个变量
			if (hostrender)
			{
				hostrender.currentHorseBitMap.x = element.bounds2d.x;
				hostrender.currentHorseBitMap.y = element.bounds2d.y;
			}
		}
		
		// 这个就是直接生成帧序列， act: 生成的动作的索引 direction : 0 - 7    
		override protected function buildFrames(act:uint, direction:uint):void
		{
			var el:fObject = this.element as fObject;
			
			// KBEN:
			if (!isLoaded(act, direction))
				return;
			// KBEN: 动作只初始化一边    
			if (isActInit(act, direction))
				return;
			
			// KBEN: 取出帧数量
			// 二维数组，一维是8个方向，二维才是图片序列   
			if (!_framesDic[act])	// 如果动作字典不存在
			{
				_framesDic[act] = new Dictionary();
			}
			
			// 如果方向序列不存在
			if(!_framesDic[act][direction])
			{
				_framesDic[act][direction] = new Vector.<BitmapData>();
			}

			var frame:Dictionary;
			frame = _framesDic[act];
			
			var action:fActDefinition;
			var actDirect:fActDirectDefinition;
			var globalIdx:int = 0;
			var idx:int = 0;

			action = el.definition.dicAction[act];
			
			// KBEN: 所有的动作资源一次全部构造出来，这个以后可以改   
			actDirect = action.directDic[direction];
			var bitmapdata:BitmapData;
			
			idx = 0;
			while (idx < action.xCount)
			{
				// KBEN: 图像序列优化，所有的公用一个图像
				if (actDirect.flipMode == EntityCValue.FLPX)
				{
					bitmapdata = el._resDic[act][direction].getExportedAsset(actDirect.spriteVec[idx].startName, true, actDirect.flipMode);
				}
				else
				{
					bitmapdata = el._resDic[act][direction].getExportedAsset(actDirect.spriteVec[idx].startName, true);
				}
				
				frame[direction][globalIdx] = bitmapdata;
				if (bitmapdata == null)
				{
					var strInfo:String = "name";
					if (el is fObject)
					{
						strInfo = (el as fObject).definitionID + "_" + (el as fObject).m_insID;
						strInfo += "_" + act + "_" + direction + ".swf\n";
						strInfo += actDirect.spriteVec[idx].startName;
					}
					strInfo += "\n(bitmapdata == null)"
					DebugBox.info(strInfo);
				}
				
				++idx;
				++globalIdx;
			}
			
			// bug 需要动作方向都一致才行，开始只有动作方向相同，没有判断方向，尤其是 direction 这个方向资源加载进来的时候， direction 已经改变成其它方向了，这个时候就会有问题
			if (el.isEqualMount2PlayerActDir(act, direction))
			{
				_frames = _framesDic[act][direction];
			}
		}
	}	
}