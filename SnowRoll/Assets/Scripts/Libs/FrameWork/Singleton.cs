namespace SDK.Lib
{
    public class Singleton<T> where T : class, IMyDispose, new()
    {
	    protected static T msSingleton;

        public static T getSingletonPtr()
        {
            if (null == msSingleton)
            {
                msSingleton = new T();
                msSingleton.init();
            }

            return msSingleton;
        }

        public static void deleteSingletonPtr()
        {
            if (null != msSingleton)
            {
                msSingleton.dispose();
                msSingleton = null;
            }
        }
    }
}