using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public enum GAME_STATUS { Play, Clear, Pause, GameOver };
    public static GAME_STATUS status;

    void Start()
    {
        // �X�e�[�^�X��Play��
        status = GAME_STATUS.Play;
    }

    void Update()
    {

    }
}