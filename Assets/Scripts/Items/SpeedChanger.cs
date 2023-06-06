using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedChanger : MonoBehaviour, IItem
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
        //sphereCollider = GetComponent<CustomSphereCollider>();
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }
    }

    public void Use(Player player)
    {
        //StartCoroutine(EnterCoroutine(player));
        player.SpeedChanger(100f);
        Debug.Log("SpeedChanger ");
    }

}