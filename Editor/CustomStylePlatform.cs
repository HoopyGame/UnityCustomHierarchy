using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DD_Tools
{
    public class CustomStylePlatform : ScriptableObject
    {
        //启用Font自定义
        public bool EnableFontStyleFunction = true;
        //启用Icon自定义
        public bool EnableIconStyleFuntion = true;
        //有多少配置文件
        public List<CustomStyle> CustomStyles = new List<CustomStyle>();

        [Conditional("UNITY_EDITOR")]
        //创建一个Asset文件
        [MenuItem("DD_Tools/CreateStylePlatform")]
        public static void CreateStylePlatform()
        {
            var stylePlatform = ScriptableObject.CreateInstance<CustomStylePlatform>();
            var path = Application.dataPath + "/DD_Tools/CustomStyle";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            AssetDatabase.CreateAsset(stylePlatform, "Assets/DD_Tools/CustomStyle/CustomStylePlatform.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [System.Serializable]
    public class CustomStyle
    {
        //标识符
        public string nameKey;
        //字体颜色
        public Color fontColor;
        //背景颜色
        public Color backgroundColor;
        //字体锚定
        public TextAnchor textAnchor;
        //字体样式
        public FontStyle fontStyle;
    }
}