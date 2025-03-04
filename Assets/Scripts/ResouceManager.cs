using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UObject = UnityEngine.Object;

public class ResouceManager : MonoBehaviour
{
    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }
    // 存放 Bundle 信息的集合
    private Dictionary<string, BundleInfo> _BundleIbfos = new Dictionary<string, BundleInfo>(); 

    private void ParseVersionFile()
    {
        // 版本控制
        string url = Path.Combine(PathUtil.BundleResourcePath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);

        // 解析文件信息
        for (int i = 0; i < data.Length; i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split("|");
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];

            // List特性 ：本质是数组，但可以动态扩容
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
                yield return LoadBundleAsync(dependences[i]);
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

        action?.Invoke(bundleRequest?.asset);
    }

    public void LoadAssets(string assetsName, Action<UObject> action)
    {
        StartCoroutine(LoadBundleAsync(assetsName, action));
    }


    private void Start()
    {
        ParseVersionFile();


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
