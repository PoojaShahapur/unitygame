using SDK.Common;
using System.Collections.Generic;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
    public class MonsterMgr : BeingMgr
    {
        //protected Dictionary<int, List<Vehicle>> m_group2RadarDic = new Dictionary<int, List<Vehicle>>();

        public Monster createMonster()
        {
            return new Monster();
        }

        //public List<Vehicle> getOrAddGroup(int groupID)
        //{
        //    if(!m_group2RadarDic.ContainsKey(groupID))
        //    {
        //        m_group2RadarDic[groupID] = new List<Vehicle>();
        //    }

        //    return m_group2RadarDic[groupID];
        //}

        public void addGroupMember(Monster monster)
        {
            //m_group2RadarDic[monster.groupID].Add(monster.aiController.vehicle);
            //int idx = 0;
            //int idy = 0;
            //while(idx < m_list.Count)
            //{
            //    idy = 0;
            //    while (idy < m_list.Count)
            //    {
            //        m_list[idx].aiController.radar.Vehicles.Clear();
            //        if (m_list[idx] != m_list[idy])
            //        {
            //            m_list[idx].aiController.radar.Vehicles.Add(m_list[idy].aiController.vehicle);
            //        }

            //        ++idy;
            //    }

            //    (m_list[idx].aiController.vehicle.Steerings[1] as SteerForNeighborGroup).HandleDetection(m_list[idx].aiController.radar);

            //    ++idx;
            //}
        }
    }
}