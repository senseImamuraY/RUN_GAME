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
    Gravity gravity;
    //CustomCubeCollider cubeCollider;

    Vector3 endPos;
    Vector3 previousPos, currentPos;

    Vector3 prevPlayerPosition,playerPosition;

    public bool isRunning;
    public float sensitivity = 1f;

    const float LOAD_WIDTH = 10f;
    const float MOVE_MAX = 4.5f;
    float speed = 20f;
    float moveDistance;
    private bool isJump = false;
    private float jumpPower = 10f;
    private bool onFloor;
    private Vector3 planePosition;


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
        
        onFloor = false;
        SetCapsulePosition();
        // 床と衝突しているか確認
        foreach (IPlane target in planeTargetsList)
        {
            if (capsuleCollider.CheckCollisionWithPlane(target))
            {
                onFloor = true;
                planePosition = target.GetCenter();
            }
        }
        if (onFloor)
        {
            Debug.Log("Planeと衝突しました");
            gravity.SetIsGravity(true);
            //isJump = false;
        }
        else
        {
            gravity.SetIsGravity(false);
        }

        foreach (ISphere target in sphereTargetsList)
        {
            if (capsuleCollider.CheckCollisionWithSphere(target))
            {
                Debug.Log("Sphereと衝突しました。");
            }
        }
        Debug.Log("onFloor = " + onFloor);

    }
    // setterを使ってプレイヤーの位置をコライダーに伝える

    void Update()
    {
        //this.transform.position = new Vector3(transform.position.x, playerPosition.y, transform.position.z);
       // SetCapsulePosition();
        //SetCubePosition();
        // プレイ中以外は無効にする
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            helpUI.SetActive(false);
            return;
        }

        // スワイプによる移動処理
        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("IsRunning", true);
            helpUI.SetActive(false);
            // スワイプによる移動距離を取得
            currentPos = Input.mousePosition;
            float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;
            diffDistance *= sensitivity;

            // 次のローカルx座標を設定 ※道の外にでないように
            float newX = Mathf.Clamp(transform.position.x + diffDistance, -MOVE_MAX, MOVE_MAX);
            //transform.localPosition = new Vector3(newX, 0, 0);
            moveDistance += speed * Time.deltaTime;
            if (onFloor)
            {
                transform.position = new Vector3(newX, transform.position.y, moveDistance);
                //transform.position = new Vector3(newX, playerPosition.y, moveDistance);
            }
            else
            {
                transform.position = new Vector3(newX, this.transform.position.y, moveDistance);
            }
            
            
            // タップ位置を更新
            previousPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsRunning", false);
            helpUI.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            //isJump = true;
            
            //if (animator.GetBool("IsGround"))
            
            animator.SetTrigger("IsJumping");
            gravity.SetVelocity(jumpPower);

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
        //float tmp = this.transform.position.y + 1f;
        //tmp = Mathf.Round(tmp * 10f) / 10f;
        //capsuleCollider.SetCenter(new Vector3(this.transform.position.x, tmp, this.transform.position.z));
        //capsuleCollider.SetCapsuleBottom();
        capsuleCollider.SetCenter(this.transform.position + new Vector3(0, 1f, 0));
        //capsuleCollider.SetCenter(this.transform.position + new Vector3(0, 1f, 0));
        capsuleCollider.SetCapsuleBottom();
    }

    public void setPlayerPosition(float position)
    {
        //if (Mathf.Abs(prevPlayerPosition.y - playerPosition.y) <= 0.1)
        //{
        //    playerPosition = (prevPlayerPosition + playerPosition) / 2;
        //}
        playerPosition = new Vector3(transform.position.x, position, transform.position.z);
        //playerPosition.y = Mathf.Round(playerPosition.y * 10f) / 10f;
        this.transform.position = playerPosition;
        //prevPlayerPosition = playerPosition;

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

