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
         * @brief 获取图片的宽度值，所有获取宽度的都使用这个值，测试的时候也好测试
         */
        public int getWidth()
        {
            //return m_heightMap.width;
            // Test
            return 129;
        }

        /**
         * @获取图片的高度值，所有获取高度的都使用这个值，测试的时候也好测试
         */
        public int getHeight()
        {
            //return m_heightMap.height;
            // Test
            return 129;
        }

        /**
         * @brief 加载高度图
         */
        public void loadHeightMap(string path_, bool async = false)
        {
            TextureRes tex = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>(path_);
            setHeightMapData(tex);
        }

        /**
         * @brief 加载 raw 纹理数据
         */
        public void loadRawTextureData(byte[] data)
        {
            m_heightMap.LoadRawTextureData(data);
        }

        /**
         * @brief 获取灰度值，灰度图已经是灰度缩放的值，如果再取 grayscale ，就是缩放了两次了
         */
        public float getPixHeight(int x, int z)
        {
            return getColorGrayScaleValue(x, z);
        }

        /**
         * @brief 获取像素的高度
         * @ret 返回值是一个 int 值，就是 byte 值
         */
        public int getPixel(int x, int z)
        {
            return getColorGrayScaleValue(x, z);
        }

        /**
         * @brief 获取一个像素的灰度值，如果这个图像已经是灰度图了，就会计算两次灰度值，返回值将会是错误的
         */
        public float getGrayScaleValue(int x, int z)
        {
            return m_heightMap.GetPixel(x, z).grayscale;    // grayscale 是 [0, 1] 之间的值
        }

        /**
         * @brief 获取灰度缩放值
         * @ret 返回值是一个 int 类型
         */
        public int getGrayScale(int x, int z)
        {
            return (int)m_heightMap.GetPixel(x, z).grayscale;
        }

        /**
         * @brief 获取颜色高度值
         */
        public int getColorGrayScaleValue(int x, int z)
        {
            Color color = m_heightMap.GetPixel(x, z);       // Color 中的值 r 是 [0, 1] 之间的值
            return (int)(color.r * 0xFF);                   // 灰度图中的 Color 值是 [0, 1] 的灰度值，需要缩放到 [0, 255]
        }

        /**
         * @brief 设置像素值
         * @param color 是一个整数值
         */
        public void setPixel(int x, int y, uint color)
        {
            setPixel(x, y, UtilMath.Int24ToColor((int)color));
        }

        /**
         * @brief 设置像素值
         * @param r， g， b， a 的范围是 [0, 1]
         */
        public void setPixel(int x, int y, float r, float g, float b, float a)
        {
            m_heightMap.SetPixel(x, y, new Color(r, g, b, a));
        }

        /**
         * @brief 设置像素的值， color 中的 r， g， b， a 的范围都是 [0, 1]
         */
        public void setPixel(int x, int y, Color color)
        {
            m_heightMap.SetPixel(x, y, color);
        }

        /**
         * @brief 锁定显存
         */
        public void lockMem()
        {

        }

        /**
         * @brief 释放锁定个显存
         */
        public void unlock()
        {

        }

        /**
         * @brief 日志输出所有的顶点
         */
        public void print()
        {
            int iH = 0;
            float fH = 0;
            string str = "";
            for(int idz = 0; idz < getHeight(); ++idz)
            {
                for (int idx = 0; idx < getWidth(); ++idx)
                {
                    //iH = getColor(idx, idz);
                    fH = getPixHeight(idx, idz);
                    //Debug.Log(fH);
                    str += iH;
                    str += "\n";
                    str += fH;
                    str += "\n";
                }
            }
        }
    }
}