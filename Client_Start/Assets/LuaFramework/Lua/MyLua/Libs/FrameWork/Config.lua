MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "Config";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mAllowCallCS = false;     -- 是否允许调用 CS
	
	self.mPathLst = {};
	self.mPathLst[GlobalNS.ResPathType.ePathScene] = "Scenes/";
	self.mPathLst[GlobalNS.ResPathType.ePathSceneXml] = "Scenes/Xml/";
	self.mPathLst[GlobalNS.ResPathType.ePathModule] = "Module/";
	self.mPathLst[GlobalNS.ResPathType.ePathComUI] = "UI/";
	self.mPathLst[GlobalNS.ResPathType.ePathComUIScene] = "UIScene/";
	self.mPathLst[GlobalNS.ResPathType.ePathBeingPath] = "Being/";
	self.mPathLst[GlobalNS.ResPathType.ePathAIPath] = "AI/";
	self.mPathLst[GlobalNS.ResPathType.ePathTablePath] = "Table/";
	self.mPathLst[GlobalNS.ResPathType.ePathLangXml] = "Languages/";
	self.mPathLst[GlobalNS.ResPathType.ePathXmlCfg] = "XmlConfig/";
	self.mPathLst[GlobalNS.ResPathType.ePathModel] = "Model/";
	self.mPathLst[GlobalNS.ResPathType.ePathMaterial] = "Model/Materials/";
	self.mPathLst[GlobalNS.ResPathType.ePathBuildImage] = "Image/Build/";
	self.mPathLst[GlobalNS.ResPathType.ePathCardImage] = "Image/Card/";
	self.mPathLst[GlobalNS.ResPathType.ePathWord] = "Word/";
	self.mPathLst[GlobalNS.ResPathType.ePathAudio] = "Sound/";
	self.mPathLst[GlobalNS.ResPathType.ePathAtlas] = "Atlas/";
	self.mPathLst[GlobalNS.ResPathType.ePathSpriteAni] = "Effect/SpriteEffect/";
	self.mPathLst[GlobalNS.ResPathType.ePathSceneAnimatorController] = "Animation/Scene/";
	self.mPathLst[GlobalNS.ResPathType.ePathULua] = "LuaScript/";
	self.mPathLst[GlobalNS.ResPathType.ePathLuaScript] = "LuaScript/";
	self.mPathLst[GlobalNS.ResPathType.ePathSkillAction] = "SkillAction/";
end

function M:isAllowCallCS()
    return self.mAllowCallCS;
end

return M;