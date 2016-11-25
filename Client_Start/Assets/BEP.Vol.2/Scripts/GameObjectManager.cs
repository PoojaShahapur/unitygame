using UnityEngine;
using System.Collections.Generic;
using Assets.BEP.Vol._2.Scripts;
using System;

// 允许 Dictionary 存在相同的key,相同key的情况，即重量相同的情况下,排名随机
// 以后相同重量,要按会员等级等排序的话,就定义一个新的结构体,包含重量和比较参数
// public class NonCollidingFloatComparer : IComparer<float>
// {
//     public int Compare(float left, float right)
//     {
//         return (right > left) ? -1 : 1;// 返回0的话会抛出异常
//     }
//

namespace Assets.BEP.Vol._2.Scripts
{
    // 按倒序排序,key 越大排在越前面
    class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }

    public class GameObjectManager
    {
        public GameObject obj;
        private static GameObjectManager m_instance;

        // key : (半径 * 1000) << 32 | charid
        SortedDictionary<ulong, SceneEntity> m_allGameObjs;

        public static GameObjectManager getInstance()
        {
            if (m_instance == null)
            {
                m_instance = new GameObjectManager();
                return m_instance;
            }

            return m_instance;
        }

        GameObjectManager()
        {
            m_allGameObjs = new SortedDictionary<ulong, SceneEntity>(new DescendingComparer<ulong>());
        }
       
        void test()
        {
            // 为什么要new 3个,就像 C++ 指针一样,就一个指针的话,其中一个更改了,3个都更改了
            SceneEntity e1 = new SceneEntity();
            e1.m_charid = 1;
            setEntityByRadius(1.01f, e1);
            SceneEntity e2 = new SceneEntity();
            e2.m_charid = 2;
            setEntityByRadius(1.01f, e2);
            SceneEntity e3 = new SceneEntity();
            e3.m_charid = 3;
            setEntityByRadius(1.11f, e3);
            DumpAllEntity();
            setEntityByRadius(1.5f, e2);
            DumpAllEntity();
        }

        // 当新的物件被构造出来

        //当场景被摧毁时候
        void OnDestroy()
        {
            m_allGameObjs.Clear();
        }

        // Update is called once per frame
        public void CreateSnowBall()
        {
            // 如果有要创建的小球,创建下            
            for (int i = 0; i < CreateRobot.Instance.toBeCreateNames.Count; ++i)
            {
                CreateRobot.Instance.CreateSnowFoodWithNameCharID(
                    CreateRobot.Instance.toBeCreateNames[i].name
                    , CreateRobot.Instance.toBeCreateNames[i].charid);
            }
            CreateRobot.Instance.toBeCreateNames.Clear();
        }

        // 删除某个物件
        public void removeEntityByCharID(uint charid)
        {
            foreach (var each in m_allGameObjs)
            {
                if (each.Value.m_charid == charid)
                {
                    m_allGameObjs.Remove(each.Key);
                    break;
                }
            }
        }

        // 添加某个物件
        public void setEntityByRadius(float scale, SceneEntity entity)
        {
            ulong uniqueid = 0;
            uniqueid = (uint)(scale * 1000f);
            uniqueid = (uniqueid << 32 | entity.m_charid);
            ulong tmp = getUniqueIDByCharID(entity.m_charid);
            if (tmp != 0)
            {
                // 删除旧的
                m_allGameObjs.Remove(tmp);
            }

            // 添加新的
            m_allGameObjs.Add(uniqueid, entity);
        }

        // 通过 entity id 找到某个 entity
        public ulong getUniqueIDByCharID(uint id)
        {
            ulong ret = 0;
            foreach (var each in m_allGameObjs)
            {
                if (each.Value.m_charid == id)
                {
                    ret = each.Key;
                    break;
                }
            }

            return ret;
        }

        public void DumpAllEntity()
        {
            log.logHelper.DebugLog("开始打印所有排行榜物件");
            foreach (var temp in m_allGameObjs)
            {
                log.logHelper.DebugLog(temp.Key + ",id=" + temp.Value.m_charid + ",name=" + temp.Value.m_name);
            }
            log.logHelper.DebugLog("结束打印所有排行榜物件");
        }


        // 获得 top N 排名的玩家列表，N==0获得所有项
        public void getTopNEntity(ref List<SceneEntity> topRank, int N)
        {
            int maxNumber = N;
            if (N >= m_allGameObjs.Count || 0 == N)
            {
                maxNumber = m_allGameObjs.Count;
            }

            int startIndex = 0;
            foreach (var each in m_allGameObjs)
            {
                if (!(startIndex < maxNumber))
                {
                    break;
                }
                topRank.Add(each.Value);
                ++startIndex;
            }
        }
    }

    public class ChildrenItemInfo
    {
        public float startX;
        public float startZ;
        public GameObject childrenObj;

        public ChildrenItemInfo(float x = 0, float z = 0, GameObject obj = null)
        {
            startX = x;
            startZ = z;
            childrenObj = obj;
        }
    }
}