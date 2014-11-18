using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief ���ߣ���Ҫ����������
     */
    public class UtilSkin
    {
        public string[] Bones;

        // ��Ƥ������һ��Ҫ�Ƚ���ģ�͹ҵ������ϣ��ٸ���ģ����Ƥ��Ȼ���������ֵ����ģ�͵�λ����Ϣ
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
                            // ��ֵ����ͷ������Ĭ���ǲ��ḳֵ��ȥ�ģ����°�Χ�в��ܸ��£�����ƶ����ܵ��°�Χ�в��ܱ����£�����ƶ�Զ�ˣ����ܾͻᱻ�ü���
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