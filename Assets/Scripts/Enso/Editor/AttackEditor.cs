using System.Runtime.InteropServices;
using Enso.CombatSystem;
using Enso.Enums;
using Framework.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(Attack))]
    public class AttackEditor : CharacterAnimationEditor
    {
        private Attack AttackTarget => target as Attack;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            Undo.RecordObject(AttackTarget, "Attack Properties");

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
        
            AttackTarget.HitboxSize = EditorGUILayout.Vector2Field("", AttackTarget.HitboxSize);
        
            GUILayout.EndVertical();
        }

        private void DrawAttackType()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);
            
            GUILayout.Label("Attack Type", Styles.NormalTextLeftStyle);
            AttackTarget.Type = (AttackType)EditorGUILayout.EnumPopup(AttackTarget.Type);
            
            GUILayout.EndVertical();
        }
        
        private void DrawDamage()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);
            
            GUILayout.Label("Damage", Styles.NormalTextLeftStyle);
            AttackTarget.Damage = EditorGUILayout.IntField(AttackTarget.Damage);
            
            GUILayout.EndVertical();
        }
    }
}