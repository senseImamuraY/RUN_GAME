using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float gravity = -9.81f; // 重力の大きさ
    public float groundHeight = 0f; // 地面の高さ
    public float groundCheckThreshold = 0.1f; // 接地判定の閾値

    private Vector3 velocity; // 速度
    private bool isGrounded; // 接地しているかどうか
    private bool OnObject;
    private bool isColliding;

    private float objectHeight;
    public Player player;

    void Start()
    {
        player = GetComponent<Player>();
        player.ClimbOnObject += OnSubjectCollision;
    }

    private void OnDestroy()
    {
        player.ClimbOnObject -= OnSubjectCollision;
    }

    void Update()
    {
        // 接地判定
        isGrounded = transform.position.y <= groundHeight + groundCheckThreshold ;
        Debug.Log(isGrounded);
    }

    void FixedUpdate()
    {
        if (isGrounded && velocity.y < 0)
        {
            // 接地している場合、y方向の速度をリセット
            Debug.Log("重力が掛かっていません");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        else if(OnObject && isColliding)
        {
            Debug.Log("Objectに乗っている");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, objectHeight, transform.position.z);
        }
        else if (!isGrounded && !OnObject)
        {
            Debug.Log("重力がかかっています");
            // 重力を適用
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // 速度を使ってオブジェクトを移動
        transform.position += velocity * Time.fixedDeltaTime;
    }

    private void OnSubjectCollision(ICube collistion)
    {
        if(!collistion.IsGround)
        {
            OnObject = true;
            objectHeight = collistion.GetCenter.y;
            isColliding = collistion.GetIsColliding;
            
        }
        else
        {
            OnObject = false;
            isColliding = collistion.GetIsColliding;
        }
        Debug.Log("isColliding = "+isColliding);
        Debug.Log("onObject = " + OnObject);
    }
}
