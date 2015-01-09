using Game.UI;
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
        void setclass(EnPlayerCareer c);
        void setClassname(string n);
        void setclasspic(Material pic);
        void setCardGroup(moshicardset value);

        EnPlayerCareer getClass();
    }
}