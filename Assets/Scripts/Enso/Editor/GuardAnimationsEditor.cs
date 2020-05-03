using Enso.CombatSystem;
using Framework.Animations;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(GuardAnimations))]
    public class GuardAnimationsEditor : UnityEditor.Editor
    {
        private GuardAnimations GuardAnimationsTarget => target as GuardAnimations;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Separator();
            
            GUILayout.Label("Guard", EditorStyles.miniLabel);
            DrawElements(GuardAnimationsTarget.StartGuardAnimationClipHolder, "Start Guard");
            DrawElements(GuardAnimationsTarget.GuardIdleAnimationClipHolder, "Guard Idle");
            DrawElements(GuardAnimationsTarget.EndGuardAnimationClipHolder, "End Guard");
            
            EditorGUILayout.Separator();
            
            GUILayout.Label("Walk", EditorStyles.miniLabel);
            DrawElements(GuardAnimationsTarget.ForwardGuardWalkAnimationClipHolder, "Guard Walk Forward");
            DrawElements(GuardAnimationsTarget.BackwardGuardWalkAnimationClipHolder, "Guard Walk Backward");
            DrawElements(GuardAnimationsTarget.LeftGuardWalkAnimationClipHolder, "Guard Walk Left");
            DrawElements(GuardAnimationsTarget.RightGuardWalkAnimationClipHolder, "Guard Walk Right");
            
            EditorGUILayout.Separator();
            
            GUILayout.Label("Actions", EditorStyles.miniLabel);
            DrawElements(GuardAnimationsTarget.BlockAnimationClipHolder, "Block");
            DrawElements(GuardAnimationsTarget.ParryAnimationClipHolder, "Parry");
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

            if (GuardAnimationsTarget.StartGuardAnimationClipHolder.AnimationClips.Count > 0)
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