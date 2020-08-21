/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

using Mochizuki.VRChat.Extensions.Convenience;
using Mochizuki.VRChat.Extensions.Unity;
using Mochizuki.VRChat.VRC2CompatView.Internal;

using UnityEditor;

using UnityEngine;

namespace Mochizuki.VRChat.VRC2CompatView
{
    public class CompatViewEditor : EditorWindow
    {
        private const string Version = "0.0.0";
        private const string Product = "Mochizuki VRC2 Compat View";
        private static readonly VersionManager Manager;

        private Object _object;

        static CompatViewEditor()
        {
            Manager = new VersionManager("mika-f/VRChat-VRC2CompatView", Version, new Regex("v(?<version>.*)"));
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            Manager.CheckNewVersion();
        }

        [MenuItem("Mochizuki/VRChat/VRC2 Compat View/Documents")]
        public static void ShowDocuments()
        {
            Process.Start("https://docs.mochizuki.moe/VRChat/VRC2CompatView/");
        }

        [MenuItem("Mochizuki/VRChat/VRC2 Compat View/Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<CompatViewEditor>();
            window.titleContent = new GUIContent("VRC2 Compat View");

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

            _object = EditorGUILayoutExtensions.ObjectPicker("VRChat SDK2 Asset", _object);

            EditorGUILayout.Space();

            switch (_object)
            {
                case AnimatorOverrideController o:
                    ShowGui(o);
                    break;
            }
        }

        private static void ShowGui(AnimatorOverrideController o)
        {
            EditorGUILayout.LabelField("Show as Animator Override Controller");
            using (var sr = new StreamReader(AssetDatabase.GetAssetPath(o)))
            {
                using (var yr = new YamlReader(sr.ReadToEnd()))
                {
                    using (var r = yr.FindProperty("AnimatorOverrideController"))
                    {
                        ulong GetSize()
                        {
                            using (var c = r.FindRelative("m_Clips"))
                                return c.ArraySize;
                        }

                        var size = (int) GetSize();
                        var overrides = new List<KeyValuePair<uint, Motion>>();

                        for (var i = 0; i < size; i++)
                        {
                            var fileId = r.GetRelativeValueAs<long>($"m_Clips.{i}.m_OriginalClip.fileID");
                            var guid = r.GetRelativeValueAs<string>($"m_Clips.{i}.m_OverrideClip.guid");

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
            }
        }
    }
}