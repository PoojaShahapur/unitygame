using System.Collections.Generic;
using SDK.Lib;
using System.IO;

namespace UnitTestSrc
{
    public class TestAStar
    {
        public void run()
        {
            MGraph pMGraph = new MGraph();
            pMGraph.init(3, 3, 1, 1);

            List<Vertex> vertList;
            FileStream pFile = new FileStream("E:\\aaa.txt", FileMode.Create);
            string strStream = "";

            //vertList = test5Stop(pMGraph);
            //serializePath(vertList, ref strStream);
            //pMGraph.clearAllStopPoint();
            //pMGraph.clearPath();

            //vertList = test4Stop(pMGraph);
            //serializePath(vertList, ref strStream);
            //pMGraph.clearAllStopPoint();
            //pMGraph.clearPath();

            //vertList = test2Stop(pMGraph);
            //serializePath(vertList, ref strStream);
            //pMGraph.clearAllStopPoint();
            //pMGraph.clearPath();

            //vertList = test2Stop(pMGraph);
            //serializePath(vertList, ref strStream);
            //pMGraph.clearAllStopPoint();
            //pMGraph.clearPath();

            vertList = test6Stop(pMGraph);
            serializePath(vertList, ref strStream);
            pMGraph.clearAllStopPoint();
            pMGraph.clearPath();

            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(strStream);
            pFile.Write(inBytes, 0, inBytes.Length);
            pFile.Close();
            pFile.Dispose();
        }

        // ²âÊÔ 0 ¸ö×èµ²µã
        public List<Vertex> test0Stop(MGraph pMGraph)
        {
            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(0, 4))
            {
                vertList = pMGraph.getShortestPathFromPathCache(0, 4);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(0, 4);
            }

            return vertList;
        }

        // ²âÊÔ 1 ¸ö×èµ²µã
        public List<Vertex> test1Stop(MGraph pMGraph)
        {
            StopPoint pStopPoint = null;

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 0, pStopPoint);

            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(0, 8))
            {
                vertList = pMGraph.getShortestPathFromPathCache(0, 8);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(0, 8);
            }

            return vertList;
        }

        // ²âÊÔ 2 ¸ö×èµ²µã
        public List<Vertex> test2Stop(MGraph pMGraph)
        {
            StopPoint pStopPoint = null;

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 0, pStopPoint);

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 1, pStopPoint);

            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(0, 8))
            {
                vertList = pMGraph.getShortestPathFromPathCache(0, 8);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(0, 8);
            }

            return vertList;
        }

        // ²âÊÔ 3 ¸ö×èµ²µã
        public List<Vertex> test3Stop(MGraph pMGraph)
        {
            StopPoint pStopPoint = null;

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 0, pStopPoint);

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 1, pStopPoint);

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 2, pStopPoint);

            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(0, 8))
            {
                vertList = pMGraph.getShortestPathFromPathCache(0, 8);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(0, 8);
            }

            return vertList;
        }

        // ²âÊÔ¶Ô½ÇÏß×èµ²µã
        public List<Vertex> test4Stop(MGraph pMGraph)
        {
            StopPoint pStopPoint = null;

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 0, pStopPoint);

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(0, 1, pStopPoint);

            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(1, 1))
            {
                vertList = pMGraph.getShortestPathFromPathCache(1, 1);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(1, 1);
            }

            return vertList;
        }

        // ²âÊÔ×î³¤×èµ²µã
        public List<Vertex> test5Stop(MGraph pMGraph)
        {
            StopPoint pStopPoint = null;

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(1, 1, pStopPoint);

            pStopPoint = new StopPoint();
            pMGraph.addStopPoint(0, 1, pStopPoint);

            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(0, 6))
            {
                vertList = pMGraph.getShortestPathFromPathCache(0, 6);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(0, 6);
            }

            return vertList;
        }

        // ²âÊÔÐ±ÏßÂ·¾¶Æ½»¬
        public List<Vertex> test6Stop(MGraph pMGraph)
        {
            List<Vertex> vertList;
            if (pMGraph.isPathCacheValid(0, 7))
            {
                vertList = pMGraph.getShortestPathFromPathCache(0, 7);
            }
            else
            {
                vertList = pMGraph.getOrCreateShortestPath(0, 7);
            }

            return vertList;
        }

        public void serializePath(List<Vertex> vertList, ref string strStream)
        {
            if (vertList.Count > 0)
            {
                foreach (var pVert in vertList)
                {
                    strStream += string.Format("Vert ID = {0} \n", pVert.m_id);
                }
            }
            else
            {
                strStream += "can not find path \n";
            }
        }
    }
}