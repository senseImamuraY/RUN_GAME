using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityChanger : MonoBehaviour, IItem
{
    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        sphereCollider = GetComponent<CustomSphereCollider>();
        sphereCollider.Item = this;
        Debug.Log(sphereCollider.Item);

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

    //public void Enter(Player player)
    //{
    //    float prevNum = player.GetGravityNum();
    //    player.GravityChanger(-100f);
    //    Debug.Log("GravityChanger");

    //}
    public void Enter(Player player)
    {
        // ���ʂ̒l
        float ChangeNum = -100f;

        // ���ʂ�K�p
        player.GravityChanger(ChangeNum);
        Debug.Log("GravityChanger");

        // �ҋ@���ԁi�b�j
        float delay = 3f;

        // �ҋ@���Ԍ�Ɍ��̒l�ɖ߂�
        StartCoroutine(RevertGravity(player, delay, ChangeNum));
    }

    private IEnumerator RevertGravity(Player player, float delay, float prevNum)
    {
        yield return new WaitForSeconds(delay);

        // ���ʂ����ɖ߂�
        player.GravityChanger(-prevNum);
        Debug.Log("GravityChanger Reverted");
    }

}