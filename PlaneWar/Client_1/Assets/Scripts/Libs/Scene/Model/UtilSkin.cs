using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 工具，主要是修正骨骼
     */
    public class UtilSkin
    {
        // 蒙皮子网格，根骨骼赋值给子模型的位置信息
        public static void skinSkel(GameObject subMesh, GameObject skel, params string[] bonesList)
        {
            bool firstBone = false;
            SkinnedMeshRenderer skinMeshRenderer = subMesh.GetComponent<SkinnedMeshRenderer>();
            if (skinMeshRenderer != null)
            {
                Transform[] bindBonesList = new Transform[bonesList.Length];
                Transform[] allTransforms = skel.transform.GetComponentsInChildren<Transform>();
                for (int i = 0; i < bonesList.Length; i++)
                {
                    string bone = bonesList[i];
                    foreach (Transform trans in allTransforms)
                    {
                        if (trans.name == bone)
                        {
                            bindBonesList[i] = trans;
                            // 赋值根骨头，否则默认是不会赋值过去的，导致包围盒不能更新，如果移动可能导致包围盒不能被更新，如果移动远了，可能就会被裁剪掉
                            if (!firstBone)
                            {
                                firstBone = true;
                                skinMeshRenderer.rootBone = trans;
                            }
                            break;
                        }
                    }
                }
                skinMeshRenderer.bones = bindBonesList;
            }
        }

        static public string convID2PartName(int id)
        {
            if ((int)ePlayerModelType.eModelHead == id)
            {
                return "body";
            }
            else if ((int)ePlayerModelType.eModelChest == id)
            {
                return "lwteeth";
            }
            else if ((int)ePlayerModelType.eModelWaist == id)
            {
                return "tounge";
            }
            else if ((int)ePlayerModelType.eModelLeg == id)
            {
                return "upteeth";
            }

            return "upteeth";
        }
    }
}