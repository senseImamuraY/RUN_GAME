using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float groundHeight = 0f; // 地面の高さ
    public float groundCheckThreshold = 0.1f; // 接地判定の閾値
    public Vector3 velocity = new Vector3(0 ,0, 0); // 速度

    public bool isGrounded; // 接地しているかどうか
    public float gravity = -36.0f; // 重力の大きさ
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
            // 接地している場合、y方向の速度をリセット
            velocity.y = 0f;
        }

        // ジャンプキーが押されてかつ、Playerが接地しているときに、ジャンプアニメーションをスタート
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
