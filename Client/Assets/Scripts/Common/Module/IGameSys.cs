using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public interface IGameSys
    {
        void Start();
        void loadGameScene();
        void loadDZScene(uint sceneNumber);
    }
}