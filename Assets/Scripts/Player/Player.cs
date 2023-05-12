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
        if (transform.position.y < -10) return;
        onFloor = false;
        SetCapsulePosition();
        // 床と衝突しているか確認
        foreach (IPlane target in planeTargetsList)
        {
            if (capsuleCollider.CheckCollisionWithPlane(target))
            {
                onFloor = true;
                planeNormal = target.GetNormal();
            }
        }
        if (onFloor)
        {
            Debug.Log("Planeと衝突しました");
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
                Debug.Log("Sphereと衝突しました。");
            }
        }
        Debug.Log("onFloor = " + onFloor);
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
            prevPlayerPosition = this.transform.position;
        }

        if (Input.GetMouseButton(0))
        {

            animator.SetBool("IsRunning", true);
            helpUI.SetActive(false);
            // スワイプによる移動距離を取得
            currentPos = Input.mousePosition;
            float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;
            //Debug.Log($"{diffDistance}");
            //if(diffDistance > 6)
            //{
            //    Debug.Log("いっぱい移動しました。");
            //}

            diffDistance *= sensitivity;
           // Debug.Log("diffDistance = "+ diffDistance);
            // 次のローカルx座標を設定 ※道の外にでないように
            float newX = Mathf.Clamp(transform.position.x + diffDistance, -MOVE_MAX, MOVE_MAX);
            //transform.localPosition = new Vector3(newX, 0, 0);

            moveDistance += speed * Time.deltaTime;
            if (onFloor)
            {
                // 坂の傾斜角（θ）を計算
                Vector3 up = transform.up;
                // normalとupはどちらも正規化されて計算されるのでcosθの値を得ることができる
                float dotProduct = Vector3.Dot(planeNormal, up);
                dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
                float angle = Mathf.Acos(dotProduct); // ラジアンで得られる
                Debug.Log("angle = " + angle);
                // tanθを計算
                //float tanTheta = 20.0f;
                float tanTheta = Mathf.Tan(angle);
                //Debug.Log("tanTheta = " + tanTheta);
               // Debug.Log("y = " + (diffDistance * tanTheta));
                float y = diffDistance * tanTheta;

                float d = planeNormal.x * newX + planeNormal.y * this.transform.position.y + planeNormal.z * moveDistance;
                //d = Mathf.Floor(d * 100 + 0.5f) / 100;
                planeY = -(planeNormal.x * newX + planeNormal.z * moveDistance + d) / planeNormal.y;
                //planeY = Mathf.Floor(planeY * 100 + 0.5f) / 100;

                //Debug.Log("planeY in Player = " +  planeY);
                //Debug.Log("planeY = " + planeY);
                //Debug.Log("praviouPos - currentPos = " + (playerPosition.x - prevPlayerPosition.x));

                //if (playerPosition.x - prevPlayerPosition.x < 0)
                //{
                float diffY = tanTheta * playerPosition.x - prevPlayerPosition.x;
                //    //Debug.Log("diffY = " + diffY);
                //gravity.SetVelocity(diffY*1000f);
                //    transform.position = new Vector3(newX, planeY  , moveDistance);
                //}
                //else
                //{
                //    transform.position = new Vector3(newX, planeY, moveDistance);
                //}

                planeY = Mathf.Floor(planeY * 100 + 0.5f) / 100;
                //float maxYChange = 0.1f;
                Debug.Log("planeY = " + planeY);
                //planeY = y;
                //if (Mathf.Abs(planeY - prevPlaneY) < maxYChange)
                {
                    if (planeY > prevPlaneY)
                    {
                   //     planeY = prevPlaneY;
                    }
                    else
                    {
                    //    planeY = prevPlaneY;
                    }
                    //transform.position = new Vector3(newX, planeY, moveDistance);
                }
                
                //if()
                prevPlaneY = planeY;
                Debug.Log("prevPlaneY = " + prevPlaneY);
                //transform.position = new Vector3(newX, y, moveDistance);
                //transform.position = new Vector3(newX, planeY, moveDistance);
            }
            else
            {
                //transform.position = new Vector3(newX, this.transform.position.y, moveDistance);
            }
           // transform.position = new Vector3(newX, planeY, moveDistance);
            transform.position = new Vector3(newX, transform.position.y, moveDistance);
            prevPlayerPosition = playerPosition;
            // タップ位置を更新
            previousPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsRunning", false);
            helpUI.SetActive(true);
        }


        gravity.VelocityUpdate();
    }
    // setterを使ってプレイヤーの位置をコライダーに伝える

    void Update()
    {

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

