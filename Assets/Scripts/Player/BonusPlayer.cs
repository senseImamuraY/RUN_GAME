using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPlayer : MonoBehaviour
{
    Animator animator;

    public bool isRunning;

    public float sensitivity = 1f;
    const float LOAD_WIDTH = 6f;
    const float MOVE_MAX = 2.5f;
    Vector3 previousPos, currentPos;

    Vector3 dest; // 次の目的地。クリア時に使用
    float speed = 6f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // クリア時の処理
        if (GameManager.status == GameManager.GAME_STATUS.Clear)
        {
            // 目的地の方向を向く
            transform.LookAt(dest);

            // 目的地の方向に移動させる
            Vector3 dir = (dest - transform.position).normalized;
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

        // プレイ以外なら無効にする
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            animator.SetBool("IsRunning", false);
            return;
        }

        // スワイプによる移動処理
        if (Input.GetMouseButtonDown(0))
        {
            previousPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            // スワイプによる移動距離を取得
            currentPos = Input.mousePosition;
            float diffDistance = (currentPos.x - previousPos.x) / Screen.width * LOAD_WIDTH;
            diffDistance *= sensitivity;

            // 次のローカルx座標を設定 ※道の外にでないように
            float newX = Mathf.Clamp(transform.localPosition.x + diffDistance, -MOVE_MAX, MOVE_MAX);
            transform.localPosition = new Vector3(newX, 0, 0);

            // タップ位置を更新
            previousPos = currentPos;
        }

        // isRunning = true; ※削除してください
        animator.SetBool("IsRunning", isRunning);
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
}