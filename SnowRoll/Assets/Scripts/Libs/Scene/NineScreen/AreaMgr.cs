using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 九屏管理器
     */
    public class AreaMgr
    {
        protected int mOneAreaWidth;
        protected int mOneAreaDepth;
        protected int mWorldWidth;
        protected int mWorldDepth;
        protected int mWidthSize;
        protected int mDepthSize;

        protected int mCurAreaIdx;        // 当前 Area 索引
        protected int mPreAreaIdx;        // 之前 Area 索引
        protected MList<int>[] mAreaIdList; // Area id 索引
        protected MList<int> mRemovedAreaIdList;
        protected Dictionary<int, Area> mId2AreaDic; // Area id 到 Area 索引

        public AreaMgr()
        {
            init();
        }

        public void init()
        {
            mOneAreaWidth = 30;
            mOneAreaDepth = 30;
            mWorldWidth = 30000;
            mWorldDepth = 30000;
            mWidthSize = mWorldWidth / mOneAreaWidth;
            mDepthSize = mWorldDepth / mOneAreaDepth;

            mCurAreaIdx = 0;
            mPreAreaIdx = 1;
            mAreaIdList = new MList<int>[2];
            mAreaIdList[mCurAreaIdx] = new MList<int>();
            mAreaIdList[mPreAreaIdx] = new MList<int>();
            mRemovedAreaIdList = new MList<int>();
            mId2AreaDic = new Dictionary<int, Area>();
        }

        public void addEntity(SceneEntityBase entity)
        {
            int areaid = getEntityAreaId(entity);
            if(mAreaIdList[mCurAreaIdx].IndexOf(areaid) != -1)
            {
                if(!mId2AreaDic.ContainsKey(areaid))
                {
                    mId2AreaDic[areaid] = new Area();
                }

                mId2AreaDic[areaid].addEntity(entity);
            }
        }

        public int getEntityAreaId(SceneEntityBase entity)
        {
            int ret = 0;
            ret = UtilMath.floorToInt(entity.getWorldPosY() / mOneAreaDepth) * mWidthSize + UtilMath.floorToInt(entity.getWorldPosX() / mOneAreaWidth);
            return ret;
        }

        public bool isEntityInCurAreaList(SceneEntityBase entity)
        {
            return isPosInCurAreaList(entity.getWorldPos());
        }

        public bool isPosInCurAreaList(MVector3 pos)
        {
            int x = UtilMath.floorToInt(pos.x / mOneAreaWidth);
            int y = UtilMath.floorToInt(pos.y / mOneAreaDepth);
            int areaId = y * mWidthSize + x;
            if(mAreaIdList[mCurAreaIdx].IndexOf(areaId) == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void flipAreaId()
        {
            mPreAreaIdx = mCurAreaIdx;
            mCurAreaIdx = (mCurAreaIdx + 1) % 2;
            mAreaIdList[mCurAreaIdx].Clear();
        }

        public void updateCenter(MVector3 centerPt)
        {
            flipAreaId();
            int x = UtilMath.floorToInt(centerPt.x / mOneAreaWidth);
            int y = UtilMath.floorToInt(centerPt.y / mOneAreaDepth);
            
            int areaId = 0;
            // 左下
            if(y - 1 >= 0 && x - 1 >= 0)
            {
                areaId = (y - 1) * mWidthSize + (x - 1);
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 中下
            if (y - 1 >= 0 && x >= 0)
            {
                areaId = (y - 1) * mWidthSize + x;
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 右下
            if (y - 1 >= 0 && x + 1 < mWidthSize)
            {
                areaId = (y - 1) * mWidthSize + (x + 1);
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 左中
            if (y >= 0 && x - 1 >= 0)
            {
                areaId = y * mWidthSize + (x - 1);
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 中心
            areaId = y * mWidthSize + x;
            mAreaIdList[mCurAreaIdx].Add(areaId);
            // 右中
            if (y >= 0 && x + 1 < mWidthSize)
            {
                areaId = y * mWidthSize + (x + 1);
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 左上
            if (y + 1 < mDepthSize && x - 1 >= 0)
            {
                areaId = (y + 1) * mWidthSize + (x - 1);
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 中上
            if (y + 1 < mDepthSize && x >= 0)
            {
                areaId = (y + 1) * mWidthSize + x;
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }
            // 右上
            if (y + 1 < mDepthSize && x + 1 < mWidthSize)
            {
                areaId = (y + 1) * mWidthSize + (x + 1);
                mAreaIdList[mCurAreaIdx].Add(areaId);
            }

            int idx = 0;
            int len = mAreaIdList[mPreAreaIdx].Count();
            while(idx < len)
            {
                if(mAreaIdList[mCurAreaIdx].IndexOf(mAreaIdList[mPreAreaIdx][idx]) == -1)
                {
                    if (mId2AreaDic[mAreaIdList[mPreAreaIdx][idx]] != null)
                    {
                        mId2AreaDic[mAreaIdList[mPreAreaIdx][idx]].clearArea();
                    }
                }
                ++idx;
            }
        }

        public MList<int> getRemovedAreaIdList()
        {
            mRemovedAreaIdList.Clear();
            int idx = 0;
            int len = mAreaIdList[mPreAreaIdx].Count();
            while (idx < len)
            {
                if (mAreaIdList[mCurAreaIdx].IndexOf(mAreaIdList[mPreAreaIdx][idx]) == -1)
                {
                    mRemovedAreaIdList.Add(mAreaIdList[mPreAreaIdx][idx]);
                }
                ++idx;
            }

            return mRemovedAreaIdList;
        }
    }
}