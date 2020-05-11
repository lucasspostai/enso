using Enso.CombatSystem;
using Framework.Audio;
using Framework.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Enso.Editor
{
    [CustomEditor(typeof(CharacterAnimation), true)]
    public class CharacterAnimationEditor : UnityEditor.Editor
    {
        protected CharacterAnimation CharacterAnimationTarget => target as CharacterAnimation;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.Separator();

            Undo.RecordObject(CharacterAnimationTarget, "Animation Properties");

            GUILayout.BeginHorizontal();

            DrawAnimatorStateName();
            DrawAnimatorLayerNumber();

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            //Animation Clips ReorderableList
            clipsList?.DoLayoutList();

            if (CharacterAnimationTarget.ClipHolder.AnimationClips.Count <= 0 ||
                CharacterAnimationTarget.ClipHolder.AnimationClips[0] == null)
                return;

            DrawFrameChecker();
        }

        private void DrawAnimatorStateName()
        {
            CharacterAnimationTarget.ClipHolder.AnimatorStateName =
                EditorGUILayout.TextField(CharacterAnimationTarget.ClipHolder.AnimatorStateName,
                    Styles.MiddleCenterTextStyle);
        }

        private void DrawAnimatorLayerNumber()
        {
            CharacterAnimationTarget.ClipHolder.LayerNumber =
                EditorGUILayout.IntField(CharacterAnimationTarget.ClipHolder.LayerNumber, Styles.MiddleCenterTextStyle,
                    GUILayout.MaxWidth(50));
        }

        #region Animation Clips Reoderable List

        private ReorderableList clipsList;

        private void OnEnable()
        {
            if (CharacterAnimationTarget.ClipHolder == null)
                return;

            clipsList = new ReorderableList(CharacterAnimationTarget.ClipHolder.AnimationClips,
                typeof(AnimationClip), true, true, false, false);

            if (CharacterAnimationTarget.ClipHolder.AnimationClips.Count < 8)
            {
                for (int i = 0; CharacterAnimationTarget.ClipHolder.AnimationClips.Count < 8; i++)
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

            CharacterAnimationTarget.ClipHolder.AnimationClips[index] = (AnimationClip) EditorGUI.ObjectField(
                new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight),
                CharacterAnimationTarget.ClipHolder.AnimationClips[index],
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
            //DrawHitBoxFrames();
            DrawCanCutFrame();
            DrawMovementOffset();
            DrawPlayAudio();
        }

        protected void DrawHitBoxFrames()
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
        }

        private void DrawCanCutFrame()
        {
            string canCutFrameText =
                "Cut Frame: <b>" + CharacterAnimationTarget.AnimationFrameChecker.CanCutFrame + "</b>";

            GUILayout.BeginVertical(Styles.BoxStyle);

            CharacterAnimationTarget.CanBeCut =
                GUILayout.Toggle(CharacterAnimationTarget.CanBeCut, "Can Cut Animation");

            if (CharacterAnimationTarget.CanBeCut)
            {
                float canCutFrame = GUILayout.HorizontalSlider(
                    CharacterAnimationTarget.AnimationFrameChecker.CanCutFrame,
                    0,
                    CharacterAnimationTarget.ClipHolder.GetTotalFrames() - 1);

                CharacterAnimationTarget.AnimationFrameChecker.CanCutFrame = Mathf.RoundToInt(canCutFrame);

                GUILayout.Label(canCutFrameText, Styles.NormalTextCenterStyle);
            }

            GUILayout.EndVertical();
        }

        private void DrawMovementOffset()
        {
            float startMovementOffsetFrame = CharacterAnimationTarget.AnimationFrameChecker.StartMovementFrame;
            float endMovementOffsetFrame = CharacterAnimationTarget.AnimationFrameChecker.EndMovementFrame;

            string startMovementOffsetText = "Offset Start: <b>" +
                                             CharacterAnimationTarget.AnimationFrameChecker.StartMovementFrame + "</b>";
            string endMovementOffsetText =
                "Offset End: <b>" + CharacterAnimationTarget.AnimationFrameChecker.EndMovementFrame + "</b>";

            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Movement Offset", Styles.NormalTextLeftStyle);

            EditorGUILayout.MinMaxSlider(ref startMovementOffsetFrame, ref endMovementOffsetFrame, 0,
                CharacterAnimationTarget.ClipHolder.GetTotalFrames() - 1);

            CharacterAnimationTarget.AnimationFrameChecker.StartMovementFrame =
                Mathf.RoundToInt(startMovementOffsetFrame);
            CharacterAnimationTarget.AnimationFrameChecker.EndMovementFrame = Mathf.RoundToInt(endMovementOffsetFrame);

            GUILayout.BeginHorizontal();
            GUILayout.Label(startMovementOffsetText + " | " + endMovementOffsetText, Styles.NormalTextCenterStyle);
            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Offset:", GUILayout.MaxWidth(50));
            CharacterAnimationTarget.AnimationFrameChecker.MovementOffset =
                EditorGUILayout.FloatField(CharacterAnimationTarget.AnimationFrameChecker.MovementOffset);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DrawPlayAudio()
        {
            string playingAudioFrame =
                "Playing Audio in Frame: <b>" + CharacterAnimationTarget.AnimationFrameChecker.PlayAudioFrame + "</b>";

            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Play Audio", Styles.NormalTextLeftStyle);

            float playAudioFrame = GUILayout.HorizontalSlider(
                CharacterAnimationTarget.AnimationFrameChecker.PlayAudioFrame,
                0,
                CharacterAnimationTarget.ClipHolder.GetTotalFrames() - 1);

            CharacterAnimationTarget.AnimationFrameChecker.PlayAudioFrame = Mathf.RoundToInt(playAudioFrame);

            GUILayout.Label(playingAudioFrame, Styles.NormalTextCenterStyle);

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Sound Cue:", GUILayout.MaxWidth(80));

            CharacterAnimationTarget.AnimationFrameChecker.AnimationSoundCue = (SoundCue) EditorGUILayout.ObjectField(
                CharacterAnimationTarget.AnimationFrameChecker.AnimationSoundCue,
                typeof(SoundCue),
                false
            );
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}