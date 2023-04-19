using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomCapsuleCollider : MonoBehaviour, ICollider, ICapsule
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
    private float radius = 0.5f;
    public float GetRadius { get { return radius; } }

    [SerializeField]
    private Vector3 center;
    public Vector3 GetCenter() { return center; }
    public void SetCenter(Vector3 value) { center = value; }

    [SerializeField]
    private Direction direction = Direction.YAxis;
    
    [SerializeField]
    private float height = 2.0f;
    public float GetHeight { get { return height; } }

    private Vector3 capsulePosition;
    // Start is called before the first frame update
    void Start()
    {
        radius = 0.5f;
        capsulePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckCollisionWithCube(ICube box)
    {
        // ���ꂼ��̒��S����̋����𑫂����l�ƁA�x�N�g���̑傫�����r����
        //float dist = (box.transform.position - this.transform.position).magnitude;
        //float wR = box.GetHalfWidth + this.Radius;

        //if (dist < wR)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        Debug.Log("Capsule�R���C�_�[��WithBox");
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithCapsule(ICapsule capsule)
    {
        // ���ꂼ��̒��S����̋����𑫂����l�ƁA�x�N�g���̑傫�����r����
        //float dist = (capsule.transform.position - this.transform.position).magnitude;
        //float wR = capsule.Radius + this.Radius;
        //if (dist < wR)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        // �J�v�Z���̋�ԂƂ̕ϊ��s��
        var forward = Vector3.zero;
        forward[(int)direction] = 1.0f;
        var casuleToLocal = Matrix4x4.TRS(center, Quaternion.LookRotation(forward), Vector3.one);
        var worldToCapsule = casuleToLocal.inverse * transform.worldToLocalMatrix;

        // �J�v�Z���̋�Ԃɂ����鋅�̒��S
        var sphereCenter = worldToCapsule.MultiplyPoint(sphere.GetWorldCenter);

        // �J�v�Z�����\�������̔����̒��S���W
        var end = new Vector3(0.0f, 0.0f, 1.0f) * (height - radius * 2.0f) / 2.0f;
        var start = end * -1.0f;

        // ��̋����Ȃ������Ƃ̋ߖT�_�����߂�
        var startToSphere = sphereCenter - start;
        var startToEnd = end - start;
        var nearLength = Vector3.Dot(startToEnd.normalized, startToSphere);
        var nearLengthRate = nearLength / startToEnd.magnitude;
        var near = start + startToEnd * Mathf.Clamp01(nearLengthRate);

        // ���ƃJ�v�Z���Ƃ̍ŒZ���������߂�
        var sqrDistance = 0.0f;
        var endToSphere = sphereCenter - end;
        var nearToSphere = sphereCenter - near;
        if (nearLengthRate < 0)
        {
            // �ߖT�_��������ɂȂ��Astart���ɂ���ꍇ
            sqrDistance = startToSphere.sqrMagnitude;
        }
        else if (nearLengthRate > 1)
        {
            // �ߖT�_��������ɂȂ��Aend���ɂ���ꍇ
            sqrDistance = endToSphere.sqrMagnitude;
        }
        else
        {
            // �ߖT�_��������ɂ���ꍇ
            sqrDistance = nearToSphere.sqrMagnitude;
        }

        return sqrDistance - (sphere.GetRadius + radius) * (sphere.GetRadius + radius) <= 0;
    }
}
