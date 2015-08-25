using SDK.Lib;
using SDK.Lib;
﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.Rendering;
using SDK.Lib;

[InitializeOnLoad]
public static class LuaBinding
{
    //自动生成wrap功能
    //static LuaBinding()
    //{        
    //    string dir = Application.dataPath + "/Source/LuaWrap/";

    //    if (!Directory.Exists(dir))
    //    {
    //        Directory.CreateDirectory(dir);
    //    }
        
    //    string[] files = Directory.GetFiles(dir);

    //    if (files.Length <= 0)
    //    {            
    //        GenLuaDelegates();
    //        Binding();
    //        GenLuaBinder();
    //    }
    //}


    public class BindType
    {
        public string name;                 //类名称
        public Type type;
        public bool IsStatic;
        public string baseName = null;      //继承的类名字
        public string wrapName = "";        //产生的wrap文件名字
        public string libName = "";         //注册到lua的名字

        string GetTypeStr(Type t)
        {
            string str = t.ToString();

            if (t.IsGenericType)
            {
                str = GetGenericName(t);
            }
            else if (str.Contains("+"))
            {
                str = str.Replace('+', '.');
            }

            return str;
        }

        private static string[] GetGenericName(Type[] types)
        {
            string[] results = new string[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsGenericType)
                {
                    results[i] = GetGenericName(types[i]);
                }
                else
                {
                    results[i] = ToLuaExport.GetTypeStr(types[i]);
                }

            }

            return results;
        }

        private static string GetGenericName(Type type)
        {
            if (type.GetGenericArguments().Length == 0)
            {
                return type.Name;
            }

            Type[] gArgs = type.GetGenericArguments();
            string typeName = type.Name;
            string pureTypeName = typeName.Substring(0, typeName.IndexOf('`'));

            return pureTypeName + "<" + string.Join(",", GetGenericName(gArgs)) + ">";
        }

        public BindType(Type t)
        {
            type = t;

            name = ToLuaExport.GetTypeStr(t);

            if (t.IsGenericType)
            {
                libName = ToLuaExport.GetGenericLibName(t);
                wrapName = ToLuaExport.GetGenericLibName(t);
            }
            else
            {
                libName = t.FullName.Replace("+", ".");
                wrapName = name.Replace('.', '_');

                if (name == "object")
                {
                    wrapName = "System_Object";
                }
            }

            if (t.BaseType != null)
            {
                baseName = ToLuaExport.GetTypeStr(t.BaseType);

                if (baseName == "ValueType")
                {
                    baseName = null;
                }
            }

            if (t.GetConstructor(Type.EmptyTypes) == null && t.IsAbstract && t.IsSealed)
            {
                baseName = null;
                IsStatic = true;
            }
        }

        public BindType SetBaseName(string str)
        {
            baseName = str;
            return this;
        }

        public BindType SetWrapName(string str)
        {
            wrapName = str;
            return this;
        }

