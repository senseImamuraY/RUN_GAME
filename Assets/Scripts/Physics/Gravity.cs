using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = -9.81f; // 重力の大きさ
    public float groundHeight = 0f; // 地面の高さ
    public float groundCheckThreshold = 0.01f; // 接地判定の閾値

    private Vector3 velocity; // 速度
    private bool isGrounded; // 接地しているかどうか

    void Update()
    {
        // 接地判定
        isGrounded = transform.position.y <= groundHeight + groundCheckThreshold && velocity.y <= 0;
        Debug.Log(isGrounded);
    }

    void FixedUpdate()
    {
        if (isGrounded && velocity.y < 0)
        {
            // 接地している場合、y方向の速度をリセット
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        else
        {
            // 重力を適用
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // 速度を使ってオブジェクトを移動
        transform.position += velocity * Time.fixedDeltaTime;
    }

}
