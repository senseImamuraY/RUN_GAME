using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    //public float velocity = 0; 
    //public float gravity = -9.81f; // �d�͂̑傫��
    public float groundHeight = 0f; // �n�ʂ̍���
    public float groundCheckThreshold = 0.1f; // �ڒn�����臒l
    public Vector3 velocity = new Vector3(0 ,0, 0); // ���x
    private Vector3 prevVelocity = new Vector3 (0 ,0 ,0);

    public bool isGrounded; // �ڒn���Ă��邩�ǂ���
    private bool OnObject;
    private bool isColliding;
    public float gravity = -36.0f; // �d�͂̑傫��

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
        // �ڒn����
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
            // �ڒn���Ă���ꍇ�Ay�����̑��x�����Z�b�g
            //Debug.Log("�d�͂��|�����Ă��܂���");
            velocity.y = 0f;
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            //transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        else if(OnObject && isColliding)
        {
            Debug.Log("Object�ɏ���Ă���");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, objectHeight, transform.position.z);
        }
        else if (!isGrounded && !OnObject)
        {
            //Debug.Log("�d�͂��������Ă��܂�");
            // �d�͂�K�p
           
        }
        if (player.isJump && isGrounded)
        {
            player.isJump = false;

            //if (animator.GetBool("IsGround"))

            player.animator.SetTrigger("IsJumping");
            SetVelocity(player.jumpPower);

        }

        //velocity.y = Mathf.Round(velocity.y * 10f) / 10f;  // myFloat�̏����_��2�ʂ�؂�グ��
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
        // ���x���g���ăI�u�W�F�N�g���ړ�
        
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
