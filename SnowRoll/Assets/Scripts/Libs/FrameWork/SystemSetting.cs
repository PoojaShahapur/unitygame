using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 系统设置
     */
    public class SystemSetting
    {
        public const string USERNAME = "username";
        public const string PASSWORD = "password";

        public void setString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string getString(string key)
        {
            if (hasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }

            return default(string);
        }

        public void setInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int getInt(string key)
        {
            if(hasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            return 0;
        }

        public bool hasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SetServerIP()
        {
            KBEngine.KBEngineApp.app.setServerIP();
        }
    }
}