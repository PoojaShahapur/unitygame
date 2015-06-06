using EditorTool;
using SDK.Lib;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace AtlasPrefabSys
{
    public class FileData
    {
        protected string m_fullPath;
        protected string m_subPathNoExt;
        protected DirData m_dirData;

        protected string[] m_pathArr;

        public FileData(string path, DirData dir)
        {
            m_fullPath = path;
            m_dirData = dir;
        }

        public void buildData()
        {
            int dotIdx = m_fullPath.IndexOf(".");
            string pathNoExt = m_fullPath.Substring(0, dotIdx);
            m_subPathNoExt = pathNoExt.Substring(m_dirData.fullDirPath.Length + 1);

            pathNoExt = pathNoExt.Substring(m_dirData.fullDirPath.Length + 1);
            char[] separator = new char[1];
            separator[0] = '/';
            m_pathArr = pathNoExt.Split(separator);
        }

        public void addImageComponent(GameObject go_)
        {
            GameObject parGo = go_;
            GameObject curGo = null;
            for(int idx = 0; idx < m_pathArr.Length; ++idx)
            {
                curGo = new GameObject(m_pathArr[idx]);
                curGo.transform.SetParent(parGo.transform);

                parGo = curGo;
            }

            Image image = curGo.AddComponent<Image>();
            string assetsImagePath = ExportUtil.convFullPath2AssetsPath(m_fullPath);
            Sprite[] allSpritesArr = AtlasPrefabUtil.loadAllSprite(assetsImagePath);
            image.sprite = allSpritesArr[0];
        }

        public void addSprite2SO(SOSpriteList soSprite)
        {
            string assetsImagePath = ExportUtil.convFullPath2AssetsPath(m_fullPath);
            Sprite[] allSpritesArr = AtlasPrefabUtil.loadAllSprite(assetsImagePath);
            soSprite.addSprite(m_subPathNoExt, allSpritesArr[0]);
        }

        public void addAnimator2SO(SOAnimatorList soAnimator)
        {
            string assetsImagePath = ExportUtil.convFullPath2AssetsPath(m_fullPath);
            RuntimeAnimatorController asset = AtlasPrefabUtil.loadAllAsset<RuntimeAnimatorController>(assetsImagePath);
            soAnimator.addAnimator(m_subPathNoExt, asset);
        }
    }
}