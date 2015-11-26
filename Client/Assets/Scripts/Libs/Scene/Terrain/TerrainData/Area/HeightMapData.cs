﻿using UnityEngine;

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
         * @brief 获取灰度值，灰度图已经是灰度缩放的值，如果再取 grayscale ，就是缩放了两次了
         */
        public float getPixHeight(int x, int z)
        {
            return getColorGrayScaleValue(x, z);
        }

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
            return (int)(color.r * 255);                      // 灰度图中的 Color 值是 [0, 1] 的灰度值，需要缩放到 [0, 255]
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