using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    Transform endPoint;

    Vector3 startPos;  // 開始位置
    Vector3 endPos;    // 終端位置
    Vector3 destPos;   // 次の目的地

    float speed = 1f;　　　　　// 移動速度
    float rotateSpeed = 180f;  // 回転速度
    float rotateNum;           // 方向転換時の回転量

    void Start()
    {
        anim = GetComponent<Animator>();

        startPos = transform.position;
        endPos = endPoint.position;
        destPos = endPos;

        // 移動速度をランダムに
        speed = Random.Range(1.0f, 3.0f);

        // 開始位置をランダムに
        transform.position = Vector3.Lerp(startPos, endPos, Random.Range(0.0f, 1.0f));
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }

        // 端に到達した時の方向転換処理
        if ((destPos - transform.position).magnitude < 0.1f)
        {
            // 回転途中の場合
            if (rotateNum < 180)
            {
                anim.SetBool("walk", false);
                transform.position = destPos;

                float addNum = rotateSpeed * Time.deltaTime;
                rotateNum += addNum;
                transform.Rotate(0, addNum, 0);
                return;
            }
            // 回転し切った場合
            // 次の目的地の設定と回転量のリセット
            else
            {
                destPos = destPos == startPos ? endPos : startPos;
                rotateNum = 0;
            }
        }

        // 次の目的地に向けて移動する
        anim.SetBool("walk", true);
        transform.LookAt(destPos);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            transform.LookAt(other.gameObject.transform);
            other.gameObject.transform.LookAt(transform);
            anim.SetBool("walk", false);
            anim.SetTrigger("attack01");

            GetComponent<AudioSource>().Play();

            GameManager.status = GameManager.GAME_STATUS.Pause;
        }
    }
}