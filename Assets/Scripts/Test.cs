using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/prafabs/testui.prefab.ab");
        yield return request;
        AssetBundleCreateRequest request1 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/ui_feature_bg.png.ab");
        yield return request;
        AssetBundleCreateRequest request2 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui/res/ui_feature_bg1.png.ab");
        yield return request;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync("Assets/BuildResources/UI/Prafabs/TestUI.prefab");
        yield return bundleRequest;

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
