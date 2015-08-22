#include "CardEffectCfg.h"
#include "Zebra.h"
#include "Card.h"
#include "zXMLParser.h"
///////////////////////////////////////////////
//
//code[ScenesServer/CardEffectCfg.cpp] defination by codemokey
//
//
///////////////////////////////////////////////
#include <dirent.h>
#include <string>


CardEffectCfg::CardEffectCfg()
{}

CardEffectCfg::~CardEffectCfg()
{}

bool CardEffectCfg::loadAllEffect()
{
    effectMap.clear();
    std::string EffectDir = Zebra::global["configdir"] + "CardEffect/";
    DIR* pDir = opendir(EffectDir.c_str());	//打开目录
    if(NULL == pDir)
	return true;

    struct dirent* pEntry;
    while(NULL != (pEntry=readdir(pDir)))
    {
	if(pEntry->d_type != DT_REG)	//普通文件
	    continue;
	DWORD dwCardID = atoi(pEntry->d_name);

	loadOneEffect(dwCardID);
    }
    closedir(pDir);	    //关闭目录
    Zebra::logger->debug("[卡牌效果]总共加载到 %u 个效果",effectMap.size());
    return true;
}


bool CardEffectCfg::loadOneEffect(DWORD cardID)
{
    std::string sPath = Zebra::global["configdir"] + "CardEffect/";
    char name[48];
    bzero(name, sizeof(name));
    sprintf(name, "%d", cardID);
    std::string strName(name);
    sPath += (strName + ".xml");
    //Zebra::logger->debug("准备加载卡牌效果配置:%s", sPath.c_str());

    zXMLParser xml;
    if(!xml.initFile(sPath))
    {
	Zebra::logger->debug("打开 卡牌效果配置:%s 文件失败", sPath.c_str());
	return false;
    }
    xmlNodePtr root = xml.getRootNode("card");
    if(!root)
	return false;
    EffectInfo info;
    xmlNodePtr node1 = xml.getChildNode(root, "deadLanguage");
    if(node1)
    {
	xmlNodePtr skillNode = xml.getChildNode(node1, "skill");
	while(skillNode)
	{
	    DWORD deadID = 0;
	    xml.getNodePropNum(skillNode, "id", &deadID, sizeof(deadID));
	    info.deadIDVec.push_back(deadID);
	    skillNode = xml.getNextNode(skillNode, "skill");
	}
    }
    xmlNodePtr node2 = xml.getChildNode(root, "roundSID");
    if(node2)
    {
	xmlNodePtr skillNode = xml.getChildNode(node2, "skill");
	while(skillNode)
	{
	    t_EffectUnit einfo;
	    xml.getNodePropNum(skillNode, "id", &einfo.id, sizeof(einfo.id));
	    info.roundSIDVec.push_back(einfo);
	    skillNode = xml.getNextNode(skillNode, "skill");
	}
    }
    xmlNodePtr node3 = xml.getChildNode(root, "roundEID");
    if(node3)
    {
	xmlNodePtr skillNode = xml.getChildNode(node3, "skill");
	while(skillNode)
	{
	    t_EffectUnit einfo;
	    xml.getNodePropNum(skillNode, "id", &einfo.id, sizeof(einfo.id));
	    info.roundEIDVec.push_back(einfo);
	    skillNode = xml.getNextNode(skillNode, "skill");
	}
    }
    xmlNodePtr node4 = xml.getChildNode(root, "sAttackID");
    if(node4)
    {
	xmlNodePtr skillNode = xml.getChildNode(node4, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.sAttackID, sizeof(info.sAttackID));
    }
    xmlNodePtr node5 = xml.getChildNode(root, "beAttackID");
    if(node5)
    {
	xmlNodePtr skillNode = xml.getChildNode(node5, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.beAttackID, sizeof(info.beAttackID));
    }
    xmlNodePtr node6 = xml.getChildNode(root, "drawID");
    if(node6)
    {
	xmlNodePtr skillNode = xml.getChildNode(node6, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.drawID, sizeof(info.drawID));
    }
    xmlNodePtr node7 = xml.getChildNode(root, "beHurtID");
    if(node7)
    {
	xmlNodePtr skillNode = xml.getChildNode(node7, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.beHurtID, sizeof(info.beHurtID));
    }
    xmlNodePtr node8 = xml.getChildNode(root, "beCureID");
    if(node8)
    {
	xmlNodePtr skillNode = xml.getChildNode(node8, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.beCureID, sizeof(info.beCureID));
    }
    xmlNodePtr node9 = xml.getChildNode(root, "drawedID");
    if(node9)
    {
	xmlNodePtr skillNode = xml.getChildNode(node9, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.drawedID, sizeof(info.drawedID));
    }
    xmlNodePtr node10 = xml.getChildNode(root, "otherBeHurt");
    if(node10)
    {
	char tempChar[32];
	memset(tempChar, 0, sizeof(tempChar));
	xmlNodePtr conditionNode = xml.getChildNode(node10, "condition");
	if(conditionNode)
	    xml.getNodePropStr(conditionNode,"status",tempChar,sizeof(tempChar));
	set_ConditionState(tempChar, info.hurtStatus);

	xmlNodePtr skillNode = xml.getChildNode(node10, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.otherBeHurtID, sizeof(info.otherBeHurtID));
    }
    xmlNodePtr node11 = xml.getChildNode(root, "otherDead");
    if(node11)
    {
	char tempChar[32];
	memset(tempChar, 0, sizeof(tempChar));
	xmlNodePtr conditionNode = xml.getChildNode(node11, "condition");
	if(conditionNode)
	    xml.getNodePropStr(conditionNode,"status",tempChar,sizeof(tempChar));
	set_ConditionState(tempChar, info.deadStatus);

	xmlNodePtr skillNode = xml.getChildNode(node11, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.otherDeadID, sizeof(info.otherDeadID));
    }
    xmlNodePtr node12 = xml.getChildNode(root, "otherBeCure");
    if(node12)
    {
	char tempChar[32];
	memset(tempChar, 0, sizeof(tempChar));
	xmlNodePtr conditionNode = xml.getChildNode(node12, "condition");
	if(conditionNode)
	    xml.getNodePropStr(conditionNode,"status",tempChar,sizeof(tempChar));
	set_ConditionState(tempChar, info.cureStatus);

	xmlNodePtr skillNode = xml.getChildNode(node12, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.otherBeCureID, sizeof(info.otherBeCureID));
    }
    xmlNodePtr node13 = xml.getChildNode(root, "selfUseMagic");
    if(node13)
    {
	xmlNodePtr skillNode = xml.getChildNode(node13, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.selfUseMagic, sizeof(info.selfUseMagic));
    }
    xmlNodePtr node14 = xml.getChildNode(root, "enemyUseMagic");
    if(node14)
    {
	xmlNodePtr skillNode = xml.getChildNode(node14, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.enemyUseMagic, sizeof(info.enemyUseMagic));
    }
    xmlNodePtr node15 = xml.getChildNode(root, "useAttend");
    if(node15)
    {
	char tempChar[32];
	memset(tempChar, 0, sizeof(tempChar));
	xmlNodePtr conditionNode = xml.getChildNode(node15, "condition");
	if(conditionNode)
	    xml.getNodePropStr(conditionNode,"status",tempChar,sizeof(tempChar));
	set_ConditionState(tempChar, info.useAttendStatus);

	xmlNodePtr skillNode = xml.getChildNode(node15, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.otherUseAttendID, sizeof(info.otherUseAttendID));
    }
    xmlNodePtr node16 = xml.getChildNode(root, "attendIn");
    if(node16)
    {
	char tempChar[32];
	memset(tempChar, 0, sizeof(tempChar));
	xmlNodePtr conditionNode = xml.getChildNode(node16, "condition");
	if(conditionNode)
	    xml.getNodePropStr(conditionNode,"status",tempChar,sizeof(tempChar));
	set_ConditionState(tempChar, info.attendInStatus);

	xmlNodePtr skillNode = xml.getChildNode(node16, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.otherAttendInID, sizeof(info.otherAttendInID));
    }
    xmlNodePtr node17 = xml.getChildNode(root, "haloID");
    if(node17)
    {
	xmlNodePtr conditionNode = xml.getChildNode(node17, "haloCondition");
	if(conditionNode)
	{
	    xml.getNodePropNum(conditionNode, "ctype", &info.halo_Ctype, sizeof(info.halo_Ctype));
	    xml.getNodePropNum(conditionNode, "cid", &info.halo_Cid, sizeof(info.halo_Cid));
	}
	xmlNodePtr skillNode = xml.getChildNode(node17, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.haloID, sizeof(info.haloID));
    }
    xmlNodePtr node18 = xml.getChildNode(root, "attackEnd");
    if(node18)
    {
	xmlNodePtr skillNode = xml.getChildNode(node18, "skill");
	if(skillNode)
	    xml.getNodePropNum(skillNode, "id", &info.attackEndID, sizeof(info.attackEndID));
	xmlNodePtr conditionNode = xml.getChildNode(node18, "condition");
	if(conditionNode)
	    xml.getNodePropNum(conditionNode, "id", &info.attackEndCondition, sizeof(info.attackEndCondition));
    }
    xmlNodePtr node19 = xml.getChildNode(root, "enemyroundSID");
    if(node19)
    {
	xmlNodePtr skillNode = xml.getChildNode(node19, "skill");
	while(skillNode)
	{
	    t_EffectUnit einfo;
	    xml.getNodePropNum(skillNode, "id", &einfo.id, sizeof(einfo.id));
	    info.enemyroundSIDVec.push_back(einfo);
	    skillNode = xml.getNextNode(skillNode, "skill");
	}
    }
    xmlNodePtr node20 = xml.getChildNode(root, "enemyroundEID");
    if(node20)
    {
	xmlNodePtr skillNode = xml.getChildNode(node20, "skill");
	while(skillNode)
	{
	    t_EffectUnit einfo;
	    xml.getNodePropNum(skillNode, "id", &einfo.id, sizeof(einfo.id));
	    info.enemyroundEIDVec.push_back(einfo);
	    skillNode = xml.getNextNode(skillNode, "skill");
	}
    }

    EffectMap_IT it = effectMap.find(cardID);
    if(it == effectMap.end())
    {
	effectMap.insert(std::make_pair(cardID, info));
    }
    else
    {
	effectMap[cardID] = info;
    }
    return true;
}

