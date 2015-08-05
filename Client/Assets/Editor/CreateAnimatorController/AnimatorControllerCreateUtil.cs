using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EditorTool
{
    public class AnimatorControllerCreateUtil
    {
        // 暂时就支持一层，只支持一个参数
        public static AnimatorController BuildAnimationController(XmlAnimatorController controllerData)
        {
            // 创建动画控制器
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerData.controllerFullPath);
            controllerData.animatorController = animatorController;

            // 设置控制器参数
            foreach (var param in controllerData.getParams.paramList)
            {
                animatorController.AddParameter(controllerData.getParams.paramList[0].name, AnimatorControllerParameterType.Int);
            }
            AnimatorControllerParameter[] parameters = animatorController.parameters;
            controllerData.getParams.parameters = parameters;

            int layerIdx = 0;
            for(; layerIdx < controllerData.layers.layerList.Count; ++layerIdx)
            {
                // 获取当前层
                AnimatorControllerLayer layer = animatorController.layers[layerIdx];
                controllerData.layers.layerList[layerIdx].animatorControllerLayer = layer;
                BuildAnimationStateLayer(controllerData.layers.layerList[layerIdx]);
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

        static public void BuildAnimationStateLayer(XmlLayer xmlLayer)
        {
            int stateMachineIdx = 0;

            for(stateMachineIdx = 0; stateMachineIdx < xmlLayer.stateMachineList.Count; ++stateMachineIdx)
            {
                // 获取当前状态机
                AnimatorStateMachine stateMachine = xmlLayer.animatorControllerLayer.stateMachine;
                xmlLayer.stateMachineList[stateMachineIdx].animatorStateMachine = stateMachine;

                BuildAnimationStateMachine(xmlLayer.stateMachineList[stateMachineIdx]);
                BuildTransition(xmlLayer.stateMachineList[stateMachineIdx]);
            }
        }

        static public void BuildAnimationStateMachine(XmlStateMachine xmlStateMachine)
        {
            // 创建 Default State ，第一个创建的状态默认就是 Default State
            AnimatorState animatorState = null;
            AnimatorStateTransition trans = null;

            animatorState = xmlStateMachine.animatorStateMachine.AddState("Start");
            animatorState.writeDefaultValues = true;

            // 添加一个默认的状态，当没有所有的状态的时候，可以循环播放这个状态
            animatorState = xmlStateMachine.animatorStateMachine.AddState("Idle");          // 待机状态
            animatorState.writeDefaultValues = false;
            trans = xmlStateMachine.animatorStateMachine.AddAnyStateTransition(animatorState);
            trans.hasExitTime = true;
            trans.exitTime = 0;
            trans.duration = 0;
            trans.canTransitionToSelf = true;           // 可以循环自己
            trans.AddCondition(AnimatorConditionMode.Equals, 0, "StateId");

            foreach (XmlClip xmlClip in xmlStateMachine.clipList)
            {
                BuildAnimationClip(xmlClip);
            }
        }

        static public void BuildAnimationClip(XmlClip xmlClip)
        {
            AnimationClip clip = null;
            int idx = 0;
            UnityEngine.Object[] assetArr = null;
            XmlState xmlState = null;
            AnimatorState animatorState;

            //AnimationClip clip = AssetDatabase.LoadAssetAtPath(state.fullMotion, typeof(AnimationClip)) as AnimationClip;
            clip = null;
            idx = 0;
            string preFixStr = "__preview__";

            assetArr = AssetDatabase.LoadAllAssetsAtPath(xmlClip.fullMotion);   // 一个 anim 就一个动画，但是一个 fbx 可能有多个动画，因此同一使用这个加载资源，否则只能加载第一个。在 fbx 中还会有一个奇怪的 Clip ，名字类似 "__preview___72_a_U1_M_P_PlantNTurn90_Run_R_Fb_p90_No_0_PJ_4"，这个也要排除，排除 Clip 前缀是 "__preview__" 的 Clip 就行了
            for (idx = 0; idx < assetArr.Length; ++idx)
            {
                clip = assetArr[idx] as AnimationClip;
                if (clip != null)
                {
                    if (!clip.name.Contains(preFixStr))
                    {
                        xmlState = xmlClip.getXmlStateByName(clip.name);
                        animatorState = xmlClip.stateMachine.animatorStateMachine.AddState(clip.name);
                        xmlState.animatorState = animatorState;
                        animatorState.motion = clip;
                        BuildState(xmlState);
                    }
                }
            }
        }

        static public void BuildState(XmlState xmlState)
        {
            AnimatorStateTransition trans = null;
            xmlState.animatorState.writeDefaultValues = false;
            trans = xmlState.stateMachine.animatorStateMachine.AddAnyStateTransition(xmlState.animatorState);
            trans.hasExitTime = true;
            trans.exitTime = 0;
            trans.duration = 0;
            trans.canTransitionToSelf = false;

            foreach (XmlCondition xmlCond in xmlState.condList)
            {
                trans.AddCondition(xmlCond.opMode, xmlCond.getFloatValue(), xmlCond.name);
            }
        }

        static public void BuildTransition(XmlStateMachine xmlStateMachine)
        {
            XmlState srcXmlState = null;
            XmlState destXmlState = null;

            foreach(var tran in xmlStateMachine.tranList)
            {
                srcXmlState = xmlStateMachine.getXmlStateByName(tran.srcStateName);
                destXmlState = xmlStateMachine.getXmlStateByName(tran.destStateName);
                tran.animatorTransition = xmlStateMachine.animatorStateMachine.AddStateMachineTransition(xmlStateMachine.animatorStateMachine, destXmlState.animatorState);

                foreach (var xmlCond in tran.condList)
                {
                    tran.animatorTransition.AddCondition(xmlCond.opMode, xmlCond.getFloatValue(), xmlCond.name);
                }
            }
        }
    }
}