        public BindType SetLibName(string str)
        {
            libName = str;
            return this;
        }
    }

    static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    static BindType[] binds = new BindType[]
    {
        _GT(typeof(object)),
        _GT(typeof(System.String)),
        _GT(typeof(System.Enum)),   
        _GT(typeof(IEnumerator)),
        _GT(typeof(System.Delegate)),        
        _GT(typeof(Type)).SetBaseName("System.Object"),                                                     
        _GT(typeof(UnityEngine.Object)),
        
        //测试模板
        ////_GT(typeof(Dictionary<int,string>)).SetWrapName("DictInt2Str").SetLibName("DictInt2Str"),
        
        //custom    
		
		_GT(typeof(Util)),
		_GT(typeof(Const)),
        _GT(typeof(LuaEnumType)),
        _GT(typeof(Debugger)),
        
        //unity                        
        _GT(typeof(Component)),
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),        
        _GT(typeof(GameObject)),
        _GT(typeof(Transform)),
        _GT(typeof(Space)),

        _GT(typeof(Camera)),   
        _GT(typeof(CameraClearFlags)),           
        _GT(typeof(Material)),
        _GT(typeof(Renderer)),        
        _GT(typeof(MeshRenderer)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Light)),
        _GT(typeof(LightType)),     
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)),        
                
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        
        _GT(typeof(CharacterController)),

        _GT(typeof(Animation)),             
        _GT(typeof(AnimationClip)).SetBaseName("UnityEngine.Object"),
        _GT(typeof(TrackedReference)),
        _GT(typeof(AnimationState)),  
        _GT(typeof(QueueMode)),  
        _GT(typeof(PlayMode)),                  
        
        _GT(typeof(AudioClip)),
        _GT(typeof(AudioSource)),                
        
        _GT(typeof(Application)),
        _GT(typeof(Input)),    
        _GT(typeof(TouchPhase)),            
        _GT(typeof(KeyCode)),             
        _GT(typeof(Screen)),
        _GT(typeof(Time)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(SleepTimeout)),        

        _GT(typeof(AsyncOperation)).SetBaseName("System.Object"),
        _GT(typeof(AssetBundle)),   
        _GT(typeof(BlendWeights)),   
        _GT(typeof(QualitySettings)),          
        _GT(typeof(AnimationBlendMode)),    
        _GT(typeof(Texture)),
        _GT(typeof(RenderTexture)),
        _GT(typeof(ParticleSystem)),
        

        //ngui
        /*_GT(typeof(UICamera)),
        _GT(typeof(Localization)),
        _GT(typeof(NGUITools)),

        _GT(typeof(UIRect)),
        _GT(typeof(UIWidget)),        
        _GT(typeof(UIWidgetContainer)),     
        _GT(typeof(UILabel)),        
        _GT(typeof(UIToggle)),
        _GT(typeof(UIBasicSprite)),        
        _GT(typeof(UITexture)),
        _GT(typeof(UISprite)),           
        _GT(typeof(UIProgressBar)),
        _GT(typeof(UISlider)),
        _GT(typeof(UIGrid)),
        _GT(typeof(UIInput)),
        _GT(typeof(UIScrollView)),
        
        _GT(typeof(UITweener)),
        _GT(typeof(TweenWidth)),
        _GT(typeof(TweenRotation)),
        _GT(typeof(TweenPosition)),
        _GT(typeof(TweenScale)),
        _GT(typeof(UICenterOnChild)),    
        _GT(typeof(UIAtlas)),*/ 
   
        //_GT(typeof(LeanTween)),
        //_GT(typeof(LTDescr)),

         _GT(typeof(AuxComponent)),
         _GT(typeof(AuxDynModel)),
         _GT(typeof(AuxDynModelDynTex)),
         _GT(typeof(ModelItem)),
         _GT(typeof(AuxResComponent)),
         _GT(typeof(AuxStaticModel)),
         _GT(typeof(AuxStaticModelDynTex)),
         _GT(typeof(SlideList)),
         _GT(typeof(SlideListItem)),
         _GT(typeof(AuxBasicButton)),
         _GT(typeof(AuxDynAtlasImage)),
         _GT(typeof(AuxDynImage)),
         _GT(typeof(AuxDynImageDynGOImage)),
         _GT(typeof(AuxDynImageDynGoButton)),
         _GT(typeof(AuxDynImageStaticGOImage)),
         _GT(typeof(AuxDynImageStaticGoButton)),
         _GT(typeof(AuxDynTexDynGOButton)),
         _GT(typeof(AuxDynTexDynGOImage)),
         _GT(typeof(AuxDynTexImage)),
         _GT(typeof(AuxInputField)),
         _GT(typeof(AuxLabel)),
         _GT(typeof(AuxLayoutBase)),
         _GT(typeof(AuxLayoutH)),
         _GT(typeof(AuxLayoutV)),
         _GT(typeof(AuxScrollbar)),
         _GT(typeof(AuxStaticImage)),
         _GT(typeof(AuxStaticImageStaticGoImage)),
         _GT(typeof(PopupTipsItemBase)),
         _GT(typeof(PopupTipsMgr)),
         _GT(typeof(PopupTipsWordItem)),
         _GT(typeof(ByteBuffer)),
         _GT(typeof(CirculeBuffer)),
         _GT(typeof(ClientBuffer)),
         _GT(typeof(DataCV)),
         _GT(typeof(MsgBuffer)),
         _GT(typeof(FlyNumItem)),
         _GT(typeof(FlyNumMgr)),
         _GT(typeof(NumResItem)),
         _GT(typeof(IpSelect)),
         _GT(typeof(ZoneSelect)),
         _GT(typeof(BasicConfig)),
         _GT(typeof(Config)),
         _GT(typeof(Ctx)),
         _GT(typeof(FactoryBuild)),
         _GT(typeof(GameAttr)),
         _GT(typeof(EGameStage)),
         _GT(typeof(GameRunStage)),
         _GT(typeof(GameStateCV)),
         _GT(typeof(LayerMgr)),
         _GT(typeof(NotDestroyPath)),
         _GT(typeof(RandName)),
         _GT(typeof(ShareData)),
         _GT(typeof(SystemSetting)),
         _GT(typeof(TimerMsgHandle)),
         _GT(typeof(TimerType)),
         _GT(typeof(WordFilterManager)),
         _GT(typeof(EnPlayerCareer)),
         _GT(typeof(EnDZPlayer)),
         _GT(typeof(DZData)),
         _GT(typeof(DZPlayer)),
         _GT(typeof(DataPlayer)),
         _GT(typeof(CardGroupItem)),
         _GT(typeof(CardGroupAttrMatItem)),
         _GT(typeof(CardGroupModelAttrItem)),
         _GT(typeof(CardItemBase)),
         _GT(typeof(CardModelItem)),
         _GT(typeof(DataItemObjectBase)),
         _GT(typeof(DataItemShop)),
         _GT(typeof(HeroItem)),
         _GT(typeof(SceneCardItem)),
         _GT(typeof(DataCard)),
         _GT(typeof(DataHero)),
         _GT(typeof(DataPack)),
         _GT(typeof(DataShop)),
         _GT(typeof(stObjectLocation)),
         _GT(typeof(t_Card)),
         _GT(typeof(t_CardPK)),
         _GT(typeof(t_MagicPoint)),
         _GT(typeof(t_Object_mobile)),
         _GT(typeof(t_Tujian)),
         _GT(typeof(t_group_list)),
         _GT(typeof(t_hero)),
         _GT(typeof(AccountData)),
         _GT(typeof(ChatData)),
         _GT(typeof(t_MainUserData)),
         _GT(typeof(DataFriend)),
         _GT(typeof(XmlCfgID)),
         _GT(typeof(XmlCfgBase)),
         _GT(typeof(XmlItemBase)),
         _GT(typeof(XmlCfgMgr)),
         _GT(typeof(XmlMarketCfg)),
         _GT(typeof(XmlItemMarket)),
         _GT(typeof(IOSceneHandle)),
         _GT(typeof(SceneEntityName)),
         _GT(typeof(PointF)),
         _GT(typeof(RectangleF)),
         _GT(typeof(SizeF)),
         _GT(typeof(LoginState)),
         _GT(typeof(ModulePath)),
         _GT(typeof(ModuleName)),
         _GT(typeof(ModuleID)),
         _GT(typeof(ModuleHandleItem)),
         _GT(typeof(ModuleSys)),
         _GT(typeof(SocketOpenedMR)),
         _GT(typeof(SocketCloseedMR)),
         _GT(typeof(LoadedWebResMR)),
         _GT(typeof(ThreadLogMR)),
         _GT(typeof(MsgRouteType)),
         _GT(typeof(MsgRouteID)),
         _GT(typeof(MsgRouteBase)),
         _GT(typeof(MsgRouteDispHandle)),
         _GT(typeof(MsgRouteDispList)),
         _GT(typeof(MsgRouteHandleBase)),
         _GT(typeof(SysMsgRoute)),
         _GT(typeof(DopeSheetAni)),
         _GT(typeof(ITweenAniBase)),
         _GT(typeof(NumAniParallel)),
         _GT(typeof(NumAniSeqBase)),
         _GT(typeof(NumAniSequence)),
         _GT(typeof(PosAni)),
         _GT(typeof(RSTAni)),
         _GT(typeof(RTAni)),
         _GT(typeof(RotAni)),
         _GT(typeof(STAni)),
         _GT(typeof(ScaleAni)),
         _GT(typeof(SimpleCurveAni)),
         _GT(typeof(PoolSys)),
         _GT(typeof(TableCardAttrName)),
         _GT(typeof(TableCardItemBody)),
         _GT(typeof(TableJobItemBody)),
         _GT(typeof(TableObjectItemBody)),
         _GT(typeof(TableRaceItemBody)),
         _GT(typeof(TableSkillItemBody)),
         _GT(typeof(TableSpriteAniItemBody)),
         _GT(typeof(TableStateItemBody)),
         _GT(typeof(TableBase)),
         _GT(typeof(TableID)),
         _GT(typeof(TableItemBase)),
         _GT(typeof(TableItemBodyBase)),
         _GT(typeof(TableItemHeader)),
         _GT(typeof(TableSys)),
         _GT(typeof(UtilTable)),
         _GT(typeof(CompressionAlgorithm)),
         _GT(typeof(CoordConv)),
         _GT(typeof(CryptAlgorithm)),
         _GT(typeof(Endian)),
         _GT(typeof(SystemEndian)),
         _GT(typeof(EventID)),
         _GT(typeof(GkEncode)),
         _GT(typeof(UtilApi)),
         _GT(typeof(UtilByte)),
         _GT(typeof(UtilLogic)),
         _GT(typeof(UtilMsg)),
         _GT(typeof(UtilXml)),
         _GT(typeof(BtnStyleID)),
         _GT(typeof(ButtonStyleBase)),
         _GT(typeof(LabelStyleBase)),
         _GT(typeof(LabelStyleID)),
         _GT(typeof(WidgetStyle)),
         _GT(typeof(WidgetStyleID)),
         _GT(typeof(WidgetStyleMgr)),
         _GT(typeof(ComponentType)),
         _GT(typeof(DepthMgr)),
         _GT(typeof(Form)),
         _GT(typeof(GOExtraInfo)),
         _GT(typeof(GUIWin)),
         _GT(typeof(LblType)),
         _GT(typeof(UIAttrItem)),
         _GT(typeof(UIAttrs)),
         _GT(typeof(UICanvasID)),
         _GT(typeof(UICanvas)),
         _GT(typeof(UIFormID)),
         _GT(typeof(WinIDCnt)),
         _GT(typeof(UILayerID)),
         _GT(typeof(UILayer)),
         _GT(typeof(UILoadingItem)),
         _GT(typeof(UIMgr)),
         _GT(typeof(UISceneType)),
         _GT(typeof(Window)),
         _GT(typeof(JobSelectMode)),
         _GT(typeof(AuxJobSelectData)),
         _GT(typeof(ETuJianMenu)),
         _GT(typeof(AuxTuJian)),
         _GT(typeof(AuxUIHelp)),
         _GT(typeof(InfoBoxModeType)),
         _GT(typeof(InfoBoxBtnType)),
         _GT(typeof(InfoBoxParam)),
         _GT(typeof(BezierCurve)),
         _GT(typeof(CurveBase)),
         _GT(typeof(NodeAnimation)),
         _GT(typeof(NodeAnimationTrack)),
         _GT(typeof(TweenAniBase)),
         _GT(typeof(Compress)),
         _GT(typeof(CryptContext)),
         _GT(typeof(Crypt)),
         _GT(typeof(CryptKeyBase)),
         _GT(typeof(CryptUtil)),
         _GT(typeof(UnionCls)),
         _GT(typeof(DES_key_schedule)),
         _GT(typeof(RC5_32_KEY)),
         _GT(typeof(RC5)),
         _GT(typeof(DelayAddParam)),
         _GT(typeof(DelayDelParam)),
         _GT(typeof(DelayHandleMgrBase)),
         _GT(typeof(DelayHandleObject)),
         _GT(typeof(DelayHandleParamBase)),
         _GT(typeof(AddOnceAndCallOnceEventDispatch)),
         _GT(typeof(AddOnceEventDispatch)),
         _GT(typeof(CallOnceEventDispatch)),
         _GT(typeof(EventDispatch)),
         _GT(typeof(EventDispatchFunctionObject)),
         _GT(typeof(ResEventDispatchGroup)),
         _GT(typeof(ResEventDispatch)),
         _GT(typeof(DaoJiShiTimer)),
         _GT(typeof(FrameTimerItem)),
         _GT(typeof(FrameTimerMgr)),
         _GT(typeof(ResizeMgr)),
         _GT(typeof(SystemFrameData)),
         _GT(typeof(SystemTimeData)),
         _GT(typeof(TickMgr)),
         _GT(typeof(TickProcessObject)),
         _GT(typeof(TimerItemBase)),
         _GT(typeof(TimerMgr)),
         _GT(typeof(CoroutineComponent)),
         _GT(typeof(CoroutineMgr)),
         _GT(typeof(EngineLoop)),
         _GT(typeof(ProcessSys)),
         _GT(typeof(ScenePlaceHolder)),
         _GT(typeof(InputKey)),
         _GT(typeof(InputMgr)),
         _GT(typeof(LangAttrItem)),
         _GT(typeof(LangID)),
         _GT(typeof(LangTypeId)),
         _GT(typeof(LangItemID)),
         _GT(typeof(LangItem)),
         _GT(typeof(LangMgr)),
         _GT(typeof(LocalFileSys)),
         _GT(typeof(FileLogDevice)),
         _GT(typeof(LogColor)),
         _GT(typeof(LogSys)),
         _GT(typeof(LoggerTool)),
         _GT(typeof(NetLogDevice)),
         _GT(typeof(UtilRes)),
         _GT(typeof(WinLogDevice)),
         _GT(typeof(AppConst)),
         _GT(typeof(LuaCSBridge)),
         _GT(typeof(LuaCSBridgeByteBuffer)),
         _GT(typeof(LuaCSBridgeDispatch)),
         _GT(typeof(LuaCSBridgeForm)),
         _GT(typeof(LuaCSBridgeNetDispHandle)),
         _GT(typeof(LuaCSBridgeUICore)),
         _GT(typeof(TestStaticHandle)),
         _GT(typeof(Util)),
         _GT(typeof(CVMsg)),
         _GT(typeof(ERetResult)),
         _GT(typeof(ChallengeGameType)),
         _GT(typeof(ChallengeState)),
         _GT(typeof(CardArea)),
         _GT(typeof(CardType)),
         _GT(typeof(StateID)),
         _GT(typeof(AttackTarget)),
         _GT(typeof(EDeleteType)),
         _GT(typeof(NetCmdHandleBase)),
         _GT(typeof(NetDispHandle)),
         _GT(typeof(NetDispList)),
         _GT(typeof(stNullUserCmd)),
         _GT(typeof(MWebSocketClient)),
         _GT(typeof(NetTCPClient)),
         _GT(typeof(NetThread)),
         _GT(typeof(NetworkMgr)),
         _GT(typeof(WebSocketMgr)),
         _GT(typeof(AtlasGoRes)),
         _GT(typeof(AtlasMgr)),
         _GT(typeof(AtlasScriptRes)),
         _GT(typeof(CVAtlasName)),
         _GT(typeof(ImageItem)),
         _GT(typeof(AutoUpdateSys)),
         _GT(typeof(BytesRes)),
         _GT(typeof(ControllerRes)),
         _GT(typeof(InsResBase)),
         _GT(typeof(MatRes)),
         _GT(typeof(ModelRes)),
         _GT(typeof(TextRes)),
         _GT(typeof(TextureRes)),
         _GT(typeof(UIPrefabRes)),
         _GT(typeof(BytesResMgr)),
         _GT(typeof(ControllerMgr)),
         _GT(typeof(MaterialID)),
         _GT(typeof(MaterialName)),
         _GT(typeof(MaterialMgr)),
         _GT(typeof(ModelMgr)),
         _GT(typeof(ResMgrBase)),
         _GT(typeof(ShaderMgr)),
         _GT(typeof(SkelAniMgr)),
         _GT(typeof(TextureMgr)),
         _GT(typeof(UIPrefabMgr)),
         _GT(typeof(ArchiveHeader)),
         _GT(typeof(FileHeaderFlag)),
         _GT(typeof(FileHeader)),
         _GT(typeof(MLzma)),
         _GT(typeof(PakItem)),
         _GT(typeof(ResListItem)),
         _GT(typeof(PakSys)),
         _GT(typeof(NonRefCountResLoadResultNotify)),
         _GT(typeof(RefCount)),
         _GT(typeof(RefCountResLoadResultNotify)),
         _GT(typeof(ResLoadResultNotify)),
         _GT(typeof(ABPakLoadItem)),
         _GT(typeof(ABUnPakLoadItem)),
         _GT(typeof(BundleLoadItem)),
         _GT(typeof(DataLoadItem)),
         _GT(typeof(LevelLoadItem)),
         _GT(typeof(LoadItem)),
         _GT(typeof(ResourceLoadItem)),
         _GT(typeof(ResLoadData)),
         _GT(typeof(ResLoadMgr)),
         _GT(typeof(ABMemUnPakComFileResItem)),
         _GT(typeof(ABMemUnPakFileResItemBase)),
         _GT(typeof(ABMemUnPakLevelFileResItem)),
         _GT(typeof(ABPakComFileResItem)),
         _GT(typeof(ABPakFileResItemBase)),
         _GT(typeof(ABPakLevelFileResItem)),
         _GT(typeof(ABUnPakComFileResItem)),
         _GT(typeof(ABUnPakFileResItemBase)),
         _GT(typeof(ABUnPakLevelFileResItem)),
         _GT(typeof(BundleResItem)),
         _GT(typeof(DataResItem)),
         _GT(typeof(FileResItem)),
         _GT(typeof(LevelResItem)),
         _GT(typeof(PrefabResItem)),
         _GT(typeof(ResItem)),
         _GT(typeof(LoadParam)),
         _GT(typeof(CVResLoadState)),
         _GT(typeof(ResLoadState)),
         _GT(typeof(ResLoadType)),
         _GT(typeof(ResPackType)),
         _GT(typeof(ResPathType)),
         _GT(typeof(ResMsgRouteCB)),
         _GT(typeof(ScriptDynLoad)),
         _GT(typeof(ScriptRes)),
         _GT(typeof(SOAnimatorController)),
         _GT(typeof(SOSpriteList)),
         _GT(typeof(FileVerInfo)),
         _GT(typeof(FilesVerType)),
         _GT(typeof(FilesVer)),
         _GT(typeof(VersionSys)),
         _GT(typeof(AILocalState)),
         _GT(typeof(eBeingActId)),
         _GT(typeof(BehaviorState)),
         _GT(typeof(BeingBehaviour)),
         _GT(typeof(BeingEntity)),
         _GT(typeof(BeingState)),
         _GT(typeof(BeingSubState)),
         _GT(typeof(FObjectMgr)),
         _GT(typeof(FallObjectEntity)),
         _GT(typeof(Monster)),
         _GT(typeof(MonsterMgr)),
         _GT(typeof(Npc)),
         _GT(typeof(NpcMgr)),
         _GT(typeof(NpcVisit)),
         _GT(typeof(Player)),
         _GT(typeof(PlayerMain)),
         _GT(typeof(PlayerMgr)),
         _GT(typeof(PlayerOther)),
         _GT(typeof(BoxCam)),
         _GT(typeof(CamEntity)),
         _GT(typeof(CamSys)),
         _GT(typeof(DzCam)),
         _GT(typeof(AnimatorControl)),
         _GT(typeof(CardSubPartType)),
         _GT(typeof(CardSubPart)),
         _GT(typeof(BlackCardRender)),
         _GT(typeof(CanOutCardRender)),
         _GT(typeof(CardRenderBase)),
         _GT(typeof(ChangCardRender)),
         _GT(typeof(EquipRender)),
         _GT(typeof(EquipSkillRenderBase)),
         _GT(typeof(ExceptBlackCardRender)),
         _GT(typeof(HeroRender)),
         _GT(typeof(SelfHandCardRender)),
         _GT(typeof(SkillRender)),
         _GT(typeof(TuJianCardRender)),
         _GT(typeof(WhiteCardRender)),
         _GT(typeof(MapCfg)),
         _GT(typeof(MapXml)),
         _GT(typeof(DZDaoJiShiXmlLimit)),
         _GT(typeof(MapXmlItem)),
         _GT(typeof(BeingBehaviorControl)),
         _GT(typeof(BeingControlBase)),
         _GT(typeof(ControlBase)),
         _GT(typeof(EffectControl)),
         _GT(typeof(MoveControl)),
         _GT(typeof(MyNcParticleSystem)),
         _GT(typeof(EffectBase)),
         _GT(typeof(EffectMoveControl)),
         _GT(typeof(EffectRenderBase)),
         _GT(typeof(EffectSpriteRender)),
         _GT(typeof(EffectType)),
         _GT(typeof(EffectRenderType)),
         _GT(typeof(FxEffectRender)),
         _GT(typeof(LinkEffect)),
         _GT(typeof(MoveEffect)),
         _GT(typeof(SceneEffect)),
         _GT(typeof(SceneEffectMgr)),
         _GT(typeof(ShurikenEffectRender)),
         _GT(typeof(SpriteEffectRender)),
         _GT(typeof(ShurikenParticleSystem)),
         _GT(typeof(ImageSpriteAni)),
         _GT(typeof(SpritePlayState)),
         _GT(typeof(SpriteAni)),
         _GT(typeof(SpriteAniMgr)),
         _GT(typeof(SpriteRenderSpriteAni)),
         _GT(typeof(AttackSeqData)),
         _GT(typeof(AttackSeqItem)),
         _GT(typeof(FightSeqData)),
         _GT(typeof(HurtSeqData)),
         _GT(typeof(HurtSeqItem)),
         _GT(typeof(OneAttackFlowSeq)),
         _GT(typeof(OneHurtFlowSeq)),
         _GT(typeof(SkillAttackFlowMgr)),
         _GT(typeof(EImmeAttackType)),
         _GT(typeof(EImmeAttackRangeType)),
         _GT(typeof(EImmeHurtType)),
         _GT(typeof(EImmeHurtItemState)),
         _GT(typeof(EImmeHurtExecState)),
         _GT(typeof(ImmeAttackData)),
         _GT(typeof(ImmeAttackItemBase)),
         _GT(typeof(ImmeComAttackItem)),
         _GT(typeof(ImmeComHurtItem)),
         _GT(typeof(ImmeDieItem)),
         _GT(typeof(ImmeFightData)),
         _GT(typeof(ImmeFightItemBase)),
         _GT(typeof(ImmeFightListBase)),
         _GT(typeof(ImmeHurtData)),
         _GT(typeof(ImmeHurtItemBase)),
         _GT(typeof(ImmeSkillAttackItem)),
         _GT(typeof(ImmeSkillHurtItem)),
         _GT(typeof(ImmeFightMsgMgr)),
         _GT(typeof(AnimControl)),
         _GT(typeof(eEquipSlotType)),
         _GT(typeof(ePlayerModelType)),
         _GT(typeof(eNpcModelType)),
         _GT(typeof(eMonstersModelType)),
         _GT(typeof(SkinSubModel)),
         _GT(typeof(SkinModelSkelAnim)),
         _GT(typeof(UtilSkin)),
         _GT(typeof(AuxSceneComponent)),
         _GT(typeof(EntityMgrBase)),
         _GT(typeof(EntityRenderBase)),
         _GT(typeof(SceneEntity)),
         _GT(typeof(SceneEntityBase)),
         _GT(typeof(GridElementType)),
         _GT(typeof(GridElementState)),
         _GT(typeof(GridElementBase)),
         _GT(typeof(ScaleGridElement)),
         _GT(typeof(AttackActionItem)),
         _GT(typeof(AttackActionNode)),
         _GT(typeof(AttackActionSeq)),
         _GT(typeof(AttackEffectList)),
         _GT(typeof(AttackEffectNode)),
         _GT(typeof(FightActionNodeBase)),
         _GT(typeof(FightEffectNodeBase)),
         _GT(typeof(HurtActionNode)),
         _GT(typeof(HurtEffectList)),
         _GT(typeof(HurtEffectNode)),
         _GT(typeof(SkillActionMgr)),
         _GT(typeof(SkillActionRes)),
         _GT(typeof(Area)),
         _GT(typeof(Scene)),
         _GT(typeof(SceneCfg)),
         _GT(typeof(SceneNodeCfg)),
         _GT(typeof(SceneParse)),
         _GT(typeof(TerrainCfg)),
         _GT(typeof(SceneSys)),
         _GT(typeof(DangerZone)),
         _GT(typeof(ZoneSys)),
         _GT(typeof(UniqueId)),
         _GT(typeof(SoundClipItem)),
         _GT(typeof(SoundPlayState)),
         _GT(typeof(SoundResType)),
         _GT(typeof(SoundItem)),
         _GT(typeof(SoundMgr)),
         _GT(typeof(SoundParam)),
         _GT(typeof(SoundPrefabItem)),
         _GT(typeof(TaskQueue)),
         _GT(typeof(TaskThread)),
         _GT(typeof(TaskThreadPool)),
         _GT(typeof(MCondition)),
         _GT(typeof(MEvent)),
         _GT(typeof(MLock)),
         _GT(typeof(MMutex)),
         _GT(typeof(MThread)),
         _GT(typeof(UAssert)),
         _GT(typeof(stRequestClientIP)),
