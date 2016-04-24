﻿using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形是用的材质
     */
    public class TerrainMat
    {
        protected MatRes m_diffuseMatRes;           // 漫反射材质资源
        protected MatRes m_specularMatRes;          // 高光材质资源
        protected TextureRes m_diffuseTexRes;       // 漫反射纹理资源
        protected TextureRes m_hightTexRes;         // 高度纹理资源
        protected TextureRes m_normalTexRes;        // 法向量纹理资源

        protected Material m_diffuseMat;            // 漫反射材质
        protected Material m_specularMat;           // 高光材质
        protected Texture m_diffuseTex;             // 漫反射纹理
        protected Texture m_heightTex;              // 高度纹理
        protected Texture m_normalTex;              // 法向量纹理
        protected Shader m_diffuseShader;                  // 动态材质使用的纹理
        protected Shader m_specularShader;                  // 动态材质使用的纹理

        protected Material m_splatMat;
        protected Texture m_splat0Tex;
        protected Texture m_splat1Tex;
        protected Texture m_splat2Tex;
        protected Texture m_splat3Tex;
        protected Texture m_controlTex;
        protected Shader m_splatShader;
        protected MatRes m_splatMatRes;
        //protected TextureRes m_splat0TexRes;
        //protected TextureRes m_splat1TexRes;
        //protected TextureRes m_splat2TexRes;
        //protected TextureRes m_splat3TexRes;
        //protected TextureRes m_controlTexRes;

        protected AuxTexComponent mSplat0TexRes;
        protected AuxTexComponent mSplat1TexRes;
        protected AuxTexComponent mSplat2TexRes;
        protected AuxTexComponent mSplat3TexRes;
        protected AuxTexComponent mControlTexRes;

        protected string m_matPreStr;               // 材质前缀字符
        protected string m_difffuseMatName;         // 材质的名字
        protected string m_specularMatName;         // 材质的名字
        protected string m_diffuseShaderName;              // shader 的名字
        protected string m_specularShaderName;              // shader 的名字
        protected string m_diffuseTexName;          // 漫反射纹理名字
        protected string m_heightTexName;           // 高度纹理名字
        protected string m_normalTexName;           // 法向量纹理名字

        protected string m_splatMatName;
        protected string m_splatShaderName;
        protected string m_splat0TexName;
        protected string m_splat1TexName;
        protected string m_splat2TexName;
        protected string m_splat3TexName;
        protected string m_controlTexName;

        protected Vector4 mUVMultiplier;

        public TerrainMat()
        {
            m_difffuseMatName = "Materials/Terrain/TerrainDiffuse";
            m_specularMatName = "Materials/Terrain/TerrainBumpSpecular";
            m_matPreStr = "Dyn_";
            m_diffuseShaderName = "My/Terrain/Diffuse";
            m_specularShaderName = "Mobile/Bumped Specular (1 Directional Light)";
            m_diffuseTexName = "Materials/Textures/Terrain/TerrainDiffuse_1.jpg";
            m_heightTexName = "Materials/Textures/Terrain/terrain.png";
            m_normalTexName = "Materials/Textures/Terrain/terrain_normal.jpg";

            m_splatMatName = "Materials/Terrain/TerrainSplatDiffuse";
            m_splatShaderName = "My/Terrain/TerrainSplatDiffuse";
            m_splat0TexName = "Materials/Textures/Terrain/TerrainSplat_0.jpg";
            m_splat1TexName = "Materials/Textures/Terrain/TerrainSplat_1.jpg";
            m_splat2TexName = "Materials/Textures/Terrain/TerrainSplat_2.jpg";
            m_splat3TexName = "Materials/Textures/Terrain/TerrainSplat_3.jpg";
            m_controlTexName = "Materials/Textures/Terrain/TerrainControl.png";
        }

        public void initSplatPath(MImportData importData)
        {
            Vector4 vec = new Vector4(0, 0, 0, 0);
            if (importData.layerList.length() > 0)
            {
                m_splat0TexName = importData.layerList[0].textureName;
                vec.x = Ctx.m_instance.mTerrainGlobalOption.mTerrainSize / importData.layerList[0].worldSize;
            }
            if (importData.layerList.length() > 1)
            {
                m_splat1TexName = importData.layerList[1].textureName;
                vec.y = Ctx.m_instance.mTerrainGlobalOption.mTerrainSize / importData.layerList[1].worldSize;
            }
            if (importData.layerList.length() > 2)
            {
                m_splat2TexName = importData.layerList[2].textureName;
                vec.z = Ctx.m_instance.mTerrainGlobalOption.mTerrainSize / importData.layerList[2].worldSize;
            }
            if (importData.layerList.length() > 3)
            {
                m_splat3TexName = importData.layerList[3].textureName;
                vec.w = Ctx.m_instance.mTerrainGlobalOption.mTerrainSize / importData.layerList[3].worldSize;
            }
            setUVMultiplier(vec);
            m_controlTexName = importData.mAlphaTexName;
        }

        public void setUVMultiplier(float value)
        {
            mUVMultiplier = new Vector4(value, value, value, value);
        }

        public void setUVMultiplier(Vector4 value)
        {
            mUVMultiplier = value;
        }

        public void setDiffuseMap(string path)
        {
            m_diffuseTexName = path;
        }

        // 加载漫反射材质
        public void loadDiffuseMat()
        {
            m_diffuseMatRes = Ctx.m_instance.m_matMgr.getAndSyncLoad<MatRes>(m_difffuseMatName);
            m_diffuseMat = m_diffuseMatRes.getMat();

            m_diffuseShader = Shader.Find(m_diffuseShaderName);
            m_diffuseMat.shader = m_diffuseShader;

            m_diffuseTexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_diffuseTexName);
            m_diffuseTex = m_diffuseTexRes.getTexture();

            if (m_diffuseMat.HasProperty("_MainTex"))
            {
                m_diffuseMat.SetTexture("_MainTex", m_diffuseTex);
            }
        }

        // 加载高光材质
        public void loadSpecularMat()
        {
            m_specularMatRes = Ctx.m_instance.m_matMgr.getAndSyncLoad<MatRes>(m_specularMatName);
            m_specularMat = m_specularMatRes.getMat();

            m_specularShader = Shader.Find(m_specularShaderName);
            m_specularMat.shader = m_specularShader;

            m_diffuseTexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_diffuseTexName);
            m_diffuseTex = m_diffuseTexRes.getTexture();
            if (m_splatMat.HasProperty("_MainTex"))
            {
                m_specularMat.SetTexture("_MainTex", m_diffuseTex);
            }

            m_normalTexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_normalTexName);
            m_normalTex = m_normalTexRes.getTexture();
            if (m_splatMat.HasProperty("_BumpMap"))
            {
                m_specularMat.SetTexture("_BumpMap", m_normalTex);
            }
        }

        public void loadSplatDiffuseMat()
        {
            m_splatMatRes = Ctx.m_instance.m_matMgr.getAndSyncLoad<MatRes>(m_splatMatName);
            m_splatMat = m_splatMatRes.getMat();

            m_splatShader = Shader.Find(m_splatShaderName);
            m_splatMat.shader = m_splatShader;

            /*
            m_splat0TexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_splat0TexName);
            m_splat0Tex = m_splat0TexRes.getTexture();
            if (m_splatMat.HasProperty("_MainTex"))
            {
                m_splatMat.SetTexture("_MainTex", m_splat0Tex);
            }

            m_splat1TexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_splat1TexName);
            m_splat1Tex = m_splat1TexRes.getTexture();
            if (m_splatMat.HasProperty("_Splat1"))
            {
                m_splatMat.SetTexture("_Splat1", m_splat1Tex);
            }

            m_splat2TexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_splat2TexName);
            m_splat2Tex = m_splat2TexRes.getTexture();
            m_splatMat.SetTexture("_Splat2", m_splat2Tex);

            m_splat3TexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_splat3TexName);
            m_splat3Tex = m_splat3TexRes.getTexture();
            m_splatMat.SetTexture("_Splat3", m_splat3Tex);

            m_controlTexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_controlTexName);
            m_controlTex = m_controlTexRes.getTexture();
            if (m_splatMat.HasProperty("_Control"))
            {
                m_splatMat.SetTexture("_Control", m_controlTex);
            }
            */

            if (m_splatMat.HasProperty("_UVMultiplier"))
            {
                m_splatMat.SetVector("_UVMultiplier", mUVMultiplier);
            }

            mSplat0TexRes = new AuxTexComponent();
            mSplat0TexRes.asyncLoad(m_splat0TexName, onSplat0TexResLoaded);

            mSplat1TexRes = new AuxTexComponent();
            mSplat1TexRes.asyncLoad(m_splat1TexName, onSplat1TexResLoaded);

            mSplat2TexRes = new AuxTexComponent();
            mSplat2TexRes.asyncLoad(m_splat2TexName, onSplat2TexResLoaded);

            mSplat3TexRes = new AuxTexComponent();
            mSplat3TexRes.asyncLoad(m_splat3TexName, onSplat3TexResLoaded);

            mControlTexRes = new AuxTexComponent();
            mControlTexRes.asyncLoad(m_controlTexName, onControlTexResLoaded);
        }

        public Material getDiffuseMaterial()
        {
            return m_diffuseMat;
        }

        public Material getSpecularMaterial()
        {
            return m_specularMat;
        }

        public Material getSplatMaterial()
        {
            return m_splatMat;
        }

        public void onSplat0TexResLoaded(IDispatchObject dispObj)
        {
            mSplat0TexRes = dispObj as AuxTexComponent;
            if(m_splatMat.HasProperty("_MainTex"))
            {
                m_splatMat.SetTexture("_MainTex", mSplat0TexRes.getTexture());
            }
        }

        public void onSplat1TexResLoaded(IDispatchObject dispObj)
        {
            mSplat1TexRes = dispObj as AuxTexComponent;
            if (m_splatMat.HasProperty("_Splat1"))
            {
                m_splatMat.SetTexture("_Splat1", mSplat1TexRes.getTexture());
            }
        }

        public void onSplat2TexResLoaded(IDispatchObject dispObj)
        {
            mSplat2TexRes = dispObj as AuxTexComponent;
            if (m_splatMat.HasProperty("_Splat2"))
            {
                m_splatMat.SetTexture("_Splat2", mSplat2TexRes.getTexture());
            }
        }

        public void onSplat3TexResLoaded(IDispatchObject dispObj)
        {
            mSplat3TexRes = dispObj as AuxTexComponent;
            if (m_splatMat.HasProperty("_Splat3"))
            {
                m_splatMat.SetTexture("_Splat3", mSplat3TexRes.getTexture());
            }
        }

        public void onControlTexResLoaded(IDispatchObject dispObj)
        {
            mControlTexRes = dispObj as AuxTexComponent;
            if (m_splatMat.HasProperty("_Control"))
            {
                m_splatMat.SetTexture("_Control", mControlTexRes.getTexture());
            }
        }
    }
}