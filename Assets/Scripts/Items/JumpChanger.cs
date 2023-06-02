using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpChanger : MonoBehaviour, IItem
{
    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        CustomSphereCollider collider = GetComponent<CustomSphereCollider>();
        collider.Item = this;
        Debug.Log(collider.Enemy);
    }

    void Start()
    {
        sphereCollider = GetComponent<CustomSphereCollider>();
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

    public void Use(Player player)
    {
        StartCoroutine(EnterCoroutine(player));
    }

}