/**
 * \brief AI部分的实现
 * 包括AI控制器和SceneNpc中AI相关的方法
 *
 * 
 */
#include "SceneNpcAI.h"
#include "SceneNpc.h"
#include "Scene.h"
#include "TimeTick.h"
#include "Chat.h"
#include "zDatabaseManager.h"

//std::list<stNewAIData> NpcAiXmlData;

/**
 * \brief 设置NPC的AI
 *
 * \param ai 要设置的AI结构的引用
 */
void SceneNpc::setAI(const t_NpcAIDefine &ai)
{
    AIDefine = ai;
}

/*
   const t_NpcAIDefine & SceneNpc::getAI()
   {
   return AIDefine;
   }
   */

/**
 * \brief 普通Npc动作
 * \return 动作执行是否成功
 */
bool SceneNpc::normalAction()
{
#ifdef _DEBUG
    //Channel::sendNine(this,"AIDefine.type=%u",AIDefine.type);
#endif

    //sky 建造状态下不做任何动作
    if(issetUState(Cmd::NPCSTATE_MAKE))
	return false;

    //TODO只有在发生变化时才发送
    if (hp!=lasthp)
    {
	//掉血重新计算回血时间
	if (hp<lasthp)
	    setRecoverTime(SceneTimeTick::currentTime,3000);

	sendtoSelectedHpAndMp();
	lasthp = hp;
#if 0
	if (Cmd::PET_TYPE_NOTPET!=getPetType())
	    ((ScenePet *)this)->sendHpExp();
#endif
    }

    recover();
    //moveToMaster();

    if (dread)
    {
	if(_half_sec(SceneTimeTick::currentTime))
	    dreadProcess();

	return true;
    }
#if 0
    if (blind)
    {
	if(_3_sec(SceneTimeTick::currentTime))
	    blindProcess();

	return true;
    }
#endif
    switch (AIDefine.type)
    {
	case NPC_AI_NORMAL:
	    {
		int ret = doNormalAI();
		return ret;
	    }
	    break;
	case NPC_AI_SAY:
	    {
		return doSayAI();
	    }
	    break;
	case NPC_AI_MOVETO:
	    {
		return doMovetoAI();
	    }
	    break;
	case NPC_AI_ATTACK:
	    {
		return doAttackAI();
	    }
	    break;
	case NPC_AI_GO_ON_PATH:
	    {
		return doGoOnPathAI();
	    }
	    break;
	case NPC_AI_PATROL:
	    {
		return doPatrolAI();
	    }
	    break;
	case NPC_AI_FLEE:
	    {
		return doFleeAI();
	    }
	    break;
	case NPC_AI_RETURN_TO_REGION:
	    {
		return doReturnToRegionAI();
	    }
	    break;
	case NPC_AI_CHANGE_MAP:
	    {
		return doChangeMapAI();
	    }
	    break;
	case NPC_AI_WARP:
	    {
		return doWarpAI();
	    }
	    break;
	case NPC_AI_CLEAR:
	    {
		return doClearAI();
	    }
	    break;
	case NPC_AI_DROP_ITEM:
	    {
		return doDropItemAI();
	    }
	    break;
	case NPC_AI_RANDOM_CHAT:
	    {
		return doRandomChatAI();
	    }
	    break;
	case NPC_AI_WAIT:
	    {
		return true;
	    }
	    break;
	default:
	    {
		return false;
	    }
	    break;
    }
#ifdef _DEBUG
    //t = SceneTimeTick::currentTime - t;
    //Zebra::logger->debug("处理 %s 结束,用时 %u",t);
#endif
}

/**
 * \brief 切换地图的AI处理
 * \return 执行AI是否成功
 */
bool SceneNpc::doChangeMapAI()
{
#if 0
    std::string string = AIDefine.str;
    std::string mapName(string);
    if (std::string::npos==string.find("·",0))
    {
	mapName = SceneManager::getInstance().getCountryNameByCountryID(scene->getCountryID());
	mapName += "·" + string;
    }

    Scene * s = SceneManager::getInstance().getSceneByName(mapName.c_str());
    if (!s)
    {
	Zebra::logger->info("doChangeMapAI:npc跳转地图失败,未找到地图 %s",mapName.c_str());
	return false;
    }

    if (s!=scene)
	changeMap(s,AIDefine.pos);
    else
	warp(AIDefine.pos);

    AIC->setActRegion(getPos());
    t_NpcAIDefine ad;
    ad.type = NPC_AI_WAIT;
    AIC->setAI(ad,false);
#endif
    return true;
}

/**
 * \brief 瞬间移动的AI处理
 * \return 执行AI是否成功
 */
bool SceneNpc::doWarpAI()
{
    warp(AIDefine.pos);
    AIC->setActRegion(getPos());
    t_NpcAIDefine ad;
    ad.type = NPC_AI_WAIT;
    AIC->setAI(ad,false);
    return true;
}

/**
 * \brief 普通AI处理
 * \return 执行AI是否成功[sky] AI的开端
 */
bool SceneNpc::doNormalAI()
{
    //if ((Cmd::PET_TYPE_NOTPET!=getPetType())&&(0==getMaster())) return true;

    if (dwStandTimeCount>0)
    {
#if 0
	if (dwStandTime <SceneTimeTick::currentTime.sec())
	{
	    SceneUser * pMaster = (SceneUser *)this->getMaster();

	    if (pMaster && pMaster->killOnePet((ScenePet *)this))
	    {
		setState(SceneEntry_Death);
		setClearState();
	    }
#ifdef _DEBUG
	    Zebra::logger->debug("定时NPC死亡timeCount=%u 过期时间[%u]当前时间[%u]",dwStandTimeCount,dwStandTime,SceneTimeTick::currentTime.sec());
#endif
	    return true;
	}
#endif
#ifdef _DEBUG
	Zebra::logger->debug("定时NPC持续timeCount=%u 过期时间[%u]当前时间[%u]",dwStandTimeCount,dwStandTime,SceneTimeTick::currentTime.sec());
#endif
    }

    switch (npc->kind)
    {
	case NPC_TYPE_TOTEM:      /// 图腾类型
	case NPC_TYPE_SURFACE:      /// 地表类型
	    {
#if 0
		if (canFight())
		{
		    if( !NpcAiTimeList.empty()/* && m_bFighting*/ )  //图腾直接就在战斗状态
		    {
			if (GetTickCount() - m_dwNowTime > 1000 ) 
			{
			    m_dwNowTime = GetTickCount();
			    m_dwGtime ++;
			}
			std::vector<NpcAiCs>::iterator it;
			for( it=NpcAiTimeList.begin(); it!=NpcAiTimeList.end(); it++ )
			{
			    lua_timeSkill( it->id, it->level, it->target, it->Rate, m_dwGtime, it->cond1 );
			}
		    }

		    if( !NpcAiList.empty() && m_SkillList.size() < 10)
		    {
			std::vector<NpcAiCs>::iterator it;
			for( it=NpcAiList.begin(); it!=NpcAiList.end(); it++ )
			{
			    lua_useSkill( it->id, it->level, it->target, it->Rate, it->cond1, it->cond2 );
			}
		    }

		    if( !m_SkillList.empty() )
		    {
			std::vector<m_NpcSkill>::iterator it;
			it = m_SkillList.begin();
			switch( it->SkillType )
			{
			    case 1:
				{
				    useSkill( it->target, it->id, it->level );
				}
				break;
			    case 2:
				{
				    useMobSkill( it->target, it->id, it->level );
				}
				break;
			}

			m_SkillList.erase( it );   //使用完第技能后将其删除掉
		    }
		    else
		    {
			SceneEntryPk_vec enemies;

			//计算半径
			int r = npc_search_region;
			if (aif&AIF_DOUBLE_REGION) r *= 2;
			BYTE at = getAType();
			if (NPC_ATYPE_FAR==at || NPC_ATYPE_MFAR==at)
			    r +=3;
			if (npc->kind == NPC_TYPE_SURFACE) r=10;

			getEntries(r,enemies,1);

			this->setPetAI((Cmd::petAIMode)(Cmd::PETAI_MOVE_STAND|Cmd::PETAI_ATK_ACTIVE));
			SceneEntryPk * enemy = chooseEnemy(enemies);
			if (enemy)
			{
			    //sendMeToNine();
			    /*
			       Cmd::stAddMapNpcMapScreenUserCmd addNpc;
			       full_t_MapNpcData(addNpc.data);
			       scene->sendCmdToNine(getPosI(),&addNpc,sizeof(addNpc));
			       */
			    npcSkill skill;
			    npc->getRandomSkillByType(SKILL_TYPE_DAMAGE,skill);
			    return useSkill(enemy,skill.id);
			}
		    }
		}
#endif
	    }
	    break;
	case NPC_TYPE_TRAP:        /// 陷阱类型
	    {
#if 0
		if (canFight())
		{
		    SceneEntryPk_vec enemies;

		    getEntries(2,enemies,1);

		    this->setPetAI((Cmd::petAIMode)(Cmd::PETAI_MOVE_STAND|Cmd::PETAI_ATK_ACTIVE));

		    SceneEntryPk * enemy = chooseEnemy(enemies);
		    if (enemy)
		    {
			npcSkill skill;
			if (npc->getRandomSkillByType(SKILL_TYPE_DAMAGE,skill))
			{
			    sendMeToNine();
			    /*
			       Cmd::stAddMapNpcMapScreenUserCmd addNpc;
			       full_t_MapNpcData(addNpc.data);
			       scene->sendCmdToNine(getPosI(),&addNpc,sizeof(addNpc));
			       */
			    if (useSkill(enemy,skill.id))
			    {
				SceneUser * pMaster = (SceneUser *)this->getMaster();
				if (pMaster && pMaster->killOnePet((ScenePet *)this))
				{
				    setState(SceneEntry_Death);
				    setClearState();
				}
			    }
			    return true;
			}
			else
			{
			    Zebra::logger->error("[怪物AI]NPC[%s]的技能没有加载无法使用！",this->name);
			}
		    }
		}
#endif
	    }
	    break;
	case NPC_TYPE_HUMAN:      ///人型
	case NPC_TYPE_TASK:      ///任务类型
	    {}
	    break;
	case NPC_TYPE_NORMAL:      /// 普通类型
	case NPC_TYPE_BBOSS:      /// 大Boss类型
	case NPC_TYPE_PBOSS:      /// 紫Boss类型
	case NPC_TYPE_LBOSS:      /// 小Boss类型
	case NPC_TYPE_BACKBONE:      /// 精英类型
	case NPC_TYPE_GOLD:        /// 黄金类型
	case NPC_TYPE_AGGRANDIZEMENT:  /// 强化类型
	case NPC_TYPE_BACKBONEBUG:    /// 精怪类型
	case NPC_TYPE_ABERRANCE:    /// 变异类型
	case NPC_TYPE_PET:  /// 宠物类型
	case NPC_TYPE_GUARD:  /// 士兵类型
	case NPC_TYPE_SUMMONS:      /// 召唤类型
	case NPC_TYPE_LIVENPC:      /// 生活npc
	case NPC_TYPE_SOLDIER:  /// 士兵类型
	case NPC_TYPE_CARTOONPET:  /// 卡通宝宝
	case NPC_TYPE_UNIONGUARD:  /// 城战守卫
	case NPC_TYPE_UNIONATTACKER:  /// 攻方士兵
	case NPC_TYPE_GHOST:			///sky 元神类
	case NPC_TYPE_TURRET:			///sky 炮台类
	case NPC_TYPE_BARRACKS:
	case NPC_TYPE_CAMP:
	case NPC_TYPE_ANIMON:
	    if ((checkAttackTime(SceneTimeTick::currentTime))
		    ||(checkMoveTime(SceneTimeTick::currentTime)))
	    {   	
		if(canFight())
		{
		    SceneEntryPk * enemy = 0;
		    SceneEntryPk_vec enemies;

		    //当前的目标
		    checkChaseAttackTarget(enemy);

		    //主动搜索
		    if (!enemy && isActive())
		    {

			//计算半径
			int r = npc_search_region;
			if (aif&AIF_DOUBLE_REGION) r *= 2;
			BYTE at = getAType();
			if (NPC_ATYPE_FAR==at || NPC_ATYPE_MFAR==at)
			    r +=3;

			getEntries(r,enemies,1);
		    }
		    //对列表处理
		    if (enemies.size())
		    {
			//逃离玩家
			if (runOffEnemy(enemies)) return true;

			//吸引玩家
			if (catchme) 
			{
			    SceneEntryPk_vec::iterator it;
			    for(it = enemies.begin(); it!= enemies.end(); it++)
			    {
				if (zMisc::selectByPercent(catchme)) (*it)->setCurTarget(this->tempid,this->getType());
			    }
			}

			//选择敌人
			enemy = chooseEnemy(enemies);
		    }
		    //对找到的敌人处理
		    if (enemy)
		    {
			switch (zMisc::randBetween(1,10))
			{
			    case 1:
				if (buffSelf()) return true;
				break;
			    case 2:
				if (debuffEnemy(enemy)) return true;
				break;
			    default:
				break;
			}
			return attackEnemy(enemy);
		    }	 
		}
		if (NPC_AI_PATROL==AIDefine.type)
		    return doMovetoAI();
		else
		    return randomMove();
	    }
	    break;
	default:
	    break;
    }
    return true;
}

