using PathCreation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using System.Numerics;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class Player : MonoBehaviour
{

    [SerializeField]
    GameObject helpUI;
    public Animator animator;
    List<ICube> cubeTargetsList;
    List<ISphere> sphereTargetsList;
    List<IPlane> planeTargetsList;

    CustomCapsuleCollider capsuleCollider;
    public CustomCapsuleCollider GetCustomCapsuleCollider() { return capsuleCollider; } 
    Gravity gravity;

    Vector3 endPos;
    Vector3 previousPos, currentPos;

    Vector3 playerPosition;

    public bool isRunning;
    public float sensitivity = 1f;

    const float LOAD_WIDTH = 10f;
    const float MOVE_MAX = 4.5f;
    [SerializeField]
    float speed = 20f;
    float moveDistance;
    public bool isJump = false;
    float jumpDelay = 1f; // 1�b�̃f�B���C
    float nextJumpTime = 0f;

    Vector3 planeForward;

    [SerializeField]
    public float jumpPower = 10f;
    private bool onFloor;
    private Vector3 planeNormal;

    Vector3 dest; // ���̖ړI�n�B�N���A���Ɏg�p

    void Start()
    {
        animator = GetComponent<Animator>();
        cubeTargetsList = GameManager.Instance.GetCubeList;
        sphereTargetsList = GameManager.Instance.GetSphereList;
        planeTargetsList = GameManager.Instance.GetLaneList;
        capsuleCollider = gameObject.GetComponent<CustomCapsuleCollider>();
        gravity = gameObject.GetComponent<Gravity>();
    }



    private void FixedUpdate()
    {
        //if (transform.position.y < -10) return;
        onFloor = false;
        SetCapsulePosition();
        // ���ƏՓ˂��Ă��邩�m�F
        foreach (IPlane target in planeTargetsList)
        {
            //Debug.Log("PlaneCount = " + planeTargetsList.Count);
            if (capsuleCollider.CheckCollisionWithPlane(target))
            {
                onFloor = true;
                planeNormal = target.GetNormal();
            }
        }
        if (onFloor)
        {
            Debug.Log("Plane�ƏՓ˂��܂���");
            gravity.SetIsGround(true);
            //isJump = false;
        }
        else
        {
            gravity.SetIsGround(false);
        }

        foreach (ISphere target in sphereTargetsList)
        {
            if (capsuleCollider.CheckCollisionWithSphere(target))
            {
                target.Enter();
                Debug.Log("Sphere�ƏՓ˂��܂����B");
            }
        }

        foreach(ICube target in cubeTargetsList)
        {
            //Debug.Log("CubeCount = "+ cubeTargetsList.Count);
            if (capsuleCollider.CheckCollisionWithCube(target))
            {
                Debug.Log("Cube�ƏՓ˂��܂����B");
            }
        }
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

        animator.SetBool("IsRunning", true);
        helpUI.SetActive(false);

        // �X���C�v�ɂ��ړ��������擾
        currentPos = Input.mousePosition;
        float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;

        diffDistance *= sensitivity;

        // ���̃��[�J��x���W��ݒ� �����̊O�ɂłȂ��悤��
        float newX = Mathf.Clamp(transform.position.x + diffDistance, -MOVE_MAX, MOVE_MAX);

        moveDistance += speed * Time.deltaTime;
        transform.position += planeForward * moveDistance * Time.deltaTime;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // �^�b�v�ʒu���X�V
        previousPos = currentPos;
        
        // �����x��ݒ�
        gravity.VelocityUpdate();
    }

    void Update()
    {
        if (isJump) { return; }

        // �N���A���̏���
        if (GameManager.status == GameManager.GAME_STATUS.Clear)
        {
            // �ړI�n�̕���������
            transform.LookAt(dest);

            // �ړI�n�̕����Ɉړ�������
            Vector3 dir = (dest - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            // �ړI�n�ɏ\���߂Â�����A�ŏI���o
            if ((dest - transform.position).magnitude < 0.5f)
            {
                transform.position = dest;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("Clear");

                // Update���\�b�h������ȏ���s����Ȃ��Ȃ�
                enabled = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextJumpTime)
        {
            nextJumpTime = Time.time + jumpDelay;
            isJump = true;
        }

    }
    public void setForward(Vector3 forward)
    {
        planeForward = forward;
        //Debug.Log("planeforward = " + planeForward);
    }

    public void setRotation(Quaternion rot)
    {
        this.transform.rotation = rot;
        //this.transform.localRotation = rot;
    }

    public void Clear(Vector3 pos)
    {
        GameManager.status = GameManager.GAME_STATUS.Clear;
        dest = pos;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Damaged");
        GameManager.status = GameManager.GAME_STATUS.GameOver;
    }

    public void SetCapsulePosition()
    {
        capsuleCollider.SetCenter(this.transform.position + new Vector3(0, 1f, 0));
        capsuleCollider.SetCapsuleBottom();
    }

    public void setPlayerPosition(float position)
    {
        playerPosition = new Vector3(transform.position.x, position, transform.position.z);
        this.transform.position = playerPosition;

    }

    private void ClimbOnCube(ICube cube)
    {
        //this.transform.position += new Vector3(0,5f,0);
        this.transform.position = cube.GetCenter + new Vector3(0, 0.5f, 0);
    }

    public delegate void ClimbOnObjectHandler(ICube collider);
    public event ClimbOnObjectHandler ClimbOnObject;

    private void ClimbOnObjectEnter(ICube collider)
    {
        //Debug.Log("isColliding + = " + collider.GetIsColliding);
        ClimbOnObject(collider);
    }
}

