using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform pivot;   // 振り子の軸となる位置
    public float length = 5f;   // 振り子の長さ
    public float gravity = 9.81f;   // 重力加速度

    private float angle;
    private float angularVelocity;

    private void Start()
    {
        angle = Mathf.Atan2(transform.position.y - pivot.position.y, transform.position.x - pivot.position.x);
        angularVelocity = 0f;
    }

    private void Update()
    {
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