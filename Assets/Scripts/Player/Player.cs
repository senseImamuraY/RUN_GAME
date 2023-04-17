using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    GameObject helpUI;
    Animator animator;
    List<IBox> boxTargetsList;
    CustomCapsuleCollider capsuleCollider;

    Vector3 endPos;
    Vector3 previousPos, currentPos;

    public bool isRunning;
    public float sensitivity = 1f;

    const float LOAD_WIDTH = 10f;
    const float MOVE_MAX = 4.5f;
    float speed = 20f;
    float moveDistance;
    private bool isJump = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxTargetsList = GameManager.Instance.GetBoxList;
        capsuleCollider = gameObject.GetComponent<CustomCapsuleCollider>();
    }

    private void FixedUpdate()
    {
        Debug.Log(boxTargetsList);
        foreach (IBox target in boxTargetsList)
        {
            //switch (target)
            //{
            //    case CustomBoxCollider box:
            //        if (capsuleCollider.CheckCollisionWithBox(box) == true)
            //        {
            //            Debug.Log("Box�ɂԂ���܂���");
            //        }
            //        break;
            //    case CustomCapsuleCollider capsule:
            //        if (capsuleCollider.CheckCollisionWithCapsule(capsule) == true)
            //        {
            //            Debug.Log("Capsule�ɂԂ���܂���");
            //        }
            //        break;
            //    case CustomSphereCollider sphere:
            //        if (capsuleCollider.CheckCollisionWithSphere(sphere) == true)
            //        {
            //            Debug.Log("Sphere�ɂԂ���܂���");
            //        }
            //        break;
            //    default:
            //        Debug.Log("�\�z�O�̌`�������Ă��܂��B");
            //        break;
            //}
            if (capsuleCollider.CheckCollisionWithBox(target))
            {
                Debug.Log("Player����Ăяo���܂���");
            }
        }

    }

    void Update()
    {
        // �v���C���ȊO�͖����ɂ���
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            helpUI.SetActive(false);
            return;
        }

        // �X���C�v�ɂ��ړ�����
        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("IsRunning", true);
            helpUI.SetActive(false);
            // �X���C�v�ɂ��ړ��������擾
            currentPos = Input.mousePosition;
            float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;
            diffDistance *= sensitivity;

            // ���̃��[�J��x���W��ݒ� �����̊O�ɂłȂ��悤��
            float newX = Mathf.Clamp(transform.position.x + diffDistance, -MOVE_MAX, MOVE_MAX);
            //transform.localPosition = new Vector3(newX, 0, 0);
            moveDistance += speed * Time.deltaTime;
            transform.position = new Vector3(newX, 0, moveDistance);
            // �^�b�v�ʒu���X�V
            previousPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsRunning", false);
            helpUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
            
            if (isJump && animator.GetBool("IsGround"))
            {
                animator.SetTrigger("IsJumping");
            }

        }
    }

    public void Clear(Vector3 pos)
    {
        GameManager.status = GameManager.GAME_STATUS.Clear;
        //dest = pos;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Damaged");
        GameManager.status = GameManager.GAME_STATUS.GameOver;
    }
}

