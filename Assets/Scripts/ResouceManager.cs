using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UObject = UnityEngine.Object;
using UnityEngine.UIElements;
//using UnityEngine.UI;


public class ResouceManager : MonoBehaviour
{
    internal class BundleInfo
    {
        // unity 根目录
        public string AssetsName;
        // 需要打bundle的目录
        public string BundleName;
        // bundle 输出目录
        public List<string> Dependences;
    }
    // 存放 Bundle 信息的集合
    private Dictionary<string, BundleInfo> _BundleIbfos = new Dictionary<string, BundleInfo>(); 

    /// <summary>
    ///  解析版本文件
    /// </summary>
    private void ParseVersionFile()

    {
        // 版本文件路径
        string url = Path.Combine(PathUtil.BundleResourcePath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);  //  读取所有文件路径

        // ??????????
        for (int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split("|");
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];

            // List本质是数组，但是可以动态扩容，   增删查改方便
            bundleInfo.Dependences = new List<string>(info.Length - 2);
            for (int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }

            _BundleIbfos.Add(bundleInfo.AssetsName, bundleInfo);
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="action">完成回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName, Action<UObject> action = null)
    {
        string bundleName = _BundleIbfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BundleResourcePath, bundleName);
        List<string> dependences = _BundleIbfos[assetName].Dependences;
        if(dependences != null && dependences.Count > 0)
        {
            for (int i = 0;i < dependences.Count;i++)
            {
                yield return LoadBundleAsync(dependences[i]); // 递归加载
            }
        }

        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;

        //if (action != null && bundleRequest != null)
        //{
        //    action.Invoke(bundleRequest.asset);
        //}
        // 简写空判断
        action?.Invoke(bundleRequest?.asset);
    }

    public void LoadAssets(string assetsName, Action<UObject> action)
    {
        StartCoroutine(LoadBundleAsync(assetsName, action));
    }


    private void Start()
    {
        ParseVersionFile(); // 解析版本文件
    
        Debug.Log("shhd");

        LoadAssets("Assets/BuildResources/UI/Prafabs/TestUI.prefab", OnComplete); 


         
    }

    private void OnComplete(UObject obj)
    {
        GameObject go = Instantiate(obj) as GameObject;
        go.transform.SetParent(transform, false);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;

    }
} 
