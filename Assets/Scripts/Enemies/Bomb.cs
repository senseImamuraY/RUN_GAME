using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IEnemy
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
    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponentメソッドを使ってCustomColliderを取得し、Enemyプロパティを設定します
        CustomSphereCollider collider = GetComponent<CustomSphereCollider>();
        collider.Enemy = this;
        Debug.Log(collider.Enemy);

    }

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
        sphereCollider = GetComponent<CustomSphereCollider>();
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }

        //sphereCollider.SetPosition();

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


    IEnumerator WaitForSecond(float num)
    {
        yield return new WaitForSeconds(num);
    }

    //public void Enter(Player player)
    //{
    //    if (GameManager.status != GameManager.GAME_STATUS.Play)
    //    {
    //        return;
    //    }
    //    Debug.Log("bomb");
    //    if (player.CompareTag("Player"))
    //    {
    //        transform.LookAt(player.gameObject.transform);
    //        //player.gameObject.transform.LookAt(transform);
    //        anim.SetBool("walk", false);
    //        anim.SetTrigger("attack01");
    //        GetComponent<AudioSource>().Play();
    //        // 1秒待機
    //        StartCoroutine(WaitForSecond(10));

    //        player.TakeDamage();
    //        GameManager.status = GameManager.GAME_STATUS.Pause;
    //    }
    //}

    IEnumerator EnterCoroutine(Player player)
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            yield break;  // コルーチンを終了します。
        }

        Debug.Log("bomb");
        if (player.CompareTag("Player"))
        {
            transform.LookAt(player.gameObject.transform);
            //player.gameObject.transform.LookAt(transform);
            anim.SetBool("walk", false);
            anim.SetTrigger("attack01");
            GameManager.status = GameManager.GAME_STATUS.Pause;

            // 数秒待機
            yield return new WaitForSeconds(0.7f);
            GetComponent<AudioSource>().Play();

            // 数秒待機
            yield return new WaitForSeconds(1.25f);

            player.TakeDamage();
            
        }
    }

    public void Enter(Player player)
    {
        StartCoroutine(EnterCoroutine(player));
    }

}