using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform pivot;   // 振り子の軸となる位置
    public float length = 7f;   // 振り子の長さ
    public float gravity = 9.81f;   // 重力加速度

    private float angle;
    private float angularVelocity;

    [SerializeField]
    private float delay = 0f;       // オブジェクトの開始遅延時間
    private float elapsedTime = 0f; // 経過時間

    private void Start()
    {
        angle = Mathf.Atan2(transform.position.y - pivot.position.y, transform.position.x - pivot.position.x);
        angularVelocity = 0f;
    }

    private void Update()
    {
        // 指定した遅延時間が経過するまで待機
        if (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            return;
        }

        // 振り子の角度と角速度を更新
        float acceleration = -gravity / length * Mathf.Sin(angle);
        angularVelocity += acceleration * Time.deltaTime;
        angle += angularVelocity * Time.deltaTime;

        // オブジェクトの位置を更新
        Vector3 newPosition = pivot.position + new Vector3(length * Mathf.Sin(angle), -length * Mathf.Cos(angle), 0f);
        transform.position = newPosition;
        //Debug.Log("transform pendulum = " + transform.position);
    }
}