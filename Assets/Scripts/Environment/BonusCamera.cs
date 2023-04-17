using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCamera : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    float yOffset; // y���W�̏����ʒu���i�[
    float zOffset; // z���W�̏����ʒu���i�[

    const float X_RATIO = 0.7f; // ���̒��S�ƃv���C���[�̊Ԃ̐�����̉����ڂɔz�u���邩

    private void Start()
    {
        yOffset = transform.localPosition.y;
        zOffset = transform.localPosition.z;
    }

    void Update()
    {
        // �e���W�̌v�Z
        var x = player.transform.localPosition.x * X_RATIO;
        var y = player.transform.localPosition.y + yOffset;
        var z = player.transform.localPosition.z + zOffset;

        // �ڕW�n�_�̈ʒu�x�N�g������
        Vector3 newLocalPos = new Vector3(x, y, z);

        // �ڕW�n�_�ւ������ړ�������
        transform.localPosition = Vector3.Lerp(transform.localPosition, newLocalPos, 0.2f);
    }
}