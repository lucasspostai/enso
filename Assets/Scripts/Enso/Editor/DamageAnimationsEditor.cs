using Enso.CombatSystem;
using Framework.Animations;
using Framework.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(DamageAnimations))]
    public class DamageAnimationsEditor : UnityEditor.Editor
    {
        private DamageAnimations DamageAnimationsTarget => target as DamageAnimations;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.Separator();
            
            GUILayout.Label("Damage Animations", EditorStyles.miniLabel);
            DrawElements(DamageAnimationsTarget.LoseBalanceAnimationClipHolder, "Lose Balance");
            DrawElements(DamageAnimationsTarget.DamageAnimationClipHolder, "Damage");
            DrawElements(DamageAnimationsTarget.HeavyAnimationClipHolder, "Heavy Damage");
            DrawElements(DamageAnimationsTarget.DeathAnimationClipHolder, "Death");
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

            if (animationClipHolder == DamageAnimationsTarget.DamageAnimationClipHolder)
            {
                clipsList?.DoLayoutList();
            }
            else if (animationClipHolder.AnimationClips.Count > 0)
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
        
        #region Animation Clips Reoderable List

        private ReorderableList clipsList;

        private void OnEnable()
        {
            if (DamageAnimationsTarget.DamageAnimationClipHolder == null)
                return;
            
            clipsList = new ReorderableList(DamageAnimationsTarget.DamageAnimationClipHolder.AnimationClips,
                typeof(AnimationClip), true, true, true, true);

            if (DamageAnimationsTarget.DamageAnimationClipHolder.AnimationClips.Count == 0)
            {
                AddItem(clipsList);
            }

            clipsList.drawHeaderCallback += DrawHeader;
            clipsList.drawElementCallback += DrawElement;

            clipsList.onAddCallback += AddItem;
            clipsList.onRemoveCallback += RemoveItem;
        }

        private void OnDisable()
        {
            if (clipsList == null)
                return;

            clipsList.drawHeaderCallback -= DrawHeader;
            clipsList.drawElementCallback -= DrawElement;

            clipsList.onAddCallback -= AddItem;
            clipsList.onRemoveCallback -= RemoveItem;
        }

        /// Draws the header of the Reorderable List
        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Animation Clips");
        }

        /// Draws one element of the Reorderable List
        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            EditorGUI.BeginChangeCheck();

            DamageAnimationsTarget.DamageAnimationClipHolder.AnimationClips[index] = (AnimationClip) EditorGUI.ObjectField(
                new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight),
                DamageAnimationsTarget.DamageAnimationClipHolder.AnimationClips[index],
                typeof(AnimationClip),
                false
            );

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void AddItem(ReorderableList list)
        {
            clipsList.list.Add(null);

            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list)
        {
            clipsList.list.RemoveAt(list.index);

            EditorUtility.SetDirty(target);
        }

        #endregion
    }
}
