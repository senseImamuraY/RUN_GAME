using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Buildする際に使用
public class TestInitialize
{
    // 属性の設定
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("After Scene is loaded and game is running");
        // スクリーンサイズの指定
        Screen.SetResolution(960, 540, false);
    }
}