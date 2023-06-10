using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform pivot;   // �U��q�̎��ƂȂ�ʒu
    public float length = 5f;   // �U��q�̒���
    public float gravity = 9.81f;   // �d�͉����x

    private float angle;
    private float angularVelocity;

    private void Start()
    {
        angle = Mathf.Atan2(transform.position.y - pivot.position.y, transform.position.x - pivot.position.x);
        angularVelocity = 0f;
    }

    private void Update()
    {
        // �U��q�̊p�x�Ɗp���x���X�V
        float acceleration = -gravity / length * Mathf.Sin(angle);
        angularVelocity += acceleration * Time.deltaTime;
        angle += angularVelocity * Time.deltaTime;

        // �I�u�W�F�N�g�̈ʒu���X�V
        Vector3 newPosition = pivot.position + new Vector3(length * Mathf.Sin(angle), -length * Mathf.Cos(angle), 0f);
        transform.position = newPosition;
        //Debug.Log("transform pendulum = " + transform.position);
    }
}