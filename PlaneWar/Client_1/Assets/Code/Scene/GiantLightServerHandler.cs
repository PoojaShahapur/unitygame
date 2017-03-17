using UnityEngine;
using System.Collections.Generic;
using GameBox.Service.GiantLightServer;
using GameBox.Service.ByteStorage;
using GameBox.Framework;

namespace Giant
{
    public class GiantLightServerHandler : IGiantLightClient
    {
        private static readonly uint MaxServiceCount = 100000;
        public System.Action OnDisconnectHandler;
        public System.Action OnConnectHandler;
        class MethodEntry
        {
            public string name;
            public System.Type req;
            public System.Type ret;
            public System.Delegate handler;
        }

        class ServiceEntry
        {
            public string name;
            public List<MethodEntry> methods = new List<MethodEntry>();
        }

        private List<ServiceEntry> services = new List<ServiceEntry>();
        private Dictionary<string, Dictionary<string, MethodEntry>> servicesIndex = new Dictionary<string, Dictionary<string, MethodEntry>>();

        private ServiceEntry _CurServiceEntry;
        private Dictionary<string, MethodEntry> _CurServiceEntryIndex;

        private IGiantLightServer mServer;
        public GiantLightServerHandler(IGiantLightServer server)
        {
            this.mServer = server;
        }

        public void OnConnect()
        {
            if (OnConnectHandler != null)
            {
                OnConnectHandler.Invoke();
                OnConnectHandler = null;
            }
        }

        public void OnDisconnect()
        {
            if (OnDisconnectHandler != null)
                OnDisconnectHandler.Invoke();
            this.mServer = null;
        }

        public void BeginService(string name)
        {
            _CurServiceEntry = new ServiceEntry();
            _CurServiceEntry.name = name;
            services.Add(_CurServiceEntry);

            _CurServiceEntryIndex = new Dictionary<string, MethodEntry>();
            servicesIndex.Add(name, _CurServiceEntryIndex);
        }

        public delegate void ResponseHandler<T>(T obj);
        public delegate V RequestHandler<U,V>(U reqObj);

        private void Register<U, V>(string name, System.Delegate handler)
        {
            MethodEntry m;
            if (_CurServiceEntryIndex.TryGetValue(name, out m))
            {
                m.handler = handler;
            }
            else
            { 
                m = new MethodEntry();
                m.name = name;
                m.req = typeof(U);
                m.ret = typeof(V);
                m.handler = handler;

                _CurServiceEntry.methods.Add(m);
                _CurServiceEntryIndex.Add(name, m);
            }
        }

        public void Register<U, V>(string name, ResponseHandler<V> handler = null)
        {
            this.Register<U, V>(name,(System.Delegate)handler);
        }

        public void Register<U, V>(string name, RequestHandler<U, V> handler)
        {
            this.Register<U, V>(name, (System.Delegate)handler);
        }

        private uint HashID(uint serviceid, uint methodid)
        {
            return serviceid * MaxServiceCount + methodid;
        }

        private MethodEntry Name2Method(string serviceName,string methodName)
        {
            Dictionary<string, MethodEntry> index;
            MethodEntry method;
            if (servicesIndex.TryGetValue(serviceName,out index) && index.TryGetValue(methodName,out method))
            {
                return method;
            }
            return null;
        }

        public void EndService(string serviceName)
        {
          
        }

        public void RequestSend<T>(string serviceName,string methodName,T obj)
        {
            AnyLogger.L("RequestSend: " + serviceName + "->" + methodName);
            if (mServer != null)
            {
                var method = Name2Method(serviceName, methodName);
                if (method != null)
                {
                    mServer.SendRequest(serviceName, methodName, ByteConverter.ProtoBufToBytes(obj));
                }
            }
        }

        public bool PushRequest(uint id, string serviceName, string methodName, byte[] content)
        {
            AnyLogger.L("PushRequest: " + serviceName + "->" + methodName);
            if (mServer != null)
            {
                var method = Name2Method(serviceName, methodName);
                if (method != null && method.handler != null)
                {
                   var reqObj = ByteConverter.BytesToProtoBuf(method.req, content, 0, content.Length);
                   var retObj = method.handler.DynamicInvoke(reqObj);
                   if (reqObj != null)
                   {
                        mServer.SendResponse(id, ByteConverter.ProtoBufToBytes(reqObj));
                   }
                }
            }
            return true;
        }

        public bool PushResponse(string serviceName, string methodName, byte[] content)
        {
            AnyLogger.L("PushResponse: " + serviceName + "->" + methodName);
            var method = Name2Method(serviceName, methodName);
            if (method != null && method.handler != null)
            {
                object package = ByteConverter.BytesToProtoBuf(method.ret, content, 0, content.Length);
                method.handler.DynamicInvoke(package);
                return true;
            }
            return true;
        }
    }

}