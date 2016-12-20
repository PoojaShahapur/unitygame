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
        protected string[] mIpList = new string[2];
        protected IpSelect mIpSelect;

        protected ushort[] mZoneList = new ushort[2];
        public ZoneSelect mZoneSelect;

        public BasicConfig()
        {
            this.mIpList[0] = "192.168.125.79";
            this.mIpList[1] = "192.168.125.254";
            this.mIpSelect = IpSelect.IP_192_168_125_254;

            this.mZoneList[0] = 30;
            this.mZoneList[1] = 31;
            this.mZoneSelect = ZoneSelect.Zone_30;
        }

        public string getIp()
        {
            return this.mIpList[(int)this.mIpSelect];
        }

        public ushort getPort()
        {
            return this.mZoneList[(int)this.mZoneSelect];
        }
    }
}