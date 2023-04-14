using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField]
    GameObject helpUI;
    Animator animator;
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
    }

    void Update()
    {
        // プレイ中以外は無効にする
        if (GameManagerScript.status != GameManagerScript.GAME_STATUS.Play)
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
            transform.position = new Vector3(newX, 0, moveDistance);
            // タップ位置を更新
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
        GameManagerScript.status = GameManagerScript.GAME_STATUS.Clear;
        //dest = pos;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Damaged");
        GameManagerScript.status = GameManagerScript.GAME_STATUS.GameOver;
    }
}

