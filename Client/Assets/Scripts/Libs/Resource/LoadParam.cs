﻿using SDK.Common;
using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public class LoadParam : IRecycle
    {
        public ResPackType m_resPackType;           // 加载资源的类型
        public ResLoadType m_resLoadType;           // 资源加载类型

        public string m_path = "";                  // 资源路径，传递进来都是完成的路径，都是相对 Prefabs\Resources 开始的，例如 Table\CardBase_client.txt，然后内部解析后
        public string m_pathNoExt = "";             // 这个数据变成了从 Resources 目录开始，没有扩展名字，打包的包名字在包加载的时候判断
        protected string m_prefabName = "";         // 预设的名字，就是文件名字，没有路径和扩展名字
        protected string m_extName = "prefab";      // 加载的资源的扩展名字

        public string m_version = "";               // 加载的资源的版本号
        public string m_lvlName = "";               // 关卡名字
        public Action<IDispatchObject> m_loaded;    // 加载成功回调函数
        public Action<IDispatchObject> m_failed;    // 加载失败回调函数

        public bool m_resNeedCoroutine = true;      // 资源是否需要协同程序
        public bool m_loadNeedCoroutine = true;     // 加载是否需要协同程序

        public string prefabName
        {
            get
            {
                return m_prefabName;
            }
        }

        public string extName
        {
            get
            {
                return m_extName;
            }
        }

        public void resetDefault()          // 将数据清空，有时候上一次调用的时候的参数 m_loaded 还在，结果被认为是这一次的回调了
        {
            m_loaded = null;
            m_failed = null;
            m_version = "";
            m_extName = "prefab";
        }

        // 解析目录
        public void resolvePath()
        {
            int dotIdx = m_path.IndexOf(".");
            int slashIdx = m_path.LastIndexOf("/");

            if (-1 == dotIdx)       // 材质是没有扩展名字的
            {
                m_extName = "";
                m_pathNoExt = m_path;

                if (-1 == slashIdx)  // 没有路径，只有一个文件名字
                {
                    m_prefabName = m_path;
                }
                else
                {
                    m_prefabName = m_path.Substring(slashIdx + 1);
                }
            }
            else
            {
                m_extName = m_path.Substring(dotIdx + 1);
                m_pathNoExt = m_path.Substring(0, dotIdx);

                if (-1 == slashIdx)  // 没有路径，只有一个文件名字
                {
                    m_prefabName = m_path.Substring(0, dotIdx);
                }
                else
                {
                    m_prefabName = m_path.Substring(slashIdx + 1, dotIdx - slashIdx - 1);
                }
            }
        }
    }
}