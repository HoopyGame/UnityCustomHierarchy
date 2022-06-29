using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DD_Tools
{
    public class CustomStylePlatform : ScriptableObject
    {
        //����Font�Զ���
        public bool EnableFontStyleFunction = true;
        //����Icon�Զ���
        public bool EnableIconStyleFuntion = true;
        //�ж��������ļ�
        public List<CustomStyle> CustomStyles = new List<CustomStyle>();

        [Conditional("UNITY_EDITOR")]
        //����һ��Asset�ļ�
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
        //��ʶ��
        public string nameKey;
        //������ɫ
        public Color fontColor;
        //������ɫ
        public Color backgroundColor;
        //����ê��
        public TextAnchor textAnchor;
        //������ʽ
        public FontStyle fontStyle;
    }
}