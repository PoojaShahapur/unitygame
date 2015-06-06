﻿using EditorTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UI;

namespace AtlasPrefabSys
{
    public class AtlasPrefabUtil
    {
        public static void createPrefab(string name)
        {
            string path = "Assets/Prefabs/Resources/Atlas";
            string prefabName = "Common";
            string prefabExtName = "prefab";

            string resFullPath = string.Format("{0}.{1}", Path.Combine(path, prefabName), prefabExtName);
            resFullPath = ExportUtil.normalPath(resFullPath);

            Type[] comArr = new Type[1];
            comArr[0] = typeof(Image);
            GameObject prefabRootGo = new GameObject(prefabName, comArr);

            Image image = prefabRootGo.GetComponent<Image>();
            Sprite[] allSpritesArr = AtlasPrefabUtil.loadAllSprite();
            image.sprite = allSpritesArr[0];

            // 创建预制，并且添加到编辑器中，以便进行检查
            PrefabUtility.CreatePrefab(resFullPath, prefabRootGo, ReplacePrefabOptions.ConnectToPrefab);
            //UnityEngine.Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(resFullPath);
            //PrefabUtility.ReplacePrefab(prefabRootGo, emptyPrefab, ReplacePrefabOptions.ConnectToPrefab);
            //刷新编辑器
            AssetDatabase.Refresh();
        }

        // 加载所有的精灵，这个精灵如果在地图集中，那么就是地图集的精灵，直接赋值给 Image ，打包成 unity3d 就是将地图集打包进入了
        public static Sprite[] loadAllSprite(string spritePath = "Assets/Res/Image/UI/OpenPack/shangdian_kaibao.png")
        {
            //spritePath = "E:/Work/Code20150402/client/trunk/Client/Assets/Res/Image/UI/Common";
            //spritePath = "Assets/Prefabs/Resources/UI";
            Sprite[] spriteArr;
            List<Sprite> spritesList = new List<Sprite>();

            UnityEngine.Object[] objArr = AssetDatabase.LoadAllAssetRepresentationsAtPath(spritePath);
            foreach(UnityEngine.Object obj in objArr)
            {
                if(obj != null && obj as Sprite)
                {
                    spritesList.Add(obj as Sprite);
                }
            }

            spriteArr = spritesList.ToArray();
            //SpriteUtility.GetSpriteTexture();

            return spriteArr;
        }

        public static T[] loadAllAsset<T>(string path) where T : UnityEngine.Object
        {
            T[] spriteArr;
            List<T> spritesList = new List<T>();

            UnityEngine.Object[] objArr = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
            foreach (UnityEngine.Object obj in objArr)
            {
                if (obj != null && obj as T)
                {
                    spritesList.Add(obj as T);
                }
            }

            spriteArr = spritesList.ToArray();

            return spriteArr;
        }
    }
}