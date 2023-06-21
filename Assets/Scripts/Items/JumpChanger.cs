using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpChanger : MonoBehaviour, IItem
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

    public void Enter(Player player)
    {
        //効果の値
        float ChangeNum = 20f;

        // 効果を適用
        player.JumpPowerChanger(ChangeNum);
        Debug.Log("JumpPowerChanger");

        // 待機時間（秒）
        float delay = 3f;

        // 待機時間後に元の値に戻す
        StartCoroutine(RevertGravity(player, delay, ChangeNum));
    }

    private IEnumerator RevertGravity(Player player, float delay, float prevNum)
    {
        yield return new WaitForSeconds(delay);

        // 効果を元に戻す
        player.JumpPowerChanger(-prevNum);
        Debug.Log("JumpPowerChanger Reverted");
    }
}