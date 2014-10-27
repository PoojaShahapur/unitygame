using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    /**
     * @brief 工具，主要是修正骨骼
     */
    public class UtilSkeleton
    {
        public string[] Bones;

        // 修正骨骼，一定要先改变子模型，再修正骨骼
        public static void CorrectSkel(GameObject subMesh, params string[] bonesList)
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
    }
}