﻿    };


    [MenuItem("Lua/Gen Lua Wrap Files", false, 11)]
    public static void Binding()
    {
        if (!Application.isPlaying)
        {
            EditorApplication.isPlaying = true;
        }

        BindType[] list = binds;

        for (int i = 0; i < list.Length; i++)
        {
            ToLuaExport.Clear();
            ToLuaExport.className = list[i].name;
            ToLuaExport.type = list[i].type;
            ToLuaExport.isStaticClass = list[i].IsStatic;
            ToLuaExport.baseClassName = list[i].baseName;
            ToLuaExport.wrapClassName = list[i].wrapName;
            ToLuaExport.libClassName = list[i].libName;
            ToLuaExport.Generate(null);
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Length; i++)
        {
            sb.AppendFormat("\t\t{0}Wrap.Register();\r\n", list[i].wrapName);
        }

        EditorApplication.isPlaying = false;
        //StringBuilder sb1 = new StringBuilder();

        //for (int i = 0; i < binds.Length; i++)
        //{
        //    sb1.AppendFormat("\t\t{0}Wrap.Register(L);\r\n", binds[i].wrapName);
        //}

        GenLuaBinder();
        GenLuaDelegates();
        Debug.Log("Generate lua binding files over");
        AssetDatabase.Refresh();        
    }

    //[MenuItem("Lua/Gen LuaBinder File", false, 12)]
    static void GenLuaBinder()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine();
        sb.AppendLine("public static class LuaBinder");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic static void Bind(IntPtr L)");
        sb.AppendLine("\t{");        

        string[] files = Directory.GetFiles("Assets/Source/LuaWrap/", "*.cs", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < files.Length; i++)
        {
            string wrapName = Path.GetFileName(files[i]);
            int pos = wrapName.LastIndexOf(".");
            wrapName = wrapName.Substring(0, pos);
            sb.AppendFormat("\t\t{0}.Register(L);\r\n", wrapName);
        }

        sb.AppendLine("\t}");
        sb.AppendLine("}");

        string file = Application.dataPath + "/Source/Base/LuaBinder.cs";

        using (StreamWriter textWriter = new StreamWriter(file, false, Encoding.UTF8))
        {
            textWriter.Write(sb.ToString());
            textWriter.Flush();
            textWriter.Close();
        }
    }

    [MenuItem("Lua/Clear LuaBinder File + Wrap Files", false, 13)]
    public static void ClearLuaBinder()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine();
        sb.AppendLine("public static class LuaBinder");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic static void Bind(IntPtr L)");
        sb.AppendLine("\t{");
        sb.AppendLine("\t}");
        sb.AppendLine("}");

        string file = Application.dataPath + "/Source/Base/LuaBinder.cs";

        using (StreamWriter textWriter = new StreamWriter(file, false, Encoding.UTF8))
        {
            textWriter.Write(sb.ToString());
            textWriter.Flush();
            textWriter.Close();
        }
        ClearFiles(Application.dataPath + "/Source/LuaWrap/");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 清除缓存文件
    /// </summary>
    /// <param name="path"></param>
    static void ClearFiles(string path) {
        string[] names = Directory.GetFiles(path);
        foreach (var filename in names) {
            File.Delete(filename); //删除缓存
        }
    }

    static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }

    [MenuItem("Lua/Gen Lua Delegates", false, 12)]
    static void GenLuaDelegates()
    {
        ToLuaExport.Clear();

        DelegateType[] list = new DelegateType[]
        {
            _DT(typeof(Action<GameObject>)),
            //_DT(typeof(Action<GameObject, int, string>)),
            //_DT(typeof(Action<int, int, int, List<int>>)),
            //_DT(typeof(UIEventListener.VoidDelegate)).SetName("VoidDelegate"),            
        };

        ToLuaExport.GenDelegates(list);

        Debug.Log("Create lua delegate over");
    }

    /// <summary>
    /// 编码LUA文件用UTF-8
    /// </summary>
    [MenuItem("Lua/Encode LuaFile with UTF-8", false, 50)]
    public static void EncodeLuaFile() {
        string path = Application.dataPath + "/Lua";
        string[] files = Directory.GetFiles(path, "*.lua", SearchOption.AllDirectories);
        foreach (string f in files) {
            string file = f.Replace('\\', '/');

            string content = File.ReadAllText(file);
            using (var sw = new StreamWriter(file, false, new UTF8Encoding(false))) {
                sw.Write(content);
            }
            Debug.Log("Encode file::>>" + file + " OK!");
        }
    }

    [MenuItem("Lua/Gen u3d Wrap Files(慎用)", false, 51)]
    public static void U3dBinding()
    {
        List<string> dropList = new List<string>
        {      
            //特殊修改
            "UnityEngine.Object",

            //一般情况不需要的类, 编辑器相关的
            "HideInInspector",
            "ExecuteInEditMode",
            "AddComponentMenu",
            "ContextMenu",
            "RequireComponent",
            "DisallowMultipleComponent",
            "SerializeField",
            "AssemblyIsEditorAssembly",
            "Attribute",  //一些列文件，都是编辑器相关的     
            "FFTWindow",
  
            "Types",
            "UnitySurrogateSelector",            
            "TypeInferenceRules",            
            "ThreadPriority",
            "Debug",        //自定义debugger取代
            "GenericStack",

            //异常，lua无法catch
            "PlayerPrefsException",
            "UnassignedReferenceException",            
            "UnityException",
            "MissingComponentException",
            "MissingReferenceException",

            //RPC网络
            "RPC",
            "Network",
            "MasterServer",
            "BitStream",
            "HostData",
            "ConnectionTesterStatus",

            //unity 自带编辑器GUI
            "GUI",
            "EventType",
            "EventModifiers",
            //"Event",
            "FontStyle",
            "TextAlignment",
            "TextEditor",
            "TextEditorDblClickSnapping",
            "TextGenerator",
            "TextClipping",
            "TextGenerationSettings",
            "TextAnchor",
            "TextAsset",
            "TextWrapMode",
            "Gizmos",
            "ImagePosition",
            "FocusType",
            

            //地形相关
            "Terrain",                            
            "Tree",
            "SplatPrototype",
            "DetailPrototype",
            "DetailRenderMode",

            //其他
            "MeshSubsetCombineUtility",
            "AOT",
            "Random",
            "Mathf",
            "Social",
            "Enumerator",       
            "SendMouseEvents",               
            "Cursor",
            "Flash",
            "ActionScript",
            
    
            //非通用的类
            "ADBannerView",
            "ADInterstitialAd",            
            "Android",
            "jvalue",
            "iPhone",
            "iOS",
            "CalendarIdentifier",
            "CalendarUnit",
            "CalendarUnit",
            "FullScreenMovieControlMode",
            "FullScreenMovieScalingMode",
            "Handheld",
            "LocalNotification",
            "Motion",   //空类
            "NotificationServices",
            "RemoteNotificationType",      
            "RemoteNotification",
            "SamsungTV",
            "TextureCompressionQuality",
            "TouchScreenKeyboardType",
            "TouchScreenKeyboard",
            "MovieTexture",

            //我不需要的
            //2d 类
            "AccelerationEventWrap", //加速
            "AnimatorUtility",
            "AudioChorusFilter",		
		    "AudioDistortionFilter",
		    "AudioEchoFilter",
		    "AudioHighPassFilter",		    
		    "AudioLowPassFilter",
		    "AudioReverbFilter",
		    "AudioReverbPreset",
		    "AudioReverbZone",
		    "AudioRolloffMode",
		    "AudioSettings",		    
		    "AudioSpeakerMode",
		    "AudioType",
		    "AudioVelocityUpdateMode",
            
            "Ping",
            "Profiler",
            "StaticBatchingUtility",
            "Font",
            "Gyroscope",                        //不需要重力感应
            "ISerializationCallbackReceiver",   //u3d 继承的序列化接口，lua不需要
            "ImageEffectOpaque",                //后处理
            "ImageEffectTransformsToLDR",
            "PrimitiveType",                // 暂时不需要 GameObject.CreatePrimitive           
            "Skybox",                       //不会u3d自带的Skybox
            "SparseTexture",                // mega texture 不需要
            "Plane",
            "PlayerPrefs",

            //不用ugui
            "SpriteAlignment",
		    "SpriteMeshType",
		    "SpritePackingMode",
		    "SpritePackingRotation",
		    "SpriteRenderer",
		    "Sprite",
            "UIVertex",
            "CanvasGroup",
            "CanvasRenderer",
            "ICanvasRaycastFilter",
            "Canvas",
            "RectTransform",
            "DrivenRectTransformTracker",
            "DrivenTransformProperties",
            "RectTransformAxis",
		    "RectTransformEdge",
		    "RectTransformUtility",
		    "RectTransform",
            "UICharInfo",
		    "UILineInfo",

            //不需要轮子碰撞体
            "WheelCollider",
		    "WheelFrictionCurve",
		    "WheelHit",

            //手机不适用雾
            "FogMode",

            "UnityEventBase",
		    "UnityEventCallState",
		    "UnityEvent",

            "LightProbeGroup",
            "LightProbes",

            "NPOTSupport", //只是SystemInfo 的一个枚举值

            //没用到substance纹理
            "ProceduralCacheSize",
		    "ProceduralLoadingBehavior",
		    "ProceduralMaterial",
		    "ProceduralOutputType",
		    "ProceduralProcessorUsage",
		    "ProceduralPropertyDescription",
		    "ProceduralPropertyType",
		    "ProceduralTexture",

            //物理关节系统
		    "JointDriveMode",
		    "JointDrive",
		    "JointLimits",		
		    "JointMotor",
		    "JointProjectionMode",
		    "JointSpring",
            "SoftJointLimit",
            "SpringJoint",
            "HingeJoint",
            "FixedJoint",
            "ConfigurableJoint",
            "CharacterJoint",            
		    "Joint",

            "LODGroup",
		    "LOD",

            "DataUtility",          //给sprite使用的
            "CrashReport",
            "CombineInstance",
        };

        List<BindType> list = new List<BindType>();
        Assembly assembly = Assembly.Load("UnityEngine");
        Type[] types = assembly.GetExportedTypes();

        for (int i = 0; i < types.Length; i++)
        {
            //不导出： 模版类，event委托, c#协同相关, obsolete 类
            if (!types[i].IsGenericType && types[i].BaseType != typeof(System.MulticastDelegate) &&
                !typeof(YieldInstruction).IsAssignableFrom(types[i]) && !ToLuaExport.IsObsolete(types[i]))
            {
                list.Add(_GT(types[i]));
            }
            else
            {
                Debug.Log("drop generic type " + types[i].ToString());
            }
        }

        for (int i = 0; i < dropList.Count; i++)
        {
            list.RemoveAll((p) => { return p.type.ToString().Contains(dropList[i]); });
        }

        //for (int i = 0; i < list.Count; i++)
        //{
        //    if (!typeof(UnityEngine.Object).IsAssignableFrom(list[i].type) && !list[i].type.IsEnum && !typeof(UnityEngine.TrackedReference).IsAssignableFrom(list[i].type)
        //        && !list[i].type.IsValueType && !list[i].type.IsSealed)            
        //    {
        //        Debug.Log(list[i].type.Name);
        //    }
        //}

        for (int i = 0; i < list.Count; i++)
        {
            try
            {
                ToLuaExport.Clear();
                ToLuaExport.className = list[i].name;
                ToLuaExport.type = list[i].type;
                ToLuaExport.isStaticClass = list[i].IsStatic;
                ToLuaExport.baseClassName = list[i].baseName;
                ToLuaExport.wrapClassName = list[i].wrapName;
                ToLuaExport.libClassName = list[i].libName;
                ToLuaExport.Generate(null);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Generate wrap file error: " + e.ToString());
            }
        }

        GenLuaBinder();
        Debug.Log("Generate lua binding files over， Generate " + list.Count + " files");
        AssetDatabase.Refresh();
    }
}
