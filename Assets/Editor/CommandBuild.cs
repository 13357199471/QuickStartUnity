using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;


public class CommandBuild
{
    public static void BuildIOS() {
        string[] scenes = { "Assets/Demo/Scenes/QuickStart.unity" };
        EditorUserBuildSettings.development = false;
        string XCODE_EXPORTPATH = "./build/ios";
        BuildPipeline.BuildPlayer(scenes, XCODE_EXPORTPATH, BuildTarget.iOS, BuildOptions.None);
    }

    public static void BuildAndroid() {
        string[] scenes = { "Assets/Demo/Scenes/QuickStart.unity" };
        string ANDROID_EXPORTPATH = "./build/android/VideoSDKDemo.apk";
        BuildPipeline.BuildPlayer(scenes, ANDROID_EXPORTPATH, BuildTarget.Android, BuildOptions.None);
    }

    public static void BuildWindows64() {
        string[] scenes = { "Assets/Demo/Scenes/QuickStart.unity" };
        string WINDOWS_EXPORTPATH = "./build/windows/VideoSDKDemo64.exe";
        BuildPipeline.BuildPlayer(scenes, WINDOWS_EXPORTPATH, BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
