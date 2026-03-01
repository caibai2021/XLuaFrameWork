using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //  异步加载方式 加载  ui  图片资源
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/prafabs/testui.prefab.ab");
        yield return request; //  异步加载ab包，
        AssetBundleCreateRequest request1 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/ui_feature_bg.png.ab");
        yield return request1;
        AssetBundleCreateRequest request2 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/ui_feature_bg1.png.ab");
        yield return request2;
        AssetBundleCreateRequest request3 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/icon/gem.png.ab");
        yield return request3;


        // 加载  TestUi
        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync("Assets/BuildResources/UI/Prafabs/TestUI.prefab");
        yield return bundleRequest;

        //  实例化Ui

        GameObject go = Instantiate(bundleRequest.asset) as GameObject;
        go.transform.SetParent(transform, false);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;


        
    }

    
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
