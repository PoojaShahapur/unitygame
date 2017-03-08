namespace SDK.Lib
{
    /**
     * @brief 数字间隔
     */
    public class NumInterval
    {
        protected float mTotalValue;
        protected float mCurValue;

        protected NumIntervalMode mNumIntervalMode;

        public NumInterval()
        {
            this.mTotalValue = 0;
            this.mCurValue = 0;

            this.mNumIntervalMode = NumIntervalMode.eNIM_Inc;
        }

        public void setTotalValue(float value)
        {
            this.mTotalValue = value;
        }

        public void setCurValue(float value)
        {
            this.mCurValue = value;
        }

        public void reset()
        {
            if (NumIntervalMode.eNIM_Inc == this.mNumIntervalMode)
            {
                this.mCurValue = 0;
            }
            else
            {
                this.mCurValue = this.mTotalValue;
            }
        }

        public bool canExec(float delta)
        {
            bool ret = false;

            if (NumIntervalMode.eNIM_Inc == this.mNumIntervalMode)
            {
                this.mCurValue += delta;

                if (this.mCurValue <= this.mTotalValue)
                {
                    ret = true;
                }
            }
            else
            {
                this.mCurValue -= delta;

                if (this.mCurValue >= 0)
                {
                    ret = true;
                }
            }

            return ret;
        }
    }
}