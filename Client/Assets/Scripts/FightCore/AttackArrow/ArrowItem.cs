﻿using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 箭头 Item
     */
    public class ArrowItem
    {
        public Transform m_parentTran = null;          // 父节点
        public GameObject m_go = null;                 // 显示的内容
        public string m_path;                          // 目录

        public virtual void onLoadEventHandle(IDispatchObject dispObj)
        {
            ModelRes res = dispObj as ModelRes;
            m_go = res.InstantiateObject(m_path);
            m_go.transform.parent = m_parentTran;
            UtilApi.normalPos(m_go.transform);
        }

        public void unload()
        {
            if (m_go != null)
            {
                UtilApi.Destroy(m_go);
                m_go = null;
                Ctx.m_instance.m_resLoadMgr.unload(m_path, onLoadEventHandle);
            }
        }

        public void load()
        {
            if (!string.IsNullOrEmpty(m_path))
            {
                LoadParam param;
                param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                param.setPath(m_path);
                param.m_loadEventHandle = onLoadEventHandle;

                // 这个需要立即加载
                param.m_loadNeedCoroutine = false;
                param.m_resNeedCoroutine = false;
                Ctx.m_instance.m_modelMgr.load<ModelRes>(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);
            }
        }

        public void normalRot()
        {
            UtilApi.normalRot(m_go.transform);
        }
    }
}