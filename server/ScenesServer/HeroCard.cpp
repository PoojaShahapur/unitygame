#include "SceneUser.h"
#include "CardTujianManager.h"
#include "Mail.h"

/**     
 * \brief  处理stHeroCardCmd指令
 *
 *
 * 处理stHeroCardCmd指令
 *      
 * \param rev: 接受到的指令内容
 * \param cmdLen: 接受到的指令长度
 * \return 处理指令成功返回真,否则返回false
 */       
bool SceneUser::doHeroCardCmd(const Cmd::stHeroCardCmd *rev,DWORD cmdLen)
{
    switch(rev->byParam)
    {
#if 0
	case Cmd::REQ_CARD_GROUP_LIST_INFO_CMD:
	    {
		GroupCardManager::getMe().notifyAllGroupListToMe(*this);
		//Mail::sendSysText("ϵͳ�ʼ�", this->id, "��ӭ��������Ϸ!ף��������!");
		return true;
	    }
	    break;
#endif
	case Cmd::REQ_ONE_CARD_GROUP_INFO_CMD:
	    {
		Cmd::stRetOneCardGroupInfoUserCmd *cmd = (Cmd::stRetOneCardGroupInfoUserCmd*)rev;
		GroupCardManager::getMe().notifyOneGroupInfoToMe(*this, cmd->index);
		return true;
	    }
	    break;
	case Cmd::REQ_CREATE_ONE_CARD_GROUP_CMD:
	    {
		Cmd::stReqCreateOneCardGroupUserCmd *cmd = (Cmd::stReqCreateOneCardGroupUserCmd*)rev;
		GroupCardManager::getMe().handleCreateOneGroup(*this, cmd->occupation);
		return true;
	    }
	    break;
	case Cmd::REQ_SAVE_ONE_CARD_GROUP_CMD:
	    {
		Cmd::stReqSaveOneCardGroupUserCmd *cmd = (Cmd::stReqSaveOneCardGroupUserCmd*)rev;
		if(cmdLen < (sizeof(Cmd::stReqSaveOneCardGroupUserCmd)+sizeof(DWORD)*cmd->count))
		{
		    Zebra::logger->debug("[���]%s(%u) stReqSaveOneCardGroupUserCmd size=%u cmdLen=%u",this->name, this->id, cmd->count, cmdLen);
		    return true;
		}
		GroupCardManager::getMe().handleSaveOneGroup(*this, cmd->id, cmd->count, cmd->index);
		return true;
	    }
	    break;
	case Cmd::REQ_DELETE_ONE_CARD_GROUP_CMD:
	    {
		Cmd::stReqDeleteOneCardGroupUserCmd *cmd = (Cmd::stReqDeleteOneCardGroupUserCmd*)rev;
		GroupCardManager::getMe().handleDeleteOneGroup(*this, cmd->index);
		return true;
	    }
	    break;
	case Cmd::RENAME_CARD_GROUP_USERCMD_PARAMETER:
	    {//�������
		Cmd::stRenameCardGroupUserCmd *cmd = (Cmd::stRenameCardGroupUserCmd *)rev;
		GroupCardManager::getMe().handleRenameOneGroup(*this, cmd->index, cmd->name);
		return true;
	    }
	    break;
#if 0
	case Cmd::REQ_ALL_CARD_TUJIAN_DATA_CMD:
	    {
		CardTujianManager::getMe().notifyAllTujianDataToMe(*this);
		return true;
	    }
	    break;
	case Cmd::REQ_ALL_HERO_INFO_CMD:
	    {
		HeroInfoManager::getMe().notifyAllHeroInfoToMe(*this);
		return true;
	    }
	    break;
#endif
	case Cmd::REQ_HERO_FIGHT_MATCH_CMD:
	    {
		Cmd::stReqHeroFightMatchUserCmd *cmd = (Cmd::stReqHeroFightMatchUserCmd *)rev;

		if(ChallengeGameManager::getMe().isFighting(this))
		    return false;

		if(GroupCardManager::getMe().canUseOneGroup(*this, cmd->index))
		{
		    Cmd::Session::t_ReqFightMatch_SceneSession send;
		    send.userID = this->id;
		    send.cardsNumber = cmd->index;	    //����
		    send.score = 1;			    //ս��ֵ
		    send.type = cmd->fightType;	    //��ս����
		    send.cancel = cmd->cancel;	    
		    return sessionClient->sendCmd(&send, sizeof(send));
		}
		else
		{
		    Zebra::logger->error("[�����ս] ������Ʋ����� ���� ���Ʋ���30�� ");
		}
		return false;
	    }
	    break;
	case Cmd::REQ_END_MY_ROUND_CMD:		    //�����غ�
	    {
		ChallengeGameManager::getMe().handleEndOneRound(*this);
		return true;
	    }
	    break;
	case Cmd::REQ_GIVEUP_ONE_BATTLE_CMD:	    //����
	    {
		ChallengeGameManager::getMe().handleGiveUpBattle(*this);
		return true;
	    }
	    break;
	case Cmd::REQ_FIGHT_PREPARE_OVER_CMD:
	    {
		Cmd::stReqFightPrepareOverUserCmd *cmd = (Cmd::stReqFightPrepareOverUserCmd *)rev;
		if(cmd)
		{
		    ChallengeGameManager::getMe().handleStartGame(*this, cmd->change);
		}
		return true;
	    }
	    break;
	case Cmd::REQ_ENTER_UNFINISHED_GAME_CMD:
	    {
		ChallengeGameManager::getMe().handlesecondEnter(this);
		return true;
	    }
	    break;
	case Cmd::REQ_CARD_MAGIC_USERCMD_PARA:
	    {//��ͨ����
		Cmd::stCardAttackMagicUserCmd *cmd =(Cmd::stCardAttackMagicUserCmd *)rev;
		ChallengeGameManager::getMe().handleCardAttackMagic(*this, cmd);
		return true;
	    }
	    break;
	case Cmd::REQ_CARD_MOVE_AND_MAGIC_USERCMD_PARA:
	    {//����
		Cmd::stCardMoveAndAttackMagicUserCmd *cmd = (Cmd::stCardMoveAndAttackMagicUserCmd *)rev;
		ChallengeGameManager::getMe().handleCardMoveAndAttackMagic(*this, cmd);
		return true;
	    }
	    break;
	default:
	    break;
    }
    Zebra::logger->error("SceneUser::doHeroCardCmd\tparam:%d",rev->byParam);
    return false;
}

