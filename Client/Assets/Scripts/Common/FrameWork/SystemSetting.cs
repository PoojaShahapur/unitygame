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
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }

            return default(string);
        }
    }
}