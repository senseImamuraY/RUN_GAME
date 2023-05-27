using PathCreation;
using System.Collections;
using System.Collections.Generic;
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
    Gravity gravity;

    Vector3 endPos;
    Vector3 previousPos, currentPos;

    Vector3 prevPlayerPosition,playerPosition;

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

    [SerializeField]
    public float jumpPower = 10f;
    private bool onFloor;
    private Vector3 planeNormal;

    float prevPlaneY = 0;
    float planeY = 0;
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
            Debug.Log("CubeCount = "+ cubeTargetsList.Count);
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
            prevPlayerPosition = this.transform.position;
        }

        if (true)
        //if (Input.GetMouseButton(0))
        {

            animator.SetBool("IsRunning", true);
            helpUI.SetActive(false);
            // �X���C�v�ɂ��ړ��������擾
            currentPos = Input.mousePosition;
            float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;

            diffDistance *= sensitivity;
            // ���̃��[�J��x���W��ݒ� �����̊O�ɂłȂ��悤��
            float newX = Mathf.Clamp(transform.position.x + diffDistance, -MOVE_MAX, MOVE_MAX);
            moveDistance += speed * Time.deltaTime;
            if (onFloor)
            {
                // ��̌X�Ίp�i�Ɓj���v�Z
                Vector3 up = transform.up;
                // normal��up�͂ǂ�������K������Čv�Z�����̂�cos�Ƃ̒l�𓾂邱�Ƃ��ł���
                float dotProduct = Vector3.Dot(planeNormal, up);
                dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
                float angle = Mathf.Acos(dotProduct); // ���W�A���œ�����
                //Debug.Log("angle = " + angle);
                // tan�Ƃ��v�Z
                float tanTheta = Mathf.Tan(angle);

                float y = diffDistance * tanTheta;

                float d = planeNormal.x * newX + planeNormal.y * this.transform.position.y + planeNormal.z * moveDistance;
                //d = Mathf.Floor(d * 100 + 0.5f) / 100;
                planeY = -(planeNormal.x * newX + planeNormal.z * moveDistance + d) / planeNormal.y;
                float diffY = tanTheta * playerPosition.x - prevPlayerPosition.x;
                Debug.Log("planeY = " + planeY);
                //planeY = Mathf.Floor(planeY * 100 + 0.5f) / 100;
                prevPlaneY = planeY;
            }
            transform.position = new Vector3(newX, transform.position.y, moveDistance);
            prevPlayerPosition = playerPosition;
            // �^�b�v�ʒu���X�V
            previousPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsRunning", false);
            helpUI.SetActive(true);
        }


        gravity.VelocityUpdate();
    }
    // setter���g���ăv���C���[�̈ʒu���R���C�_�[�ɓ`����

    void Update()
    {
        if (isJump) { return; }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextJumpTime)
        {
            nextJumpTime = Time.time + jumpDelay;
            isJump = true;
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

