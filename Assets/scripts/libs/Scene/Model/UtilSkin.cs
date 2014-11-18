using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 工具，主要是修正骨骼
     */
    public class UtilSkin
    {
        public string[] Bones;

        // 蒙皮子网格，一定要先改变子模型，再修正骨骼
        public static void skinSkel(GameObject subMesh, params string[] bonesList)
        {
            SkinnedMeshRenderer skinRenderer = subMesh.GetComponent<SkinnedMeshRenderer>();
            if (skinRenderer != null)
            {
                Transform[] bindBones = new Transform[bonesList.Length];
                Transform[] transforms = subMesh.transform.parent.GetComponentsInChildren<Transform>();
                for (int i = 0; i < bonesList.Length; i++)
                {
                    string bone = bonesList[i];
                    foreach (Transform trans in transforms)
                    {
                        if (trans.name == bone)
                        {
                            bindBones[i] = trans;
                            break;
                        }
                    }
                }
                skinRenderer.bones = bindBones;
            }
        }

        static public string convID2PartName(int id)
        {
            if ((int)PlayerModelDef.eModelHead == id)
            {
                return "body";
            }
            else if ((int)PlayerModelDef.eModelChest == id)
            {
                return "lwteeth";
            }
            else if ((int)PlayerModelDef.eModelWaist == id)
            {
                return "tounge";
            }
            else if ((int)PlayerModelDef.eModelLeg == id)
            {
                return "upteeth";
            }

            return "upteeth";
        }
    }
}