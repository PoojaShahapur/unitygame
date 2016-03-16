using UnityEngine;

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

        protected string m_matPreStr;               // 材质前缀字符
        protected string m_difffuseMatName;         // 材质的名字
        protected string m_specularMatName;         // 材质的名字
        protected string m_diffuseShaderName;              // shader 的名字
        protected string m_specularShaderName;              // shader 的名字
        protected string m_diffuseTexName;          // 漫反射纹理名字
        protected string m_heightTexName;           // 高度纹理名字
        protected string m_normalTexName;           // 法向量纹理名字

        public TerrainMat()
        {
            m_difffuseMatName = "Materials/Terrain/TerrainDiffuse";
            m_specularMatName = "Materials/Terrain/TerrainBumpSpecular";
            m_matPreStr = "Dyn_";
            m_diffuseShaderName = "Mobile/Diffuse";
            m_specularShaderName = "Mobile/Bumped Specular (1 Directional Light)";
            m_diffuseTexName = "Materials/Textures/Terrain/TerrainDiffuse_1.jpg";
            m_heightTexName = "Materials/Textures/Terrain/terrain.png";
            m_normalTexName = "Materials/Textures/Terrain/terrain_normal.jpg";
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

            m_diffuseMat.SetTexture("_MainTex", m_diffuseTex);
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
            m_specularMat.SetTexture("_MainTex", m_diffuseTex);

            m_normalTexRes = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(m_normalTexName);
            m_normalTex = m_normalTexRes.getTexture();
            m_specularMat.SetTexture("_BumpMap", m_normalTex);
        }

        public Material getDiffuseMaterial()
        {
            return m_diffuseMat;
        }

        public Material getSpecularMaterial()
        {
            return m_specularMat;
        }
    }
}