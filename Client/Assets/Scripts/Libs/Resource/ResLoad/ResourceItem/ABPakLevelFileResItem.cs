using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 打包的 Level stream 系统
     */
    public class ABPakLevelFileResItem : ABPakFileResItemBase
    {
        override public void init(LoadItem item)
        {
            base.init(item);
        }

        protected string m_levelName;

        public string levelName
        {
            set
            {
                m_levelName = value;
            }
        }
    }
}