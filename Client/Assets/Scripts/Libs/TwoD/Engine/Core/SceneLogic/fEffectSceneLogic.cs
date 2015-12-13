package org.ffilmation.engine.core.sceneLogic
{
	import com.pblabs.engine.entity.EffectEntity;
	import com.pblabs.engine.entity.EntityCValue;
	import org.ffilmation.engine.core.fScene;
	/**
	 * ...
	 * @author 
	 */
	public class fEffectSceneLogic 
	{
		// 进入新的单元格子处理   
		public static function processNewCellEffect(scene:fScene, effect:EffectEntity, forceReset:Boolean = false):void
		{
			// KBEN: 是否更改 Floor 
			//effect.updateFloorInfo(EntityCValue.TEfffect);
		}
		
		// 移动处理  
		public static function renderEffect(scene:fScene, effect:EffectEntity):void
		{
			scene.renderEngine.updateEffectPosition(effect);
		}
	}
}