using System.Runtime.InteropServices;
using Enso.CombatSystem;
using Enso.Enums;
using Framework.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(AttackAnimation))]
    public class AttackEditor : CharacterAnimationEditor
    {
        private AttackAnimation AttackAnimationTarget => target as AttackAnimation;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            Undo.RecordObject(AttackAnimationTarget, "Attack Properties");

            EditorGUILayout.Separator();
            
            if (CharacterAnimationTarget.ClipHolder.AnimationClips.Count <= 0 ||
                CharacterAnimationTarget.ClipHolder.AnimationClips[0] == null)
                return;
            
            DrawHitBoxFrames();
        }
        
        private void DrawHitBoxFrames()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            CharacterAnimationTarget.AnimationFrameChecker.DealDamage =
                GUILayout.Toggle(CharacterAnimationTarget.AnimationFrameChecker.DealDamage, "Deal Damage");

            if (CharacterAnimationTarget.AnimationFrameChecker.DealDamage)
            {
                float hitFrameStart = CharacterAnimationTarget.AnimationFrameChecker.StartHitFrame;
                float hitFrameEnd = CharacterAnimationTarget.AnimationFrameChecker.EndHitFrame;

                //Texts
                string totalFramesText =
                    "Total Frames: <b>" + CharacterAnimationTarget.ClipHolder.GetTotalFrames() + "</b>";
                string startUpText = "<color=green>Startup: <b>" + (hitFrameStart) + "</b></color>";
                string activeText = "<color=red>Active: <b>" + (hitFrameEnd - hitFrameStart + 1) + "</b></color>";
                string recoveryText = "<color=blue>Recovery: <b>" +
                                      (CharacterAnimationTarget.ClipHolder.GetTotalFrames() - hitFrameEnd - 1) +
                                      "</b></color>";
                string hitFrameStartText =
                    "Hit Start: <b>" + CharacterAnimationTarget.AnimationFrameChecker.StartHitFrame + "</b>";
                string hitFrameEndText =
                    "Hit End: <b>" + CharacterAnimationTarget.AnimationFrameChecker.EndHitFrame + "</b>";

                //All Frames
                GUILayout.BeginHorizontal();

                GUILayout.Label(startUpText + " | " + activeText + " | " + recoveryText, Styles.NormalTextLeftStyle);
                GUILayout.Label(totalFramesText, Styles.NormalTextRightStyle);

                GUILayout.EndHorizontal();

                //Hitbox Frames
                GUILayout.BeginVertical(Styles.BoxStyle);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Hitbox Frames", Styles.NormalTextLeftStyle);

                GUILayout.EndHorizontal();
                EditorGUILayout.MinMaxSlider(ref hitFrameStart, ref hitFrameEnd, 0,
                    CharacterAnimationTarget.ClipHolder.GetTotalFrames() - 1);

                CharacterAnimationTarget.AnimationFrameChecker.StartHitFrame = Mathf.RoundToInt(hitFrameStart);
                CharacterAnimationTarget.AnimationFrameChecker.EndHitFrame = Mathf.RoundToInt(hitFrameEnd);

                GUILayout.BeginHorizontal();
                GUILayout.Label(hitFrameStartText + " | " + hitFrameEndText, Styles.NormalTextCenterStyle);
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
                
                GUILayout.BeginHorizontal();
            
                DrawHitboxSize();
                DrawAttackType();
                DrawDamage();

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        private void DrawHitboxSize()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);
        
            GUILayout.Label("Hitbox Size", Styles.NormalTextLeftStyle);
        
            AttackAnimationTarget.HitboxSize = EditorGUILayout.Vector2Field("", AttackAnimationTarget.HitboxSize);
        
            GUILayout.EndVertical();
        }

        private void DrawAttackType()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);
            
            GUILayout.Label("Attack Type", Styles.NormalTextLeftStyle);
            AttackAnimationTarget.Type = (AttackType)EditorGUILayout.EnumPopup(AttackAnimationTarget.Type);
            
            GUILayout.EndVertical();
        }
        
        private void DrawDamage()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);
            
            GUILayout.Label("Damage", Styles.NormalTextLeftStyle);
            AttackAnimationTarget.Damage = EditorGUILayout.IntField(AttackAnimationTarget.Damage);
            
            GUILayout.EndVertical();
        }
    }
}