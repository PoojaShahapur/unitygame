using SDK.Lib;
using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 生成战斗用的数据，SkillActionMgr 只是不变的数据，这个里面是不变加可变的数据
     */
    public class SkillAttackFlowMgr
    {
        protected Dictionary<string, MList<OneAttackFlowSeq>> m_cycleAttackList;       // 循环列表
        protected Dictionary<string, MList<OneHurtFlowSeq>> m_cycleHurtList;              // 循环列表

        public SkillAttackFlowMgr()
        {
            m_cycleAttackList = new Dictionary<string, MList<OneAttackFlowSeq>>();
            m_cycleHurtList = new Dictionary<string, MList<OneHurtFlowSeq>>();
        }

        public OneAttackFlowSeq getOneAttackFlowSeq(string skillId)
        {
            OneAttackFlowSeq ret;
            if(m_cycleAttackList.ContainsKey(skillId))
            {
                if(m_cycleAttackList[skillId].Count() > 0)
                {
                    ret = m_cycleAttackList[skillId][0];
                    m_cycleAttackList[skillId].RemoveAt(0);
                    return ret;
                }
            }

            string _path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSkillAction], "1000.xml");
            SkillActionRes res = Ctx.m_instance.m_skillActionMgr.getAndSyncLoad<SkillActionRes>(_path);
            ret = new OneAttackFlowSeq();
            ret.initAttackFlowSeq(res.attackActionSeq);

            return ret;
        }

        public void releaseOneAttackFlowSeq(string skillId, OneAttackFlowSeq seq)
        {
            if (!m_cycleAttackList.ContainsKey(skillId))
            {
                m_cycleAttackList[skillId] = new MList<OneAttackFlowSeq>();
            }

            m_cycleAttackList[skillId].Add(seq);
        }

        public OneHurtFlowSeq getOneHurtFlowSeq(string skillId)
        {
            OneHurtFlowSeq ret;
            if (m_cycleHurtList.ContainsKey(skillId))
            {
                if (m_cycleHurtList[skillId].Count() > 0)
                {
                    ret = m_cycleHurtList[skillId][0];
                    m_cycleHurtList[skillId].RemoveAt(0);
                    return ret;
                }
            }

            string _path = string.Format("{0}{1}", Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathSkillAction], "1000.xml");
            SkillActionRes res = Ctx.m_instance.m_skillActionMgr.getAndSyncLoad<SkillActionRes>(_path);
            ret = new OneHurtFlowSeq();
            ret.initHurtFlowSeq(res.attackActionSeq);

            return ret;
        }

        public void releaseOneHurtFlowSeq(string skillId, OneHurtFlowSeq seq)
        {
            if (!m_cycleHurtList.ContainsKey(skillId))
            {
                m_cycleHurtList[skillId] = new MList<OneHurtFlowSeq>();
            }

            m_cycleHurtList[skillId].Add(seq);
        }
    }
}