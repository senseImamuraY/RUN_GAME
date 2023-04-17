using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    float yOffset; // y座標の初期位置を格納
    float zOffset; // z座標の初期位置を格納

    const float X_RATIO = 0.9f; // 道の中心とプレイヤーの間の線分上の何分目に配置するか

    private void Start()
    {
        yOffset = transform.position.y;
        zOffset = transform.position.z;
    }

    void Update()
    {
        // 各座標の計算
        var x = player.transform.position.x * X_RATIO;
        var y = player.transform.position.y + yOffset;
        var z = player.transform.position.z + zOffset;

        // 目標地点の位置ベクトル生成
        Vector3 newLocalPos = new Vector3(x, y, z);

        // 目標地点へゆっくり移動させる
        transform.position = Vector3.Lerp(transform.position, newLocalPos, 0.2f);
    }
}