using UnityEngine;

namespace SDK.Lib
{
    public enum eSceneNodeId
    {
        eSceneRootNode = 0,         // 场景根节点
        eSceneTerrainRoot = 1,      // 场景地形根节点
        eSceneEntityRoot = 2,       // 场景实体根节点

        eSceneTotal,
    }

    public class SceneNodeGraph
    {
        public GameObject[] mSceneNodes;

        public SceneNodeGraph()
        {
            
        }

        public void init()
        {
            mSceneNodes = new GameObject[(int)eSceneNodeId.eSceneTotal];
            mSceneNodes[(int)eSceneNodeId.eSceneRootNode] = UtilApi.createGameObject("SceneRootNode");
            mSceneNodes[(int)eSceneNodeId.eSceneTerrainRoot] = UtilApi.createGameObject("SceneTerrainRoot");
            mSceneNodes[(int)eSceneNodeId.eSceneEntityRoot] = UtilApi.createGameObject("SceneEntityRoot");

            UtilApi.SetParent(mSceneNodes[(int)eSceneNodeId.eSceneTerrainRoot], mSceneNodes[(int)eSceneNodeId.eSceneRootNode]);
            UtilApi.SetParent(mSceneNodes[(int)eSceneNodeId.eSceneEntityRoot], mSceneNodes[(int)eSceneNodeId.eSceneRootNode]);
        }
    }
}