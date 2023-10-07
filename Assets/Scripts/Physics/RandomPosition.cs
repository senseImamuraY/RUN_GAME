using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    // ���炷�͈͂��w�肷��
    [SerializeField]
    float rangeX = 0f, rangeY = 0f, rangeZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // �I�u�W�F�N�g�̌��݂̈ʒu���擾����
        Vector3 position = transform.position;

        // ���炷�ʂ������_���Ɍ��߂�
        float offsetX = Random.Range(-rangeX, rangeX);
        float offsetY = Random.Range(-rangeY, rangeY);
        float offsetZ = Random.Range(-rangeZ, rangeZ);

        // ���炵���ʒu�Ɉړ�����
        position.x += offsetX;
        position.y += offsetY;
        position.z += offsetZ;
        transform.position = position;
    }
}