/**
 * \brief 移动AI处理
 * 每移动一次就设置活动范围,以免NPC超出活动范围
 * \return 执行是否成功
 */
bool SceneNpc::doMovetoAI()
{
    if (!AIC->phaseTimeOver())
	if (checkMoveTime(SceneTimeTick::currentTime) && canMove())
	{
	    if (!gotoFindPath(getPos(),AIDefine.pos))//向目标移动
		goTo(AIDefine.pos);//换一种方式向目标移动
	    if (AIC)
	    {
		if (NPC_AI_PATROL==AIDefine.type)
		    AIC->setActRegion(getPos(),5,5);
		else
		    AIC->setActRegion(getPos());
	    }
	}
    return true;
}

/**
 * \brief 攻击AI处理
 * 就是加了时间限制的普通行动
 * \return 执行AI是否成功
 */
bool SceneNpc::doAttackAI()
{
    return doNormalAI();
}

/**
 * \brief 设置npc普通AI
 * 普通AI时AIDefine的其他参数均无效
 */
void NpcAIController::setNormalAI()
{
    curAI.type = NPC_AI_NORMAL;
    setAI(curAI);
}

/**
 * \brief NPC被攻击的事件处理
 *
 * \param pAtk 攻击者
 */
void NpcAIController::on_hit(SceneEntryPk *pAtk)
{
#if 0
    if (0>=npc->hp) return;

    npc->randomChat(NPC_CHAT_ON_HIT);

    //[sky] NPC第一次受到攻击激活AI定时器
    if(!npc->m_bFighting )
    {
	npc->m_dwNowTime = GetTickCount();
	npc->m_mobType = false;
	npc->m_bFighting	= true;
	npc->m_dwGtime	= 0;

    }

    //npc->delayMoveTime(npc->npc->distance/2);//被击动作的延迟
    switch(npc->npc->kind)
    {
	case NPC_TYPE_HUMAN:      ///人型
	case NPC_TYPE_NORMAL:      /// 普通类型
	case NPC_TYPE_BBOSS:      /// 大Boss类型
	case NPC_TYPE_PBOSS:      /// 紫Boss类型
	case NPC_TYPE_LBOSS:      /// 小Boss类型
	case NPC_TYPE_BACKBONE:      /// 精英类型
	case NPC_TYPE_GOLD:        /// 黄金类型
	case NPC_TYPE_SUMMONS:      /// 召唤类型
	case NPC_TYPE_AGGRANDIZEMENT:  /// 强化类型
	case NPC_TYPE_ABERRANCE:    /// 变异类型
	case NPC_TYPE_BACKBONEBUG:    /// 精怪类型
	case NPC_TYPE_TURRET:			/// 炮塔
	case NPC_TYPE_BARRACKS:
	case NPC_TYPE_CAMP:
	case NPC_TYPE_GHOST:			/// 元神
	case NPC_TYPE_ANIMON:        /// 动物类
	    {
		//hp1/3以下时逃跑
		if (npc->aif&AIF_FLEE_30_HP)
		{
		    if ((npc->hp*3<npc->getMaxHP()) && (false==curAI.flee))
		    {
			t_NpcAIDefine ad;
			ad.type = NPC_AI_FLEE;
			ad.flee = true;
			ad.fleeCount = npc_flee_distance;
			ad.fleeDir = pAtk->getPos().getDirect(npc->getPos());
			setAI(ad,false);

			//Channel::sendNine(this,"hp不到1/3了,我逃~");
		    }
		}

		//被3个以上围攻逃跑
		if (npc->aif&AIF_FLEE_3_ENEMY_4)
		{
		    int side = 0;
		    int direct = 0;
		    int clockwise = 1;
		    int enemyCount = 0;
		    int count = 0;//计数,防止死循环
		    zPos pos;
		    npc->scene->getNextPos(side,direct,npc->getPos(),clockwise,pos);
		    do                      
		    {                       
			SceneUser *sceneUser = npc->scene->getSceneUserByPos(pos);
			if (sceneUser && sceneUser->getState() == zSceneEntry::SceneEntry_Normal)
			{
			    if (npc->isAttackMe(sceneUser))
				enemyCount++;
			}    
			if (++count>=8)
			    break;
		    } while(npc->scene->getNextPos(side,direct,npc->getPos(),clockwise,pos) && side <= 1);

		    if (enemyCount>=3)
		    {
			t_NpcAIDefine ad;
			ad.type = NPC_AI_FLEE;
			ad.flee = true;
			ad.fleeCount = npc_flee_distance;
			ad.fleeDir = pAtk->getPos().getDirect(npc->getPos());
			setAI(ad,false);
			//Channel::sendNine(this,"%d 个人围攻我！",enemyCount);
		    }
		}

		//生命20%以下移动速度提高
		if ((npc->aif&AIF_SPD_UP_HP20)&&(npc->hp*5<npc->getMaxHP()))
		    npc->speedUpUnder20 = true;
		else
		    npc->speedUpUnder20 = false;
		npc->setSpeedRate(npc->getSpeedRate());

		//生命50%以下攻击速度提高
		if ((npc->aif&AIF_ASPD_UP_HP50)&&(npc->hp*2<npc->getMaxHP()))
		    npc->aspeedUpUnder50 = true;
		else
		    npc->aspeedUpUnder50 = false;
		npc->resetAspeedRate();

		npc->setSecondTarget(pAtk);

		//生命60%以下召唤
		if (!npc->define->summonList.empty() && npc->hp*10<npc->getMaxHP()*6 &&!npc->summoned)
		{
		    npc->summonByNpcMap(npc->define->summonList);
		    npc->summoned = true;
		}
	    }
	    break;
	case NPC_TYPE_GUARD:  /// 士兵类型
	case NPC_TYPE_PET:  /// 宠物类型
	case NPC_TYPE_TASK:    /// 任务类型
	case NPC_TYPE_TOTEM:      /// 图腾类型
	case NPC_TYPE_TRADE:  /// 买卖类型
	    break;
	default:
	    break;
    }
    /*
       switch (curAI.type)
       {
       case NPC_AI_MOVETO:
       {
       npc->delayMoveTime(npc_onhit_stop_time*1000);
       }
       break;
       default:
       break;
       }
       */
#endif
}

/**
 * \brief 固定路线行走的AI处理
 * \return 执行AI是否成功
 */
bool SceneNpc::doGoOnPathAI()
{
    /*
       if (!AIC) return true;
       check();
       if (AIC->phaseTimeOver() && moveAction )
       warp(AIDefine.pos);

    //到达目标附近
    if (scene->zPosShortRange(getPos(),AIDefine.pos,AIDefine.regionX))
    {
    if (dstPos<define->path.size()-1) 
    dstPos++;
    else {
    on_reached();
    dstPos = 0;
    }
    AIDefine.pos = define->path[dstPos];
    AIC->delayPhaseTime(npc_one_checkpoint_time);
    }

    if (checkMoveTime(SceneTimeTick::currentTime) && moveAction)
    {
    if (!gotoFindPath(getPos(),AIDefine.pos))//向目标移动
    goTo(AIDefine.pos);//换一种方式向目标移动
    }
    */
    return true;
}

/**
 * \brief 巡逻AI处理
 * 巡逻基本是带攻击的移动
 * 只攻击红名的对象和正在攻击人的怪物
 *
 * \return 执行AI是否成功
 */
bool SceneNpc::doPatrolAI()
{
    return doNormalAI();
}

/**
 * \brief 逃跑AI的处理
 *
 * \return 执行AI是否成功
 */
bool SceneNpc::doFleeAI()
{
    if (checkMoveTime(SceneTimeTick::currentTime) && canMove())
    {
	if (AIDefine.flee&&(AIDefine.fleeCount>0))
	{
	    int clockwise = zMisc::selectByPercent(50)?-1:1;
	    int tryDir = AIDefine.fleeDir + 8;
	    for (int i=0; i<8; i++)
	    {
		if (shiftMove(tryDir%8))
		{
		    AIDefine.fleeCount--;
		    if (0==AIDefine.fleeCount)
		    {
			delayMoveTime(3000);
			AIC->switchAI(false);
		    }
		    return true;
		}
		tryDir += clockwise;
	    }
	    return doNormalAI();
	}
    }
    return true;
}

/**
 * \brief 回到活动范围的AI处理
 *
 * \return 执行AI是否成功
 */
bool SceneNpc::doReturnToRegionAI()
{
    if (!AIC->phaseTimeOver())
	if (checkMoveTime(SceneTimeTick::currentTime) && canMove())
	{
	    if (aif&AIF_WARP_MOVE)
		warp(AIDefine.pos);
	    else
		if (!gotoFindPath(getPos(),AIDefine.pos))//向目标移动
		    goTo(AIDefine.pos);//换一种方式向目标移动
	}
    return true;
}

/**
 * \brief NPC发现敌人时的事件处理
 *
 * \param pFound 发现的敌人
 */
