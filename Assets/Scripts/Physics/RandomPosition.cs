using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    // ずらす範囲を指定する
    [SerializeField]
    float rangeX = 0f, rangeY = 0f, rangeZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトの現在の位置を取得する
        Vector3 position = transform.position;

        // ずらす量をランダムに決める
        float offsetX = Random.Range(-rangeX, rangeX);
        float offsetY = Random.Range(-rangeY, rangeY);
        float offsetZ = Random.Range(-rangeZ, rangeZ);

        // ずらした位置に移動する
        position.x += offsetX;
        position.y += offsetY;
        position.z += offsetZ;
        transform.position = position;
    }
}
