using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathUtil 
{
    // 根目录
    public static readonly string AssetsPath = Application.dataPath;

    // 需要打 bundle 的目录
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources/";

    // bundle 输出目录
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        return path.Substring(path.IndexOf("Assets"));
    }

    public static string GetStandardPath(string path)
    {
        if (string.IsNullOrEmpty(path)) { return string.Empty; }

        return path.Trim().Replace("\\", "/");
    }
}