void NpcAIController::on_find_enemy(const SceneEntryPk *pFound)
{
    if (0>=npc->hp) return;

    if(npc->dupIndex != pFound->dupIndex)
	return;

    int side = 0;
    if (npc->aif&AIF_CALL_FELLOW_7)
	side = 3;
    if (npc->aif&AIF_CALL_FELLOW_9)
	side = 4;

    SceneEntryPk_vec fellows;
    if (npc->getEntries(side,fellows,0))
    {
	for (DWORD i=0;i<fellows.size();i++)
	{
	    if (fellows[i]->getType()!=zSceneEntry::SceneEntry_NPC) continue;
	    //if (SceneNpc::CHASE_NONE != ((SceneNpc *)fellows[i])->getChaseMode()) continue;
	    if (0 != fellows[i]->curTargetID) continue;
	    //if (!zMisc::selectByPercent(50)) continue;
	    if (npc->aif&AIF_CALL_BY_ATYPE)
		if (!(npc->getAType()==((SceneNpc *)fellows[i])->getAType()))
		    continue;
	    ((SceneNpc *)fellows[i])->chaseSceneEntry(pFound->getType(),pFound->tempid);
	}

    }

    npc->randomChat(NPC_CHAT_ON_FIND_ENEMY);

    /*
       switch(npc->npc->kind)
       {
       case NPC_TYPE_HUMAN:      ///人型
       case NPC_TYPE_NORMAL:      /// 普通类型
       case NPC_TYPE_BBOSS:      /// 大Boss类型
       case NPC_TYPE_PBOSS:      /// 紫Boss类型
       case NPC_TYPE_LBOSS:      /// 小Boss类型
       case NPC_TYPE_BACKBONE:      /// 精英类型
       case NPC_TYPE_GOLD:        /// 黄金类型
       case NPC_TYPE_SUMMONS:      /// 召唤类型
       case NPC_TYPE_AGGRANDIZEMENT:  /// 强化类型
       case NPC_TYPE_ABERRANCE:    /// 变异类型
       case NPC_TYPE_BACKBONEBUG:    /// 精怪类型
       {
       if (zMisc::selectByPercent(npc_call_fellow_rate))
       {
    //召唤同伴
    int direct = zMisc::randBetween(0,7);
    int clockwise = zMisc::selectByPercent(50)?-1:1;
    int fellowCount = 0;
    int region = 7;//召唤同伴的范围
    int trycount = 0;//查找次数限制,防止死循环
    zPos pos;
    npc->scene->getNextPos(side,direct,npc->getPos(),clockwise,pos);
    do                      
    {                       
    if ((++trycount)>(region*region))
    break;
    SceneNpc * sceneNpc = npc->scene->getSceneNpcByPos(pos);
    if (sceneNpc&&(0==sceneNpc->getChaseSceneEntry()))
    {
    if (sceneNpc->canChaseTarget(pFound))
    if (sceneNpc->chaseSceneEntry(pFound->getType(),pFound->tempid))
    fellowCount++;
    }                               
    } while(npc->scene->getNextPos(side,direct,npc->getPos(),clockwise,pos) && side <= 7);
    //if (fellowCount>0)
    //Channel::sendNine(npc,"我叫 %d 个同伴来帮我打 %s",fellowCount,pFound->name);
    }
    }
    break;
    case NPC_TYPE_GUARD:  /// 士兵类型
    case NPC_TYPE_PET:  /// 宠物类型
    case NPC_TYPE_TASK:    /// 任务类型
    case NPC_TYPE_TOTEM:      /// 图腾类型
    case NPC_TYPE_TRADE:  /// 买卖类型
    break;
    default:
    break;
    }
    */
}

/**
 * \brief 判断Npc是否走出了自身固有范围之外
 * 如果没有在跟踪用户状态,需要向范围方向行走
 * 追逐时范围扩大10
 * \return 是否超出活动范围
 */
bool NpcAIController::outOfRegion() const
{
    if (!(npc->canMove())) return false;
    const zPos &pos = npc->getPos();
    //if (npc->getChaseMode() == SceneNpc::CHASE_NONE)
    if (0==npc->curTargetID)
	return !((pos.x>=actPos.x-actRegionX)
		&&(pos.x<=actPos.x+actRegionX)
		&&(pos.y>=actPos.y-actRegionY)
		&&(pos.y<=actPos.y+actRegionY));
    else
	return !((pos.x>=actPos.x-(actRegionX+10))
		&&(pos.x<=actPos.x+(actRegionX+10))
		&&(pos.y>=actPos.y-(actRegionY+10))
		&&(pos.y<=actPos.y+(actRegionY+10)));
}

//回到活动范围,追踪敌人超出范围时以5倍速返回
/**
 * \brief 设置回到范围的AI使NPC回到活动范围
 * 如果NPC是跟踪敌人而超出了活动范围,则以5倍速返回活动范围内,同时放弃跟踪目标
 *
 */
void NpcAIController::returnToRegion()
{
    t_NpcAIDefine def = curAI;
    def.type = NPC_AI_RETURN_TO_REGION;
    def.pos = actPos;
    def.regionX = zMisc::randBetween(2,actRegionX);
    def.regionY = zMisc::randBetween(2,actRegionY);
    def.lasttime = 10;
    setAI(def);
    if (0!=npc->curTargetID)
    {
	npc->randomChat(NPC_CHAT_ON_RETURN);
	npc->unChaseUser();
	npc->setSpeedRate(npc->getSpeedRate()*4.0);
    }
    //Channel::sendNine(npc,"我得回到(%d,%d)范围%d,%d以内",curAI.pos.x,curAI.pos.y,def.regionX,def.regionY);
}

/**
 * \brief 创建一次攻城
 *
 * \return 是否创建成功
 */
bool SceneNpc::createRush()
{
#if 0
    Rush * rush = new Rush(define->rushID,define->rushDelay,scene->getCountryID());
    if (rush)
    {
	if (rush->init())
	{
	    setMoveTime(SceneTimeTick::currentTime,(define->rushDelay+rush->lasttime+define->interval) * 1000);//攻城结束再过一周期重生
	    mayRush = false;

	    //发出公告
	    Cmd::Session::t_cityRush_SceneSession send;
	    bzero(send.bossName,MAX_NAMESIZE);
	    bzero(send.rushName,MAX_NAMESIZE);
	    bzero(send.mapName,MAX_NAMESIZE);
	    strncpy(send.bossName,rush->bossName,MAX_NAMESIZE-1);
	    strncpy(send.rushName,rush->rushName,MAX_NAMESIZE-1);
	    strncpy(send.mapName,rush->mapName,MAX_NAMESIZE-1);
	    send.delay = rush->rushDelay;
	    send.countryID = scene->getCountryID();
	    sessionClient->sendCmd(&send,sizeof(send));
	    return true;
	}
	Zebra::logger->debug("初始化攻城数据失败");
	SAFE_DELETE(rush);
    }
#endif
    return false;
}

/**
 * \brief 检查是否达到攻城条件
 *
 * \return 是否可以攻城
 */
bool SceneNpc::canRush()
{
    if (mayRush && (define->rushID) && (zSceneEntry::SceneEntry_Death==getState()))
    {
	Zebra::logger->info("%s 怪物攻城的几率 %d%%",name,define->rushRate);
	if (define->rushRate >= zMisc::randBetween(0,100))
	    return true;
    }
    return false;
}

const int NpcAIController::npc_call_fellow_rate = 30;///NPC召唤同伴的几率
const int NpcAIController::npc_one_checkpoint_time = 60;///NPC按照路线移动时,走一个路点的最长时间
const int NpcAIController::npc_checkpoint_region = 2;///NPC移动,到达一个路点的判定范围
const int NpcAIController::npc_onhit_stop_time = 2;///任务NPC移动中被攻击时,停止的时间
const int NpcAIController::npc_flee_distance = 4;///NPC逃离攻击者的距离
const int NpcAIController::npc_min_act_region = 5;///NPC逃离攻击者的距离

/*------------NpcAIController-------------------*/
/*------------NpcAIController-------------------*/
/*------------NpcAIController-------------------*/
/*------------NpcAIController-------------------*/

/**
 * \brief 设置NPC的活动范围
 *
 * \param pos 中心位置
 * \param x,y 范围的宽和高
 * \return 
 */
void NpcAIController::setActRegion(zPos pos,int x,int y)
{
    if (pos==zPos(0,0)) pos = npc->getPos();
    actPos = pos;
    if (0<=x) actRegionX = x>=npc_min_act_region?x:npc_min_act_region;
    if (0<=y) actRegionY = y>=npc_min_act_region?y:npc_min_act_region;
    //Channel::sendNine(npc,"活动范围:(%d,%d) x=%d y=%d",actPos.x,actPos.y,actRegionX,actRegionY);
}

/**
 * \brief 得到NPC的活动范围
 *
 * \param pos 输出：中心位置
 * \param x,y 输出：范围的宽和高
 */
void NpcAIController::getActRegion(zPos &pos,int &x,int &y)
{
    pos = actPos;
    x = actRegionX;
    y = actRegionY;
}

/**
 * \brief 构造函数
 *
 * \param sn 要控制的npc指针
 */
NpcAIController::NpcAIController(SceneNpc * sn)
:curPhase(0),repeat(-1),active(false),npc(sn),reached(false)
{
    chaseOutSize = 10;
    bzero(dstMap,sizeof(dstMap));
    if (sn)
	strncpy(dstMap,sn->scene->name,MAX_NAMESIZE-1);
    dstPos = zPos(0,0);
    curAI.type = NPC_AI_NORMAL;
}

/**
 * \brief 读取NPC智能脚本[sky特别注释]
 *
 * \param id 脚本ID
 * \return 是否加载成功
 */
