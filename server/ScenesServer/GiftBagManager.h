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
	 * \brief ���ݵ���id��ȡ����Ʒ������
	 * \param   in:id, out:qualitys
	 * \return void
	 */
	void getGiftBagQualityVec(const DWORD id, std::vector<DWORD> &qualitys);

	/**
	 * \brief   ����index��Ʒ�������һ�ſ���
	 * \param   in:����index,  in:Ʒ��level
	 * \return  DWORD 
	 */
	DWORD randomOneCard(const DWORD index, const DWORD level);

	/**
	 * \brief ���ݵ���ID������
	 * \param   in:objid, out:cards
	 * \return void
	 */
	void openOneGiftBag(const DWORD objid, std::vector<DWORD> &cards);
    public:
	bool useGiftBag(SceneUser& user, zObject *obj);
};

#endif //_SCENESSERVER_GIFTBAGMANAGER_H_

