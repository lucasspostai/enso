using System.Runtime.InteropServices;
using Enso.CombatSystem;
using Framework.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(Attack))]
    public class AttackEditor : UnityEditor.Editor
    {
        private Attack AttackTarget => target as Attack;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.Separator();

            Undo.RecordObject(AttackTarget, "Attack Properties");

            GUILayout.BeginHorizontal();

            DrawAnimatorStateName();
            DrawAnimatorLayerNumber();

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            //Animation Clips ReorderableList
            clipsList?.DoLayoutList();

            if (AttackTarget.AttackAnimationClipHolder.AnimationClips.Count <= 0 ||
                AttackTarget.AttackAnimationClipHolder.AnimationClips[0] == null)
                return;

            DrawFrameChecker();
            GUILayout.BeginHorizontal();

            DrawHitboxSize();
            DrawMovementOffset();

            GUILayout.EndHorizontal();
        }

        private void DrawAnimatorStateName()
        {
            AttackTarget.AttackAnimationClipHolder.AnimatorStateName =
                EditorGUILayout.TextField(AttackTarget.AttackAnimationClipHolder.AnimatorStateName,
                    Styles.MiddleCenterTextStyle);
        }

        private void DrawAnimatorLayerNumber()
        {
            AttackTarget.AttackAnimationClipHolder.LayerNumber =
                EditorGUILayout.IntField(AttackTarget.AttackAnimationClipHolder.LayerNumber, Styles.MiddleCenterTextStyle,
                    GUILayout.MaxWidth(50));
        }

        #region Animation Clips Reoderable List

        private ReorderableList clipsList;

        private void OnEnable()
        {
            if (AttackTarget.AttackAnimationClipHolder == null)
                return;
            
            clipsList = new ReorderableList(AttackTarget.AttackAnimationClipHolder.AnimationClips,
                typeof(AnimationClip), true, true, false, false);

            if (AttackTarget.AttackAnimationClipHolder.AnimationClips.Count < 8)
            {
                for (int i = 0; AttackTarget.AttackAnimationClipHolder.AnimationClips.Count < 8; i++)
                {
                    AddItem(clipsList);
                }
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

            AttackTarget.AttackAnimationClipHolder.AnimationClips[index] = (AnimationClip) EditorGUI.ObjectField(
                new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight),
                AttackTarget.AttackAnimationClipHolder.AnimationClips[index],
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

        private void DrawFrameChecker()
        {
            float hitFrameStart = AttackTarget.AttackFrameChecker.HitFrameStart;
            float hitFrameEnd = AttackTarget.AttackFrameChecker.HitFrameEnd;

            //Texts
            string totalFramesText =
                "Total Frames: <b>" + AttackTarget.AttackAnimationClipHolder.GetTotalFrames() + "</b>";
            string startUpText = "<color=green>Startup: <b>" + (hitFrameStart) + "</b></color>";
            string activeText = "<color=red>Active: <b>" + (hitFrameEnd - hitFrameStart + 1) + "</b></color>";
            string recoveryText = "<color=blue>Recovery: <b>" +
                                  (AttackTarget.AttackAnimationClipHolder.GetTotalFrames() - hitFrameEnd - 1) +
                                  "</b></color>";
            string hitFrameStartText = "Hit Start: <b>" + AttackTarget.AttackFrameChecker.HitFrameStart + "</b>";
            string hitFrameEndText = "Hit End: <b>" + AttackTarget.AttackFrameChecker.HitFrameEnd + "</b>";
            string canCutFrameText = "Cut Frame: <b>" + AttackTarget.AttackFrameChecker.CanCutFrame + "</b>";

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
                AttackTarget.AttackAnimationClipHolder.GetTotalFrames() - 1);

            AttackTarget.AttackFrameChecker.HitFrameStart = Mathf.RoundToInt(hitFrameStart);
            AttackTarget.AttackFrameChecker.HitFrameEnd = Mathf.RoundToInt(hitFrameEnd);

            GUILayout.BeginHorizontal();
            GUILayout.Label(hitFrameStartText + " | " + hitFrameEndText, Styles.NormalTextCenterStyle);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            //Can Cut Animation Frames
            GUILayout.BeginVertical(Styles.BoxStyle);

            AttackTarget.CanBeCut = GUILayout.Toggle(AttackTarget.CanBeCut, "Can Cut Animation");

            if (AttackTarget.CanBeCut)
            {
                float canCutFrame = GUILayout.HorizontalSlider(
                    AttackTarget.AttackFrameChecker.CanCutFrame,
                    0,
                    AttackTarget.AttackAnimationClipHolder.GetTotalFrames() - 1);

                AttackTarget.AttackFrameChecker.CanCutFrame = Mathf.RoundToInt(canCutFrame);

                GUILayout.Label(canCutFrameText, Styles.NormalTextCenterStyle);
            }

            GUILayout.EndVertical();
        }

        private void DrawHitboxSize()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Hitbox Size", Styles.NormalTextLeftStyle);

            AttackTarget.HitboxSize = EditorGUILayout.Vector2Field("", AttackTarget.HitboxSize);

            GUILayout.EndVertical();
        }

        private void DrawMovementOffset()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Movement Offset", Styles.NormalTextLeftStyle);

            AttackTarget.MovementOffset = EditorGUILayout.FloatField(AttackTarget.MovementOffset);

            GUILayout.EndVertical();
        }
    }
}