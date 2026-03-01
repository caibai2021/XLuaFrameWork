using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuildTool : Editor
{
    
    
    
    [MenuItem("Tools/Build Windows Bundle")]   // 多平台构建  target

    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);  // windows构建
    }

    [MenuItem("Tools/Build Android Bundle")]
    static void BundleWindowsAndroidBuild()
    {
        Build(BuildTarget.Android); // 安卓 包  构建
    }

    [MenuItem("Tools/Build iOS Bundle")]
    static void BundleiOSBuild()
    {
        Build(BuildTarget.iOS);  //   ios  构建
    }


    static void Build(BuildTarget target)
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
        
        // 文件信息列表
        List<string> bundleInfos = new List<string>();

        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories); // 递归查找所有子目录文件夹

        //  需要排除meta文件

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;  // 排除meta
                //Debug.LogWarning("meta排除");
            }


            AssetBundleBuild assetBundle = new AssetBundleBuild(); // unity相对目录

            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file: " + fileName);

            string assestName = PathUtil.GetUnityPath(fileName);
            assetBundle.assetNames = new string[] { assestName };
            string bundleName = fileName.Replace(PathUtil.BuildResourcesPath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);

            // 添加文件和依赖信息
            List<string> dependenceInfo = GetDependence(assestName);
            string bundleInfo = assestName + "|" + bundleName + ".ab";

            if (dependenceInfo.Count > 0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);
            }

            bundleInfos.Add(bundleInfo);
        }


        if (Directory.Exists(PathUtil.BundleOutPath))
        {
            Directory.Delete(PathUtil.BundleOutPath, true);
        }

        Directory.CreateDirectory(PathUtil.BundleOutPath);

        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        
        // 将依赖文件信息保存
        File.WriteAllLines(PathUtil.BundleOutPath + "/" + AppConst.FileListName, bundleInfos);
        AssetDatabase.Refresh();

    }

    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);

        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
