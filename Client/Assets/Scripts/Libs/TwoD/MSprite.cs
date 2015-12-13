using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 这个是精灵 Sprite
     */
    public class MSprite : AuxComponent
    {
        protected SpriteRenderer m_spriteRender;    // 精灵渲染器

        public int getOrderInLayer()
        {
            return this.m_spriteRender.sortingOrder;
        }

        public void setOrderInLayer(int value)
        {
            this.m_spriteRender.sortingOrder = value;
        }

        public int getSortingLayer()
        {
            return this.m_spriteRender.sortingLayerID;
        }

        public void setSortingLayer(int value)
        {
            this.m_spriteRender.sortingLayerID = value;
        }

        public float getSpriteWidth()
        {
            return this.selfGo.GetComponent<Renderer>().bounds.size.x;
        }

        public float getSpriteHeight()
        {
            return this.selfGo.GetComponent<Renderer>().bounds.size.y;
        }

        public Color getColor()
        {
            return this.m_spriteRender.color;
        }

        public void setColor(Color color)
        {
            this.m_spriteRender.color = color;
        }

        public bool getFlipX()
        {
            return this.m_spriteRender.flipX;
        }

        public void flipX(bool value)
        {
            this.m_spriteRender.flipX = value;
        }

        public bool flipY()
        {
            return this.m_spriteRender.flipY;
        }

        public void flipY(bool value)
        {
            this.m_spriteRender.flipY = value;
        }

        public Sprite getSprite()
        {
            return this.m_spriteRender.sprite;
        }

        public void setSprite(Sprite value)
        {
            this.m_spriteRender.sprite = value;
        }
    }
}