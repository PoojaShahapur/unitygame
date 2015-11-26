using System.Collections.Generic;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
    public class DangerZone
    {
        protected List<Vehicle> m_vehicleList = new List<Vehicle>();
        protected Dictionary<Vehicle, bool> m_vehicleDic = new Dictionary<Vehicle, bool>();

        public List<Vehicle> vehicleList
        {
            get
            {
                return m_vehicleList;
            }
            set
            {
                m_vehicleList = value;
            }
        }

        public void addMonster(Monster monster)
        {
            if (!m_vehicleDic.ContainsKey(monster.aiController.vehicle))
            {
                m_vehicleList.Add(monster.aiController.vehicle);
            }
        }
    }
}
