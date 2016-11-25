using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.BEP.Vol._2.Scripts
{        
    public class Logging
    {                                
        //所有要打印的日志,'\n'用来分隔        
        private Encoding utf8 = Encoding.GetEncoding("utf-8");       
        private System.IO.StreamWriter sw;


        public void Init(string path)
        {
            UnityEngine.Debug.Log("file path = " + path);
            if (sw == null)
            {
                sw = new StreamWriter(path, false, utf8);
            }
            
            if(sw == null)
            {
                    UnityEngine.Debug.LogError("fuck ,open log file failed!");
            }        
        }

        ~Logging()
        {
            sw.Flush();
            sw.Close();            
        }

        public void DebugLog(string content)
        {            
            // 将要打印的日志放在byte数组里,下一个时间片打印
            if(sw == null)
            {
                UnityEngine.Debug.Log(content);
            }
            else
            {
                //                 byte[] tmp;
                //                 tmp = utf8.GetBytes(content + "\n");
                sw.Write(content + "\n");
                sw.Flush();
            }
            
        }
    }    
}
