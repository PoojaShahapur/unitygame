#include "DynamicSceneManager.h"
///////////////////////////////////////////////
//
//code[ScenesServer/DynamicSceneManager.cpp] defination by codemokey
//
//
///////////////////////////////////////////////
#include "Scene.h"
#include "zXMLParser.h"
#include "SessionCommand.h"


DynamicSceneManager::DynamicSceneManager()
{}

DynamicSceneManager::~DynamicSceneManager()
{}

bool DynamicSceneManager::loadConfig()
{
    zXMLParser xml;
    if(!xml.initFile(Zebra::global["configdir"] + "Challenge.xml"))
    {
	return false;
    }
    xmlNodePtr root = xml.getRootNode("Config");
    if(!root)
    {
	return false;
    }
    xmlNodePtr mapNode = xml.getChildNode(root, "map");
    if(!mapNode)
	return false;
    xmlNodePtr ItemNode = xml.getChildNode(mapNode, "mapid");
    while(ItemNode)
    {
	dynMapInfo info;
	xml.getNodePropNum(ItemNode, "id", &info.dwMapID, sizeof(DWORD));
	Scene *lpmapTemplate = new StaticScene();
	if(!lpmapTemplate->init(6, info.dwMapID))
	{
	    SAFE_DELETE(lpmapTemplate);
	    Zebra::logger->error("[动态地图] 加载配置地图失败 %d", info.dwMapID);
	    return false;
	}
	info.mapTemplate = lpmapTemplate;
	_mapVec.push_back(info);
	ItemNode = xml.getNextNode(ItemNode, "mapid");
    }
    if(_mapVec.empty())
    {
	Zebra::logger->error("[动态地图] 加载配置地图失败 容器数据空");
	return false;
    }
    return true;
}

Scene* DynamicSceneManager::getOneDynScene()
{
    for(std::vector<dynMapInfo>::iterator it = _mapVec.begin(); it!=_mapVec.end(); it++)
    {
	return ((*it).mapTemplate);
    }
    return NULL;
}

bool DynamicSceneManager::createDynamicScene(DWORD uniqID)
{
    Scene *templetScene = getOneDynScene();
    if(!templetScene)
    {
	Zebra::logger->error("[动态地图] 找不到地图模板");
	return false;
    }
    DWORD mapID = (6<<16) + uniqID;
    char mapName[MAX_NAMESIZE];
    snprintf(mapName, MAX_NAMESIZE-1, "中立区·对战%u赛场", uniqID);
    Scene *s = SceneManager::getInstance().loadChallengeGameMap(mapID, mapName, templetScene);
    if(s)
    {
	s->dynMapType = 1;
	Cmd::Session::t_regScene_SceneSession regscene;
	regscene.dwCountryID = mapID>>16;
	regscene.dwID = s->id;
	regscene.dwTempID = s->tempid;
	strncpy(regscene.byName, s->name, MAX_NAMESIZE);
	strncpy(regscene.fileName, s->getFileName(), MAX_NAMESIZE);
	regscene.dynMapType = s->dynMapType;
	sessionClient->sendCmd(&regscene, sizeof(regscene));

	Zebra::logger->debug("[动态地图] 加载动态地图 发往会话注册 %s(%u %u)", s->getName(), s->id, s->tempid);
	return true;
    }
    return false;
}

bool DynamicSceneManager::unloadDynamicScene(Scene *s)
{
    if(!s)
	return false;

    Cmd::Session::t_unloadScene_SceneSession ret;
    ret.map_id = s->id;
    ret.map_tempid = s->tempid;
    sessionClient->sendCmd(&ret, sizeof(ret));

    Zebra::logger->debug("[动态地图] 注销动态地图 %s", s->getName());
    return true;
}