bool NpcAIController::loadScript(DWORD id)
{
    if ((!npc)||(!id)) return false;

    if (!phaseVector.empty())
	unloadScript();

    zXMLParser xml;
    if (!xml.initFile(Zebra::global["configdir"] + "NpcAIScript.xml"))
    {
	Zebra::logger->error("打开NpcAIScript.xml失败");
	return false;
    }

    xmlNodePtr root = xml.getRootNode("info");
    if (!root) return false;
    xmlNodePtr scriptNode = xml.getChildNode(root,"script");

    while (scriptNode)
    {
	DWORD num=0;
	xml.getNodePropNum(scriptNode,"id",&num,sizeof(num));
	if (num==id)
	{
	    xml.getNodePropNum(scriptNode,"repeat",&repeat,sizeof(repeat));

	    xmlNodePtr phaseNode = xml.getChildNode(scriptNode,"phase");
	    char action[32];
	    t_NpcAIDefine ad;
	    while (phaseNode)
	    {
		bzero(action,sizeof(action));
		xml.getNodePropStr(phaseNode,"action",action,sizeof(action));
		ad.type = parseAction(action);
		if ((ad.type==NPC_AI_MOVETO)||(ad.type==NPC_AI_PATROL))
		    dstPos = ad.pos;
		xml.getNodePropStr(phaseNode,"str",ad.str);
		//Zebra::logger->debug("loadScript:%s",ad.str);
#if 0
		if (ad.type==NPC_AI_CHANGE_MAP)
		    strncpy(dstMap,ad.str,MAX_NAMESIZE-1);
#endif
		xml.getNodePropNum(phaseNode,"x",&ad.pos.x,sizeof(ad.pos.x));
		xml.getNodePropNum(phaseNode,"y",&ad.pos.y,sizeof(ad.pos.y));
		xml.getNodePropNum(phaseNode,"regionX",&ad.regionX,sizeof(ad.regionX));
		xml.getNodePropNum(phaseNode,"regionY",&ad.regionY,sizeof(ad.regionY));
		xml.getNodePropNum(phaseNode,"chaseOut",&ad.chaseOut,sizeof(ad.chaseOut));
		xml.getNodePropNum(phaseNode,"lasttime",&ad.lasttime,sizeof(ad.lasttime));

		phaseVector.push_back(ad);
		phaseNode = xml.getNextNode(phaseNode,"phase");
	    }

	    if (!phaseVector.empty())
	    {
		if (id>500) active = true;//500以下是共享脚本,不算作特殊npc
		nextPhase(0);
		//Zebra::logger->info("[AI]%s 读取NPC智能脚本 id=%d phase=%d",npc->name,id,phaseVector.size());
		return true;
	    }
	    else
	    {
		Zebra::logger->info("读取空智能脚本 id=%d",id);
		return false;
	    }
	}
	scriptNode = xml.getNextNode(scriptNode,"script");
    }

    Zebra::logger->warn("未找到AI脚本 id=%d",id);
    return false;
}

/**
 * \brief 卸载脚本
 *
 */
void NpcAIController::unloadScript()
{
    active = false;
    phaseVector.clear();
}

/**
 * \brief 设置脚本循环次数
 * 次数有3个范围：
 * -1：无限循环
 *  0：已经结束
 *  >0：递减
 *
 * \param re 次数
 */
void NpcAIController::setRepeat(int re)
{
    repeat = re;
}

/**
 * \brief 得到循环次数
 *
 * \return 循环次数
 */
int NpcAIController::getRepeat()
{
    return repeat;
}

/**
 * \brief 检查是否起用了脚本
 *
 * \return 是否起用了脚本
 */
bool NpcAIController::isActive()
{
    return active;
}

/**
 * \brief 根据字符串得到动作标识
 *
 * \param action 传入的字符串
 * \return 解析出的标识
 */
SceneNpcAIType NpcAIController::parseAction(char * action)
{
    if (0 == strcmp(action,"moveto")) return NPC_AI_MOVETO;
    if (0 == strcmp(action,"say")) return NPC_AI_SAY;
    if (0 == strcmp(action,"patrol")) return NPC_AI_PATROL;
    if (0 == strcmp(action,"attack")) return NPC_AI_ATTACK;
    //if (0 == strcmp(action,"recover")) return NPC_AI_RECOVER;
    if (0 == strcmp(action,"changemap")) return NPC_AI_CHANGE_MAP;
    if (0 == strcmp(action,"warp")) return NPC_AI_WARP;
    if (0 == strcmp(action,"clear")) return NPC_AI_CLEAR;
    if (0 == strcmp(action,"wait")) return NPC_AI_WAIT;
    if (0 == strcmp(action,"dropitem")) return NPC_AI_DROP_ITEM;
    if (0 == strcmp(action,"randomchat")) return NPC_AI_RANDOM_CHAT;

    Zebra::logger->error("parseAction : 未知的脚本动作 %s",action);
    return NPC_AI_NORMAL;
}

/**
 * \brief 设置该阶段的结束时间
 *
 * \param delay 从现在开始的延迟,毫秒为单位
 */
void NpcAIController::setPhaseTime(const int delay)
{
    phaseEndTime = SceneTimeTick::currentTime;
    phaseEndTime.addDelay(delay);
}

/**
 * \brief 延长该阶段结束的时间
 *
 * \param delay 延长的时间,单位毫秒
 */
void NpcAIController::delayPhaseTime(const int delay)
{
    phaseEndTime.addDelay(delay);
}

/**
 * \brief 检查阶段时间是否结束
 *
 * \return 是否到了阶段结束时间
 */
bool NpcAIController::phaseTimeOver()
{
    if (NPC_AI_NORMAL!=curAI.type)
    {
	return SceneTimeTick::currentTime >= phaseEndTime;
    }
    else
	return false;
}

/**
 * \brief 进入下一阶段
 *
 * \param index 可以指定要进入的阶段编号,-1表示下一阶段
 */
void NpcAIController::nextPhase(int index = -1)
{
    if (index>-1)
	curPhase = index;
    else
    {
	if (curPhase==phaseVector.size()-1)
	{
	    if (-2==repeat)
	    {
		curPhase = zMisc::randBetween(0,phaseVector.size()-1);
	    }
	    else if (-1==repeat)
	    {
		curPhase = 0;
	    }
	    else if (1==repeat)
	    {
		active = false;
		phaseVector.clear();
		repeat--;
		npc->on_reached();
		setNormalAI();
		return;
	    }
	    else if (repeat>1)
	    {
		curPhase = 0;
		repeat--;
		//Channel::sendNine(npc,"repeat=%d",repeat);
	    }
	}
	else
	    curPhase++;
    }


    if (phaseVector.size())
	setAI(phaseVector[curPhase]);
    //Zebra::logger->debug("size:%u curPhase:%u",phaseVector.size(),curPhase);
    //Channel::sendNine(npc," ����׶�%d",curPhase);
}

/**
 * \brief 处理阶段结束的事件
 * 不同阶段结束时处理不同
 * 移动/巡逻/回到范围：如果时间结束未到达目的地则瞬移过去
 *
 */
void NpcAIController::on_phaseEnd()
{
    switch (curAI.type)
    {
	case NPC_AI_PATROL:
	    {
		if (!(npc->checkMoveTime(SceneTimeTick::currentTime)&&npc->canMove())) return;

		npc->warp(curAI.pos);
		//if (!arrived(curAI.pos))
		setActRegion(npc->getPos());
	    }
	    break;
	case NPC_AI_MOVETO:
	    {
		if (!(npc->checkMoveTime(SceneTimeTick::currentTime)&&npc->canMove())) return;

		if (npc->warp(curAI.pos))
		    setActRegion(npc->getPos());
		else
		    return;
	    }
	    break;
	case NPC_AI_RETURN_TO_REGION:
	    {
		if (!(npc->checkMoveTime(SceneTimeTick::currentTime)&&npc->canMove())) return;

		if (NPC_AI_NORMAL!=oldAI.type)
		    npc->warp(curAI.pos);
		setAI(oldAI);
		npc->resetSpeedRate();
	    }
	    break;
	default:
	    break;
    }
    nextPhase();
}

/**
 * \brief 检查各种事件,并切换相应状态
 * 该方法在SceneNpc::action中执行
 *
 */
void NpcAIController::processPhase()
{
    if (zSceneEntry::SceneEntry_Death==npc->getState()) return;
#ifdef _DEBUG
    //Channel::sendNine(npc,"ai=%u speed=%f skillValue.movespeed=%d",curAI.type,npc->speedRate,npc->skillValue.movespeed);
#endif

    if (phaseTimeOver() && curAI.type!=NPC_AI_FLEE)
      on_phaseEnd();
#if 0
    if (curAI.type!=NPC_AI_RETURN_TO_REGION
	    &&curAI.type!=NPC_AI_MOVETO
	    &&curAI.type!=NPC_AI_FLEE
	    &&curAI.type!=NPC_AI_WAIT
	    &&(Cmd::PET_TYPE_NOTPET==npc->getPetType())//宠物不判断范围
	    &&((NPC_TYPE_GUARD==npc->npc->kind)||(npc->aif&AIF_ATK_REDNAME)||curAI.type==NPC_AI_PATROL||npc->aif&AIF_LIMIT_REGION)//卫兵和巡逻才回到原位
	    &&outOfRegion())
	returnToRegion();
#endif
    if(curAI.type == NPC_AI_PATROL)
    {
	if(outOfRegion())
	    returnToRegion();
    }

    if (0!=npc->curTargetID)
    {
	if (npc->checkEndBattleTime(SceneTimeTick::currentTime))
	    if (!(npc->lockTarget || npc->aif&AIF_LOCK_TARGET))
		npc->unChaseUser();
    }

    switch (curAI.type)
    { 
	case NPC_AI_MOVETO:
	    { 
		if (arrived(curAI.pos))
		{
		    setActRegion(curAI.pos,curAI.regionX,curAI.regionY);
		    chaseOutSize = curAI.chaseOut;
		    nextPhase();
		}
	    }
	    break;
	case NPC_AI_PATROL:
	    {
		if (arrived(curAI.pos))
		    nextPhase();
	    }
	    break;
	case NPC_AI_RETURN_TO_REGION:
	    {
		if (arrived())
		{
		    setAI(oldAI);
		    npc->resetSpeedRate();
		}
	    }
	    break;
	default:
	    break;
    }
}

/**
 * \brief npc复活事件的处理
 *
 */
void NpcAIController::on_relive()
{
    //if (!active) return;

    if (curAI.type==NPC_AI_FLEE)
	setNormalAI();

    setActRegion(npc->getPos(),npc->define->width/2,npc->define->height/2);
    //setNormalAI();

    nextPhase(0);

}

/**
 * \brief 设置npc的AI,设置阶段时间
 *
 * \param ai 要设置的AI（采用引用会有问题
 * \param setTime 是否同时设置时间,默认为真
 */
void NpcAIController::setAI(const t_NpcAIDefine ai,const bool setTime)
{
    oldAI = curAI;
    npc->setAI(ai);
    chaseOutSize = curAI.chaseOut;
    curAI = ai;
    if (setTime)
	setPhaseTime(ai.lasttime*1000);

    switch (curAI.type)
    {
	case NPC_AI_ATTACK:
	    {
		setActRegion(curAI.pos,curAI.regionX,curAI.regionY);
	    }
	    break;
	case NPC_AI_FLEE:
	    {
		npc->randomChat(NPC_CHAT_ON_FLEE);
	    }
	    break;
	default:
	    break;
    }
    //Zebra::logger->debug("setAI(): %s AI=%d",npc->name,ai.type);
}


/**
 * \brief 交换oldAI和curAI
 *
 *
 * \param setTime 是否同时设置时间,默认为真
 */
void NpcAIController::switchAI(const bool setTime)
{
    if (oldAI.type!=NPC_AI_FLEE)
	setAI(oldAI,setTime);
    else
	setNormalAI();
}

/**
 * \brief 判断npc是否到达某位置的某范围内
 *
 * \param pos 中心位置,默认是当前AI的目标位置
 * \param regionX 范围宽,默认是当前AI的范围宽
 * \param regionY 范围高,默认是当前AI的范围高
 * \return 是否在范围内
 */
bool NpcAIController::arrived(zPos pos,int regionX,int regionY)
{
    if (pos == zPos(0,0)) pos = curAI.pos;
    if (-1==regionX) regionX = curAI.regionX;
    if (-1==regionY) regionY = curAI.regionY;

    //zPos npcPos = npc->getPos();
    return (npc->getPos().x>=pos.x-regionX)   
	&&(npc->getPos().x<=pos.x+regionX)  
	&&(npc->getPos().y>=pos.y-regionY)  
	&&(npc->getPos().y<=pos.y+regionY); 
}

