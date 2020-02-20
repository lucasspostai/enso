using System;
using System.Reflection;
using Framework.Audio;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

namespace Framework.Editor
{
    [CustomEditor(typeof(SoundCue))]
    public class SoundCueEditor : UnityEditor.Editor
    {
        private SoundCue SoundCueTarget => target as SoundCue;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            Undo.RecordObject(SoundCueTarget, "Sound Cue Properties");

            DrawPlayerButtons();

            EditorGUILayout.Separator();

            //Audio Clips ReorderableList
            clipsList.DoLayoutList();

            DrawAudioMixerGroupArea();
            DrawVolumeArea();
            DrawPitchArea();
            DrawSpatialBlendArea();

            EditorUtility.SetDirty(SoundCueTarget);
        }

        #region Audio Clip Reoderable List

        private ReorderableList clipsList;

        private void OnEnable()
        {
            clipsList = new ReorderableList(SoundCueTarget.Clips, typeof(AudioClip), true, true, true, true);

            clipsList.drawHeaderCallback += DrawHeader;
            clipsList.drawElementCallback += DrawElement;

            clipsList.onAddCallback += AddItem;
            clipsList.onRemoveCallback += RemoveItem;
        }

        private void OnDisable()
        {
            clipsList.drawHeaderCallback -= DrawHeader;
            clipsList.drawElementCallback -= DrawElement;

            clipsList.onAddCallback -= AddItem;
            clipsList.onRemoveCallback -= RemoveItem;
        }

        /// Draws the header of the Reorderable List
        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Audio Clips");
        }

        /// Draws one element of the Reorderable List
        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            EditorGUI.BeginChangeCheck();

            SoundCueTarget.Clips[index] = (AudioClip) EditorGUI.ObjectField(
                new Rect(rect.x, rect.y + 2f, rect.width, EditorGUIUtility.singleLineHeight),
                SoundCueTarget.Clips[index],
                typeof(AudioClip),
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

        private void DrawPlayerButtons()
        {
            GUILayout.BeginHorizontal();

            SoundCueTarget.Loop = GUILayout.Toggle(SoundCueTarget.Loop, "Loop", Styles.BigButtonLeft, GUILayout.Height(50));

            if (GUILayout.Button("Play", Styles.BigButtonMid, GUILayout.Height(50)))
            {
                PlayClip(SoundCueTarget.GetClip());
            }

            if (GUILayout.Button("Stop", Styles.BigButtonRight, GUILayout.Height(50)))
            {
                StopAllClips();
            }

            GUILayout.EndHorizontal();
        }
        
        private void DrawAudioMixerGroupArea()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Audio Mixer Group", EditorStyles.whiteLabel);

            SoundCueTarget.MixerGroup = (AudioMixerGroup) EditorGUILayout.ObjectField(
                SoundCueTarget.MixerGroup,
                typeof(AudioMixerGroup),
                false
            );

            GUILayout.EndVertical();
        }

        private void DrawVolumeArea()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Volume", EditorStyles.whiteLabel);

            SoundCueTarget.Volume = EditorGUILayout.Slider(SoundCueTarget.Volume, 0f, 1f);

            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width - 85f));

            GUILayout.Label("0", Styles.MiniTextLeftStyle);
            GUILayout.Label("1", Styles.MiniTextRightStyle);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DrawPitchArea()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Pitch Variation", EditorStyles.whiteLabel);
            EditorGUILayout.MinMaxSlider(ref SoundCueTarget.MinPitch, ref SoundCueTarget.MaxPitch, 0.5f, 1.5f);

            SoundCueTarget.MinPitch = (float) Math.Round(SoundCueTarget.MinPitch, 2);
            SoundCueTarget.MaxPitch = (float) Math.Round(SoundCueTarget.MaxPitch, 2);

            GUILayout.BeginHorizontal();

            GUILayout.Label("0.5", Styles.MiniTextLeftStyle);
            GUILayout.Label($"{SoundCueTarget.MinPitch:0.00} ~ {SoundCueTarget.MaxPitch:0.00}", Styles.MiniTextCenterStyle);
            GUILayout.Label("1.5", Styles.MiniTextRightStyle);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void DrawSpatialBlendArea()
        {
            GUILayout.BeginVertical(Styles.BoxStyle);

            GUILayout.Label("Spatial Blend", EditorStyles.whiteLabel);
            SoundCueTarget.SpatialBlend = EditorGUILayout.Slider(SoundCueTarget.SpatialBlend, 0f, 1f);

            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width - 85f));

            GUILayout.Label("2D", Styles.MiniTextLeftStyle);
            GUILayout.Label("3D", Styles.MiniTextRightStyle);

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        #region Audio Player Handler

        private static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null);

            method.Invoke(null, new object[] { clip, startSample, loop });
        }

        private static void StopAllClips()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { },
                null
            );
            method?.Invoke(
                null,
                new object[] { }
            );
        }

        #endregion
    }
}