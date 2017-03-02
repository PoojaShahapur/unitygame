namespace SDK.Lib
{
    /**
     * @brief 日志设备
     */
    public abstract class LogDeviceBase
    {
        public virtual void initDevice()
        {
    
        }

        public virtual void closeDevice()
        {

        }

        abstract public void logout(string message, LogColor type = LogColor.eLC_LOG);
    }
}