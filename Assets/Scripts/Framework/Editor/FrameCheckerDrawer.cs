using Framework.Animations;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    //[CustomPropertyDrawer(typeof(FrameChecker))]
    public class FrameCheckerDrawer : PropertyDrawer
    {
        /*private const int YDistance = 20;
        private const int FieldHeight = 16;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);

            SerializedProperty hitFrameStart = property.FindPropertyRelative("HitFrameStart");
            SerializedProperty hitFrameEnd = property.FindPropertyRelative("HitFrameEnd");
            SerializedProperty totalFrames = property.FindPropertyRelative("TotalFrames");

            Rect nameRect = new Rect(position.x, position.y, position.width, FieldHeight);
            Rect framesRect = new Rect(position.x, position.y + YDistance, position.width, FieldHeight);
            Rect hitFramesRect = new Rect(position.x, position.y + YDistance * 2, position.width, FieldHeight);
            Rect sliderRect = new Rect(position.x, position.y + YDistance * 3, position.width, FieldHeight);

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(nameRect, property.displayName);
            EditorGUI.indentLevel++;

            FrameRangeSlider(ref hitFrameStart, ref hitFrameEnd, totalFrames.intValue, framesRect, hitFramesRect,
                sliderRect);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return YDistance * 4;
        }

        private void FrameRangeSlider(ref SerializedProperty hitFrameStart, ref SerializedProperty hitFrameEnd,
            int totalFrames, Rect framesRect, Rect hitRect, Rect sliderRect)
        {
            // Must be a float to use the MinMaxSlider
            float start = hitFrameStart.intValue;
            float end = hitFrameEnd.intValue;

            GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            style.richText = true;

            string frames = "<b>" + totalFrames + "</b> frames";
            string startUp = "<color=green>Startup: <b>" + (start - 1) + "</b></color>";
            string active = "<color=red>Active: <b>" + (end - start + 1) + "</b></color>";
            string recovery = "<color=blue>Recovery: <b>" + (totalFrames - end) + "</b></color>";

            EditorGUI.LabelField(framesRect, frames + ": " + startUp + "| " + active + "| " + recovery, style);
            EditorGUI.LabelField(hitRect, "Hits in:  <b>" + start + "</b> to <b>" + end + "</b>", style);

            // and here we add the MinMaxSlider with our values
            EditorGUI.BeginChangeCheck();
            EditorGUI.MinMaxSlider(sliderRect, ref start, ref end, 1, totalFrames);
            if (EditorGUI.EndChangeCheck())
            {
                hitFrameStart.intValue = Mathf.RoundToInt(start);
                hitFrameEnd.intValue = Mathf.RoundToInt(end);
            }
        }*/
    }
}