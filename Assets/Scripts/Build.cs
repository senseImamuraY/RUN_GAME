using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Build����ۂɎg�p
public class TestInitialize
{
    // �����̐ݒ�
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("After Scene is loaded and game is running");
        // �X�N���[���T�C�Y�̎w��
        Screen.SetResolution(960, 540, false);
    }
}