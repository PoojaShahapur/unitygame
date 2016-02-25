using SDK.Lib;
using UnityEditor;
using UnityEngine;

namespace AtlasPrefabSys
{
    public class EditorSOSprite
    {
        public SOSpriteList m_soSprite;

        // inPath 类似这样 Assets/Res/Image/UI/Common/denglu_srk.png , outPath = Assets/Resources/Atlas/aaa.asset
        public void packSprite(string inPath, string outPath)
        {
            m_soSprite = ScriptableObject.CreateInstance<SOSpriteList>();

            Sprite[] spriteArr = AtlasPrefabUtil.loadAllSprite(inPath);
            m_soSprite.addSprite("aaa", spriteArr[0]);
            AssetDatabase.CreateAsset(m_soSprite, outPath);
        }
    }
}