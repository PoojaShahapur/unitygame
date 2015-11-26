using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 高度图数据
     */
    public class HeightMapData
    {
        protected TextureRes m_texRes;          // 原始的资源
        protected Texture2D m_heightMap;        // 高度图

        public HeightMapData(TextureRes tex = null)
        {
            m_texRes = null;
            m_heightMap = null;
            setHeightMapData(tex);
        }

        public HeightMapData(int width_, int height_)
        {
            m_texRes = null;
            m_heightMap = new Texture2D(width_, height_, TextureFormat.RGB24, false);
        }

        public void dispose()
        {

        }

        public void createHeightMap(int width_, int height_)
        {
            m_heightMap = new Texture2D(width_, height_, TextureFormat.RGB24, false);
        }

        /**
         * @brief 设置灰度图纹理资源
         */
        public void setHeightMapData(TextureRes tex)
        {
            if(m_texRes != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_texRes.GetPath(), null);
            }
            if (tex != null)
            {
                m_texRes = tex;
                m_heightMap = tex.getTexture() as Texture2D;
            }
            else
            {
                m_heightMap = null;
            }
        }

        /**
         * @brief 获取图片的宽度值
         */
        public int getWidth()
        {
            return m_heightMap.width;
        }

        /**
         * @获取图片的高度值
         */
        public int getHeight()
        {
            return m_heightMap.height;
        }

        /**
         * @brief 加载高度图
         */
        public void loadHeightMap(string path_, bool async = false)
        {
            TextureRes tex = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(path_);
            setHeightMapData(tex);
        }

        public void loadRawTextureData(byte[] data)
        {
            m_heightMap.LoadRawTextureData(data);
        }

        /**
         * @brief 获取灰度值
         */
        public float getPixHeight(int x, int z)
        {
            return m_heightMap.GetPixel(x, z).grayscale;
        }

        public int getPixel(int x, int z)
        {
            return (int)m_heightMap.GetPixel(x, z).grayscale;
        }

        public void setPixel(int x, int y, uint color)
        {

        }

        public void lockMem()
        {

        }

        public void unlock()
        {

        }
    }
}