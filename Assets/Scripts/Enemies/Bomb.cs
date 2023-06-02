using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IEnemy
{
    Animator anim;

    [SerializeField]
    Transform endPoint;

    Vector3 startPos;  // �J�n�ʒu
    Vector3 endPos;    // �I�[�ʒu
    Vector3 destPos;   // ���̖ړI�n

    float speed = 1f;�@�@�@�@�@// �ړ����x
    float rotateSpeed = 180f;  // ��]���x
    float rotateNum;           // �����]�����̉�]��
    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponent���\�b�h���g����CustomCollider���擾���AEnemy�v���p�e�B��ݒ肵�܂�
        CustomSphereCollider collider = GetComponent<CustomSphereCollider>();
        collider.Enemy = this;
        Debug.Log(collider.Enemy);

    }

    void Start()
    {
        anim = GetComponent<Animator>();

        startPos = transform.position;
        endPos = endPoint.position;
        destPos = endPos;

        // �ړ����x�������_����
        speed = Random.Range(1.0f, 3.0f);

        // �J�n�ʒu�������_����
        transform.position = Vector3.Lerp(startPos, endPos, Random.Range(0.0f, 1.0f));
        sphereCollider = GetComponent<CustomSphereCollider>();
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }

        //sphereCollider.SetPosition();

        // �[�ɓ��B�������̕����]������
        if ((destPos - transform.position).magnitude < 0.1f)
        {
            // ��]�r���̏ꍇ
            if (rotateNum < 180)
            {
                anim.SetBool("walk", false);
                transform.position = destPos;

                float addNum = rotateSpeed * Time.deltaTime;
                rotateNum += addNum;
                transform.Rotate(0, addNum, 0);
                return;
            }
            // ��]���؂����ꍇ
            // ���̖ړI�n�̐ݒ�Ɖ�]�ʂ̃��Z�b�g
            else
            {
                destPos = destPos == startPos ? endPos : startPos;
                rotateNum = 0;
            }
        }

        // ���̖ړI�n�Ɍ����Ĉړ�����
        anim.SetBool("walk", true);
        transform.LookAt(destPos);
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    IEnumerator WaitForSecond(float num)
    {
        yield return new WaitForSeconds(num);
    }

    //public void Enter(Player player)
    //{
    //    if (GameManager.status != GameManager.GAME_STATUS.Play)
    //    {
    //        return;
    //    }
    //    Debug.Log("bomb");
    //    if (player.CompareTag("Player"))
    //    {
    //        transform.LookAt(player.gameObject.transform);
    //        //player.gameObject.transform.LookAt(transform);
    //        anim.SetBool("walk", false);
    //        anim.SetTrigger("attack01");
    //        GetComponent<AudioSource>().Play();
    //        // 1�b�ҋ@
    //        StartCoroutine(WaitForSecond(10));

    //        player.TakeDamage();
    //        GameManager.status = GameManager.GAME_STATUS.Pause;
    //    }
    //}

    IEnumerator EnterCoroutine(Player player)
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            yield break;  // �R���[�`�����I�����܂��B
        }

        Debug.Log("bomb");
        if (player.CompareTag("Player"))
        {
            transform.LookAt(player.gameObject.transform);
            //player.gameObject.transform.LookAt(transform);
            anim.SetBool("walk", false);
            anim.SetTrigger("attack01");
            GameManager.status = GameManager.GAME_STATUS.Pause;

            // ���b�ҋ@
            yield return new WaitForSeconds(0.7f);
            GetComponent<AudioSource>().Play();

            // ���b�ҋ@
            yield return new WaitForSeconds(1.25f);

            player.TakeDamage();
            
        }
    }

    public void Enter(Player player)
    {
        StartCoroutine(EnterCoroutine(player));
    }

}