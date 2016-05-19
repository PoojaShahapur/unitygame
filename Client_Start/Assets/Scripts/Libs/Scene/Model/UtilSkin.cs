using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief ���ߣ���Ҫ����������
     */
    public class UtilSkin
    {
        // ��Ƥ�����񣬸�������ֵ����ģ�͵�λ����Ϣ
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
                            // ��ֵ����ͷ������Ĭ���ǲ��ḳֵ��ȥ�ģ����°�Χ�в��ܸ��£�����ƶ����ܵ��°�Χ�в��ܱ����£�����ƶ�Զ�ˣ����ܾͻᱻ�ü���
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