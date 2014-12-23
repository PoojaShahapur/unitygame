using System.Collections.Generic;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
    public class ZoneSys
    {
        protected Dictionary<int, DangerZone> m_dangerZone = new Dictionary<int,DangerZone>();

        public DangerZone getOrAddDangerZone(int groupID)
        {
            if (!m_dangerZone.ContainsKey(groupID))
            {
                m_dangerZone[groupID] = new DangerZone();
            }

            return m_dangerZone[groupID];
        }

        public void addMonster(Monster monster)
        {
            if(!m_dangerZone.ContainsKey(monster.groupID))
            {
                m_dangerZone[monster.groupID] = new DangerZone();
            }

            m_dangerZone[monster.groupID].addMonster(monster);
        }
    }
}
