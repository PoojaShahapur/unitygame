using System;
namespace SDK.Common
{
    public interface ISceneSys
    {
        void loadScene(string filename, Action<IScene> func);
    }
}