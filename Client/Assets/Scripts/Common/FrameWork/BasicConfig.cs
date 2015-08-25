using UnityEngine;

namespace SDK.Lib
{
    public enum IpSelect
    {
        IP_192_168_125_79,
        IP_192_168_125_254,
    }

    public enum ZoneSelect
    {
        Zone_30,
        Zone_31,
    }

    public class BasicConfig : MonoBehaviour
    {
        //public int m_ipidx;
        protected string[] m_ipList = new string[2];
        protected IpSelect m_IpSelect;

        protected ushort[] m_zoneList = new ushort[2];
        public ZoneSelect m_zoneSelect;

        public BasicConfig()
        {
            m_ipList[0] = "192.168.125.79";
            m_ipList[1] = "192.168.125.254";
            m_IpSelect = IpSelect.IP_192_168_125_254;

            m_zoneList[0] = 30;
            m_zoneList[1] = 31;
            m_zoneSelect = ZoneSelect.Zone_30;
        }

        public string getIp()
        {
            return m_ipList[(int)m_IpSelect];
        }

        public ushort getPort()
        {
            return m_zoneList[(int)m_zoneSelect];
        }
    }
}