using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestInitialize
{
    // �����̐ݒ�
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("After Scene is loaded and game is running");
        // �X�N���[���T�C�Y�̎w��
        Screen.SetResolution(360, 640, false);
    }
}