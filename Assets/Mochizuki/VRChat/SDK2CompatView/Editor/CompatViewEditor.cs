/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Mochizuki.VRChat.Extensions.Convenience;
using Mochizuki.VRChat.Extensions.Unity;
using Mochizuki.VRChat.SDK2CompatView.Internal;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mochizuki.VRChat.SDK2CompatView
{
    public class CompatViewEditor : EditorWindow
    {
        private const string Version = "0.1.0";
        private const string Product = "Mochizuki SDK2 Compat View";
        private static readonly Regex UnityStripped = new Regex(@"--- !u!\d+ &\d+ stripped");
        private static readonly VersionManager Manager;

        private Object _object;
        private Vector2 _scroll = Vector2.zero;

        static CompatViewEditor()
        {
            Manager = new VersionManager("mika-f/VRChat-SDK2CompatView", Version, new Regex("v(?<version>.*)"));
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            Manager.CheckNewVersion();
        }

        [MenuItem("Mochizuki/VRChat/SDK2 Compat View/Documents")]
        public static void ShowDocuments()
        {
            Process.Start("https://docs.mochizuki.moe/VRChat/SDK2CompatView/");
        }

        [MenuItem("Mochizuki/VRChat/SDK2 Compat View/Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<CompatViewEditor>();
            window.titleContent = new GUIContent("SDK2 Compat View");

            window.Show();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            EditorStyles.label.wordWrap = true;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"{Product} - {Version}");
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                EditorGUILayout.LabelField("VRCSDK2 で作成された各アセットの中身を閲覧できる Editor 拡張です。");

            EditorGUI.BeginChangeCheck();
            _object = EditorGUILayoutExtensions.ObjectPicker("VRChat SDK2 Asset", _object);

            if (EditorGUI.EndChangeCheck())
                _scroll = Vector2.zero;

            EditorGUILayout.Space();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            if (_object != null)
                switch (_object)
                {
                    // VRC Animator Override
                    case AnimatorOverrideController o:
                        ShowGui(o);
                        break;

                    // VRC Avatar Descriptor
                    case GameObject o:
                        ShowGui(o);
                        break;
                }

            if (Manager.HasNewVersion)
                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    EditorGUILayout.LabelField($"{Product} の新しいバージョンがリリースされています。");
                    if (GUILayout.Button("BOOTH からダウンロード"))
                        Process.Start("https://natsuneko.booth.pm/items/2315841");
                }

            EditorGUILayout.EndScrollView();
        }

        private static void ShowGui(AnimatorOverrideController o)
        {
            EditorGUILayout.LabelField("Show as Animator Override Controller");
            using (var sr = new StreamReader(AssetDatabase.GetAssetPath(o)))
            {
                var yr = new YamlReader(sr.ReadToEnd());
                var rt = yr.FindProperty("AnimatorOverrideController");

                var size = (int) rt.FindRelative("m_Clips").ArraySize;
                var overrides = new List<KeyValuePair<uint, Motion>>();

                for (var i = 0; i < size; i++)
                {
                    var fileId = rt.GetRelativeValueAs<long>($"m_Clips.{i}.m_OriginalClip.fileID");
                    var guid = rt.GetRelativeValueAs<string>($"m_Clips.{i}.m_OverrideClip.guid");

                    var motion = AssetDatabaseExtensions.LoadAssetFromGuid<AnimationClip>(guid);
                    if (motion != null)
                        overrides.Add(new KeyValuePair<uint, Motion>((uint) fileId, motion));
                }

                Motion FindOverride(uint id)
                {
                    return overrides.Find(w => w.Key == id).Value;
                }

                EditorGUILayoutExtensions.ReadonlyObjectPicker("CROUCHIDLE", FindOverride(7400026));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("CROUCHWALKFWD", FindOverride(7400024));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("CROUCHWALKRT", FindOverride(7400028));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE1", FindOverride(7400006));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE2", FindOverride(7400008));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE3", FindOverride(7400010));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE4", FindOverride(7400012));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE5", FindOverride(7400014));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE6", FindOverride(7400016));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE7", FindOverride(7400018));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("EMOTE8", FindOverride(7400020));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("FALL", FindOverride(7400022));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("FINGERPOINT", FindOverride(7400054));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("FIST", FindOverride(7400052));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("HANDGUN", FindOverride(7400064));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("HANDOPEN", FindOverride(7400058));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("IDLE", FindOverride(7400002));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("PROWNEFWD", FindOverride(7400066));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("PROWNEIDLE", FindOverride(7400004));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("ROCKNROLL", FindOverride(7400056));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("RUNBACK", FindOverride(7400036));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("RUNFWD", FindOverride(7400032));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("RUNSTRAFELT135", FindOverride(7400070));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("RUNSTRAFELT45", FindOverride(7400048));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("RUNSTRAFERT135", FindOverride(7400072));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("RUNSTRAFERT45", FindOverride(7400050));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("SPRINTFWD", FindOverride(7400030));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("STRAFELT135", FindOverride(7400040));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("STRAFELT45", FindOverride(7400044));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("STRAFERT", FindOverride(7400038));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("STRAFERT135", FindOverride(7400042));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("STRAFERT45", FindOverride(7400046));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("THUMBSUP", FindOverride(7400060));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("VICTORY", FindOverride(7400062));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("WALKBACK", FindOverride(7400068));
                EditorGUILayoutExtensions.ReadonlyObjectPicker("WALKFWD", FindOverride(7400034));
            }
        }

        private static void ShowGui(GameObject o)
        {
            // Check the GameObject has Missing Script
            if (o.GetComponents<MonoBehaviour>().All(w => w != null))
                return;

            var path = string.IsNullOrWhiteSpace(AssetDatabase.GetAssetPath(o)) ? SceneManager.GetActiveScene().path : AssetDatabase.GetAssetPath(o);

            using (var sr = new StreamReader(path))
            {
                var source = sr.ReadToEnd();

                // load YAML with stripped invalid tags
                // see also : https://github.com/chyh1990/yaml-rust/issues/141
                var yr = new YamlReader(UnityStripped.Replace(source, ""));
                var behaviours = yr.FindBy1stKey("MonoBehaviour").Select(w => w.FindRelative("MonoBehaviour"));
                var avatarDescriptor = behaviours.FirstOrDefault(w =>
                {
                    var component = w.FindRelative("m_Script");
                    return component.GetRelativeValueAs<long>("fileID") == -1122756102 && component.GetRelativeValueAs<string>("guid") == "f78c4655b33cb5741983dc02e08899cf";
                });

                if (avatarDescriptor == null)
                    return;

                using (new DisabledGroup(true))
                {
                    if (!EditorGUIUtility.wideMode)
                    {
                        EditorGUIUtility.wideMode = true;
                        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
                    }

                    EditorGUILayout.LabelField("Show as VRC Avatar Descriptor");

                    var x = (float) avatarDescriptor.GetRelativeValueAs<double>("ViewPosition.x");
                    var y = (float) avatarDescriptor.GetRelativeValueAs<double>("ViewPosition.y");
                    var z = (float) avatarDescriptor.GetRelativeValueAs<double>("ViewPosition.z");
                    EditorGUILayout.Vector3Field("View Position", new Vector3(x, y, z));

                    var animation = avatarDescriptor.GetRelativeValueAs<long>("Animations");
                    EditorGUILayout.Popup("Default Animation Set", (int) animation, new[] { "Male", "Female", "None" });

                    var customStandingAnim = avatarDescriptor.GetRelativeValueAs<string>("CustomStandingAnims.guid");
                    EditorGUILayoutExtensions.ReadonlyObjectPicker("Custom Standing Anims", AssetDatabaseExtensions.LoadAssetFromGuid<AnimatorOverrideController>(customStandingAnim));

                    var customSittingAnim = avatarDescriptor.GetRelativeValueAs<string>("CustomSittingAnims.guid");
                    EditorGUILayoutExtensions.ReadonlyObjectPicker("Custom Standing Anims", AssetDatabaseExtensions.LoadAssetFromGuid<AnimatorOverrideController>(customSittingAnim));

                    var isScaleIpd = avatarDescriptor.GetRelativeValueAs<long>("ScaleIPD") == 1;
                    EditorGUILayout.Toggle("Scale IPD", isScaleIpd);

                    var lipSync = avatarDescriptor.GetRelativeValueAs<long>("lipSync");
                    EditorGUILayout.Popup("Lip Sync", (int) lipSync, new[] { "Default", "Jaw Flap Bone", "Jaw Flap Blend Shape", "Viseme Blend Shape", "Viseme Parameter Only" });

                    switch (lipSync)
                    {
                        // default
                        case 0:

                            break;

                        // Jaw Flap Bone
                        case 1:
                            EditorGUILayout.LabelField(new GUIContent("Jaw Flap Bone"), new GUIContent("Not Supported"));
                            break;

                        // Jaw Flap Blend Shape
                        case 2:
                            EditorGUILayout.LabelField(new GUIContent("Jaw Flap Blend Shape"), new GUIContent("Not Supported"));
                            break;

                        // Viseme Blend Shape
                        case 3:
                            EditorGUILayout.LabelField(new GUIContent("Face Mesh"), new GUIContent("Not Supported"));
                            EditorGUILayout.Popup("Viseme: sil", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.0") });
                            EditorGUILayout.Popup("Viseme: PP", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.1") });
                            EditorGUILayout.Popup("Viseme: FF", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.2") });
                            EditorGUILayout.Popup("Viseme: TH", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.3") });
                            EditorGUILayout.Popup("Viseme: DD", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.4") });
                            EditorGUILayout.Popup("Viseme: kk", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.5") });
                            EditorGUILayout.Popup("Viseme: CH", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.6") });
                            EditorGUILayout.Popup("Viseme: SS", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.7") });
                            EditorGUILayout.Popup("Viseme: nn", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.8") });
                            EditorGUILayout.Popup("Viseme: RR", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.9") });
                            EditorGUILayout.Popup("Viseme: aa", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.10") });
                            EditorGUILayout.Popup("Viseme: E", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.11") });
                            EditorGUILayout.Popup("Viseme: ih", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.12") });
                            EditorGUILayout.Popup("Viseme: oh", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.13") });
                            EditorGUILayout.Popup("Viseme: ou", 0, new[] { avatarDescriptor.GetRelativeValueAs<string>("VisemeBlendShapes.14") });
                            break;

                        // Viseme Parameter Only
                        case 4:
                            break;
                    }
                }
            }
        }
    }
}