/**
 * \brief 是否到达目的地
 * 目的地的定义是脚本中最后一个移动的位置
 * 地图不同不算到达
 *
 */
bool NpcAIController::dstReached()
{
    if (strcmp(dstMap,npc->scene->name)) return false;

    if (arrived(dstPos,npc_checkpoint_region,npc_checkpoint_region))
	return true;

    return false;
}

/**
 * \brief 处理npc死亡的事件
 *
 */
void NpcAIController::on_die()
{
    npc->randomChat(NPC_CHAT_ON_DIE);
    npc->summoned = false;
}

/**
 * \brief 执行清除AI
 *
 * \return 是否执行成功
 */
bool SceneNpc::doClearAI()
{
    if (!clearMe)
    {
	setClearState();

	if (npc->id==ALLY_GUARDNPC)//盟国镖车
	{
	    Cmd::Session::t_allyNpcClear_SceneSession send;
	    send.dwCountryID = scene->getCountryID();
	    sessionClient->sendCmd(&send,sizeof(send));

	    Zebra::logger->debug("%s 盟国镖车 %u 到达",
		    SceneManager::getInstance().getCountryNameByCountryID(scene->getCountryID()),
		    tempid);
	}
    }
    //Channel::sendNine(this,"clear me~");
    return true;
}

/**
 * \brief 执行丢东西AI
 *
 * \return 是否执行成功
 */
bool SceneNpc::doDropItemAI()
{
    zObjectB *ob = objectbm.get(AIDefine.pos.x);
    if (!ob)
    {
	Zebra::logger->debug("%s 找不到要丢的物品 id=%u num=%u",name,AIDefine.pos.x,AIDefine.pos.y);
	return false;
    }
    for (DWORD i=0; i<AIDefine.pos.y; i++)
	scene->addObject(dupIndex,ob,1,getPos(),0,0);
    t_NpcAIDefine ad;
    ad.type = NPC_AI_WAIT;
    AIC->setAI(ad,false);
    return true;
}

/**
 * \brief 执行说话AI
 *
 * \return 是否执行成功
 */
bool SceneNpc::doSayAI()
{
    Channel::sendNine(this, AIDefine.str.c_str());
    t_NpcAIDefine ad;
    ad.type = NPC_AI_WAIT;
    AIC->setAI(ad,false);
    return true;
}

/**
 * \brief 执行随机说话AI,说话内容在NpcCommonChat.xml里的类型9
 *
 * \return 是否执行成功
 */
bool SceneNpc::doRandomChatAI()
{
    randomChat(NPC_CHAT_RANDOM);

    t_NpcAIDefine ad;
    ad.type = NPC_AI_WAIT;
    AIC->setAI(ad,false);
    return true;
}

/**
 * \brief 选择攻击目标
 *
 * \param enemies 敌人列表
 * \return 找到的敌人,没知道到返回0
 */
SceneEntryPk * SceneNpc::chooseEnemy(SceneEntryPk_vec& enemies)
{
    //非战斗npc
    if (!canFight()) return false;

    SceneEntryPk * ret = 0;

    //先检查现有的目标
    if (!ret)
	checkChaseAttackTarget(ret);

    //BOSS有10%的概率重新寻找目标
    if ((npc->kind==NPC_TYPE_BBOSS||npc->kind==NPC_TYPE_LBOSS||npc->kind==NPC_TYPE_PBOSS) && zMisc::selectByPercent(10))
	ret = 0;

    if (!ret)
    {
	//被动npc
	if (enemies.empty()) return 0;
	if (!isActive()) return 0;

	//判断特殊条件
	DWORD minValue = 0xffffffff;
	for (DWORD i=0; i<enemies.size(); i++)
	{
	    if(enemies[i]->angelMode)  //sky 如果是无敌的不列为攻击目标
		continue;

	    //sky 如果拥有阵营ID并且是战场NPC
	    if(BattCampID != 0 && scene->IsGangScene())
	    {
		//sky 如果是和自己同阵营的不列为目标
		if(enemies[i]->BattCampID == this->BattCampID)
		    continue;
	    }

	    if(npc->kind == NPC_TYPE_TURRET	||
		    npc->kind == NPC_TYPE_BARRACKS ||
		    npc->kind == NPC_TYPE_CAMP)
	    {
		// sky 炮台只锁定在自己索敌范围的敌人
		if(!scene->zPosShortRange(getPos(),enemies[i]->getPos(),npc_lost_target_region))
		    continue;
	    }

	    if(dupIndex != enemies[i]->dupIndex)
		continue;
	    if (getTopMaster()->getType()==zSceneEntry::SceneEntry_Player
		    && getPetType()!=Cmd::PET_TYPE_NOTPET)
	    {
		//宠物不会主动攻击守卫
		if (enemies[i]->getType()==zSceneEntry::SceneEntry_NPC)
		{
		    SceneNpc * n = (SceneNpc *)enemies[i];
		    if ((NPC_TYPE_GUARD==n->npc->kind)||(n->aif&AIF_ATK_REDNAME))
			continue;
		}

		//不抢怪
		SceneEntryPk *tar = enemies[i]->getCurTarget();
		if (tar && 0!=isEnemy(tar))//是别人的怪
		    continue;

		//周围没有玩家时才攻击怪物
		if (enemies[i]->getTopMaster()->getType()==zSceneEntry::SceneEntry_NPC
			&& i<enemies.size()-1)
		    continue;
	    }

	    DWORD value = minValue;
	    if (!(aif&(AIF_ATK_PDEF|AIF_ATK_MDEF|AIF_ATK_HP)))
	    {
		int x2 = abs((long)(getPos().x-enemies[i]->getPos().x))*2;
		int y2 = abs((long)(getPos().y-enemies[i]->getPos().y));
		value = x2*x2+y2*y2;
	    }
	    else
	    {
		switch (enemies[i]->getType())
		{
		    case zSceneEntry::SceneEntry_Player:
			{
			    if (aif&AIF_ATK_PDEF)
				value = ((SceneUser *)enemies[i])->charstate.pdefence;
			    if (aif&AIF_ATK_MDEF)
				value = ((SceneUser *)enemies[i])->charstate.mdefence;
			    if (aif&AIF_ATK_HP)
				value = enemies[i]->getHp();
			}
			break;
		    case zSceneEntry::SceneEntry_NPC:
			{
#if 0
			    if (aif&AIF_ATK_PDEF)
				value = ((SceneNpc *)enemies[i])->getMinPDefence();
			    if (aif&AIF_ATK_MDEF)
				value = ((SceneNpc *)enemies[i])->getMinMDefence();
			    if (aif&AIF_ATK_HP)
				value = enemies[i]->getHp();
#endif
			}
			break;
		    default:
			continue;
		}
	    }
	    if (value<minValue)
	    {
		minValue = value;
		ret = enemies[i];
	    }
	}
    }

    //直接攻击宠物的主人
    if (ret&&(aif&AIF_ATK_MASTER))
    {	  
	SceneEntryPk * tmp = ret->getMaster();

	if (tmp)
	{
	    if (scene->zPosShortRange(getPos(),tmp->getPos(),6))
	    {
		if( !ret->angelMode ) //sky 无敌的主人没办法杀！
		    ret = tmp;
	    }
	}
    }

    if (ret)
    {
	chaseSceneEntry(ret->getType(),ret->tempid);
    }
    return ret;
}

/**
 * \brief 给自己治疗
 *
 * \return 是否成功
 */
bool SceneNpc::healSelf()
{
#if 0
    if (hp*2>this->getMaxHP()) return false;

    npcSkill skill;
    if (npc->getRandomSkillByType(SKILL_TYPE_RECOVER,skill))
    {
	if (zMisc::selectByPercent(skill.rate))
	{
	    return useSkill(this,skill.id);
	}
    }
#endif
    return false;
}

/**
 * \brief 给自己加buff
 *
 * \return 是否成功
 */
bool SceneNpc::buffSelf()
{
#if 0
    npcSkill skill;
    if (npc->getRandomSkillByType(SKILL_TYPE_BUFF,skill))
	if (zMisc::selectByPercent(skill.rate))
	{
	    //Channel::sendNine(this,"我给自己buff");
	    if ((211==skill.id)&&(summon))
		return false;
	    return useSkill(this,skill.id);
	}
#endif
    return false;
}

/**
 * \brief 给敌人debuff
 *
 *
 * \param enemy 对象指针
 * \return 是否成功
 */
bool SceneNpc::debuffEnemy(SceneEntryPk * enemy)
{
#if 0
    if(dupIndex != enemy->dupIndex)
	return false;
    npcSkill skill;
    if (npc->getRandomSkillByType(SKILL_TYPE_DEBUFF,skill))
	if (zMisc::selectByPercent(skill.rate))
	{
	    //Channel::sendNine(this,"我给 %s debuff",enemy->name);
	    return useSkill(enemy,skill.id);
	}
#endif
    return false;
}


/**
 * \brief 攻击敌人     [sky] 技能的处理放在这里拉`攻击前判断
 *
 *
 * \param enemy 攻击对象
 * \return 攻击是否成功
 */
bool SceneNpc::attackEnemy(SceneEntryPk * enemy)
{
    //优先使用技能
#if 0
    npcSkill skill;
    if (npc->getRandomSkillByType(SKILL_TYPE_DAMAGE,skill))
	if (zMisc::selectByPercent(skill.rate))
	    return useSkill(enemy,skill.id);
    /*
    */
#endif
    if (inRange(enemy))
	return attackTarget(enemy);
    else
	return moveToEnemy(enemy);
    return false;
}


/**
 * \brief 向目标移动
 *
 *
 * \param enemy 移动目标对象
 * \return 是否成功
 */
bool SceneNpc::moveToEnemy(SceneEntryPk * enemy)
{

    if(dupIndex != enemy->dupIndex)
	return false;
    if (!canMove()) return false;
    if (aif&AIF_RUN_AWAY) return false;

    if (scene->zPosShortRange(getPos(),enemy->getPos(),1)) return false;
    if (checkMoveTime(SceneTimeTick::currentTime) && canMove())
    {
	if (aif&AIF_WARP_MOVE)
	    warp(enemy->getPos());
	else
	    if (!gotoFindPath(getPos(),enemy->getPos()))
	    {
#ifdef _DEBUG
		//Zebra::logger->debug("%s 寻路失败 closeCount=%u",name,closeCount);
#endif
		if ((getPetType()==Cmd::PET_TYPE_NOTPET)
			&&(npc->kind==NPC_TYPE_BBOSS||npc->kind==NPC_TYPE_LBOSS||npc->kind==NPC_TYPE_PBOSS))
		{
		    closeCount++;
		    if (closeCount>=5)
		    {
			t_NpcAIDefine ad;
			ad.type = NPC_AI_FLEE;
			ad.flee = true;
			ad.fleeCount = 20;
			ad.fleeDir = enemy->getPos().getDirect(getPos());
			AIC->setAI(ad,false);

			closeCount = 0;
		    }
		}
		return goTo(enemy->getPos());
	    }
    }
    return true;
}


