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

        // 蒙皮子网格，一定要先将子模型挂到骨骼上，再给子模型蒙皮，然后根骨骼赋值给子模型的位置信息
        public static void skinSkel(GameObject subMesh, params string[] bonesList)
        {
            bool firstBone = false;
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
                            // 赋值根骨头，否则默认是不会赋值过去的，导致包围盒不能更新，如果移动可能导致包围盒不能被更新，如果移动远了，可能就会被裁剪掉
                            if (!firstBone)
                            {
                                firstBone = true;
                                skinRenderer.rootBone = trans;
                            }
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