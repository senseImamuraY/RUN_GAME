using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = -9.81f; // �d�͂̑傫��
    public float groundHeight = 0f; // �n�ʂ̍���
    public float groundCheckThreshold = 0.1f; // �ڒn�����臒l

    private Vector3 velocity; // ���x
    private bool isGrounded; // �ڒn���Ă��邩�ǂ���

    void Update()
    {
        // �ڒn����
        isGrounded = transform.position.y <= groundHeight + groundCheckThreshold;
    }

    void FixedUpdate()
    {
        if (isGrounded && velocity.y < 0)
        {
            // �ڒn���Ă���ꍇ�Ay�����̑��x�����Z�b�g
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        else
        {
            // �d�͂�K�p
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // ���x���g���ăI�u�W�F�N�g���ړ�
        transform.position += velocity * Time.fixedDeltaTime;
    }

}