/**
 * \brief 治疗自己的同伴
 *
 * \return 是否成功
 */
bool SceneNpc::healFellow(SceneEntryPk_vec &fellows)
{
#if 0
    for (DWORD i=0;i<fellows.size();i++)
    {
	if (fellows[i]->getHp()*2<fellows[i]->getMaxHp())
	{
	    npcSkill skill;
	    if (npc->getRandomSkillByType(SKILL_TYPE_RECOVER,skill))
		if (zMisc::selectByPercent(skill.rate))
		{
		    ((SceneNpc *)fellows[i])->randomChat(NPC_CHAT_ON_BE_HELP);
		    return useSkill(fellows[i],skill.id);
		}
	}
    }
#endif
    return false;
}

/**
 * \brief 给同伴buff
 *
 *
 * \param fellows 同伴列表
 * \return 是否成功
 */
bool SceneNpc::buffFellow(SceneEntryPk_vec &fellows)
{
#if 0
    for (DWORD i=0;i<fellows.size();i++)
    {
	switch (fellows[i]->getType())
	{
	    case zSceneEntry::SceneEntry_Player:
		{
		}
		break;
	    case zSceneEntry::SceneEntry_NPC:
		{
		    //if (CHASE_NONE != ((SceneNpc *)fellows[i])->getChaseMode())
		    if (fellows[i]->isFighting())
		    {
			npcSkill skill;
			if (npc->getRandomSkillByType(SKILL_TYPE_BUFF,skill))
			    if (zMisc::selectByPercent(skill.rate))
			    {
				((SceneNpc *)fellows[i])->randomChat(NPC_CHAT_ON_BE_HELP);
				return useSkill(fellows[i],skill.id);
			    }
		    }
		}
		break;
	    default:
		break;
	}
    }
#endif
    return false;
}

/**
 * \brief 给帮助同伴攻击
 *
 *
 * \param fellows 同伴列表
 * \return 是否成功
 */
bool SceneNpc::helpFellow(SceneEntryPk_vec &fellows)
{
    if (!(aif&AIF_HELP_FELLOW_5)) return false;

    for (DWORD i=0;i<fellows.size();i++)
    {
	switch (fellows[i]->getType())
	{
	    case zSceneEntry::SceneEntry_Player:
		{
		}
		break;
	    case zSceneEntry::SceneEntry_NPC:
		{
		    //if (CHASE_NONE != ((SceneNpc *)fellows[i])->getChaseMode())
		    if (fellows[i]->isFighting())
		    {
			SceneEntryPk * pk = ((SceneNpc *)fellows[i])->getChaseSceneEntry();
			if (pk)
			{
			    chaseSceneEntry(pk->getType(),pk->tempid);
			    randomChat(NPC_CHAT_ON_HELP);
			    ((SceneNpc *)fellows[i])->randomChat(NPC_CHAT_ON_BE_HELP);
			    return true;
			}
		    }
		}
		break;
	    default:
		break;
	}
    }

    return false;
}

/**
 * \brief 随机移动
 *
 * \return 是否成功
 */
bool SceneNpc::randomMove()
{
    if (!canMove()) return false;

    if (checkMoveTime(SceneTimeTick::currentTime)
	    && zMisc::selectByPercent(100))
    {
	int dir = zMisc::randBetween(0,7);
	zPos newPos;
	scene->getNextPos(pos,dir,newPos);
	zPosI newPosI = 0;
	scene->zPos2zPosI(newPos,newPosI);
	if (getPosI()==newPosI)
	    return shiftMove(dir);//随机移动
    }
    return false;
}

/**
 * \brief 判断一个entry是不是敌人
 * 反过来用于判断一个对象是不是友方
 * 如果是主人则不是敌人
 * 如果对方有主人则跟主人性质相同
 * 如果考虑魅惑状态则相反
 *
 * 玩家默认是敌人
 * npc默认是友方
 *
 * \param entry 对象指针
 * \param notify 是否通知
 * \return 0:friend 1:enemy -1:middle
 */
int SceneNpc::isEnemy(SceneEntryPk * entry,bool notify,bool good)
{
    if (!entry) return -1;
    if (this==entry) return 0;
    if ((entry->frenzy)||(frenzy)) return 1;

    if (npc->kind==NPC_TYPE_SOLDIER)//士兵,只攻击外国人
    {
	if (entry->getCurTarget()==this) return 1;
	if (entry->getTopMaster()->getType()!=zSceneEntry::SceneEntry_Player) return -1;
	SceneUser * pUser = (SceneUser *)entry->getTopMaster();
	//if (pUser->mask.is_masking() && !pUser->isSpecWar(Cmd::COUNTRY_FORMAL_DARE)) return -1;

	//中立国
	if (6==scene->getCountryID()) return -1;

	if (pUser && pUser->charbase.country!=scene->getCountryID())  return 1;

	return -1;
    }
#if 0
    if (npc->kind==NPC_TYPE_UNIONATTACKER)//攻方士兵
    {
	if (entry->getCurTarget()==this) return 1;
	if (entry->getTopMaster()->getType()!=zSceneEntry::SceneEntry_Player) return -1;
	SceneUser * pUser = (SceneUser *)entry->getTopMaster();
	if (pUser && pUser->isAtt(Cmd::UNION_CITY_DARE))
	    return -1;
	else
	    return 1;
    }

    if (npc->kind==NPC_TYPE_UNIONGUARD)//城战卫兵
    {
	if (entry->getCurTarget()==this) return 1;
	if (entry->getTopMaster()->getType()!=zSceneEntry::SceneEntry_Player) return -1;
	SceneUser * pUser = (SceneUser *)entry->getTopMaster();
	if (pUser->isAtt(Cmd::UNION_CITY_DARE))//是否攻方
	    return 1;
	else
	    if (pUser->scene->getUnionDare() && !pUser->isSpecWar(Cmd::UNION_CITY_DARE))//城战期间又不打城战的
		return 1;
	    else
		return -1;//城战期间打城战而且不是攻方,就是守方
    }
#endif
    //卫兵
    if ((NPC_TYPE_GUARD==npc->kind)||(aif&AIF_ATK_REDNAME))
    {
	if (entry->getState()!=zSceneEntry::SceneEntry_Normal) return -1;
	if (entry->getTopMaster()->getType()==zSceneEntry::SceneEntry_NPC) return -1;
	if (entry->isRedNamed()) return 1;
	if (entry->getType()==zSceneEntry::SceneEntry_NPC
		&& ((NPC_TYPE_GUARD==((SceneNpc *)entry)->npc->kind)||(((SceneNpc *)entry)->aif&AIF_ATK_REDNAME)))
	    return 0;

	SceneEntryPk * t = entry->getCurTarget();
	if (t)
	{
	    if (t==this) return 1;
	    if (t->getState() == zSceneEntry::SceneEntry_Normal)
	    {
		if (t->isRedNamed()) return 0;
		if ((t->frenzy)||(frenzy)) return 1;

		switch (t->getType())
		{
		    case zSceneEntry::SceneEntry_Player:
			{
			    SceneUser * user = (SceneUser *)t;
			    if (user->charbase.country!=scene->getCountryID())
				return 0;
			    if (user->charbase.goodness&Cmd::GOODNESS_ATT)
				return 0;

			    //打人的怪物
			    if (entry->getType()==zSceneEntry::SceneEntry_NPC)
				return 1;
			    /*
			       if (entry->getType()==zSceneEntry::SceneEntry_Player)
			    //被玩家打后战斗状态还没取消会到这里
			    return -1;
			    else if (entry->getType()==zSceneEntry::SceneEntry_NPC)
			    return 1;
			    */
			}
			break;
		    case zSceneEntry::SceneEntry_NPC:
			{
			    SceneNpc * n = (SceneNpc *)t;
			    if ((NPC_TYPE_GUARD==n->npc->kind)||(n->aif&AIF_ATK_REDNAME))
				return 1;
			}
			break;
		    default:
			break;
		}
	    }
	}

	switch (entry->getType())
	{
	    case zSceneEntry::SceneEntry_Player:
		{
		    SceneUser * user = (SceneUser *)entry;
		    //if (user->mask.is_masking() && !user->isSpecWar(Cmd::COUNTRY_FORMAL_DARE)) return -1;
		    if (user->charbase.country!=scene->getCountryID() && user->scene
			    && user->charbase.unionid!=user->scene->getHoldUnion())
			return 1;
		    if (user->charbase.goodness&Cmd::GOODNESS_ATT)
			return 1;
		    return 0;
		}
		break;
	    case zSceneEntry::SceneEntry_NPC:
		{
		    return -1;
		}
		break;
	    default:
		return -1;
		break;
	}
    }

    switch (entry->getType())
    {
	case zSceneEntry::SceneEntry_Player:
	    {
		if (entry==getMaster())
		    return 0;
		else
		    return 1;
	    }
	    break;
	case zSceneEntry::SceneEntry_NPC:
	    {
		if (!((SceneNpc *)entry)->isBugbear()) return -1;
		if (((SceneNpc *)entry)->getPetType()==Cmd::PET_TYPE_CARTOON) return -1;
		switch(((SceneNpc *)entry)->npc->kind)
		{
		    case NPC_TYPE_HUMAN:      ///人型
		    case NPC_TYPE_NORMAL:      /// 普通类型
		    case NPC_TYPE_BBOSS:      /// 大Boss类型
		    case NPC_TYPE_PBOSS:      /// 紫Boss类型
		    case NPC_TYPE_LBOSS:      /// 小Boss类型
		    case NPC_TYPE_BACKBONE:      /// 精英类型
		    case NPC_TYPE_GOLD:        /// 黄金类型
		    case NPC_TYPE_SUMMONS:      /// 召唤类型
		    case NPC_TYPE_AGGRANDIZEMENT:  /// 强化类型
		    case NPC_TYPE_ABERRANCE:    /// 变异类型
		    case NPC_TYPE_BACKBONEBUG:    /// 精怪类型
		    case NPC_TYPE_PET:  /// 宠物类型
		    case NPC_TYPE_TOTEM:      /// 图腾类型
		    case NPC_TYPE_GHOST:			///元神类
		    case NPC_TYPE_TURRET:			/// 炮塔
		    case NPC_TYPE_BARRACKS:
		    case NPC_TYPE_CAMP:
		    case NPC_TYPE_ANIMON: /// 动物类
			{
			    //sky 如果是在战场中
			    if(BattCampID != 0 && this->scene->IsGangScene())
			    {
				//sky 被判断的对象的阵营和自己不同则认为是自己的敌人
				if(entry->BattCampID != BattCampID)
				    return 1;
			    }

			    SceneEntryPk * hisMaster = ((SceneNpc *)entry)->getTopMaster();
			    if (hisMaster->getType()==zSceneEntry::SceneEntry_Player)
				return 1;
			    else if (hisMaster->getType()==zSceneEntry::SceneEntry_NPC)
				return 0;
			}
			break;
		    case NPC_TYPE_GUARD:  /// 士兵类型
		    case NPC_TYPE_TRADE:  /// 买卖类型
		    case NPC_TYPE_TASK:    /// 任务类型
		    default:
			return -1;
			break;
		}
	    }
	    break;
	default:
	    return -1;
	    break;
    }
    return -1;
}

