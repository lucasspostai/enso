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

            GUILayout.BeginHorizontal();
            
            DrawHitboxSize();
            DrawAttackType();
            DrawDamage();

            GUILayout.EndHorizontal();
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