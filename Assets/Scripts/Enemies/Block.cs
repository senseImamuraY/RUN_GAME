using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour, IEnemy
{
    CustomCubeCollider cubeCollider;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        CustomCubeCollider collider = GetComponent<CustomCubeCollider>();
        collider.Enemy = this;
        Debug.Log(collider.Enemy);
    }

    void Start()
    {      
        cubeCollider = GetComponent<CustomCubeCollider>();
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }
    }

    IEnumerator EnterCoroutine(Player player)
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            yield break;  // �R���[�`�����I�����܂��B
        }

        Debug.Log("block");
        if (player.CompareTag("Player"))
        {
            GameManager.status = GameManager.GAME_STATUS.Pause;
            // ���b�ҋ@
            yield return new WaitForSeconds(0.25f);
            player.TakeDamage();
        }
    }

    public void Enter(Player player)
    {
        StartCoroutine(EnterCoroutine(player));
    }

}