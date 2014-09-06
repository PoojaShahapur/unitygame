using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace San.Guo
{
    public interface IResMgr
    {
        Res load(LoadParam param);
    }
}
