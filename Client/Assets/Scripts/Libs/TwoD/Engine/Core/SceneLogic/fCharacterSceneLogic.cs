namespace SDK.Lib
{
	public class fCharacterSceneLogic
	{
		// Process New cell for Characters
		public static function processNewCellCharacter(scene:fScene, character:fCharacter, forceReset:Boolean = false):void
		{
			if (scene.engine.m_context.m_profiler)
				scene.engine.m_context.m_profiler.enter("fCharacterSceneLogic.processNewCellCharacter");
				
			// KBEN: 是否更改 Floor ，EntityCValue.TPlayer 和 TNpc 同处理，因此就赋值 TPlayer
			var i:int = 0;
			
			if (scene.engine.m_context.m_profiler)
				scene.engine.m_context.m_profiler.exit("fCharacterSceneLogic.processNewCellCharacter");
		}
		
		// Main render method for characters
		public static function renderCharacter(scene:fScene, character:fCharacter):void
		{
			//if (scene.prof)
			//	scene.prof.begin("Render char:" + character.id, true);
			if (scene.engine.m_context.m_profiler)
				scene.engine.m_context.m_profiler.enter("Render char:" + character.id);
			
			//var light:fOmniLight, elements:Array, nEl:int, len:int, cache:fCharacterShadowCache;
			
			// Move character to its new position
			scene.renderEngine.updateCharacterPosition(character);

			if (scene.engine.m_context.m_profiler)
				scene.engine.m_context.m_profiler.exit("Render char:" + character.id);
		}
	}
}