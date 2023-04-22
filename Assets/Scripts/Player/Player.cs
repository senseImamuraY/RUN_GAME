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
    List<ICube> cubeTargetsList;
    List<ISphere> sphereTargetsList;
    List<IPlane> planeTargetsList;

    CustomCapsuleCollider capsuleCollider;
    //CustomCubeCollider cubeCollider;

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
        cubeTargetsList = GameManager.Instance.GetCubeList;
        sphereTargetsList = GameManager.Instance.GetSphereList;
        planeTargetsList = GameManager.Instance.GetLaneList;
        capsuleCollider = gameObject.GetComponent<CustomCapsuleCollider>();
        //cubeCollider = gameObject.GetComponent<CustomCubeCollider>();
    }

    private void FixedUpdate()
    {
        //Debug.Log(cubeTargetsList);
        foreach (IPlane target in planeTargetsList)
        {
            if (capsuleCollider.CheckCollisionWithPlane(target))
            {
                //target.SetColliding(true);
                //Debug.Log("isColliding = " + target.GetIsColliding);

                Debug.Log("Plane�ƏՓ˂��܂���");
            }
        }
        foreach (ISphere target in sphereTargetsList)
        {
            if (capsuleCollider.CheckCollisionWithSphere(target))
            {
                Debug.Log("Sphere�ƏՓ˂��܂����B");
            }
        }

    }
    // setter���g���ăv���C���[�̈ʒu���R���C�_�[�ɓ`����

    void Update()
    {
        SetCapsulePosition();
        //SetCubePosition();
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

    public void SetCapsulePosition()
    {
        capsuleCollider.SetCenter(this.transform.position + new Vector3(0 , 1f, 0));
        capsuleCollider.SetCapsuleBottom();
    }

    //public void SetCubePosition()
    //{
    //    cubeCollider.SetCenter(this.transform.position);
    //}

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

