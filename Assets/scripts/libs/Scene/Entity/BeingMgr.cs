using SDK.Common;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class BeingMgr : ITickedObject
    {
        public List<BeingEntity> m_List;

        public void OnTick(float delta)
        {
            foreach(BeingEntity being in m_List)
            {
                being.OnTick(delta);
            }
        }

        public void add(BeingEntity being)
        {
            m_List.Add(being);
        }

        public void remove(int tmpid)
        {

        }
    }
}
