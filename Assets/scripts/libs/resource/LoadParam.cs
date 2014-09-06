using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    public class LoadParam
    {
        public ResType m_type;
        public string m_path;
        public string m_lvlName;
        public Func<Res, bool> m_cb;
    }
}
