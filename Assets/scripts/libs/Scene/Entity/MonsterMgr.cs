using SDK.Common;
using System.Collections.Generic;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
    public class MonsterMgr : BeingMgr, IMonsterMgr
    {
        protected Dictionary<int, List<Vehicle>> m_group2RadarDic = new Dictionary<int, List<Vehicle>>();

        public IMonster createMonster()
        {
            return new Monster();
        }

        public List<Vehicle> getOrAddGroup(int groupID)
        {
            if(!m_group2RadarDic.ContainsKey(groupID))
            {
                m_group2RadarDic[groupID] = new List<Vehicle>();
            }

            return m_group2RadarDic[groupID];
        }

        public void addGroupMember(Monster monster)
        {
            m_group2RadarDic[monster.groupID].Add(monster.aiController.vehicle);
        }
    }
}