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
        // �ڒn����
        isGrounded = transform.position.y <= groundHeight + groundCheckThreshold ;
        Debug.Log(isGrounded);
    }

    void FixedUpdate()
    {
        if (isGrounded && velocity.y < 0)
        {
            // �ڒn���Ă���ꍇ�Ay�����̑��x�����Z�b�g
            Debug.Log("�d�͂��|�����Ă��܂���");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        else if(OnObject && isColliding)
        {
            Debug.Log("Object�ɏ���Ă���");
            velocity.y = 0f;
            transform.position = new Vector3(transform.position.x, objectHeight, transform.position.z);
        }
        else if (!isGrounded && !OnObject)
        {
            Debug.Log("�d�͂��������Ă��܂�");
            // �d�͂�K�p
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        // ���x���g���ăI�u�W�F�N�g���ړ�
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
