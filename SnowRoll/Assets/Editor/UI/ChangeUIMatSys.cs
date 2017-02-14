using SDK.Lib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EditorTool
{
    public class ChangeUIMatSys : Singleton<ChangeUIMatSys>, IMyDispose
    {
        public ChangeUIMatSys()
        {
            
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void change()
        {
            string assetPath = "Assets/Resources/UI/UIStartGame/UIStartGame.prefab";
            GameObject _go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            UtilApi.traverseActor<Image>(_go, onHandle);
            AssetDatabase.SaveAssets();
        }

        protected void onHandle(GameObject go_, Image image)
        {
            if (image.sprite)
            {
                Texture2D texture = image.sprite.texture;
                if (texture)
                {
                    string assetPath = AssetDatabase.GetAssetPath(texture);

                    //if(assetPath.Substring(assetPath.Length - 4) == "_RGB")
                    {
                        assetPath = "Assets/Resources/Shader/AlphaUI.mat";
                        Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                        if (mat)
                        {
                            image.material = mat;
                        }
                    }
                }
            }
        }
    }
}