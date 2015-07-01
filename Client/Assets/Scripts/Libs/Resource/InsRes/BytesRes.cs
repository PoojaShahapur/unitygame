﻿using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 二进制资源
     */
    class BytesRes : InsResBase
    {
        protected byte[] m_bytes;

        public BytesRes()
        {

        }

        override public void init(ResItem res)
        {
            // 获取资源单独保存
            m_bytes = (res.getObject(res.getPrefabName()) as TextAsset).bytes;
            base.init(res);
        }
    }
}