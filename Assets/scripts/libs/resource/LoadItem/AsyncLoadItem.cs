using SDK.Common;
using UnityEngine;

/**
 * @brief 线程异步加载，目前只支持 WWW 加载
 */
namespace SDK.Lib
{
    public class AsyncLoadItem : LoadItem
    {
        // 加载完成后，在线程中的初始化
        virtual public void AsyncInit()
        {

        }
    }
}
