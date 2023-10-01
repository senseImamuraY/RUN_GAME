using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif 
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class Player : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public Animator animator;

    List<ICube> cubeTargetsList;
    List<ISphere> sphereTargetsList;
    List<IPlane> planeTargetsList;

    CustomCapsuleCollider capsuleCollider;
    public CustomCapsuleCollider GetCustomCapsuleCollider() { return capsuleCollider; } 

    Gravity gravity;
    Vector3 previousPos, currentPos;
    Vector3 playerPosition;

    public bool isRunning;
    private float sensitivity = 1f;
    const float LOAD_WIDTH = 10f;
    const float MOVE_MAX = 4.5f;

    [SerializeField]
    private float speed = 20f;

    private float moveDistance;

    public bool isJump = false;
    private float jumpDelay = 1f; // 1秒のディレイ
    private float nextJumpTime = 0f;

    [SerializeField]
    private float jumpPower = 10f;

    private bool onFloor;
    Vector3 planeForward;

    Vector3 dest; // 次の目的地。クリア時に使用

    public enum ColorType { Normal, Red, Green, Violet }
    public ColorType colorType;

    void Start()
    {
        cubeTargetsList = gameManager.GetCubeList;
        sphereTargetsList = gameManager.GetSphereList;
        planeTargetsList = gameManager.GetPlaneList;

        animator = GetComponent<Animator>();
        capsuleCollider = gameObject.GetComponent<CustomCapsuleCollider>();
        gravity = gameObject.GetComponent<Gravity>();
        colorType = ColorType.Normal;

        Vector3 basePoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        previousPos = basePoint;
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
            }
        }
        if (onFloor)
        {
            gravity.SetIsGround(true);
        }
        else
        {
            gravity.SetIsGround(false);
        }

        foreach (ISphere target in sphereTargetsList)
        {
            if (target.IsColliding == true) continue;
            if (capsuleCollider.CheckCollisionWithSphere(target))
            {
                // targetをCustomColliderにキャスト
                CustomSphereCollider customCollider = target as CustomSphereCollider;
                if (customCollider == null)
                {
                    Debug.Log("キャストできませんでした");
                    continue;
                }

                // ここでIEnemyをチェックし、それがnullでない場合にはEnterを呼び出す
                IEnemy enemy = customCollider.Enemy;
                if (enemy != null)
                {
                    enemy.Enter(this);
                    Debug.Log("EnemyのEnterが呼ばれました");
                }

                // IItemをチェック。それがnullでない場合には、何らかのアイテム特有の処理を実行します
                IItem item = customCollider.Item;
                if (item != null)
                {
                    item.Enter(this);
                    Debug.Log("ItemのUseが呼ばれました");
                }

                if (enemy == null && item == null)
                {
                    Debug.Log("変換失敗");
                }

                Debug.Log("Sphereと衝突しました。");
            }
        }

        foreach(ICube target in cubeTargetsList)
        {
            if (target.IsColliding == true) continue;
            if (capsuleCollider.CheckCollisionWithCube(target))
            {
                // targetをCustomColliderにキャスト
                CustomCubeCollider customCollider = target as CustomCubeCollider;
                if (customCollider == null)
                {
                    Debug.Log("キャストできませんでした");
                    return;
                }
                IEnemy ememy = customCollider.Enemy;
                if (ememy != null)
                {
                    ememy.Enter(this);
                    Debug.Log("Enterが呼ばれました");
                }
                else
                {
                    Debug.Log("変換失敗");
                }
                Debug.Log("Cubeと衝突しました。");
            }
        }

        //// プレイ中以外は無効にする
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }

        // スワイプによる移動処理

        animator.SetBool("IsRunning", true);

        // スワイプによる移動距離を取得
        currentPos = Input.mousePosition;
        float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;
        diffDistance *= sensitivity;

        // 次のローカルx座標を設定 ※道の外にでないように
        float newX = Mathf.Clamp(transform.position.x + diffDistance, -MOVE_MAX, MOVE_MAX);

        moveDistance += speed * Time.deltaTime;
        transform.position += planeForward * moveDistance * Time.deltaTime;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // タップ位置を更新
        previousPos = currentPos;
        
        // 加速度を設定
        gravity.VelocityUpdate();

        // リストをクリア
        sphereTargetsList.Clear();
        cubeTargetsList.Clear();

    }

    void Update()
    {
        if (isJump) { return; }

        // クリア時の処理
        if (GameManager.status == GameManager.GAME_STATUS.Clear)
        {
            // 目的地の方向を向く
            transform.LookAt(dest);

            // 目的地の方向に移動させる
            Vector3 dir = (dest - transform.position).normalized;
            float speed = 3f;
            transform.position += dir * speed * Time.deltaTime;

            // 目的地に十分近づいたら、最終演出
            if ((dest - transform.position).magnitude < 0.5f)
            {
                transform.position = dest;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("Clear");

                // Updateメソッドがこれ以上実行されなくなる
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

    public void SpeedChanger(float num)
    {
        speed += num;
        Debug.Log("speed = " + speed);
    }

    public void SensitivityChanger(float num)
    {
        sensitivity += num;
    }

    public void GravityChanger(float num)
    {
        gravity.gravity += num;
    }

    public void JumpPowerChanger(float num)
    {
        jumpPower += num;
    }

    public float GetGravityNum()
    {
        return gravity.gravity;
    }

    public float GetSpeedNum()
    {
        return speed;
    }

    public float GetSlideNum()
    {
        return sensitivity;
    }

    public float GetJumpPowerNum()
    {
        return jumpPower;
    }
}

