using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float groundHeight = 0f; // 地面の高さ
    public float groundCheckThreshold = 0.1f; // 接地判定の閾値
    public Vector3 velocity = new Vector3(0 ,0, 0); // 速度
    private Vector3 prevVelocity = new Vector3 (0 ,0 ,0);

    public bool isGrounded; // 接地しているかどうか
    private bool OnObject;
    private bool isColliding;
    public float gravity = -36.0f; // 重力の大きさ

    private float objectHeight;
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
        player.ClimbOnObject += OnSubjectCollision;
    }

    private void OnDestroy()
    {
        player.ClimbOnObject -= OnSubjectCollision;
    }

    public void SetVelocity(float newVelocity)
    {
        velocity.y += newVelocity;
    }

    public void ClearVelocity()
    {
        velocity.y = 0;
    }


    public void SetIsGround(bool value)
    {
        isGrounded = value;
    }

    void Update()
    {
        // 接地判定
        //Debug.Log("isGravity = " + isGrounded);
    }

    //void FixedUpdate()
    public void VelocityUpdate()
    {

        if (isGrounded)
        {
            // 接地している場合、y方向の速度をリセット
            //Debug.Log("重力が掛かっていません");
            velocity.y = 0f;
        }
        else if(OnObject && isColliding)
        {
            Debug.Log("Objectに乗っている");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, objectHeight, transform.position.z);
        }
        if (player.isJump && isGrounded)
        {
            player.isJump = false;

            //if (animator.GetBool("IsGround"))

            player.animator.SetTrigger("IsJumping");
            SetVelocity(player.GetJumpPowerNum());

        }

        transform.position += velocity * Time.fixedDeltaTime;
        velocity.y += gravity * Time.fixedDeltaTime;

        prevVelocity = velocity;
        // 速度を使ってオブジェクトを移動
        
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
