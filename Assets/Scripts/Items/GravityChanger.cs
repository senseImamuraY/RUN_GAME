using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityChanger : MonoBehaviour, IItem
{
    CustomSphereCollider sphereCollider;

    void Awake()
    {
        // GetComponentメソッドを使ってCustomColliderを取得し、Enemyプロパティを設定します
        sphereCollider = GetComponent<CustomSphereCollider>();
        sphereCollider.Item = this;
        Debug.Log(sphereCollider.Item);

    }


    void Start()
    {
        sphereCollider = GetComponent<CustomSphereCollider>();
    }

    void Update()
    {
        if (GameManager.status != GameManager.GAME_STATUS.Play)
        {
            return;
        }
    }

    //public void Enter(Player player)
    //{
    //    float prevNum = player.GetGravityNum();
    //    player.GravityChanger(-100f);
    //    Debug.Log("GravityChanger");

    //}
    public void Enter(Player player)
    {
        // 効果の値
        float ChangeNum = -100f;

        // 効果を適用
        player.GravityChanger(ChangeNum);
        Debug.Log("GravityChanger");

        // 待機時間（秒）
        float delay = 3f;

        // 待機時間後に元の値に戻す
        StartCoroutine(RevertGravity(player, delay, ChangeNum));
    }

    private IEnumerator RevertGravity(Player player, float delay, float prevNum)
    {
        yield return new WaitForSeconds(delay);

        // 効果を元に戻す
        player.GravityChanger(-prevNum);
        Debug.Log("GravityChanger Reverted");
    }

}