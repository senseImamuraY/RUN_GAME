using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    //public float velocity = 0; 
    //public float gravity = -9.81f; // 重力の大きさ
    public float groundHeight = 0f; // 地面の高さ
    public float groundCheckThreshold = 0.1f; // 接地判定の閾値
    public Vector3 velocity = new Vector3(0 ,0, 0); // 速度
    private Vector3 prevVelocity = new Vector3 (0 ,0 ,0);

    public bool isGrounded; // 接地しているかどうか
    private bool OnObject;
    private bool isColliding;
    public float gravity = -36.0f; // 重力の大きさ

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
        //isGrounded = transform.position.y <= groundHeight + groundCheckThreshold ;
        Debug.Log("isGravity = " + isGrounded);
    }

    //void FixedUpdate()
    public void VelocityUpdate()
    {

    
        

        //if (isGrounded && velocity.y <= 0)
        //if (isGrounded && velocity.y <= 0)
        if (isGrounded)
        {
            // 接地している場合、y方向の速度をリセット
            //Debug.Log("重力が掛かっていません");
            velocity.y = 0f;
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            //transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        else if(OnObject && isColliding)
        {
            Debug.Log("Objectに乗っている");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, objectHeight, transform.position.z);
        }
        else if (!isGrounded && !OnObject)
        {
            //Debug.Log("重力がかかっています");
            // 重力を適用
           
        }
        if (player.isJump && isGrounded)
        {
            player.isJump = false;

            //if (animator.GetBool("IsGround"))

            player.animator.SetTrigger("IsJumping");
            SetVelocity(player.jumpPower);

        }

        //velocity.y = Mathf.Round(velocity.y * 10f) / 10f;  // myFloatの小数点第2位を切り上げる
        //if (Mathf.Abs(prevVelocity.y - velocity.y) <= 0.1)
        //{
        //    velocity = (prevVelocity + velocity) / 2;
        //}
        transform.position += velocity * Time.fixedDeltaTime;
        velocity.y += gravity * Time.fixedDeltaTime;

        prevVelocity = velocity;
        //Debug.Log("Velocity = " +  velocity.y);
        //Debug.Log("velocity.y = " +  velocity.y);
        //Debug.Log("velocity.y = " + velocity.y);
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

    //public bool SetIsGrounded(bool value)
    //{
    //    return isGrounded = value;
    //}
}
