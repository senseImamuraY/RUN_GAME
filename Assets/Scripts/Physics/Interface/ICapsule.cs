using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICapsule
{
    /// <summary>
    /// ���̕���
    /// �l�̓x�N�g���ɂ����Ƃ���1��������v�f��index������
    /// </summary>
    public enum Direction
    {
        XAxis = 0,
        YAxis = 1,
        ZAxis = 2,
    }

    /// <summary>
    /// �J�v�Z�����\�����锼���̔��a
    /// </summary>
    public float GetRadius { get; }
    //public  SetRadius { get; }
    /// <summary>
    /// ���S�̃��[�J�����W
    /// </summary>
    public Vector3 GetCenter();
    public void SetCenter(Vector3 center);

    //public void SetCenter(Vector3 value);
    /// <summary>
    /// ���[�J�����W�ɂ����ăJ�v�Z�����L�т����
    /// </summary>
    /// 
    //public Direction GetDirection { get; }
    //public void SetDirection(Direction value);
    /// <summary>
    /// �J�v�Z���̋�Ԃɂ����鍂��
    /// </summary>
    public float GetHeight { get; }
    //public void SetHeight(float value);
    Transform GetTransform();
}
