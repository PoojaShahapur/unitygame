#ifndef _SCENESSERVER_DYNAMICSCENEMANAGER_H_
#define _SCENESSERVER_DYNAMICSCENEMANAGER_H_
#include "zType.h"
#include "zSingleton.h"
#include <vector>
///////////////////////////////////////////////
//
//code[ScenesServer/DynamicSceneManager.h] defination by codemokey
//
//
///////////////////////////////////////////////
class Scene;

class DynamicSceneManager : public Singleton<DynamicSceneManager>
{
    public:
        friend class SingletonFactory<DynamicSceneManager>;
        DynamicSceneManager();
        ~DynamicSceneManager();
	bool loadConfig();
	Scene *getOneDynScene();
	bool createDynamicScene(DWORD uniqID);
	bool unloadDynamicScene(Scene *s);
    private:
	struct dynMapInfo
	{
	    DWORD dwMapID;
	    Scene *mapTemplate;
	};

	std::vector<dynMapInfo> _mapVec;

};

#endif //_SCENESSERVER_DYNAMICSCENEMANAGER_H_

