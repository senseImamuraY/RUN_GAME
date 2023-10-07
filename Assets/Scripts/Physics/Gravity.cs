using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float groundHeight = 0f; // �n�ʂ̍���
    public float groundCheckThreshold = 0.1f; // �ڒn�����臒l
    public Vector3 velocity = new Vector3(0 ,0, 0); // ���x

    public bool isGrounded; // �ڒn���Ă��邩�ǂ���
    public float gravity = -36.0f; // �d�͂̑傫��
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    public void SetVelocity(float newVelocity)
    {
        velocity.y += newVelocity;
    }

    public void SetIsGround(bool value)
    {
        isGrounded = value;
    }

    public void VelocityUpdate()
    {

        if (isGrounded)
        {
            // �ڒn���Ă���ꍇ�Ay�����̑��x�����Z�b�g
            velocity.y = 0f;
        }

        // �W�����v�L�[��������Ă��APlayer���ڒn���Ă���Ƃ��ɁA�W�����v�A�j���[�V�������X�^�[�g
        if (player.isJump && isGrounded)
        {
            player.isJump = false;

            player.animator.SetTrigger("IsJumping");
            SetVelocity(player.GetJumpPowerNum());
        }

        transform.position += velocity * Time.fixedDeltaTime;
        velocity.y += gravity * Time.fixedDeltaTime;
    } 
}
