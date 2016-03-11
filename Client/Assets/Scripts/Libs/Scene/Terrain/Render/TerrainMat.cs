using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 地形是用的材质
     */
    public class TerrainMat
    {
        protected MatRes m_matRes;                      // 材质资源
        protected TextureRes m_diffuseTexRes;           // 漫反射纹理资源
        protected TextureRes m_hightTexRes;           // 高度纹理资源
        protected TextureRes m_normalTexRes;           // 法向量纹理资源

        protected Material m_material;         // 使用的共享材质
        protected Texture m_diffuseTex;           // 漫反射纹理
        protected Texture m_heightTex;           // 高度纹理
        protected Texture m_normalTex;           // 法向量纹理
        protected Shader m_shader;             // 动态材质使用的纹理

        protected string m_matPreStr;           // 材质前缀字符
        protected string m_shaderName;          // shader 的名字
        protected string m_diffuseTexName;             // 漫反射纹理名字
        protected string m_heightTexName;             // 高度纹理名字
        protected string m_normalTexName;             // 法向量纹理名字

        protected TerrainMat()
        {
            m_matPreStr = "Dyn_";
            //m_shaderName = "Mobile/Diffuse";
            m_shaderName = "Legacy Shaders/Parallax Specular";
            m_diffuseTexName = "Texture/Terrain/terrain_diffuse.jpg";
            m_heightTexName = "Texture/Terrain/terrain.png";
            m_diffuseTexName = "Texture/Terrain/terrain_normal.jpg";
        }

        public void loadMat()
        {

        }
    }
}