/**
 * \brief 发送指令到地图屏索引中所有玩家的回调函数
 *
 */
struct getEntriesCallBack : public zSceneEntryCallBack
{
    SceneNpc * npc;
    int radius;
    SceneEntryPk_vec& entries;
    const int state;
    getEntriesCallBack(SceneNpc * npc,int radius,SceneEntryPk_vec& entries,const int state) : npc(npc),radius(radius),entries(entries),state(state)
    {
	entries.reserve(4 * radius * radius);
    }
    /**
     * \brief 回调函数
     * \param entry 地图物件,这里是玩家个npc 
     * \return 回调是否成功
     */
    bool exec(zSceneEntry *entry)
    {
	SceneEntryPk * e = (SceneEntryPk *)entry;
	if (entry
		&& entry->getState() == zSceneEntry::SceneEntry_Normal
		&& npc->scene->zPosShortRange(npc->getPos(),e->getPos(),radius)
		&& npc->isEnemy(e)==state
		&& npc->canReach(e)
		&& !((SceneEntryPk *)entry)->hideme)
	{
	    entries.push_back(e);
	}
	return true;
    }
};

/**
 * \brief 得到一定范围内的友方
 *
 * \param radius 范围半径
 * \param entries 输出：存放得到的指针的容器
 * \param state 要得到的对象的状态 0:友方 1:敌人 -1:中立
 * \return 是否找到
 */
bool SceneNpc::getEntries(int radius,SceneEntryPk_vec & entries,int state)
{
#ifdef _DEBUG
    //if (id==50004 || id==15001)
    //  Zebra::logger->debug("%s getEntries",name);
#endif
    const zPosIVector &pv = scene->getScreenByRange(pos,radius);

    getEntriesCallBack cb(this,radius,entries,state);
    for(zPosIVector::const_iterator it = pv.begin(); it != pv.end(); it++)
    {
	scene->execAllOfScreen(zSceneEntry::SceneEntry_Player,*it,cb);
	scene->execAllOfScreen(zSceneEntry::SceneEntry_NPC,*it,cb);
    }
    return !entries.empty();
}

#if 0
/**
 * \brief 检查主人的位置并向主人移动
 *
 */
bool SceneNpc::moveToMaster()
{
    if (getPetType()==Cmd::PET_TYPE_RIDE && getPetType()!=Cmd::PET_TYPE_PET && 
	    getPetType()!=Cmd::PET_TYPE_SUMMON && getPetType()!=Cmd::PET_TYPE_GUARDNPC) 
	((ScenePet *)this)->moveToMaster( );
    return true;
}
#endif
/**
 * \brief 逃离最近的敌人
 *
 * \return 是否成功
 */
bool SceneNpc::runOffEnemy(SceneEntryPk_vec & enemies)
{
    if (!(aif&AIF_RUN_AWAY)) return false;

    if (!(checkMoveTime(SceneTimeTick::currentTime) && canMove())) return false;

    if (enemies.empty()) return false;
    SceneEntryPk * enemy = enemies[0];
    if (!enemy) return false;

    int dir = enemy->getPos().getDirect(getPos());
    int clockwise = zMisc::selectByPercent(50)?-1:1;
    int tryDir = dir + 8;
    for (int i=0; i<8; i++)
    {
	if (shiftMove(tryDir%8))
	    return true;
	tryDir += clockwise;
    }
    return false;
}

/**
 * \brief 检查npc是否被包围
 *
 * \return 是否被包围
 * 
 */
bool SceneNpc::isSurrounded()
{
    int region = 1;

    int side = 0;
    int direct = 0;
    int clockwise = 1;
    int blockCount = 0;
    int count = 0;
    zPos pos;
    scene->getNextPos(side,direct,getPos(),clockwise,pos);
    do
    {
	if (scene->checkBlock(pos))
	    blockCount++;
	if (++count>=(region*2+1)*(region*2+1))
	    break;
    } while(scene->getNextPos(side,direct,getPos(),clockwise,pos) && side <= region);
    return blockCount==8;
}


#if 0
///////////////////////////////////[sky AI 接口]////////////////////////////////////////////
/////////////////////////////////////
////[sky]为NPC分配已读取过的AI
/////////////////////////////////////
bool SceneNpc::GetNpcAi()
{
    NpcAiList.clear();
    NpcAiTimeList.clear();

    if(!NpcAiXmlData.empty())
    {
	std::list<stNewAIData_t>::iterator it;
	for(it=NpcAiXmlData.begin(); it!=NpcAiXmlData.end(); ++it)
	{
	    if(npc->id == it->AiId)
	    {
		aif = it->aif;
		npc_search_region = it->npc_search_region;

		std::vector<NpcAiCs>::iterator iter;
		for(iter=it->NpcAiList.begin(); iter!=it->NpcAiList.end(); ++iter)
		{
		    NpcAiList.push_back((*iter));	
		}

		for(iter=it->NpcAiTimeList.begin(); iter!=it->NpcAiTimeList.end(); ++iter)
		{
		    NpcAiTimeList.push_back((*iter));
		}

		return true;
	    }
	}
    }

    return laod_ai();
}

/////////////////////////////////////
////[sky]载入AI脚本
/////////////////////////////////////
bool SceneNpc::laod_ai()
{
    stNewAIData_t m_AiXml;
    strncpy(m_AiXml.AiName, name,sizeof(m_AiXml.AiName));
    m_AiXml.AiId = npc->id;

    aif = 0; //sky 初始化行动模式
    npc_search_region = 5; //sky 初始化索敌范围

    char Ai_FileName[MAX_PATH];
    sprintf( Ai_FileName, "newAI/%s.xml", name );

    FILE * fp = fopen( Ai_FileName, "r");
    if( fp == NULL )
	return false;

    fclose( fp );

    zXMLParser xml;
    if (!xml.initFile(Ai_FileName))
    {
	Zebra::logger->error("加载AI文件 %s 失败",Ai_FileName);
	return false;
    }

    int ai_id = 0;
    NpcAiCs NpcAibuffe;
    xmlNodePtr root;

    root = xml.getRootNode("event");

    if (root)
    {
	xmlNodePtr node = xml.getChildNode(root,"actionai");

	if (0 == strcmp((char *)node->name,"actionai")) 
	{
	    if(!xml.getNodePropNum(node,"type",&aif,sizeof(aif)))
		Zebra::logger->error("读取文件 %s 怪物行为模式失败 默认行为模式为 %u",Ai_FileName, aif);

	    m_AiXml.aif = aif;
	}

	node = xml.getChildNode(root, "attRange");
	if( 0 == strcmp((char *)node->name, "attRange") )
	{
	    if(!xml.getNodePropNum(node, "range", &npc_search_region, sizeof(npc_search_region)))
		Zebra::logger->error("读取文件 %s 怪物索敌范围失败 默认索敌范围为 %u",Ai_FileName, npc_search_region);

	    m_AiXml.npc_search_region = npc_search_region;
	}

	node = xml.getChildNode(root,"quest");  
	while (node) 
	{
	    //parse quest
	    if (0 == strcmp((char *)node->name,"quest")) 
	    {
		if (!xml.getNodePropNum(node,"aiid",&ai_id,sizeof(ai_id))) 
		{
		    return false;
		}

		xmlNodePtr phaseNode = xml.getChildNode(node,"phase");
		while (phaseNode)
		{

		    if(!xml.getNodePropNum( phaseNode, "id", &NpcAibuffe.id, sizeof(NpcAibuffe.id) ) )
		    {
			return false;
		    }

		    if( !xml.getNodePropNum( phaseNode, "level", &NpcAibuffe.level, sizeof(NpcAibuffe.level) ) )
		    {
			return false;
		    }

		    if( !xml.getNodePropNum( phaseNode, "target", &NpcAibuffe.target, sizeof(NpcAibuffe.target) ) )
		    {
			return false;
		    }

		    if( !xml.getNodePropNum( phaseNode, "Rate", &NpcAibuffe.Rate, sizeof(NpcAibuffe.Rate) ) )
		    {
			return false;
		    }

		    if( !xml.getNodePropNum( phaseNode, "cond1", &NpcAibuffe.cond1, sizeof(NpcAibuffe.cond1) ) )
		    {
			return false;
		    }

		    if( !xml.getNodePropNum( phaseNode, "cond2", &NpcAibuffe.cond2, sizeof(NpcAibuffe.cond2) ) )
		    {
			return false;
		    }

		    //#ifdef _DEBUG
		    /*Zebra::logger->error*/
		    //#endif
		    if( ai_id == 1)
		    {
			NpcAiTimeList.push_back(NpcAibuffe);
			m_AiXml.NpcAiTimeList.push_back( NpcAibuffe );
		    }
		    else if( ai_id == 2)
		    {
			NpcAiList.push_back( NpcAibuffe );
			m_AiXml.NpcAiList.push_back( NpcAibuffe );
		    }

		    phaseNode = xml.getNextNode(phaseNode,"phase");
		}

		node = xml.getNextNode(node,"quest");
	    }
	}

	printf("加载AI文件 %s 成功\n",Ai_FileName);

	NpcAiXmlData.push_back(m_AiXml);
	return true;
    }

    return false;

}

/////////////////////////////////////
////[sky] 清除现在的AI(用来NPC变身重新载入AI前初始化用)
/////////////////////////////////////
bool SceneNpc::del_ai()
{
    NpcAiList.clear();
    NpcAiTimeList.clear();

    return true;
}

