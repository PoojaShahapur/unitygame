﻿using System;
using System.Collections.Generic;

namespace SDK.Common
{
    public class LoadParam
    {
        public ResPackType m_resPackType;          // 加载资源的类型
        public ResLoadType m_resLoadType;   // 资源加载类型

        public string m_path = "";               // 资源路径
        public string m_lvlName = "";            // 关卡名字
        public Action<IDispatchObject> m_loaded;        // 加载成功回调函数
        public Action<IDispatchObject> m_failed;        // 加载失败回调函数

        public bool m_resNeedCoroutine = true;     // 资源是否需要协同程序
        public bool m_loadNeedCoroutine = true;    // 加载是否需要协同程序

        public string m_prefabName = "";         // 预设的名字
    }
}