using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形是用的材质
     */
    public class TerrainMat
    {
        protected MatRes mDiffuseMatRes;           // 漫反射材质资源
        protected MatRes mSpecularMatRes;          // 高光材质资源
        protected TextureRes mDiffuseTexRes;       // 漫反射纹理资源
        protected TextureRes mHightTexRes;         // 高度纹理资源
        protected TextureRes mNormalTexRes;        // 法向量纹理资源

        protected Material mDiffuseMat;            // 漫反射材质
        protected Material mSpecularMat;           // 高光材质
        protected Texture mDiffuseTex;             // 漫反射纹理
        protected Texture mHeightTex;              // 高度纹理
        protected Texture mNormalTex;              // 法向量纹理
        protected Shader mDiffuseShader;                  // 动态材质使用的纹理
        protected Shader mSpecularShader;                  // 动态材质使用的纹理

        protected Material mSplatMat;
        protected Texture mSplat0Tex;
        protected Texture mSplat1Tex;
        protected Texture mSplat2Tex;
        protected Texture mSplat3Tex;
        protected Texture mControlTex;
        protected Shader mSplatShader;
        protected MatRes mSplatMatRes;
        //protected TextureRes m_splat0TexRes;
        //protected TextureRes m_splat1TexRes;
        //protected TextureRes m_splat2TexRes;
        //protected TextureRes m_splat3TexRes;
        //protected TextureRes m_controlTexRes;

        protected AuxTextureLoader mSplat0TexRes;
        protected AuxTextureLoader mSplat1TexRes;
        protected AuxTextureLoader mSplat2TexRes;
        protected AuxTextureLoader mSplat3TexRes;
        protected AuxTextureLoader mControlTexRes;

        protected string mMatPreStr;               // 材质前缀字符
        protected string mDifffuseMatName;         // 材质的名字
        protected string mSpecularMatName;         // 材质的名字
        protected string mDiffuseShaderName;              // shader 的名字
        protected string mSpecularShaderName;              // shader 的名字
        protected string mDiffuseTexName;          // 漫反射纹理名字
        protected string mHeightTexName;           // 高度纹理名字
        protected string mNormalTexName;           // 法向量纹理名字

        protected string mSplatMatName;
        protected string mSplatShaderName;
        protected string mSplat0TexName;
        protected string mSplat1TexName;
        protected string mSplat2TexName;
        protected string mSplat3TexName;
        protected string mControlTexName;

        protected Vector4 mUVMultiplier;

        public TerrainMat()
        {
            mDifffuseMatName = "Materials/Terrain/TerrainDiffuse";
            mSpecularMatName = "Materials/Terrain/TerrainBumpSpecular";
            mMatPreStr = "Dyn_";
            mDiffuseShaderName = "My/Terrain/Diffuse";
            mSpecularShaderName = "Mobile/Bumped Specular (1 Directional Light)";
            mDiffuseTexName = "Materials/Textures/Terrain/TerrainDiffuse_1.jpg";
            mHeightTexName = "Materials/Textures/Terrain/terrain.png";
            mNormalTexName = "Materials/Textures/Terrain/terrain_normal.jpg";

            mSplatMatName = "Materials/Terrain/TerrainSplatDiffuse";
            mSplatShaderName = "My/Terrain/TerrainSplatDiffuse";
            mSplat0TexName = "Materials/Textures/Terrain/TerrainSplat_0.jpg";
            mSplat1TexName = "Materials/Textures/Terrain/TerrainSplat_1.jpg";
            mSplat2TexName = "Materials/Textures/Terrain/TerrainSplat_2.jpg";
            mSplat3TexName = "Materials/Textures/Terrain/TerrainSplat_3.jpg";
            mControlTexName = "Materials/Textures/Terrain/TerrainControl.png";
        }

        public void initSplatPath(MImportData importData)
        {
            Vector4 vec = new Vector4(0, 0, 0, 0);
            if (importData.layerList.length() > 0)
            {
                mSplat0TexName = importData.layerList[0].textureName;
                vec.x = Ctx.mInstance.mTerrainGlobalOption.mTerrainSize / importData.layerList[0].worldSize;
            }
            if (importData.layerList.length() > 1)
            {
                mSplat1TexName = importData.layerList[1].textureName;
                vec.y = Ctx.mInstance.mTerrainGlobalOption.mTerrainSize / importData.layerList[1].worldSize;
            }
            if (importData.layerList.length() > 2)
            {
                mSplat2TexName = importData.layerList[2].textureName;
                vec.z = Ctx.mInstance.mTerrainGlobalOption.mTerrainSize / importData.layerList[2].worldSize;
            }
            if (importData.layerList.length() > 3)
            {
                mSplat3TexName = importData.layerList[3].textureName;
                vec.w = Ctx.mInstance.mTerrainGlobalOption.mTerrainSize / importData.layerList[3].worldSize;
            }
            setUVMultiplier(vec);
            mControlTexName = importData.mAlphaTexName;
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
            mDiffuseTexName = path;
        }

        // 加载漫反射材质
        public void loadDiffuseMat()
        {
            mDiffuseMatRes = Ctx.mInstance.mMatMgr.getAndSyncLoad<MatRes>(mDifffuseMatName, null);
            mDiffuseMat = mDiffuseMatRes.getMat();

            mDiffuseShader = Shader.Find(mDiffuseShaderName);
            mDiffuseMat.shader = mDiffuseShader;

            mDiffuseTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mDiffuseTexName, null);
            mDiffuseTex = mDiffuseTexRes.getTexture();

            if (mDiffuseMat.HasProperty("_MainTex"))
            {
                mDiffuseMat.SetTexture("_MainTex", mDiffuseTex);
            }
        }

        // 加载高光材质
        public void loadSpecularMat()
        {
            mSpecularMatRes = Ctx.mInstance.mMatMgr.getAndSyncLoad<MatRes>(mSpecularMatName, null);
            mSpecularMat = mSpecularMatRes.getMat();

            mSpecularShader = Shader.Find(mSpecularShaderName);
            mSpecularMat.shader = mSpecularShader;

            mDiffuseTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mDiffuseTexName, null);
            mDiffuseTex = mDiffuseTexRes.getTexture();
            if (mSplatMat.HasProperty("_MainTex"))
            {
                mSpecularMat.SetTexture("_MainTex", mDiffuseTex);
            }

            mNormalTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mNormalTexName, null);
            mNormalTex = mNormalTexRes.getTexture();
            if (mSplatMat.HasProperty("_BumpMap"))
            {
                mSpecularMat.SetTexture("_BumpMap", mNormalTex);
            }
        }

        public void loadSplatDiffuseMat()
        {
            mSplatMatRes = Ctx.mInstance.mMatMgr.getAndSyncLoad<MatRes>(mSplatMatName, null);
            mSplatMat = mSplatMatRes.getMat();

            mSplatShader = Shader.Find(mSplatShaderName);
            mSplatMat.shader = mSplatShader;

            /*
            m_splat0TexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mSplat0TexName);
            mSplat0Tex = m_splat0TexRes.getTexture();
            if (mSplatMat.HasProperty("_MainTex"))
            {
                mSplatMat.SetTexture("_MainTex", mSplat0Tex);
            }

            m_splat1TexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mSplat1TexName);
            mSplat1Tex = m_splat1TexRes.getTexture();
            if (mSplatMat.HasProperty("_Splat1"))
            {
                mSplatMat.SetTexture("_Splat1", mSplat1Tex);
            }

            m_splat2TexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mSplat2TexName);
            mSplat2Tex = m_splat2TexRes.getTexture();
            mSplatMat.SetTexture("_Splat2", mSplat2Tex);

            m_splat3TexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mSplat3TexName);
            mSplat3Tex = m_splat3TexRes.getTexture();
            mSplatMat.SetTexture("_Splat3", mSplat3Tex);

            m_controlTexRes = Ctx.mInstance.mTexMgr.getAndSyncLoad<TextureRes>(mControlTexName);
            mControlTex = m_controlTexRes.getTexture();
            if (mSplatMat.HasProperty("_Control"))
            {
                mSplatMat.SetTexture("_Control", mControlTex);
            }
            */

            if (mSplatMat.HasProperty("_UVMultiplier"))
            {
                mSplatMat.SetVector("_UVMultiplier", mUVMultiplier);
            }

            mSplat0TexRes = new AuxTextureLoader();
            mSplat0TexRes.asyncLoad(mSplat0TexName, onSplat0TexResLoaded);

            mSplat1TexRes = new AuxTextureLoader();
            mSplat1TexRes.asyncLoad(mSplat1TexName, onSplat1TexResLoaded);

            mSplat2TexRes = new AuxTextureLoader();
            mSplat2TexRes.asyncLoad(mSplat2TexName, onSplat2TexResLoaded);

            mSplat3TexRes = new AuxTextureLoader();
            mSplat3TexRes.asyncLoad(mSplat3TexName, onSplat3TexResLoaded);

            mControlTexRes = new AuxTextureLoader();
            mControlTexRes.asyncLoad(mControlTexName, onControlTexResLoaded);
        }

        public Material getDiffuseMaterial()
        {
            return mDiffuseMat;
        }

        public Material getSpecularMaterial()
        {
            return mSpecularMat;
        }

        public Material getSplatMaterial()
        {
            return mSplatMat;
        }

        public void onSplat0TexResLoaded(IDispatchObject dispObj)
        {
            mSplat0TexRes = dispObj as AuxTextureLoader;
            if(mSplatMat.HasProperty("_MainTex"))
            {
                mSplatMat.SetTexture("_MainTex", mSplat0TexRes.getTexture());
            }
        }

        public void onSplat1TexResLoaded(IDispatchObject dispObj)
        {
            mSplat1TexRes = dispObj as AuxTextureLoader;
            if (mSplatMat.HasProperty("_Splat1"))
            {
                mSplatMat.SetTexture("_Splat1", mSplat1TexRes.getTexture());
            }
        }

        public void onSplat2TexResLoaded(IDispatchObject dispObj)
        {
            mSplat2TexRes = dispObj as AuxTextureLoader;
            if (mSplatMat.HasProperty("_Splat2"))
            {
                mSplatMat.SetTexture("_Splat2", mSplat2TexRes.getTexture());
            }
        }

        public void onSplat3TexResLoaded(IDispatchObject dispObj)
        {
            mSplat3TexRes = dispObj as AuxTextureLoader;
            if (mSplatMat.HasProperty("_Splat3"))
            {
                mSplatMat.SetTexture("_Splat3", mSplat3TexRes.getTexture());
            }
        }

        public void onControlTexResLoaded(IDispatchObject dispObj)
        {
            mControlTexRes = dispObj as AuxTextureLoader;
            if (mSplatMat.HasProperty("_Control"))
            {
                mSplatMat.SetTexture("_Control", mControlTexRes.getTexture());
            }
        }
    }
}