/////////////////////////////////////
////[sky] 怪物使用技能接口
////参数1: id 技能
////参数2：level 技能等级
////参数3：target 技能使用的对象 (0 敌人 1 自己 2 友方)
////参数4：Rate  万分比几率
////参数5：cond1 条件1
////参数6：cond2 条件2 （为第一条件的辅助条件,比如第一条件为HP小于n%,他就n的具体数值）
////返回值 false 失败 true 成功
/////////////////////////////////////
bool SceneNpc::lua_useSkill( DWORD id, WORD level, int target, int Rate, DWORD cond1, DWORD cond2 )
{
    m_NpcSkill Skill_buffe;

    if( target == 0 )						//对敌人的技能AI判断
    {
	SceneEntryPk * enemy = 0;
	SceneEntryPk_vec enemies;
	checkChaseAttackTarget(enemy);		//当前的目标

	if (!enemy && isActive())			//主动搜索
	{
	    int r = npc_search_region;		//计算半径
	    if (aif&AIF_DOUBLE_REGION) r *= 2;
	    BYTE at = getAType();
	    if (NPC_ATYPE_FAR==at || NPC_ATYPE_MFAR==at)
		r +=3;
	    getEntries(r,enemies,1);
	}
	if (!enemies.empty())					//对列表处理
	{
	    enemy = chooseEnemy(enemies);	//选择敌人
	}
	if (enemy)							//对找到的敌人使用技能
	{
	    if( selectByTenTh( Rate ) )    //计算万分比几率
	    {
		if( Detection_cond( enemy, cond1, cond2 ) )	//检测是否达到使用条件
		{
		    Skill_buffe.SkillType = 1;
		    Skill_buffe.target = enemy;
		    Skill_buffe.id = id;
		    Skill_buffe.level = level;

		    m_SkillList.push_back( Skill_buffe );
		    return true;
		    //return useSkill(enemy, id, level);	//全部满足使用技能
		}
	    }
	}
    }
    else if( target == 1 )					//对自己的技能AI判断
    {
	if( selectByTenTh( Rate ) )
	{
	    if( Detection_cond( this, cond1, cond2 ) )	//检测是否达到使用条件
	    {
		Skill_buffe.SkillType = 1;
		Skill_buffe.target = this;
		Skill_buffe.id = id;
		Skill_buffe.level = level;

		m_SkillList.push_back( Skill_buffe );
		return true;
		//return useSkill(this, id, level);	//全部满足使用技能
	    }
	}
    }
    else if( target == 2 )
    {
	SceneEntryPk_vec fellows;
	int r = npc_search_region;

	if (aif&AIF_DOUBLE_REGION)
	    r *= 2;
	if (getEntries(r,fellows,0))    //获取友方的ID列表
	{
	    for ( DWORD i=0;i<fellows.size();i++ )  //循环友方ID
	    {
		if( fellows[i] == this )			//既然是给友方帮助 就不要把自己算上拉
		    continue;

		if ( zMisc::selectByPercent( Rate ) )
		{
		    if( Detection_cond( fellows[i], cond1, cond2 ) )
		    {
			Skill_buffe.SkillType = 1;
			Skill_buffe.target = fellows[i];
			Skill_buffe.id = id;
			Skill_buffe.level = level;

			m_SkillList.push_back( Skill_buffe );
			return true;
			//return useSkill(fellows[i], id, level);
		    }
		}
	    }
	}
    }
    else if( target == 3 )  //召唤兽 宠物 图腾对主人专用
    {
	if( selectByTenTh( Rate ) )
	{
	    if( npc->kind == NPC_TYPE_PET ||
		    npc->kind == NPC_TYPE_SUMMONS ||
		    npc->kind == NPC_TYPE_TOTEM  )

	    {
		SceneEntryPk * master;
		master = ((ScenePet *)this)->getMaster();
		if( Detection_cond( master, cond1, cond2 ) )	//检测是否达到使用条件
		{
		    Skill_buffe.SkillType = 1;
		    Skill_buffe.target = master;
		    Skill_buffe.id = id;
		    Skill_buffe.level = level;

		    m_SkillList.push_back( Skill_buffe );
		    return true;
		    //return useSkill(this, id, level);	//全部满足使用技能
		}
	    }
	}
    }
    else if( target == 4 )
    {
	if( selectByTenTh( Rate ) )    //计算万分比几率
	{
	    if( Detection_cond( this, cond1, cond2 ) && !m_mobType )	//检测是否达到使用条件
	    {
		m_mobType = true;
		Skill_buffe.SkillType = 2;
		Skill_buffe.target = this;
		Skill_buffe.id = id;
		Skill_buffe.level = level;

		m_SkillList.push_back( Skill_buffe );
		return true;
		//return useMobSkill(this, id, level);	//全部满足使用技能
	    }
	}		
    }

    return false;
}

///////////////////////////////////////
////[sky] 检测NPC技能条件是否达到
////参数3：cond1 条件1
////参数3：cond2 条件2
///////////////////////////////////////
bool SceneNpc::Detection_cond( SceneEntryPk * target, DWORD cond1, DWORD cond2 )
{
    switch( cond1 )
    {
	case NPC_COND_ALWAYS:			//无条件
	    {
		return true;
	    }
	    break;
	case NPC_COND_HPGMAXRATE:		//自身HP高于n%时
	    {
		if( cond2 != -1 )
		{
		    if(  hp > ( getMaxHP() * (cond2/100.0f) ) )
			return true;
		}
	    }
	    break;
	case NPC_COND_HPLMAXRATE:		//自身HP低于n%时
	    {
		if( cond2 != -1 )
		{
		    {
			if(  hp < ( getMaxHP() * (cond2/100.0f) ) )
			    return true;
		    }
		}
	    }
	    break;
	case NPC_COND_FHPLMAXRATE:		//友方魔物HP低于n%时
	    {
		if( cond2 != -1 )
		{
		    if( target->getHp() < ( target->getMaxHp() * (cond2/100.0f) ) )
			return true;
		}
	    }
	    break;
	case NPC_COND_ATPCGE:			//攻击的PC数量>=n的话
	    {
		if( cond2 != -1 )
		{
		    int side = 0;
		    int direct = 0;
		    int clockwise = 1;
		    int enemyCount = 0;
		    int count = 0;//计数,防止死循环
		    zPos pos;
		    scene->getNextPos(side,direct,getPos(),clockwise,pos);
		    do                      
		    {                       
			SceneUser *sceneUser = scene->getSceneUserByPos(pos);
			if (sceneUser && sceneUser->getState() == zSceneEntry::SceneEntry_Normal)
			{
			    if (isAttackMe(sceneUser))
				enemyCount++;
			}    
			if (++count>=8)
			    break;
		    } while(scene->getNextPos(side,direct,getPos(),clockwise,pos) && side <= 1);

		    if (enemyCount >= cond2)
		    {
			return true;
		    }
		}
	    }
	    break;
	case NPC_COND_TARGEHPGMAX:		//攻击对象的HP高于n%时
	    {
		if( cond2 != -1 )
		{
		    if( target->getHp() > ( target->getMaxHp() * (cond2*0.01f) ) )
			return true;
		}
	    }
	    break;
	case NPC_COND_TARGEHPLMAX:		//攻击对象的HP低于n%时
	    {
		if( cond2 != -1 )
		{
		    if( target->getHp() < ( target->getMaxHp() * (cond2*0.01f) ) )
			return true;
		}
	    }
	    break;
	case NPC_COND_TARGETSTATUON:	//当攻击对象得到某个异常状态时(以cond2为基准)
	case NPC_COND_TARGETSTATUOFF:	//当攻击对象没有某个异常状态时(以cond2为基准)
	    break;
	case NPC_COND_MAXSTHPGMAX:		//主人的HP高于n时(预留给智能BB用)
	case NPC_COND_MAXSTHPLMAX:		//主人的HP低于n时(预留给智能BB用)
	    break;
	case NPC_COND_MASTERSTATUON:	//当主人没有某个异常状态时(以cond2为基准)(预留给智能BB用)
	case NPC_COND_MASTERSTATUOFF:	//当主人得到某个异常状态时(以cond2为基准)(预留给智能BB用)
	    break;
	case NPC_COND_CLOSEDATACKED:	//被近身攻击时
	    {
		if( scene->zPosShortRange(getPos(),target->getPos(),1) )
		    return true;
	    }
	    break;
	case NPC_COND_LONGRANGEATTACKED://被远程攻击时
	    {
		if( !scene->zPosShortRange(getPos(),target->getPos(),1) )
		    return true;
	    }
	    break;
	case NPC_COND_RUDEATTACKED:		//被攻击而不能反击时(暂时用不到)
	    break;
    }

    return false;
}


/////////////////////////////////////
////[sky] 怪物使用技能接口
////参数1: id 技能
////参数2：level 技能等级
////参数3：target 技能使用的对象 (0 敌人 1 自己 2 友方)
////参数4：Rate  万分比几率
////参数5：time1 当前经过时间
////参数6：time2 定时器触发时间
////返回值 false 失败 true 成功
/////////////////////////////////////
bool SceneNpc::lua_timeSkill( DWORD id, WORD level, int target, int Rate, DWORD time1, DWORD time2 )
{
    m_NpcSkill Skill_buffe;

    if( target == 0 )						//对敌人的技能AI判断
    {
	SceneEntryPk * enemy = 0;
	SceneEntryPk_vec enemies;
	checkChaseAttackTarget(enemy);		//当前的目标

	if (!enemy && isActive())			//主动搜索
	{		
	    int r = npc_search_region;		//计算半径
	    if (aif&AIF_DOUBLE_REGION) r *= 2;
	    BYTE at = getAType();
	    if (NPC_ATYPE_FAR==at || NPC_ATYPE_MFAR==at)
		r +=3;
	    getEntries(r,enemies,1);
	}
	if (!enemies.empty())					//对列表处理
	{
	    enemy = chooseEnemy(enemies);	//选择敌人
	}
	if (enemy)							//对找到的敌人使用技能
	{
	    if( selectByTenTh( Rate ) )    //计算万分比几率
	    {
		if( time1%time2 == 0 )	//检测是否达到使用时间
		{
		    Skill_buffe.SkillType = 1;
		    Skill_buffe.target = enemy;
		    Skill_buffe.id = id;
		    Skill_buffe.level = level;

		    m_SkillList.push_back( Skill_buffe );
		    return true;
		}
	    }
	}
    }
    else if( target == 1 )					//对自己的技能AI判断
    {
	if( selectByTenTh( Rate ) )
	{
	    if( time1%time2 == 0 )	//检测是否达到使用时间
	    {
		Skill_buffe.SkillType = 1;
		Skill_buffe.target = this;
		Skill_buffe.id = id;
		Skill_buffe.level = level;

		m_SkillList.push_back( Skill_buffe );
		return true;
	    }
	}
    }
    else if( target == 2 )   //定时器不触发友方处理
    {
	return true;
    }
    else if( target == 3 )   //召唤兽 宠物 图腾对主人专用
    {
	if( selectByTenTh( Rate ) )
	{
	    if( npc->kind == NPC_TYPE_PET ||
		    npc->kind == NPC_TYPE_SUMMONS ||
		    npc->kind == NPC_TYPE_TOTEM )

	    {
		SceneEntryPk * master;
		master = ((ScenePet *)this)->getMaster();
		if( time1%time2 == 0 )	//检测是否达到使用时间
		{
		    Skill_buffe.SkillType = 1;
		    Skill_buffe.target = master;
		    Skill_buffe.id = id;
		    Skill_buffe.level = level;

		    m_SkillList.push_back( Skill_buffe );
		    return true;
		}
	    }
	}
    }
    else if( target == 4 )
    {
	if( selectByTenTh( Rate ) )    //计算万分比几率
	{
	    if( time1%time2 == 0 && !m_mobType )	//检测是否达到使用条件
	    {
		m_mobType = true;
		Skill_buffe.SkillType = 2;
		Skill_buffe.target = this;
		Skill_buffe.id = id;
		Skill_buffe.level = level;


		m_SkillList.push_back( Skill_buffe );
		return true;
	    }
	}
    }

    return false;
}

///sky 判断是否超出随机范围
bool SceneNpc::outOfRandRegion(zPos * pos)
{
    return (pos->x>=RandPos.x-RandRegionX)
	&&(pos->x<=RandPos.x+RandRegionX)
	&&(pos->y>=RandPos.y-RandRegionY)
	&&(pos->y<=RandPos.y+RandRegionY);	
}
#endif

