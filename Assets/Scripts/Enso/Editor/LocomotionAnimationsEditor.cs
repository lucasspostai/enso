using Enso.CombatSystem;
using Framework.Animations;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(LocomotionAnimations))]
    public class LocomotionAnimationsEditor : UnityEditor.Editor
    {
        private LocomotionAnimations LocomotionAnimationsTarget => target as LocomotionAnimations;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Separator();
            
            Undo.RecordObject(LocomotionAnimationsTarget, "Locomotion Properties");
            
            EditorGUI.BeginChangeCheck();

            DrawFaceDirectionsStrings();
            
            EditorGUILayout.Separator();
            
            GUILayout.Label("Animations", EditorStyles.miniLabel);
            DrawElements(LocomotionAnimationsTarget.IdleAnimationClipHolder, "Idle");
            DrawElements(LocomotionAnimationsTarget.WalkAnimationClipHolder, "Walk");
            DrawElements(LocomotionAnimationsTarget.RunAnimationClipHolder, "Run");
            DrawElements(LocomotionAnimationsTarget.SprintAnimationClipHolder, "Sprint");
            
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(target);
        }

        private void DrawFaceDirectionsStrings()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);
            
            GUILayout.Label("Directions X and Y", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            LocomotionAnimationsTarget.FaceX = EditorGUILayout.TextField(LocomotionAnimationsTarget.FaceX,
                Styles.MiddleCenterTextStyle);
            LocomotionAnimationsTarget.FaceY = EditorGUILayout.TextField(LocomotionAnimationsTarget.FaceY,
                Styles.MiddleCenterTextStyle);
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawElements(AnimationClipHolder animationClipHolder, string animationName)
        {
            if (animationClipHolder.AnimationClips.Count == 0)
            {
                AddEmptyAnimation(animationClipHolder);
            }

            GUILayout.BeginVertical(Styles.BoxStyle);
            GUILayout.Label(animationName, EditorStyles.boldLabel);
            
            GUILayout.BeginHorizontal();

            DrawAnimatorStateName(animationClipHolder);
            DrawAnimatorLayerNumber(animationClipHolder);

            GUILayout.EndHorizontal();
            
            EditorGUILayout.Separator();

            if (animationClipHolder.AnimationClips.Count > 0)
                DrawAnimationClip(animationClipHolder);
            
            GUILayout.EndVertical();
        }

        private void DrawAnimatorStateName(AnimationClipHolder animationClipHolder)
        {
            animationClipHolder.AnimatorStateName =
                EditorGUILayout.TextField(animationClipHolder.AnimatorStateName,
                    Styles.MiddleCenterTextStyle);
        }

        private void DrawAnimatorLayerNumber(AnimationClipHolder animationClipHolder)
        {
            animationClipHolder.LayerNumber =
                EditorGUILayout.IntField(animationClipHolder.LayerNumber, Styles.MiddleCenterTextStyle,
                    GUILayout.MaxWidth(50));
        }

        private void DrawAnimationClip(AnimationClipHolder animationClipHolder)
        {
            animationClipHolder.AnimationClips[0] =
                (AnimationClip) EditorGUILayout.ObjectField(animationClipHolder.AnimationClips[0],
                    typeof(AnimationClip), false);
        }

        private void AddEmptyAnimation(AnimationClipHolder animationClipHolder)
        {
            if (animationClipHolder.AnimationClips.Count > 0)
                return;
            
            animationClipHolder.AnimationClips.Add(null);
            EditorUtility.SetDirty(target);
        }
    }
}
