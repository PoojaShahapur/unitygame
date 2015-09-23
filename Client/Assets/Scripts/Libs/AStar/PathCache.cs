using System.Collections.Generic;

namespace SDK.Lib
{
    class CacheItem
    {
        public List<int> m_vertsIdVec;          // 顶点路径列表
        public bool m_bValid;
    };

    class PathCache
    {
        Dictionary<int, Dictionary<int, CacheItem>> m_rowCol2PathMap;

        public PathCache()
        {

        }

        public void dispose()
        {
            foreach (var colMap in m_rowCol2PathMap)
            {
                foreach (var cacheItem in colMap.Value)
                {

                }
            }
        }

        public CacheItem getAndAddPathCache(int startId, int endId)
        {
            if (m_rowCol2PathMap[startId][endId] == null)
            {
                m_rowCol2PathMap[startId][endId] = new CacheItem();
            }

            m_rowCol2PathMap[startId][endId].m_vertsIdVec.Clear();
            m_rowCol2PathMap[startId][endId].m_bValid = true;
            return m_rowCol2PathMap[startId][endId];
        }

        public CacheItem getPathCache(int startId, int endId)
        {
            return m_rowCol2PathMap[startId][endId];
        }

        public bool isPathValid(int startId, int endId)
        {
            if (m_rowCol2PathMap[startId][endId] == null || !m_rowCol2PathMap[startId][endId].m_bValid)
            {
                return false;
            }

            return true;
        }
    }
}