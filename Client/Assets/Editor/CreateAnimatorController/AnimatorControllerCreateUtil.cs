using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace CreateAnimatorController
{
    public class AnimatorControllerCreateUtil
    {
        // 暂时就支持一层，只支持一个参数
        public static AnimatorController BuildAnimationController(XmlAnimatorController controllerData)
        {
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerData.controllerFullPath);
            AnimatorControllerLayer layer = animatorController.layers[0];
            animatorController.AddParameter(controllerData.getParams.paramList[0].name, AnimatorControllerParameterType.Int);
            AnimatorControllerParameter[] parameters = animatorController.parameters;
            AnimatorStateMachine stateMachine = layer.stateMachine;

            // 创建 Default State ，第一个创建的状态默认就是 Default State
            AnimatorState animatorState = null;
            AnimatorStateTransition trans = null;

            animatorState = stateMachine.AddState("Start");
            animatorState.writeDefaultValues = true;

            // 添加一个默认的状态，当没有所有的状态的时候，可以循环播放这个状态
            animatorState = stateMachine.AddState("Idle");          // 待机状态
            animatorState.writeDefaultValues = false;
            trans = stateMachine.AddAnyStateTransition(animatorState);
            trans.hasExitTime = true;
            trans.exitTime = 0;
            trans.duration = 0;
            trans.canTransitionToSelf = true;           // 可以循环自己
            trans.AddCondition(AnimatorConditionMode.Equals, 0, "StateId");

            AnimationClip clip = null;
            int idx = 0;
            UnityEngine.Object[] assetArr = null;

            foreach (State state in controllerData.layers.layerList[0].stateMachineList[0].stateList)
            {
                //AnimationClip clip = AssetDatabase.LoadAssetAtPath(state.fullMotion, typeof(AnimationClip)) as AnimationClip;
                clip = null;
                idx = 0;

                assetArr = AssetDatabase.LoadAllAssetsAtPath(state.fullMotion);   // 一个 anim 就一个动画，但是一个 fbx 可能有多个动画，因此同一使用这个加载资源，否则只能加载第一个。在 fbx 中还会有一个奇怪的 Clip ，名字类似 "__preview___72_a_U1_M_P_PlantNTurn90_Run_R_Fb_p90_No_0_PJ_4"，这个也要排除，排除 Clip 前缀是 "__preview__" 的 Clip 就行了
                for (idx = 0; idx < assetArr.Length; ++idx)
                {
                    clip = assetArr[idx] as AnimationClip;
                    if (clip != null)
                    {
                        animatorState = stateMachine.AddState(clip.name);
                        animatorState.motion = clip;
                        animatorState.writeDefaultValues = false;
                        trans = stateMachine.AddAnyStateTransition(animatorState);
                        trans.hasExitTime = true;
                        trans.exitTime = 0;
                        trans.duration = 0;
                        trans.canTransitionToSelf = false;
                        trans.AddCondition(AnimatorConditionMode.Equals, state.condList[0].getFloatValue(), state.condList[0].name);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            return animatorController;
        }

        public static AnimatorController BuildAnimationController(List<AnimationClip> clips, string path, string name)
        {
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(string.Format("{0}/{1}.controller", path, name));
            AnimatorControllerLayer layer = animatorController.layers[0];
            AnimatorControllerParameter[] parameters = animatorController.parameters;
            AnimatorStateMachine sm = layer.stateMachine;
            foreach (AnimationClip newClip in clips)
            {
                AnimatorState state = sm.AddState(newClip.name);
                state.motion = newClip;
                AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
            }
            AssetDatabase.SaveAssets();
            return animatorController;
        }
    }
}