bool CardEffectCfg::fullOneCardPKData(const DWORD cardID, t_CardPK &pk)
{
    EffectMap_IT it = effectMap.find(cardID);
    if(it == effectMap.end())
	return true;
    
    if(!it->second.deadIDVec.empty())
    {
	for(std::vector<DWORD>::iterator it2=it->second.deadIDVec.begin(); it2!=it->second.deadIDVec.end(); it2++)
	{
	    pk.deadIDVec.push_back(*it2);
	}
    }

    if(!it->second.roundSIDVec.empty())
    {
	for(std::vector<t_EffectUnit>::iterator it2=it->second.roundSIDVec.begin(); it2!=it->second.roundSIDVec.end(); it2++)
	{
	    pk.roundSIDVec.push_back(*it2);
	}
    }
    if(!it->second.roundEIDVec.empty())
    {
	for(std::vector<t_EffectUnit>::iterator it2=it->second.roundEIDVec.begin(); it2!=it->second.roundEIDVec.end(); it2++)
	{
	    pk.roundEIDVec.push_back(*it2);
	}
    }
    if(!it->second.enemyroundSIDVec.empty())
    {
	for(std::vector<t_EffectUnit>::iterator it2=it->second.enemyroundSIDVec.begin(); it2!=it->second.enemyroundSIDVec.end(); it2++)
	{
	    pk.enemyroundSIDVec.push_back(*it2);
	}
    }
    if(!it->second.enemyroundEIDVec.empty())
    {
	for(std::vector<t_EffectUnit>::iterator it2=it->second.enemyroundEIDVec.begin(); it2!=it->second.enemyroundEIDVec.end(); it2++)
	{
	    pk.enemyroundEIDVec.push_back(*it2);
	}
    }

    pk.sAttackID = it->second.sAttackID;
    pk.beAttackID = it->second.beAttackID;
    pk.drawID = it->second.drawID;
    pk.beHurtID = it->second.beHurtID;
    pk.beCureID = it->second.beCureID;
    pk.drawedID = it->second.drawedID;
    
    pk.hurtStatus = it->second.hurtStatus;
    pk.otherBeHurtID = it->second.otherBeHurtID;
    pk.deadStatus = it->second.deadStatus;
    pk.otherDeadID = it->second.otherDeadID;
    pk.cureStatus = it->second.cureStatus;
    pk.otherBeCureID = it->second.otherBeCureID;

    pk.selfUseMagic = it->second.selfUseMagic;
    pk.enemyUseMagic = it->second.enemyUseMagic;
    
    pk.useAttendStatus = it->second.useAttendStatus;
    pk.otherUseAttendID = it->second.otherUseAttendID;
    pk.attendInStatus = it->second.attendInStatus;
    pk.otherAttendInID = it->second.otherAttendInID;

    pk.halo_Ctype = it->second.halo_Ctype;
    pk.halo_Cid = it->second.halo_Cid;
    pk.haloID = it->second.haloID;
    pk.attackEndCondition = it->second.attackEndCondition;
    pk.attackEndID = it->second.attackEndID;
    return true;
}
