using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour, IEnemy
{
    CustomCubeCollider cubeCollider;
    void Awake()
    {
        // GetComponentメソッドを使ってCustomColliderを取得し、Enemyプロパティを設定
        cubeCollider = GetComponent<CustomCubeCollider>();
        cubeCollider.Enemy = this;
    }

    void Start()
    {      
        cubeCollider = GetComponent<CustomCubeCollider>();
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }
    }

    IEnumerator EnterCoroutine(Player player)
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            yield break;  // コルーチンを終了
        }

        if (player.CompareTag("Player"))
        {
            GameManager.status = GameManager.GAME_STATUS.Pause;
            // 数秒待機
            yield return new WaitForSeconds(0.25f);
            player.TakeDamage();
        }
    }

    public void Enter(Player player)
    {
        StartCoroutine(EnterCoroutine(player));
    }
}