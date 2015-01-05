using SDK.Lib;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 场景商店
     */
    public interface IUISceneMoShi : ISceneForm
    {
        void AddNewTaoPai();
        void setclass(CardClass c);
        void setClassname(string n);
        void setclasspic(Material pic);
    }
}