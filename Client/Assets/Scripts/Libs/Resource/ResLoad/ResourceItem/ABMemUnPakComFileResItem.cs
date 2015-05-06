using SDK.Common;
using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 没有打包的系统，在没有打包之前使用这个加载系统，每一个 ResItem 只有一个资源，打包的资源也是每一个 item 只有一个资源包
     */
    public class ABMemUnPakComFileResItem : ABUnPakComFileResItem
    {
        override public void initByPakRes()
        {
            base.initByPakRes();
            initByBytes(m_resItem.getBytes(m_path), PRE_PATH);
        }
    }
}