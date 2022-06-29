using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System;

namespace DD_Tools
{
    [InitializeOnLoad]
    public class CustomHierarchy : Editor
    {
        static CustomStylePlatform stylePlatform;
        private static bool isInitialized = false;
        static CustomHierarchy()
        {
            Start();
        }

        [MenuItem("DD_Tools/Start")]
        private static void Start()
        {
            if (!isInitialized)
            {
                //读取ScriptObjectTable文件
                string[] allFindedAssetGUID = AssetDatabase.FindAssets("t:" + typeof(CustomStylePlatform).Name);
                if (allFindedAssetGUID.Length >= 1)
                {
                    //先将找到的物体的GUID改变为路径然后加载出来
                    stylePlatform = AssetDatabase.LoadAssetAtPath<CustomStylePlatform>(AssetDatabase.GUIDToAssetPath(allFindedAssetGUID[0]));
                    //添加Hierarchy事件
                    EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyChanged;
                    isInitialized = true;
                }
            }
        }
        /// <summary>
        /// Hierarchy回调事件
        /// </summary>
        /// <param name="instanceID">物体标识符</param>
        /// <param name="selectionRect">选择的物体范围</param>
        /// <exception cref="NotImplementedException"></exception>
        private static void OnHierarchyChanged(int instanceID, Rect selectionRect)
        {
            UnityEngine.Object instanceObject = EditorUtility.InstanceIDToObject(instanceID);

            if (instanceObject != null)
            {
                OnDrawHierarchyCustomFunc(instanceObject, selectionRect);
            }
        }

        private static void OnDrawHierarchyCustomFunc(UnityEngine.Object instanceObject, Rect selectionRect)
        {
            if (stylePlatform != null)
            {
                bool isTexture = true;
                //判断是否是字体，有一次表示是字体就代表不用继续了
                for (int i = 0; i < stylePlatform.CustomStyles.Count; i++)
                {
                    CustomStyle customStyle = stylePlatform.CustomStyles[i];
                    //如果物体的名字是以我们某一个标识符为开始则确定此物体需要绘制自定义面板
                    if (instanceObject.name.StartsWith(customStyle.nameKey))
                    {
                        if (stylePlatform.EnableFontStyleFunction)
                        {
                            //绘制背景颜色
                            EditorGUI.DrawRect(selectionRect, customStyle.backgroundColor);
                            //得到标识符后面的名字
                            var name = instanceObject.name.Substring(customStyle.nameKey.Length);
                            //绘制文字
                            GUIStyle gUIStyle = new GUIStyle()
                            {
                                alignment = customStyle.textAnchor,
                                fontStyle = customStyle.fontStyle,
                                normal = new GUIStyleState()
                                {
                                    textColor = customStyle.fontColor
                                }
                            };
                            //创建一个字体
                            GUI.Label(selectionRect, name.ToUpper(), gUIStyle);
                        }
                        isTexture = false;
                        break;
                    }
                }
                if(isTexture)
                {
                    if (stylePlatform.EnableIconStyleFuntion)
                    {
                        //将Object转化为GameObject
                        var gameObject = (GameObject)instanceObject;
                        //如果一个物体含有很多Icon则需要排序
                        var index = 0;
                        //绘制一个可以勾选是否启用禁用的Toggle
                        Rect setActivityToggleRect = CalculationRectMethod(selectionRect, ref index);
                        gameObject.SetActive(GUI.Toggle(setActivityToggleRect, gameObject.activeSelf,""));
                        //绘制一个提示是否是静态的标志
                        if (gameObject.isStatic)
                        {
                            var rect = CalculationRectMethod(selectionRect, ref index);
                            GUI.Label(rect, "S ",new GUIStyle() {alignment= TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold,fontSize = 16,normal = new GUIStyleState() { textColor = Color.yellow} });
                        }
                        // Renderer
                        DrawIconOnHierarchy<MeshRenderer>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<SkinnedMeshRenderer>(selectionRect, gameObject, ref index);
                        // Colliders
                        DrawIconOnHierarchy<BoxCollider>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<SphereCollider>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<CapsuleCollider>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<MeshCollider>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<CharacterController>(selectionRect, gameObject, ref index);
                        // RigidBody
                        DrawIconOnHierarchy<Rigidbody>(selectionRect, gameObject, ref index);
                        // Lights
                        DrawIconOnHierarchy<Light>(selectionRect, gameObject, ref index);
                        // Animation / Animator
                        DrawIconOnHierarchy<Animator>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<Animation>(selectionRect, gameObject, ref index);
                        // Camera / Projector
                        DrawIconOnHierarchy<Camera>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<Projector>(selectionRect, gameObject, ref index);
                        // NavAgent
                        DrawIconOnHierarchy<NavMeshAgent>(selectionRect, gameObject, ref index);
                        DrawIconOnHierarchy<NavMeshObstacle>(selectionRect, gameObject, ref index);
                        // Particle
                        DrawIconOnHierarchy<ParticleSystem>(selectionRect, gameObject, ref index);
                        // Audio
                        DrawIconOnHierarchy<AudioSource>(selectionRect, gameObject, ref index);

                    }
                }
            }
        }
        //绘制图片
        public static void DrawIconOnHierarchy<T>(Rect position, GameObject gameObject, ref int index) where T : Component
        {
            //当这个物体包含这个组件的时候，再进行绘制
            if (gameObject.HasComponent<T>())
            {
                var icon = EditorGUIUtility.ObjectContent(null, typeof(T)).image;
                var rect = CalculationRectMethod(position, ref index);
                GUI.Label(rect, icon);
            }
        }
        //计算下一个图标所在的位置
        public static Rect CalculationRectMethod(Rect selectRect, ref int index)
        {
            var rect = new Rect(selectRect);
            rect.x += rect.width - 18 - (18 * index);
            rect.width = 18;
            index++;
            return rect;
        }
    }
    public static class CheckHasComponent
    {
        //扩展方法，检测当前物体是否含有某个组件
        public static bool HasComponent<T>(this GameObject go) where T : Component
        {
            return go.GetComponent<T>();
        }
    }
}

