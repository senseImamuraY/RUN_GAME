using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCamera : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    float yOffset; // y座標の初期位置を格納
    float zOffset; // z座標の初期位置を格納

    const float X_RATIO = 0.7f; // 道の中心とプレイヤーの間の線分上の何分目に配置するか

    private void Start()
    {
        yOffset = transform.localPosition.y;
        zOffset = transform.localPosition.z;
    }

    void Update()
    {
        // 各座標の計算
        var x = player.transform.localPosition.x * X_RATIO;
        var y = player.transform.localPosition.y + yOffset;
        var z = player.transform.localPosition.z + zOffset;

        // 目標地点の位置ベクトル生成
        Vector3 newLocalPos = new Vector3(x, y, z);

        // 目標地点へゆっくり移動させる
        transform.localPosition = Vector3.Lerp(transform.localPosition, newLocalPos, 0.2f);
    }
}