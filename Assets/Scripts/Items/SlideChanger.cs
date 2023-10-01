using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideChanger : MonoBehaviour, IItem
{
    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        sphereCollider = GetComponent<CustomSphereCollider>();
        sphereCollider.Item = this;
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

    public void Enter(Player player)
    {
        // ���ʂ̒l
        float ChangeNum = 2f;

        // ���ʂ�K�p
        player.SensitivityChanger(ChangeNum);

        // �ҋ@���ԁi�b�j
        float delay = 3f;

        // �ҋ@���Ԍ�Ɍ��̒l�ɖ߂�
        StartCoroutine(RevertGravity(player, delay, ChangeNum));
    }

    private IEnumerator RevertGravity(Player player, float delay, float prevNum)
    {
        yield return new WaitForSeconds(delay);

        // ���ʂ����ɖ߂�
        player.SensitivityChanger(-prevNum);
    }
}