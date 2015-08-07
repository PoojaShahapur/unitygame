using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EditorTool
{
    public class ExportAnimatorControllerUtil
    {
        // 暂时就支持一层，只支持一个参数
        public static AnimatorController BuildAnimationController(XmlAnimatorController controllerData)
        {
            // 创建动画控制器
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerData.controllerFullPath);
            controllerData.animatorController = animatorController;

            // 设置控制器参数
            int paramsIdx = 0;
            foreach (var param in controllerData.getParams.paramList)
            {
                animatorController.AddParameter(param.name, param.type);
                param.animatorControllerParameter = animatorController.parameters[paramsIdx];

                ++paramsIdx;
            }
            //AnimatorControllerParameter[] parameters = animatorController.parameters;
            //controllerData.getParams.parameters = parameters;

            // 创建层，默认会创建一个 Layer，名字是 BaseLayer，其它的 Layer 需要自己创建
            AnimatorControllerLayer layer = null;
            int layerIdx = 0;
            for(; layerIdx < controllerData.layers.layerList.Count; ++layerIdx)
            {
                // 获取当前层
                if (layerIdx < animatorController.layers.Length)        // 如果 Layer 已经有了，第一层默认创建的，不用自己创建
                {
                    layer = animatorController.layers[layerIdx];
                }
                else        // 除第一层外，需要自己创建，创建层的时候，会默认创建当前层的主状态机
                {
                    animatorController.AddLayer(controllerData.layers.layerList[layerIdx].name);
                    layer = animatorController.layers[layerIdx];
                }

                layer.name = controllerData.layers.layerList[layerIdx].name;        // 设置名字

                controllerData.layers.layerList[layerIdx].animatorControllerLayer = layer;
                BuildAnimationStateLayer(controllerData.layers.layerList[layerIdx]);
                //BuildStateMachineTransition(controllerData.layers.layerList[layerIdx]);
            }

            AssetDatabase.SaveAssets();
            return animatorController;
        }

        static public void BuildAnimationStateLayer(XmlLayer xmlLayer)
        {
            int stateMachineIdx = 0;
            AnimatorStateMachine stateMachine = null;

            for(stateMachineIdx = 0; stateMachineIdx < xmlLayer.stateMachineList.Count; ++stateMachineIdx)
            {
                // 获取当前状态机
                if (0 == stateMachineIdx)        // 系统默认创建一个主动画状态机，自状态机需要自己创建
                {
                    stateMachine = xmlLayer.animatorControllerLayer.stateMachine;
                    xmlLayer.stateMachineList[stateMachineIdx].animatorStateMachine = stateMachine;
                    stateMachine.name = xmlLayer.stateMachineList[stateMachineIdx].name;
                }
                else    // 自状态机需要从主状态机创建
                {
                    xmlLayer.stateMachineList[stateMachineIdx].animatorStateMachine = xmlLayer.stateMachineList[0].animatorStateMachine.AddStateMachine(xmlLayer.stateMachineList[stateMachineIdx].name, xmlLayer.stateMachineList[stateMachineIdx].pos);
                }

                BuildAnimationStateMachine(xmlLayer.stateMachineList[stateMachineIdx]);
                BuildStateTransition(xmlLayer.stateMachineList[stateMachineIdx]);
            }
        }

        static public void BuildAnimationStateMachine(XmlStateMachine xmlStateMachine)
        {
            // 创建 Default State ，第一个创建的状态默认就是 Default State
            //AnimatorState animatorState = null;
            //AnimatorStateTransition trans = null;

            //animatorState = xmlStateMachine.animatorStateMachine.AddState("Start");
            //animatorState.writeDefaultValues = true;

            // 添加一个默认的状态，当没有所有的状态的时候，可以循环播放这个状态
            //animatorState = xmlStateMachine.animatorStateMachine.AddState("Idle");          // 待机状态
            //animatorState.writeDefaultValues = false;
            //trans = xmlStateMachine.animatorStateMachine.AddAnyStateTransition(animatorState);
            //trans.hasExitTime = true;
            //trans.exitTime = 0;
            //trans.duration = 0;
            //trans.canTransitionToSelf = true;           // 可以循环自己
            //trans.AddCondition(AnimatorConditionMode.Equals, 0, "StateId");

            // 创建没有动画资源的状态
            foreach (XmlState xmlState in xmlStateMachine.noResStateList)
            {
                BuildNoResState(xmlState);
            }

            // 创建有动画资源的状态
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

            foreach (XmlCondition xmlCond in xmlState.anyCondList)
            {
                trans.AddCondition(xmlCond.opMode, xmlCond.getFloatValue(), xmlCond.name);
            }
        }

        static public void BuildNoResState(XmlState xmlState)
        {
            // 添加一个默认的状态，当没有所有的状态的时候，可以循环播放这个状态
            xmlState.animatorState = xmlState.stateMachine.animatorStateMachine.AddState(xmlState.motion);
            BuildState(xmlState);
        }

        // 这个是添加状态机内部状态的转换
        static public void BuildStateTransition(XmlStateMachine xmlStateMachine)
        {
            XmlState srcXmlState = null;
            XmlState destXmlState = null;

            foreach(var tran in xmlStateMachine.tranList)
            {
                srcXmlState = xmlStateMachine.getXmlStateByName(tran.srcStateName);
                destXmlState = xmlStateMachine.getXmlStateByName(tran.destStateName);
                //tran.animatorTransition = xmlStateMachine.animatorStateMachine.AddStateMachineTransition(xmlStateMachine.animatorStateMachine, destXmlState.animatorState);
                tran.animatorStateTransition = srcXmlState.animatorState.AddTransition(destXmlState.animatorState);

                foreach (var xmlCond in tran.condList)
                {
                    tran.animatorStateTransition.AddCondition(xmlCond.opMode, xmlCond.getFloatValue(), xmlCond.name);
                }
            }
        }

        // 添加状态机之间的转换
        static public void BuildStateMachineTransition(XmlLayer xmlLayer)
        {
            XmlStateMachine xmlSrcStateMachine = null;
            XmlStateMachine xmlDestStateMachine = null;
            XmlState xmlDestState = null;

            foreach (var stateMachineTransition in xmlLayer.xmlStateMachineTransitionList)
            {
                xmlSrcStateMachine = xmlLayer.getXmlStateMachineByName(stateMachineTransition.srcStateMachineName);
                xmlDestStateMachine = xmlLayer.getXmlStateMachineByName(stateMachineTransition.destStateMachineName);
                xmlDestState = xmlDestStateMachine.getXmlStateByName(stateMachineTransition.destStateName);

                stateMachineTransition.animatorTransition = xmlSrcStateMachine.animatorStateMachine.AddStateMachineTransition(xmlDestStateMachine.animatorStateMachine, xmlDestState.animatorState);
                //stateMachineTransition.animatorTransition = xmlDestStateMachine.animatorStateMachine.AddStateMachineTransition(xmlSrcStateMachine.animatorStateMachine, xmlDestState.animatorState);
            }
        }

        // 单独生成动画控制器
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