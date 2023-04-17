using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    float yOffset; // y���W�̏����ʒu���i�[
    float zOffset; // z���W�̏����ʒu���i�[

    const float X_RATIO = 0.9f; // ���̒��S�ƃv���C���[�̊Ԃ̐�����̉����ڂɔz�u���邩

    private void Start()
    {
        yOffset = transform.position.y;
        zOffset = transform.position.z;
    }

    void Update()
    {
        // �e���W�̌v�Z
        var x = player.transform.position.x * X_RATIO;
        var y = player.transform.position.y + yOffset;
        var z = player.transform.position.z + zOffset;

        // �ڕW�n�_�̈ʒu�x�N�g������
        Vector3 newLocalPos = new Vector3(x, y, z);

        // �ڕW�n�_�ւ������ړ�������
        transform.position = Vector3.Lerp(transform.position, newLocalPos, 0.2f);
    }
}