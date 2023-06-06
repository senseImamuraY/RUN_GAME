using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    float speed = 100f;

    bool isGet;             // �l���ς݃t���O
    float lifeTime = 0.5f;  // �l����̐������� 

    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        sphereCollider = GetComponent<CustomSphereCollider>();
        sphereCollider.Item = this;
        Debug.Log(sphereCollider.Item);
    }

    void FixedUpdate()
    {
        // �l����
        if (isGet)
        {
            // �f������]
            transform.Rotate(Vector3.up * speed * 10f * Time.deltaTime, Space.World);

            // �������Ԃ����炷
            lifeTime -= Time.deltaTime;

            // �������Ԃ�0�ȉ��ɂȂ��������
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
        // �l���O
        else
        {
            // ��������]
            transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
        }
    }

    public void Enter(Player player)
    {
        isGet = true;

        //GetComponent<AudioSource>().Play();

        GameManager.tempCoinNum++;
        Debug.Log("�R�C���̖����F" + GameManager.tempCoinNum);

        // �R�C������Ƀ|�b�v������
        transform.position += Vector3.up * 1.5f;
    }
}