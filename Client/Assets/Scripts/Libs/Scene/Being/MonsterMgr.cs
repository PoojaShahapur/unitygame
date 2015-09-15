namespace SDK.Lib
{
    public class MonsterMgr : EntityMgrBase
    {
        //protected Dictionary<int, List<Vehicle>> m_group2RadarDic = new Dictionary<int, List<Vehicle>>();

        public MonsterMgr()
        {

        }

        override protected void onTickExec(float delta)
        {
            base.onTickExec(delta);
        }

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

        public void addMonster(BeingEntity being)
        {
            this.addObject(being);
        }

        public void removeMonster(BeingEntity being)
        {
            this.delObject(being);
        }
    }
}