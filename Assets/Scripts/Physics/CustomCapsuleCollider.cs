using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //private Vector3 capsulePosition;
    private Vector3 capsuleBottom;

    float X, Y, Z;
    // Start is called before the first frame update
    void Start()
    {
        radius = 0.5f;
        //capsulePosition= transform.position;
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

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        // ���ʏ�̓_����J�v�Z���̍Œ�_�܂ł̋�����
        // ���ʂ̖@���x�N�g���ƕ��ʂ���J�v�Z���̍Œ�_�ւ̃x�N�g���̓���
        // ���Ƃ邱�Ƃŋ��߂���Ƃ��������𗘗p
        //capsuleBottom = center - new Vector3(0.0f, -(height / 2.0f), 0.0f);
        Vector3 normal = plane.GetNormal();
        // �@���x�N�g���̐�����0�̏ꍇ�A0�Ŋ��邱�ƂɂȂ�̂ŁA
        // �����������邽�߂ɒl����
        if (normal.x < 0.01f)
        {
            X = 0.01f;
        }
        else
        {
            X = normal.x;
        }
        if (normal.y < 0.01f)
        {
            Y = 0.01f;
        }
        else
        {
            Y = normal.y;
        }
        if (normal.z < 0.01f)
        {
            Z = 0.01f;
        }
        else
        {
            Z = normal.z;
        }
        // ���ʂ̕�������d�����߂�
        float d = normal.x * capsuleBottom.x + normal.y * capsuleBottom.y + normal.z * capsuleBottom.z;
        //Debug.Log("normal = " + normal);
        // ���ʏ�̓_�͌�₪�����ɑ��݂��邪�A
        // (-d/3a, -d/3b, -d/3c)���m���ɕ��ʏ�ɑ��݂��邽��
        // ����𕽖ʏ�̓_�Ƃ���
        Vector3 planePoint = new Vector3(-d / (3 * X), -d / (3 * Y), -d / (3 * Z));

        Vector3 VectorPlaneToCapsuleBottom = (capsuleBottom) - planePoint;
        float minimumDistance = Vector3.Dot(normal, VectorPlaneToCapsuleBottom);
        // minimumDistance�̒l�����̂܂܂ł͑傫������̂ŁA�PM�Ŋ����Ēl�𒲐�
        minimumDistance = minimumDistance * 0.0000001f;
        Debug.Log("miniDistance = " + minimumDistance);
        if (IsInRange(plane) == false) return false;
        //Debug.Log("capsuleBottom = " + capsuleBottom);

        //if (minimumDistance >= 0.1f)
        if (minimumDistance >= -0.1f && minimumDistance <= 0.1f)
        {
            Debug.Log("���ʏ�ɗ����Ă��܂�");
            return true;

        }
        else 
        {
            Debug.Log("�󒆂��I�u�W�F�N�g��ɂ��܂�");
            return false;
        }
        //float dist = Vector3.Dot(plane.GetNormal, capsuleBottom - plane.GetPoint);
    }

    bool IsInRange(IPlane plane)
    {
        Vector3 planeCenter = plane.GetCenter();
        float planeXSize = plane.GetXSize;
        float planeZSize = plane.GetZSize;

        if ((this.center.x >= (planeCenter.x + planeXSize)) || (this.center.x <= (planeCenter.x - planeXSize)))
        {
            return false;
        }
        if ((this.center.z >= (planeCenter.z + planeZSize)) || (this.center.z <= (planeCenter.z - planeZSize)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //public static bool operator <=(Vector3 v, float f)
    //{
    //    return v.x <= f;
    //}
    public void SetCapsuleBottom()
    {
        capsuleBottom = center - new Vector3(0.0f, (height / 2.0f), 0.0f);
    }
}
