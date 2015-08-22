#ifndef _SCENESSERVER_GIFTBAGMANAGER_H_
#define _SCENESSERVER_GIFTBAGMANAGER_H_
#include "zType.h"
#include "zSingleton.h"
#include <vector>
///////////////////////////////////////////////
//
//code[ScenesServer/GiftBagManager.h] defination by codemokey
//
//
///////////////////////////////////////////////

class SceneUser;
class zObject;

class GiftBagManager : public Singleton<GiftBagManager>
{
    public:
        friend class SingletonFactory<GiftBagManager>;
        GiftBagManager();
        ~GiftBagManager();
    private:
	/**
	 * \brief 根据道具id获取卡牌品质序列
	 * \param   in:id, out:qualitys
	 * \return void
	 */
	void getGiftBagQualityVec(const DWORD id, std::vector<DWORD> &qualitys);

	/**
	 * \brief   根据index和品质随机出一张卡牌
	 * \param   in:索引index,  in:品质level
	 * \return  DWORD 
	 */
	DWORD randomOneCard(const DWORD index, const DWORD level);

	/**
	 * \brief 根据道具ID开卡牌
	 * \param   in:objid, out:cards
	 * \return void
	 */
	void openOneGiftBag(const DWORD objid, std::vector<DWORD> &cards);
    public:
	bool useGiftBag(SceneUser& user, zObject *obj);
};

#endif //_SCENESSERVER_GIFTBAGMANAGER_H_

