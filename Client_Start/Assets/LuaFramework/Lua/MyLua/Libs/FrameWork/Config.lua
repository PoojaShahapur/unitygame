MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "Config";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_allowCallCS = false;     -- 是否允许调用 CS
	
	self.m_pathLst = {};
	self.m_pathLst[GlobalNS.ResPathType.ePathScene] = "Scenes/";
	self.m_pathLst[GlobalNS.ResPathType.ePathSceneXml] = "Scenes/Xml/";
	self.m_pathLst[GlobalNS.ResPathType.ePathModule] = "Module/";
	self.m_pathLst[GlobalNS.ResPathType.ePathComUI] = "UI/";
	self.m_pathLst[GlobalNS.ResPathType.ePathComUIScene] = "UIScene/";
	self.m_pathLst[GlobalNS.ResPathType.ePathBeingPath] = "Being/";
	self.m_pathLst[GlobalNS.ResPathType.ePathAIPath] = "AI/";
	self.m_pathLst[GlobalNS.ResPathType.ePathTablePath] = "Table/";
	self.m_pathLst[GlobalNS.ResPathType.ePathLangXml] = "Languages/";
	self.m_pathLst[GlobalNS.ResPathType.ePathXmlCfg] = "XmlConfig/";
	self.m_pathLst[GlobalNS.ResPathType.ePathModel] = "Model/";
	self.m_pathLst[GlobalNS.ResPathType.ePathMaterial] = "Model/Materials/";
	self.m_pathLst[GlobalNS.ResPathType.ePathBuildImage] = "Image/Build/";
	self.m_pathLst[GlobalNS.ResPathType.ePathCardImage] = "Image/Card/";
	self.m_pathLst[GlobalNS.ResPathType.ePathWord] = "Word/";
	self.m_pathLst[GlobalNS.ResPathType.ePathAudio] = "Sound/";
	self.m_pathLst[GlobalNS.ResPathType.ePathAtlas] = "Atlas/";
	self.m_pathLst[GlobalNS.ResPathType.ePathSpriteAni] = "Effect/SpriteEffect/";
	self.m_pathLst[GlobalNS.ResPathType.ePathSceneAnimatorController] = "Animation/Scene/";
	self.m_pathLst[GlobalNS.ResPathType.ePathULua] = "LuaScript/";
	self.m_pathLst[GlobalNS.ResPathType.ePathLuaScript] = "LuaScript/";
	self.m_pathLst[GlobalNS.ResPathType.ePathSkillAction] = "SkillAction/";
end

function M:isAllowCallCS()
    return self.m_allowCallCS;
end

return M;