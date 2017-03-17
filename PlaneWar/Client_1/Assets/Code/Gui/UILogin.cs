using UnityEngine;
using UnityEngine.UI;
using rpc;
using plane;
using System.Collections.Generic;
using GameBox.Service.GiantLightServer;
using System.Text.RegularExpressions;
using System.IO;
using SimpleJson;

namespace Giant
{
    public class UILogin : MonoBehaviour
    {
        public InputField userName;
        public InputField passWord;
        public InputField ip;
        public Button btnLogin;
        public Text txtMsg;
        public IGiantLightServer server;
        public IGiantLightProxy proxy;

        private void Start()
        {
            _loadConfig();
        }

        private void OnEnable()
        {
            btnLogin.onClick.AddListener(OnLoginClick);
        }

        private void OnDisable()
        {
            btnLogin.onClick.RemoveListener(OnLoginClick);
        }

        private string configPath
        {
            get { return Application.persistentDataPath + "/config.json"; }
        }

        private void _loadConfig()
        {
            if (File.Exists(configPath))
            {
                using (FileStream fileStream = File.OpenRead(configPath))
                {
                    using (StreamReader s = new StreamReader(fileStream))
                    {
                        var dic = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string,string>>(s.ReadToEnd());
                        if (dic != null && dic.ContainsKey("user") && dic.ContainsKey("pass") && dic.ContainsKey("ip"))
                        {
                            this.userName.text = dic["user"];
                            this.passWord.text = dic["pass"];
                            this.ip.text = dic["ip"];
                        }
                    }
                }
            }
        }

        private void _saveConfig()
        {
            using (FileStream fileStream = File.OpenWrite(configPath))
            {
                using (StreamWriter s = new StreamWriter(fileStream))
                {
                    var dic = new Dictionary<string, string>();
                    dic["user"] = this.userName.text;
                    dic["pass"] = this.passWord.text;
                    dic["ip"] = this.ip.text;
                    s.Write(SimpleJson.SimpleJson.SerializeObject(dic));
                }
            }
        }

        public void ShowMessage(string msg)
        {
            txtMsg.text = msg;
        }
        private void OnLoginClick()
        {
            if (server.Connected)
            {
                this._doLogin();
            }
            else
            {
                _connectedToServer(_doLogin);
            }
        }

        private void _connectedToServer(System.Action onConnected = null)
        {
            var strs = ip.text.Split(':');
            if (strs.Length != 2)
            {
                ShowMessage("请输入正确的服务器地址格式->IP或域名:端口");
                return;
            }
            else
            {
                if (!_validateIPAddress(strs[0]) && !_validateDomainAddress(strs[0]))
                {
                    ShowMessage("请输入正确的IP地址或域名!");
                    return;
                }

                if (!_validatePort(strs[1]))
                {
                    ShowMessage("请输入正确的端口号!");
                    return;
                }
                server.Connect(strs[0], System.Convert.ToInt32(strs[1]), Game.instance.handler);
                Game.instance.handler.OnConnectHandler = onConnected;
            }
        }

        private void _doLogin()
        {
            if (userName.text == "")
            {
                ShowMessage("名字不能为空!");
                return;
            }
            if (passWord.text == "")
            {
                ShowMessage("密码不能为空!");
                return;
            }
            var cmd = new rpc.LoginRequest();
            cmd.account = userName.text + "@Giant";
            cmd.password = userName.text;
            proxy.Call("Login", cmd);
        }

        public void Close()
        {
            GameObject.Destroy(gameObject);
        }

        private bool _validateIPAddress(string ipAddress)
        {
            Regex validipregex = new Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            return (ipAddress != "" && validipregex.IsMatch(ipAddress.Trim())) ? true : false;
        }
        
        private bool _validateDomainAddress(string domainAddress)
        {
            Regex reg = new Regex(@"^[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$");
            return reg.IsMatch(domainAddress);
        }

        private bool _validatePort(string port)
        {
            try
            {
                var uport = System.Convert.ToUInt32(port);
                if (uport > 0 && uport <= 65536)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void OnLogin(string method, LoginResponse resp)
        {
            if (resp.result == LoginResponse.LoginResult.OK)
            {
                _saveConfig();
                Game.instance.handler.RequestSend("plane.Plane", "EnterRoom", new rpc.EmptyMsg());
            }
            else
            {
                switch (resp.result)
                {
                    case LoginResponse.LoginResult.ERR_MULTI_LOGIN:
                        ShowMessage("该账号已经被其他玩家登陆!");
                        break;
                    case LoginResponse.LoginResult.ERR_SERVER_FULL:
                        ShowMessage("服务器满员!");
                        break;
                    case LoginResponse.LoginResult.ERR_VERIFY_FAIL:
                        ShowMessage("密码错误!");
                        break;
                }
            }
        }

        public void OnEnterRoom(EnterRoomResponse resp)
        {
            Game.instance.GotoScene(new FightScene(resp));
        }
    }
}
