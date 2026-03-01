using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    private Button btn1;


    private void Awake()
    {
        btn1 = GetComponentInChildren<Button>();
        btn1.onClick.AddListener(OnBtn1Click);
        Debug.Log("awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBtn1Click()
    {
        Debug.Log("on btn1 clicked !");
    }

}
