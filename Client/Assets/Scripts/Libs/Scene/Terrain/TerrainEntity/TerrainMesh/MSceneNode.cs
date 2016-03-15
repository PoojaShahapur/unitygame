using UnityEngine;

namespace SDK.Lib
{
    public class MSceneNode : MNode
    {
        public MSceneNode(string name = "")
        {
            selfGo = UtilApi.createGameObject(name);
        }
    }
}