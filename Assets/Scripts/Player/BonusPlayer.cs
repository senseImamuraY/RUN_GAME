using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPlayer : MonoBehaviour
{
    Animator animator;

    public bool isRunning;

    public float sensitivity = 1f;
    const float LOAD_WIDTH = 6f;
    const float MOVE_MAX = 2.5f;
    Vector3 previousPos, currentPos;

    Vector3 dest; // ���̖ړI�n�B�N���A���Ɏg�p
    float speed = 6f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // �N���A���̏���
        if (GameManager.status == GameManager.GAME_STATUS.Clear)
        {
            // �ړI�n�̕���������
            transform.LookAt(dest);

            // �ړI�n�̕����Ɉړ�������
            Vector3 dir = (dest - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            // �ړI�n�ɏ\���߂Â�����A�ŏI���o
            if ((dest - transform.position).magnitude < 0.5f)
            {
                transform.position = dest;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("Clear");

                // Update���\�b�h������ȏ���s����Ȃ��Ȃ�
                enabled = false;
            }
            return;
        }

        // �v���C�ȊO�Ȃ疳���ɂ���
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            animator.SetBool("IsRunning", false);
            return;
        }

        // �X���C�v�ɂ��ړ�����
        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            // �X���C�v�ɂ��ړ��������擾
            currentPos = Input.mousePosition;
            float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;
            diffDistance *= sensitivity;

            // ���̃��[�J��x���W��ݒ� �����̊O�ɂłȂ��悤��
            float newX = Mathf.Clamp(transform.localPosition.x + diffDistance, -MOVE_MAX, MOVE_MAX);
            transform.localPosition = new Vector3(newX, 0, 0);

            // �^�b�v�ʒu���X�V
            previousPos = currentPos;
        }

        // isRunning = true; ���폜���Ă�������
        animator.SetBool("IsRunning", isRunning);
    }

    public void Clear(Vector3 pos)
    {
        GameManager.status = GameManager.GAME_STATUS.Clear;
        dest = pos;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Damaged");
        GameManager.status = GameManager.GAME_STATUS.GameOver;
    }
}