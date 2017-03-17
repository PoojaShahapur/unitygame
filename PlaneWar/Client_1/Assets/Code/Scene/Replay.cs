using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Giant
{
    public class Replay 
    {
        private class Header
        {
            string version; //录制版本
            string time;    //录制时间
            int nFrame;     //帧数
            int nCmd;       //指令总数
            int crc;        //数据部分crc
        }

        private class Player
        {
            int id;    //id
            int name;  //名字
            int score; //分数
            int kill;  //击杀数
        }

        private class Result //战斗结果
        {
            public List<Player> rank; //排名
        }

        public CommandScene scene { set; get; }

        private Header header;
        private Result result;
        private List<List<Command>> data;

        public void Play()
        {
        }

        public void Record()
        {
        }

        public void Pause()
        {
        }

        public void Stop()
        {
        }

        public void Save(Stream stream)
        {
        }

    }
}
