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
        public const string SceneRootNodeName = "SceneRootNode";
        public const string SceneTerrainRootName = "SceneTerrainRoot";
        public const string SceneEntityRootName = "SceneEntityRoot";

        public GameObject[] mSceneNodes;

        public SceneNodeGraph()
        {
            
        }

        public void init()
        {
            mSceneNodes = new GameObject[(int)eSceneNodeId.eSceneTotal];
            mSceneNodes[(int)eSceneNodeId.eSceneRootNode] = UtilApi.createGameObject(SceneRootNodeName);
            mSceneNodes[(int)eSceneNodeId.eSceneTerrainRoot] = UtilApi.createGameObject(SceneTerrainRootName);
            mSceneNodes[(int)eSceneNodeId.eSceneEntityRoot] = UtilApi.createGameObject(SceneEntityRootName);

            UtilApi.SetParent(mSceneNodes[(int)eSceneNodeId.eSceneTerrainRoot], mSceneNodes[(int)eSceneNodeId.eSceneRootNode]);
            UtilApi.SetParent(mSceneNodes[(int)eSceneNodeId.eSceneEntityRoot], mSceneNodes[(int)eSceneNodeId.eSceneRootNode]);

            Ctx.m_instance.m_sceneManager.getRootSceneNode().createChildSceneNode(SceneTerrainRootName, new MVector3(0, 0, 0), MQuaternion.IDENTITY);
            Ctx.m_instance.m_sceneManager.getRootSceneNode().createChildSceneNode(SceneEntityRootName, new MVector3(0, 0, 0), MQuaternion.IDENTITY);
        }
    }
}