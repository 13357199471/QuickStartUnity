using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using System.IO;

public static class XCodePostProcess
{
    private static void AddEmbedFramework(PBXProject project, string targetGuid, string frameworkName) 
    {
#if !UNITY_2019_3_OR_NEWER
        const string defaultLocationInProj = "Frameworks/RTCVideo/Plugins/iOS";
        string framework = Path.Combine(defaultLocationInProj, frameworkName);
        string fileGuid = project.AddFile(framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
        PBXProjectExtensions.AddFileToEmbedFrameworks(project, targetGuid, fileGuid);
#endif
    }

    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if(buildTarget == BuildTarget.iOS)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
#if !UNITY_2019_3_OR_NEWER
            string mainTarget = PBXProject.GetUnityTargetName();
#else
            string mainTarget = pbxProject.GetUnityMainTargetGuid();
#endif
            string targetGuid = pbxProject.TargetGuidByName(mainTarget);
            pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            pbxProject.SetBuildProperty(targetGuid, "CODE_SIGN_STYLE", "Manual");
            pbxProject.SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", "XXHND5J98K");
            pbxProject.SetBuildProperty(targetGuid, "CODE_SIGN_IDENTITY", "iPhone Distribution: Beijing Bytedance Technology Co., Ltd");
            pbxProject.SetBuildProperty(targetGuid, "CODE_SIGN_IDENTITY[sdk=iphoneos*]", "iPhone Distribution: Beijing Bytedance Technology Co., Ltd");
            pbxProject.SetBuildProperty(targetGuid, "PROVISIONING_PROFILE_SPECIFIER", "RTCUnityDist");
            pbxProject.SetBuildProperty(targetGuid, "PRODUCT_BUNDLE_IDENTIFIER", "com.bytedance.videoarch.rtc.unity");
            //pbxProject.SetBuildProperty(targetGuid, "PRODUCT_BUNDLE_IDENTIFIER", "com.bytertc.rtc.rtcdemo");
            pbxProject.SetBuildProperty(targetGuid, "PRODUCT_NAME", "VideoSDKDemo");
       
       
            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCCWrapper.framework");

            AddEmbedFramework(pbxProject, targetGuid, "VolcEngineRTC.framework");
            AddEmbedFramework(pbxProject, targetGuid, "VolcEngineRTCScreenCapturer.framework");
            AddEmbedFramework(pbxProject, targetGuid, "effect-sdk.framework");

            AddEmbedFramework(pbxProject, targetGuid, "bmf_mods_shared.framework");
            AddEmbedFramework(pbxProject, targetGuid, "bytenn.framework");
            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCFFmpegAudioExtension.framework");
            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCNICOExtension.framework");
            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCVideoDenoiseExtension.framework");

            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCVideoSharpenExtension.framework");

            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCVideoSRExtension.framework");
            AddEmbedFramework(pbxProject, targetGuid, "ByteRTCVp8CodecExtension.framework");
            AddEmbedFramework(pbxProject, targetGuid, "h265enc.framework");
            AddEmbedFramework(pbxProject, targetGuid, "RealXBase.framework");
            AddEmbedFramework(pbxProject, targetGuid, "bdaudioeffect.framework");



            pbxProject.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
            pbxProject.WriteToFile (projectPath);

            // 修改plist
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;
            rootDict.SetString("NSMicrophoneUsageDescription", "App需要您的同意,才能访问麦克风");
            rootDict.SetString("NSCameraUsageDescription", "App需要您的同意,才能访问相机");
            rootDict.SetString("CFBundleIdentifier", "${PRODUCT_BUNDLE_IDENTIFIER}");
            PlistElementArray backgroundModesArray = rootDict.CreateArray("UIBackgroundModes");
            backgroundModesArray.AddString("audio");
            plist.WriteToFile(plistPath);
        }
    }
}