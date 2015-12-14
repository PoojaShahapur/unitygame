namespace SDK.Lib
{
	public class fCharacterSceneLogic
	{
		// 处理 Character 进入新的 Cell
		public static void processNewCellCharacter(fScene scene, fCharacter character, bool forceReset = false)
		{
            // KBEN: 是否更改 Floor ，EntityCValue.TPlayer 和 TNpc 同处理，因此就赋值 TPlayer
            int i = 0;
		}
		
		// characters 的主要渲染方法
		public static void renderCharacter(fScene scene, fCharacter character)
		{
            // 移动 character 到新的位置
            scene.renderEngine.updateCharacterPosition(character);
		}
	}
}