using LuaInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDK.Lib
{
    /**
     * @brief 对 api 的进一步 wrap 
     */
    public class UtilApi
    {
        public const string TRUE = "true";
        public const string FALSE = "false";
        public const string PREFAB_DOT_EXT = ".prefab";
        public const string PREFAB = "prefab";
        public const string SHADER = "shader";

        public const string PNG = "png";
        public const string JPG = "jpg";
        public const string TGA = "tga";
        public const string MAT = "mat";
        public const string UNITY = "unity";
        public const string TXT = "txt";
        public const string BYTES = "bytes";

        public const string DOTUNITY3D = ".unity3d";
        public const string UNITY3D = "unity3d";
        public const string DOTPNG = ".png";
        public const string DOTUNITY = ".unity";

        public static Vector3 FAKE_POS = new Vector3(-1000, 0, -1000);  // 默认隐藏到这个位置
        public const string ASSETBUNDLES = "AssetBundles";
        public const string CR_LF = "\n";       // 回车换行， Mac 下面即使写入 "\r\n"，读取出来后，也只有 "\n"，因此这里 Windows 下也只写入 "\n"，而不是 "\r\n"
        public const string SEPARATOR = "=";    // 分隔符

        public const string TEXT_IN_BTN = "Text";   // Button 组件中 Text GameObject 的名字
        public const string MODEL_NAME = "model";   // 模型 GameObject 的 name
        public const string COLLIDE_NAME = "Collide"; // 模型 GameObject 的 name

        public static GameObject[] FindGameObjectsWithTag(string tag)
        {
            return GameObject.FindGameObjectsWithTag(tag);
        }

        // 这个会根据标签查找，知道查找到第一个为止，但是不一定准确
        public static GameObject FindGameObjectWithTag(string tag)
        {
            return GameObject.FindGameObjectWithTag(tag);
        }

        // 仅仅根据名字查找 GameObject ，注意如果 GameObject 设置 SetActive 为 false ，就会查找不到，如果有相同名字的 GameObject ，不保证查找到正确的。
        static public GameObject GoFindChildByName(string name)
        {
            return GameObject.Find(name);
        }

        // 通过父对象和完整的目录查找 child 对象，如果 path=""，返回的是自己，如果 path = null ，宕机
        static public GameObject TransFindChildByPObjAndPath(GameObject pObject, string path)
        {
            Transform trans = null;
            trans = pObject.transform.Find(path);
            if (trans != null)
            {
                return trans.gameObject;
            }

            return null;
        }

        // 从 Parent 获取一个组件
        static public T getComByP<T>(GameObject go, string path) where T : Component
        {
            T ret = null;
            Transform transform = null;

            transform = go.transform.Find(path);
            if (null != transform)
            {
                ret = transform.GetComponent<T>();
            }

            return ret;
        }

        // 从 Parent 获取一个组件
        static public T getComByP<T>(GameObject go) where T : Component
        {
            return go.GetComponent<T>();
        }

        // 从 Parent 获取一个组件
        static public T getComByP<T>(string path) where T : Component
        {
            return GameObject.Find(path).GetComponent<T>();
        }

        // 设置 Label 的显示
        static public void setLblStype(Text textWidget, LabelStyleID styleID)
        {

        }

        // 按钮点击统一处理
        static public void onBtnClkHandle(BtnStyleID btnID)
        {

        }

        // 添加事件处理
        public static void addEventHandle(GameObject go, string path, UIEventListener.VoidDelegate handle)
        {
            UIEventListener.Get(go.transform.Find(path).gameObject).onClick = handle;
        }

        public static void removeEventHandle(GameObject go, string path)
        {
            UIEventListener.Get(go.transform.Find(path).gameObject).onClick = null;
        }

        public static void addEventHandle(GameObject go, UIEventListener.VoidDelegate handle)
        {
            UIEventListener.Get(go).onClick = handle;
        }

        public static void addHoverHandle(GameObject go, UIEventListener.BoolDelegate handle)
        {
            UIEventListener.Get(go).onHover = handle;
        }

        public static void addPressHandle(GameObject go, UIEventListener.BoolDelegate handle)
        {
            UIEventListener.Get(go).onPress = handle;
        }

        public static void addDragOverHandle(GameObject go, UIEventListener.VoidDelegate handle)
        {
            UIEventListener.Get(go).onDragOver = handle;
        }

        public static void addDragOutHandle(GameObject go, UIEventListener.VoidDelegate handle)
        {
            UIEventListener.Get(go).onDragOut = handle;
        }

        public static void addEventHandle(GameObject go, string path, UnityAction handle)
        {
            go.transform.Find(path).GetComponent<Button>().onClick.AddListener(handle);
        }

        public static void addEventHandle(GameObject go, UnityAction handle)
        {
            go.GetComponent<Button>().onClick.AddListener(handle);
        }

        // 给一个添加 EventTrigger 组件的 GameObject 添加单击事件
        public static void addEventTriggerHandle(GameObject go, LuaFunction handle)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if(trigger == null)
            {
                trigger = UtilApi.AddComponent<EventTrigger>(go);
            }
            if(trigger != null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                // 这一行就相当于在 EventTrigger 组件编辑器中点击[Add New Event Type] 添加一个新的事件类型
                trigger.triggers.Add(entry);
                entry.eventID = EventTriggerType.PointerClick;

                entry.callback.RemoveAllListeners();
                entry.callback.AddListener(
                    (BaseEventData eventData) =>
                    {
                        handle.Call(go);
                    }
                );
            }
        }

        public static void addEventHandle(Button btn, UnityAction handle)
        {
            btn.onClick.AddListener(handle);
        }

        public static void addEventHandle(Button btn, MAction<IDispatchObject> handle)
        {
            AuxButtonUserData userData = btn.gameObject.GetComponent<AuxButtonUserData>();
            if (userData != null)
            {
                AuxButton auxBtn = userData.getUserData();
                if (auxBtn != null)
                {
                    auxBtn.addEventHandle(null, handle, null, null);
                }
            }
        }

        public static void RemoveListener(Button btn, UnityAction handle)
        {
            btn.onClick.RemoveListener(handle);
        }

        public static void RemoveListener(Button btn, MAction<IDispatchObject> handle)
        {
            AuxButtonUserData userData = btn.gameObject.GetComponent<AuxButtonUserData>();
            if (userData != null)
            {
                AuxButton auxBtn = userData.getUserData();
                if (auxBtn != null)
                {
                    auxBtn.addEventHandle(null, handle, null, null);
                }
            }
        }

        public static void addEventHandle(UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.AddListener(unityAction);
        }

        public static void RemoveListener(UnityEvent unityEvent, UnityAction unityAction)
        {
            unityEvent.RemoveListener(unityAction);
        }

        public static void RemoveAllListener(UnityEvent unityEvent)
        {
            unityEvent.RemoveAllListeners();
        }

        public static void addEventHandle(Button.ButtonClickedEvent buttonClickedEvent, UnityAction unityAction)
        {
            buttonClickedEvent.AddListener(unityAction);
        }

        public static void addEventHandle(UnityEvent<GameObject> unityEvent, UnityAction<GameObject> unityAction)
        {
            unityEvent.AddListener(unityAction);
        }

        public static void addEventHandle(GameObject go, string path, LuaTable luaTable, LuaFunction luaFunction)
        {
            Transform goTrans = go.GetComponent<Transform>();
            if(goTrans == null)
            {
                goTrans = go.GetComponent<RectTransform>();
            }

            if(goTrans != null)
            {
                Transform evtTrans = goTrans.Find(path);
                if (evtTrans != null)
                {
                    GameObject evtGo = evtTrans.gameObject;
                    UtilApi.addEventHandle(evtGo, luaTable, luaFunction);
                }
            }
        }

        public static void addEventHandle(GameObject go, LuaTable luaTable, LuaFunction luaFunction, bool isAddToRoot = false)
        {
            Button.ButtonClickedEvent btnEvent = go.GetComponent<Button>().onClick;
            if (btnEvent != null)
            {
                // 添加到根节点，就使用闭包模拟类保存数据
                if (isAddToRoot)
                {
                    btnEvent.RemoveAllListeners();
                    btnEvent.AddListener(
                        () =>
                        {
                            if (luaTable != null)
                            {
                                luaFunction.Call(luaTable, go);
                            }
                            else
                            {
                                luaFunction.Call(go);
                            }
                        }
                     );
                }
                else
                {
                    AuxButtonUserData userData = go.GetComponent<AuxButtonUserData>();
                    if(userData == null)
                    {
                        userData = UtilApi.AddComponent<AuxButtonUserData>(go);
                    }
                    if (userData != null)
                    {
                        AuxButton auxBtn = userData.getUserData();
                        if(auxBtn == null)
                        {
                            auxBtn = userData.addUserData();
                        }
                        if (auxBtn != null)
                        {
                            auxBtn.addEventHandle(null, null, luaTable, luaFunction);
                        }
                    }
                }
            }
        }

        public static void addEventHandle(Button.ButtonClickedEvent btnEvent, LuaTable luaTable, LuaFunction luaFunction)
        {
            if (btnEvent != null)
            {
                btnEvent.RemoveAllListeners();
                btnEvent.AddListener(
                    () =>
                    {
                        if (luaTable != null)
                        {
                            luaFunction.Call(luaTable);
                        }
                        else
                        {
                            luaFunction.Call();
                        }
                    }
                );
            }
        }

        public static void addEventHandle(UnityEvent<bool> unityEvent, LuaTable luaTable, LuaFunction luaFunction)
        {
            unityEvent.AddListener(
                (param) => 
                {
                    if (luaTable != null)
                    {
                        luaFunction.Call(luaTable, param);
                    }
                    else
                    {
                        luaFunction.Call(param);
                    }
                }
            );
        }

        // 深度遍历移除 Sprite Image
        public static void DestroyComponent(GameObject go_)
        {
            // 每一个 GameObject 只能有一个 Image 组件
            Image image = go_.GetComponent<Image>();
            if (image != null && image.sprite != null)
            {
                if (image.sprite.texture != null)
                {

                }
                image.sprite = null;
            }

            int childCount = go_.transform.childCount;
            int idx = 0;
            Transform childTrans = null;
            for (idx = 0; idx < childCount; ++idx)
            {
                childTrans = go_.transform.GetChild(idx);
                UtilApi.DestroyComponent(childTrans.gameObject);
            }
        }

        // 深度遍历移除 Sprite Image
        public static bool CheckComponent<T>(GameObject go_)
        {
            T com = go_.GetComponent<T>();
            if (com != null)
            {
                return true;
            }

            int childCount = go_.transform.childCount;
            int idx = 0;
            Transform childTrans = null;
            for (idx = 0; idx < childCount; ++idx)
            {
                childTrans = go_.transform.GetChild(idx);
                if(UtilApi.CheckComponent<T>(childTrans.gameObject))
                {
                    return true;
                }
            }

            return false;
        }

        // 销毁对象，如果使用这个销毁对象，然后立刻使用 GameObject.Find 查找对象，结果仍然可以查找到，这个时候尽量使用 DestroyImmediate
        public static void Destroy(UnityEngine.Object obj)
        {
            if (obj != null)
            {
                if (obj as GameObject)
                {
                    UtilApi.DestroyComponent(obj as GameObject);
                    (obj as GameObject).transform.SetParent(null);      // 这个仅仅是移除场景中
                    UtilApi.DestroyTexMat(obj as GameObject);
                }
                UnityEngine.Object.Destroy(obj);
                obj = null;
            }
            else
            {
            }
        }

        // 立即销毁对象
        public static void DestroyImmediate(UnityEngine.Object obj)
        {
            if (obj as GameObject)
            {
                (obj as GameObject).transform.SetParent(null);      // 这个仅仅是移除场景中
                UtilApi.DestroyTexMat(obj as GameObject);
            }
            UnityEngine.Object.DestroyImmediate(obj);
        }

        // bInstance 是通过 Instance 实例画出来的，否则是直接加载的磁盘资源，这种资源是受保护的，不能设置任何里面的值
        public static void DestroyImmediate(UnityEngine.Object obj, bool allowDestroyingAssets, bool bInstance = true)
        {
            if (obj as GameObject)
            {
                if (bInstance)
                {
                    (obj as GameObject).transform.SetParent(null);      // 这个仅仅是移除场景中
                    UtilApi.DestroyTexMat(obj as GameObject);
                }
            }
            GameObject.DestroyImmediate(obj, allowDestroyingAssets);
        }

        public static void DontDestroyOnLoad(UnityEngine.Object target)
        {
            UnityEngine.Object.DontDestroyOnLoad(target);
        }

        // 纹理材质都是实例化，都对资源有引用计数，深度遍历释放资源
        public static void DestroyTexMat(UnityEngine.GameObject go_)
        {
            Material mat = go_.GetComponent<Material>();
            if(mat != null)
            {
                if (mat.mainTexture != null)
                {
                    UtilApi.UnloadAsset(mat.mainTexture);
                    mat.mainTexture = null;
                }
                UtilApi.UnloadAsset(mat);
                mat = null;
            }

            Image image = go_.GetComponent<Image>();
            if(image != null)
            {
                if(image.sprite != null)
                {
                    if(image.sprite.texture != null)
                    {
                        UtilApi.UnloadAsset(image.sprite.texture);
                    }

                    image.sprite = null;
                }

                image = null;
            }

            int childCount = go_.transform.childCount;
            int idx = 0;
            Transform childTrans = null;
            for (idx = 0; idx < childCount; ++idx)
            {
                childTrans = go_.transform.GetChild(idx);
                UtilApi.DestroyTexMat(childTrans.gameObject);
            }
        }

        public static void CleanTex(UnityEngine.GameObject go_)
        {
            if (go_ == null)
                return;
            Image image = go_.GetComponent<Image>();
            if (image != null)
            {
                if (image.sprite != null)
                {
                    ImageItem imageItem = Ctx.mInstance.mAtlasMgr.getAndSyncLoadImage(CVAtlasName.ShopDyn, image.sprite.name);
                    if (imageItem != null && imageItem.image != null)
                    {
                        if (image.sprite.texture != null)
                        {
                            UtilApi.UnloadAsset(image.sprite.texture);
                        }
                        image.sprite = null;
                        image = null;
                    }          
                }
            }

            int childCount = go_.transform.childCount;
            int idx = 0;
            Transform childTrans = null;
            for (idx = 0; idx < childCount; ++idx)
            {
                childTrans = go_.transform.GetChild(idx);
                UtilApi.CleanTex(childTrans.gameObject);
            }
        }

        public static void SetActive(GameObject target, bool bshow)
        {
            target.SetActive(bshow);
        }

        public static void fakeSetActive(GameObject target, bool bshow)
        {
            if(!bshow)
            {
                target.transform.position = UtilApi.FAKE_POS;
            }
        }

        public static bool IsActive(GameObject target)
        {
            if (target != null)
            {
                return target.activeSelf;
            }

            return false;
        }

        public static bool isInFakePos(Vector3 pos)
        {
            return UtilMath.isEqualVec3(pos, UtilApi.FAKE_POS);
        }

        public static UnityEngine.Object Instantiate(UnityEngine.Object original)
        {
            return UnityEngine.Object.Instantiate(original);
        }

        public static UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }

        public static void normalRST(Transform tran)
        {
            UtilApi.setPos(tran, new Vector3(0, 0, 0));
            UtilApi.setRot(tran, new Vector3(0, 0, 0));
            UtilApi.setScale(tran, Vector3.one);
        }

        public static void normalPosScale(Transform tran)
        {
            //tran.localPosition = Vector3.zero;
            UtilApi.setPos(tran, new Vector3(0, 0, 0));
            UtilApi.setScale(tran, Vector3.one);
        }

        public static void normalPos(Transform tran)
        {
            UtilApi.setPos(tran, Vector3.zero);
        }

        public static void normalRot(Transform tran)
        {
            tran.localRotation = Quaternion.Euler(Vector3.zero);
        }

        public static void setRot(Transform tran, Vector3 rot)
        {
            tran.localEulerAngles = rot;
        }

        public static void setRot(Transform tran, Quaternion rot)
        {
            tran.localRotation = rot;
        }

        public static void setScale(Transform tran, Vector3 scale)
        {
            tran.localScale = scale;
        }

        public static void setPos(Transform tran, Vector3 pos)
        {
            tran.localPosition = pos;
        }

        public static void setRectPos(RectTransform tran, Vector3 pos)
        {
            tran.localPosition = pos;
        }

        public static void setRectRotate(RectTransform tran, Vector3 rotate)
        {
            Vector3 rot = tran.localEulerAngles;
            rot.x = rotate.x;
            rot.y = rotate.y;
            rot.z = rotate.z;
            tran.localEulerAngles = rot;
        }

        // 设置 RectTransform大小
        public static void setRectSize(RectTransform tran, Vector2 size)
        {
            tran.sizeDelta = size;
        }

        public static void adjustEffectRST(Transform transform)
        {
            UtilApi.setPos(transform, new Vector3(-0.01f, 0, 0.46f));
            UtilApi.setRot(transform, new Vector3(90, 0, 0));
            UtilApi.setScale(transform, new Vector3(0.5f, 0.48f, 1.0f));
        }

        // 卸载内部 Resources 管理的共享的那块资源，注意这个是异步事件
        public static AsyncOperation UnloadUnusedAssets(bool needGC = false)
        {
            // 卸载从 AssetBundles 加载的 由 Resources 管理的 Asset-Object，或者直接使用 Resource.Load 加载的由 Resources 管理的 Asset-Object 资源
            AsyncOperation opt = Resources.UnloadUnusedAssets();
            if (needGC)
            {
                GC.Collect();
            }
            return opt;
        }

        // 立即垃圾回收
        public static void ImmeUnloadUnusedAssets(bool needGC = false)
        {
            // 用于释放所有没有引用的Asset对象
            Resources.UnloadUnusedAssets();     // 这个卸载好像很卡，使用的时候要小心使用，好像是遍历整个资源 Tree
            if (needGC)
            {
                // 强制垃圾收集器立即释放内存 Unity的GC功能不算好，没把握的时候就强制调用一下
                GC.Collect();
            }
        }

        // 小心使用这个资源，这个函数把共享资源卸载掉了，如果有引用，就会有问题，确切的知道释放哪个资源，这个卸载除了 GameObject 之外的资源
        public static void UnloadAsset(UnityEngine.Object assetToUnload)
        {
            // 显式的释放已加载的Asset对象，只能卸载磁盘文件加载的Asset对象,从磁盘文件夹的 Asset 对象拷贝出来的 Asset 对象不能释放
            Resources.UnloadAsset(assetToUnload);   // 不能卸载 GameObject 类型的对象，可能 GameObject 对象是一个容器，不能卸载容器，可以卸载 Texture 、 TextAsset 这类的资源
        }

        // 卸载整个 AssetBundles
        static public void UnloadAssetBundles(AssetBundle assetBundle, bool unloadAllLoadedObjects)
        {
            assetBundle.Unload(unloadAllLoadedObjects);

            //if (unloadAllLoadedObjects)
            //{
            //    UtilApi.UnloadUnusedAssets();
            //}
        }

        // 从场景图中移除,  worldPositionStays 是否在两个 local 中移动保持 world 信息不变，如果要保持 local 信息不变，就设置成 false ，通常 UI 需要设置成  false ，如果 worldPositionStays 为 true ，就是从当前局部空间变换到另外一个局部空间变换，父节点的变换会应用到对象上， worldPositionStays 为 false ，就是局部变换直接移动到另外一个局部空间，直接应用目的局部空间父变换
        public static void removeFromSceneGraph(Transform trans, bool worldPositionStays = true)
        {
            trans.SetParent(null);      // 这个仅仅是移除场景中
        }

        // 这个设置 Child 位置信息需要是 Transform 
        public static void SetParent(Transform child, Transform parent, bool worldPositionStays = true)
        {
            if (child != null && parent != null)
            {
                if (child.parent != parent)
                {
                    child.SetParent(parent, worldPositionStays);
                }
            }
            else if (child != null && parent == null)
            {
                child.SetParent(null, worldPositionStays);
            }
        }

        public static void SetParent(GameObject child, GameObject parent, bool worldPositionStays = true)
        {
            Transform childTrans = null;
            Transform parentTrans = null;

            if (child != null && parent != null)
            {
                childTrans = child.GetComponent<Transform>();
                if(childTrans == null)
                {
                    childTrans = child.GetComponent<RectTransform>();
                }
                parentTrans = parent.GetComponent<Transform>();
                if (parentTrans == null)
                {
                    parentTrans = parent.GetComponent<RectTransform>();
                }

                if (childTrans.parent != parentTrans)
                {
                    childTrans.SetParent(parentTrans, worldPositionStays);
                }
            }
            else if(child != null && parent == null)
            {
                childTrans = child.GetComponent<Transform>();
                if (childTrans == null)
                {
                    childTrans = child.GetComponent<RectTransform>();
                }
                childTrans.SetParent(null, worldPositionStays);
            }
        }

        // 这个设置 Child 位置信息需要是 RectTransform ，这个时候取 Child 的 Transform 不能使用 child.transform ，会报错误
        public static void SetRectTransParent(GameObject child, GameObject parent, bool worldPositionStays = true)
        {
            RectTransform childRectTrans = child.GetComponent<RectTransform>();
            RectTransform parentRectTrans = parent.GetComponent<RectTransform>();

            if (childRectTrans != null && childRectTrans.parent != parentRectTrans)
            {
                childRectTrans.SetParent(parentRectTrans, worldPositionStays);
            }
        }

        public static void copyTransform(Transform src, Transform dest)
        {
            UtilApi.setPos(dest, src.localPosition);
            UtilApi.setRot(dest, src.localRotation);
            UtilApi.setScale(dest, src.localScale);
        }

        // 是否包括 child 
        public static void setLayer(GameObject go_, string layerName, bool bIncludeChild = true)
        {
            // 深度优先设置
            // 设置自己
            go_.layer = LayerMask.NameToLayer(layerName);

            int childCount = go_.transform.childCount;
            int idx = 0;
            Transform childTrans = null;
            for (idx = 0; idx < childCount; ++idx)
            {
                childTrans = go_.transform.GetChild(idx);
                UtilApi.setLayer(childTrans.gameObject, layerName, bIncludeChild);
            }
        }

        public static void setGOName(GameObject go_, string name)
        {
            go_.name = name;
        }

        public static void SetNativeSize(Image image)
        {
            image.SetNativeSize();
        }

        public static void setImageType(Image image, Image.Type type)
        {
            image.type = type;
        }

        public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit = 100, uint extrude = 0, SpriteMeshType meshType = SpriteMeshType.FullRect)
        {
            return Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType);
        }

        // 创建一个精灵 GameObject ，播放场景特效
        public static GameObject createSpriteGameObject()
        {
            Type[] comArr = new Type[1];
            comArr[0] = typeof(SpriteRenderer);
            GameObject _go = new GameObject("SpriteGO", comArr);
            return _go;
        }

        public static GameObject createGameObject(string name = "PlaceHolder")
        {
            return new GameObject(name);
        }

        public static GameObject CreatePrimitive(PrimitiveType type)
        {
            return GameObject.CreatePrimitive(type);
        }

        public static T AddComponent<T>(GameObject go_) where T : Component
        {
            T ret = null;
            ret = go_.GetComponent<T>();
            if (ret == null)
            {
                ret = go_.AddComponent<T>();
            }

            return ret;
        }

        public static void AddAnimatorComponent(GameObject go_, bool applyRootMotion = false)
        {
            if (null == go_.GetComponent<Animator>())
            {
                Animator animator = UtilApi.AddComponent<Animator>(go_);
                animator.applyRootMotion = applyRootMotion;
            }
        }

        public static void copyBoxCollider(GameObject src, GameObject dest)
        {
            BoxCollider srcBox = src.GetComponent<BoxCollider>();
            BoxCollider destBox = dest.GetComponent<BoxCollider>();
            if(destBox == null)
            {
                destBox = UtilApi.AddComponent<BoxCollider>(dest);
            }
            destBox.center = srcBox.center;
            destBox.size = srcBox.size;
        }

        // 当前是否在与 UI 元素交互
        public static bool IsPointerOverGameObject()
        {
            bool ret = false;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                ret = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }
            else if(Input.GetMouseButtonDown(0))
            {
                ret = EventSystem.current.IsPointerOverGameObject();
            }

            return ret;
        }

        // 通过光线追踪判断是否相交
        public static bool IsPointerOverGameObjectRaycast()
        {
            Vector2 ioPos = Vector2.zero;
            if(Input.touchCount > 0)
            {
                ioPos = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ioPos = Input.mousePosition;
            }

            PointerEventData cursor = new PointerEventData(EventSystem.current);
            cursor.position = ioPos;
            List<RaycastResult> objectsHit = new List<RaycastResult>();
            objectsHit.Clear();
            EventSystem.current.RaycastAll(cursor, objectsHit);
            foreach(RaycastResult ray in objectsHit)
            {
                if(ray.gameObject.layer == LayerMask.NameToLayer("UGUI"))
                {
                    return true;
                }
                else
                {
                    break;
                }
            }

            return objectsHit.Count > 0;
        }

        // 剔除字符串末尾的空格
        public static void trimEndSpace(ref string str)
        {
            str.TrimEnd('\0');
        }

        // 判断两个 GameObject 地址是否相等
        public static bool isAddressEqual(GameObject a, GameObject b)
        {
            return object.ReferenceEquals(a, b);
        }

        // 判断两个 GameObject 地址是否相等
        public static bool isAddressEqual(System.Object a, System.Object b)
        {
            return object.ReferenceEquals(a, b);
        }

        // 判断向量是否相等
        public static bool isVectorEqual(Vector3 lhv, Vector3 rhv)
        {
            if (UnityEngine.Mathf.Abs(lhv.x - rhv.x) < 0.0001f)
            {
                if (UnityEngine.Mathf.Abs(lhv.y - rhv.y) < 0.0001f)
                {
                    if (UnityEngine.Mathf.Abs(lhv.z - rhv.z) < 0.0001f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected static long msCurTime;
        protected static System.TimeSpan msTimeSpan;

        // 返回 UTC 秒
        public static uint getUTCSec()
        {
            msCurTime = System.DateTime.Now.Ticks;
            msTimeSpan = new System.TimeSpan(msCurTime);
            return (uint)(msTimeSpan.TotalSeconds);
        }

        // 获取当前时间的文本可读形式
        public static string getUTCFormatText()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        public static int Range(int min, int max)
        {
            UnityEngine.Random.InitState((int)UtilApi.getUTCSec());
            return UnityEngine.Random.Range(min, max);
        }

        static public string getDataPath()
        {
            return Application.dataPath;
        }

        static public Vector3 convPtFromLocal2World(Transform trans, Vector3 localPt)
        {
            return trans.TransformPoint(localPt);
        }

        static public Vector3 convPtFromWorld2Local(Transform trans, Vector3 localPt)
        {
            return trans.InverseTransformPoint(localPt);
        }

        static public Vector3 convPtFromLocal2Local(Transform from, Transform to, Vector3 localPt)
        {
            return to.InverseTransformPoint(from.TransformPoint(localPt));
        }

        static public void PrefetchSocketPolicy(string ip, int atPort)
        {
            Security.PrefetchSocketPolicy(ip, atPort);
        }

        // 获取某一类型所有的对象
        static public T[] FindObjectsOfTypeAll<T>() where T : UnityEngine.Object
        {
            return Resources.FindObjectsOfTypeAll<T>();
        }

        static public void SetDirty(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            if (obj)
            {
                UnityEditor.EditorUtility.SetDirty(obj);
            }
#endif
        }

        /**
         * @brief 两个相机坐标之间转换
         * @Param scale 就是 Canvas 组件所在的 GameObject 中 RectTransform 组件中的 Scale 因子
         * @param srcWorldPos 一定是世界坐标空间位置
         */
        static public Vector3 convPosFromSrcToDestCam(Camera srcCam, Camera destCam, Vector3 srcWorldPos/*, float scale = 0.01173333f*/)
        {
            //Vector3 srcScreenPos = srcCam.WorldToScreenPoint(srcWorldPos);
            //srcScreenPos.z = 1.0f;
            //Vector3 destPos = destCam.ScreenToWorldPoint(srcScreenPos);
            //destPos.z = 0.0f;
            //destPos /= scale;
            //return destPos;

            Vector3 srcScreenPos = srcCam.WorldToScreenPoint(srcWorldPos);
            Vector3 destPos = destCam.ScreenToWorldPoint(srcScreenPos);
            return destPos;
        }

        // 将坐标从一个 Transform 转换到另外一个 Transform
        static public Vector3 convPosFromOneToOtherLocal(Transform srcTrans, Transform destTrans, Vector3 srcPos)
        {
            Vector3 worldPos = srcTrans.TransformPoint(srcPos);
            Vector3 destPos = destTrans.InverseTransformVector(worldPos);
            return destPos;
        }

        public static Vector2 convWorldToUIPos(Canvas canvas, Camera worldCamera, Vector3 worldPos, Camera uiCamera)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                worldCamera.WorldToScreenPoint(worldPos), uiCamera, out pos);

            return pos;

            //Vector3 srcScreenPos = worldCamera.WorldToScreenPoint(worldPos);
            //Vector3 destPos = uiCamera.ScreenToWorldPoint(srcScreenPos);
            //Vector3 destUIPos = canvas.transform.InverseTransformVector(destPos);

            //return destUIPos;
        }

        static public void set(GameObject go, int preferredHeight)
        {
            LayoutElement layoutElem = go.GetComponent<LayoutElement>();
            if(layoutElem != null)
            {
                layoutElem.preferredHeight = preferredHeight;
            }
        }

        static public int getChildCount(GameObject go)
        {
            return go.transform.childCount;
        }

        static public void setSiblingIndex(GameObject go, int index)
        {
            go.transform.SetSiblingIndex(index);
        }

        // 设置节点到倒数第二个
        static public void setSiblingIndexToLastTwo(GameObject go)
        {
            go.transform.SetSiblingIndex(go.transform.parent.childCount - 1);
        }

        static public void setTextStr(GameObject go, string str)
        {
            Text text = go.GetComponent<Text>();
            text.text = str;
        }

        static public void enableBtn(GameObject go)
        {
            Button btn = go.GetComponent<Button>();
            if(btn != null)
            {
                btn.enabled = false;
            }
        }

        // 欧拉角增加
        static public float incEulerAngles(float degree, float delta)
        {
            return (degree + delta) % 360;
        }

        static public float decEulerAngles(float degree, float delta)
        {
            return (degree - delta) % 360;
        }

        // 获取是否可视
        static public bool GetActive(GameObject go)
        {
            return go && go.activeInHierarchy;
        }

        static public int NameToLayer(string layerName)
        {
            return LayerMask.NameToLayer(layerName);
        }

        static public void assert(bool condition, string message = "")
        {
            Debug.Assert(condition, message);
        }

        static public float rangRandom(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        static public string GetRelativePath()
        {
            if (Application.isEditor)
                return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
            else if (Application.isWebPlayer)
                return System.IO.Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/StreamingAssets";
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return Application.streamingAssetsPath;
            else // For standalone player.
                return "file://" + Application.streamingAssetsPath;
        }

        static public string getRuntimePlatformFolderForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "OSX";
                // Add more build platform for your own.
                // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
                default:
                    return null;
            }
        }

        static public string getManifestName()
        {
            return UtilApi.getRuntimePlatformFolderForAssetBundles(Application.platform) + UtilApi.DOTUNITY3D;
        }

        static public void createMatIns(ref Material insMat, Material matTmpl, string matName = "", HideFlags hideFlags = HideFlags.DontSave | HideFlags.NotEditable)
        {
            insMat = new Material(matTmpl);
            insMat.name = matName;
            insMat.hideFlags = hideFlags;
            insMat.CopyPropertiesFromMaterial(matTmpl);

            string[] keywords = matTmpl.shaderKeywords;
            for (int i = 0; i < keywords.Length; ++i)
            {
                insMat.EnableKeyword(keywords[i]);
            }
        }

        // 转换二维索引到一维索引
        static public uint convTIdx2OIdx(short x, short y)
        {
            uint key = 0;
            key = (uint)(((ushort)y << 16) | (ushort)x);
            return key;
        }

        static public void setStatic(GameObject go, bool isStatic)
        {
            go.isStatic = isStatic;
        }

        static public bool isStatic(GameObject go)
        {
            return go.isStatic;
        }

        static public void setHideFlags(UnityEngine.Object obj, HideFlags flags)
        {
            obj.hideFlags = flags;
        }

        static public HideFlags getHideFlags(UnityEngine.Object obj)
        {
            return obj.hideFlags;
        }

        // 静态批次合并
        static public void drawCombine(GameObject staticBatchRoot)
        {
            StaticBatchingUtility.Combine(staticBatchRoot);
        }

        // 静态批次合并
        static public void drawCombine(GameObject[] gos, GameObject staticBatchRoot)
        {
            StaticBatchingUtility.Combine(gos, staticBatchRoot);
        }

        // 注意最大不能超过 65536
        static public uint packIndex(long x, long y)
        {
            short xs16 = (short)(x);
            short ys16 = (short)(y);

            ushort x16 = (ushort)(xs16);
            ushort y16 = (ushort)(ys16);

            uint key = 0;
            key = (uint)((x16 << 16) | y16);

            return key;
        }

        // 注意最大不能超过 65536
        static public void unpackIndex(uint key, ref long x, ref long y)
        {
            ushort y16 = (ushort)(key & 0xFFFF);
            ushort x16 = (ushort)((key >> 16) & 0xFFFF);

            x = (short)(x16);
            y = (short)(y16);
        }

        static public int getScreenWidth()
        {
            return Screen.width;
        }

        static public int getScreenHeight()
        {
            return Screen.height;
        }

        static public bool isWWWNoError(WWW www)
        {
            if(www != null)
            {
                return www.isDone && string.IsNullOrEmpty(www.error);
            }

            return true;
        }

        static public System.Text.Encoding convGkEncode2EncodingEncoding(GkEncode gkEncode)
        {
            System.Text.Encoding retEncode = System.Text.Encoding.UTF8;

            if(GkEncode.eUTF8 == gkEncode)
            {
                retEncode = System.Text.Encoding.UTF8;
            }
            else if (GkEncode.eGB2312 == gkEncode)
            {
                retEncode = System.Text.Encoding.UTF8;
            }
            else if (GkEncode.eUnicode == gkEncode)
            {
                retEncode = System.Text.Encoding.Unicode;
            }
            else if (GkEncode.eDefault == gkEncode)
            {
                retEncode = System.Text.Encoding.Default;
            }

            return retEncode;
        }

        // 设置 Text 颜色
        static public void setLabelColor(Text label, Color color)
        {
            label.color = color;
        }

        // 设置 InputField 颜色
        static public void setInputFieldColor(InputField inputField, Color color)
        {
            inputField.textComponent.color = color;
        }

        // 从 Euler 转换到 Quaternion
        public static Quaternion convQuatFromEuler(Vector3 euler)
        {
            return Quaternion.Euler(euler);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 30)
        {
            // 只能在编辑器中看到
            UnityEngine.Debug.DrawLine(start, end, color);
        }

        // 转换 Lua 包目录到磁盘目录
        public static string convPackagePath2DiscPath(string packagePath)
        {
            string path = "";

            if (packagePath.EndsWith(".lua"))
            {
                path = packagePath.Substring(0, packagePath.Length - 4);
            }
            else
            {
                path = packagePath;
            }

            path = path.Replace('.', '/');
            path = string.Format("{0}{1}", path, ".lua");
            return path;
        }

        //设置屏幕永远亮着
        public static void setSleepTimeout(int mode)
        {
            if (0 == mode)
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }
    }
}