#ifndef _GROUPCARDMANAGER_H_
#define _GROUPCARDMANAGER_H_
#include "zType.h"
#include "zSingleton.h"
#include <map>
#include <vector>
///////////////////////////////////////////////
//
//code[GroupCardManager.h] defination by codemokey
//
//
///////////////////////////////////////////////

class SceneUser;

class GroupCardManager : public Singleton<GroupCardManager>
{
    public:
        friend class SingletonFactory<GroupCardManager>;
        GroupCardManager();
        ~GroupCardManager();
    public:
	WORD getOneIdTimes(const DWORD cardID, DWORD id[], const WORD count);
	WORD getOneNameTimes(const DWORD cardID, DWORD id[], const WORD count);
	DWORD getOccupationByIndex(SceneUser& user, const DWORD index);
	bool canUseOneGroup(SceneUser& user, const DWORD index);
	bool handleCreateOneGroup(SceneUser& user, const DWORD occupation, bool gm=false);
	bool handleDeleteOneGroup(SceneUser& user, const DWORD index);
	bool handleSaveOneGroup(SceneUser& user, DWORD id[], const WORD count, const DWORD index);
	bool handleRenameOneGroup(SceneUser& user, const DWORD index, char *name);
	void notifyAllGroupListToMe(SceneUser& user);	
	void notifyOneGroupInfoToMe(SceneUser& user, const DWORD index);
	bool initOneChallengeCards(SceneUser& user, const DWORD index, std::vector<DWORD> &lib);	    //初始化一个对战库
    private:
	bool checkGroupNameExist(char *name, SceneUser& user);
	DWORD countOneGroupCard(SceneUser& user, DWORD index);
};

struct t_group 
{ 
    t_group()
    {
	bzero(cards, sizeof(cards));
	bzero(name, sizeof(name));
	occupation = 0;
    }
    DWORD cards[30];	    //30张
    char name[MAX_NAMESIZE+1];	    //牌组名字
    DWORD occupation;
}__attribute__((packed)); 

class GroupCardData
{
    public:
	GroupCardData()
	{
	    groupCardMap.clear();
	}
	/*
	 * first 1000以下的，表示基本套牌   [1,999]
	 * first 1000以上的，表示自定义套牌 [1000,1999]
	 */ 
	std::map<DWORD, t_group> groupCardMap;			//index<---->cards
	unsigned int saveCardGroupData(unsigned char* dest);
	unsigned int loadCardGroupData(unsigned char* src);

};
#endif //_GROUPCARDMANAGER_H_

