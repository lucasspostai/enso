using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    public static class Styles
    {
        public static readonly GUIStyle BoxStyle = new GUIStyle("box")
        {
            margin = new RectOffset(5, 5, 4, 4),
            padding = new RectOffset(5, 5, 3, 8),
            richText = true,
            alignment = TextAnchor.MiddleLeft,
            normal = {textColor = Color.grey}
        };
        
        public static readonly GUIStyle MiddleLeftTextStyle = new GUIStyle(EditorStyles.textField)
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 12,
            fixedHeight = 20,
            normal =
            {
                textColor = Color.black
            }
        };
        
        public static readonly GUIStyle MiniTextLeftStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleLeft,
            fontSize = 8,
            fixedHeight = 5,
            normal =
            {
                textColor = Color.grey
            }
        };
        
        public static readonly GUIStyle MiniTextRightStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleRight,
            fontSize = 8,
            fixedHeight = 5,
            normal =
            {
                textColor = Color.grey
            }
        };
        
        public static readonly GUIStyle MiniTextCenterStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 8,
            fixedHeight = 5,
            normal =
            {
                textColor = Color.grey
            }
        };
        
        public static readonly GUIStyle NormalTextCenterStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter
        };
        
        public static readonly GUIStyle NormalTextRightStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleRight
        };
        
        public static readonly GUIStyle NormalTextLeftStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleLeft
        };

        public static readonly GUIStyle BigButtonLeft = new GUIStyle(EditorStyles.miniButtonLeft) {fontSize = 12};
        public static readonly GUIStyle BigButtonMid = new GUIStyle(EditorStyles.miniButtonMid) {fontSize = 12};
        public static readonly GUIStyle BigButtonRight = new GUIStyle(EditorStyles.miniButtonRight) {fontSize = 12};
    }
}
