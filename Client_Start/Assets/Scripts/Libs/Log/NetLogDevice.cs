using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 文件日志
     */
    public class NetLogDevice : LogDeviceBase
    {        
        public void getTest()
        {
            //GET请求
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(GET("http://xuanyusong.com/"));
        }

        public void sendTest()
        {
            //登录请求 POST 把参数写在字典用 通过www类来请求
            Dictionary<string,string> dic = new Dictionary<string, string> ();
            //参数
            dic.Add("action","0");
            dic.Add("usrname","xys");
            dic.Add("psw","123456");

            Ctx.mInstance.mCoroutineMgr.StartCoroutine(POST("http://192.168.1.12/login.php", dic));
        }

        //POST请求
        IEnumerator POST(string url, Dictionary<string,string> post)
        {
            WWWForm form = new WWWForm();
            foreach(KeyValuePair<string,string> post_arg in post)
            {
                form.AddField(post_arg.Key, post_arg.Value);
            }

            WWW www = new WWW(url, form);
            yield return www;

            if (www.error != null)
	        {
	            //POST请求失败
                //Debug.Log("error is :" + www.error);
	        } 
            else
            {
                //POST请求成功
		        //Debug.Log("request ok : " + www.text);
	        }
        }

        //GET请求
        IEnumerator GET(string url)
        {
            WWW www = new WWW (url);
            yield return www;

            if (www.error != null)
	        {
	            //GET请求失败
                //Debug.Log("error is :"+ www.error);
	        } 
            else
            {
                //GET请求成功
		        //Debug.Log("request ok : " + www.text);
	        }
        }

        // 如果想通过HTTP传递二进制流的话 可以使用 下面的方法。
        public void sendBinaryData(string url, string str)
        {
            WWWForm wwwForm = new WWWForm();
            byte[] byteStream = System.Text.Encoding.Default.GetBytes(str);
            wwwForm.AddBinaryData("post", byteStream);
            WWW www = new WWW(url, wwwForm);
        }

        public override void logout(string message, LogColor type = LogColor.LOG)
        {
            //注册请求 POST
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("id", "1000");
            dic.Add("charid", Ctx.mInstance.mDataPlayer.m_dataMain.m_dwUserTempID.ToString());
            dic.Add("name", Ctx.mInstance.mDataPlayer.m_dataMain.m_name);
            dic.Add("type", "1000");
            dic.Add("platform", "1000");
            dic.Add("version", "1000");

            dic.Add("error", message);

            dic.Add("XDEBUG_SESSION_START", "ECLIPSE_DBGP");
            dic.Add("KEY", "142527193505815");

            Ctx.mInstance.mCoroutineMgr.StartCoroutine(POST(string.Format("{0}/{1}", Ctx.mInstance.mCfg.mWebIP, Ctx.mInstance.mCfg.mNetLogPhp), dic));
        }
    }
}