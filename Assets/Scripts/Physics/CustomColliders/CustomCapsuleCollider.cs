using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.LightAnchor;
using static UnityEngine.Rendering.DebugUI;

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
    private Vector3 prevCenter,center;
    public Vector3 GetCenter() { return center; }
    public Vector3 Center() { return center; }
    public void SetCenter(Vector3 value) { center = value; }

    [SerializeField]
    private Direction direction = Direction.YAxis;
    
    [SerializeField]
    private float height = 2.0f;

    [SerializeField]
    private Vector3 arangementCenter;
    public float GetHeight { get { return height; } }
    public float GetPlaneY () { return planeY; }
    private float planeY;

    private Player player;
    private Gravity gravity;
    private float prevY;
    //private Vector3 capsulePosition;
    private Vector3 capsuleBottom;

    [SerializeField]
    private Vector3 size;

    public Vector3 Size() {return size;}
    public Vector3 GetSize() { return size; }

    float X, Y, Z;
    // Start is called before the first frame update
    void Start()
    {
        radius = 0.5f;
        player = GetComponent<Player>();
        gravity = GetComponent<Gravity>();
        //capsulePosition= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "Player")
        {
            center = transform.position + transform.up;
        }
        else
        { 
            center = transform.position; 
        }
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


        Vector3 Interval = box.GetCenter - center;

        Transform targetTransform = box.GetTransform();
        Vector3 targetLocalScale = targetTransform.localScale;

        // ������Ae1
       // float rA = transform.localScale.x;
        //loat rB = LenSegOnSeparateAxis(new Vector3(targetLocalScale.normalized.x, 0, 0), targetTransform);
        //float L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(targetLocalScale.normalized.x, 0, 0)));
        //if (L > rA + rB) return false;
        float rA = transform.localScale.x - 0.5f;
        float rB = LenSegOnSeparateAxis(new Vector3(transform.localScale.normalized.x, 0, 0), targetTransform, false);
        float L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(transform.localScale.normalized.x, 0, 0)));
        if (L > rA + rB) return false;

        // ������Ae2
        rA = transform.localScale.y + 1.0f;
        rB = LenSegOnSeparateAxis(new Vector3(0, transform.localScale.normalized.y, 0), targetTransform, false);
        L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(0, transform.localScale.normalized.y, 0)));
        if (L > rA + rB) return false;

        // ������Ae3
        rA = transform.localScale.z - 0.5f;
        rB = LenSegOnSeparateAxis(new Vector3(0, 0, transform.localScale.normalized.z), targetTransform, false);
        L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(0, 0, transform.localScale.normalized.z)));
        if (L > rA + rB) return false;

        // 
        rA = LenSegOnSeparateAxis(new Vector3(targetLocalScale.normalized.x, 0, 0), this.transform, true);
        rB = targetTransform.localScale.x / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(targetTransform.localScale.x, 0, 0)));
        if (L > rA + rB) return false;

        rA = LenSegOnSeparateAxis(new Vector3(0, targetLocalScale.normalized.y, 0), this.transform, true);
        rB = targetTransform.localScale.y / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(0, targetTransform.localScale.y, 0)));
        if (L > rA + rB) return false;

        rA = LenSegOnSeparateAxis(new Vector3(0, 0, targetLocalScale.normalized.z), this.transform, true);
        rB = targetTransform.localScale.z / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(0, 0, targetTransform.localScale.z)));
        if (L > rA + rB) return false;

        Vector3[] axesA = new Vector3[3]
        {
            new Vector3(transform.localScale.normalized.x, 0, 0), // X axis
            new Vector3(0, transform.localScale.normalized.y, 0), // Y axis
            new Vector3(0, 0, transform.localScale.normalized.z)  // Z axis
        };

        Vector3[] axesB = new Vector3[3]
        {
            new Vector3(targetLocalScale.normalized.x, 0, 0), // X axis
            new Vector3(0, targetLocalScale.normalized.y, 0), // Y axis
            new Vector3(0, 0, targetLocalScale.normalized.z)  // Z axis
        };

        for (int i = 0; i < axesA.Length; i++)
        {
            for (int j = 0; j < axesB.Length; j++)
            {
                Vector3 Cross = Vector3.Cross(axesA[i], axesB[j]);
                rA = LenSegOnSeparateAxis(Cross, transform, true);
                rB = LenSegOnSeparateAxis(Cross, targetTransform, false);
                L = Mathf.Abs(Vector3.Dot(Interval, Cross));
                if (L > rA + rB)
                {
                    return false; // No collision
                }
            }
        }

        return true;
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
        // ����̓J�v�Z����z���ɐQ�����čl����
        // �J�v�Z���̋�ԂƂ̕ϊ��s��
        var forward = Vector3.zero;
        forward[(int)direction] = 1.0f;
        var casuleToLocal = Matrix4x4.TRS(arangementCenter, Quaternion.LookRotation(forward), Vector3.one);
        //var worldToCapsule = casuleToLocal * transform.worldToLocalMatrix;
        var worldToCapsule = casuleToLocal.inverse * transform.worldToLocalMatrix;

        // �J�v�Z���̋�Ԃɂ����鋅�̒��S
        var sphereCenter = worldToCapsule.MultiplyPoint(sphere.GetWorldCenter);

        // �J�v�Z�����\�������̔����̒��S���W
        var end = new Vector3(0.0f, 0.0f, 1.0f) * (height - radius * 2.0f) / 2.0f;
        //Debug.Log("end = " + end);
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
        // Capsule(Player)�����͈͓̔��ɂ��邩�ǂ������m�F�B���Ȃ��ꍇreturn����B
        if (IsInRange(plane) == false) return false;

        // ���ʏ�̓_����J�v�Z���̍Œ�_�i��ԉ��̒��_�j�܂ł̋�����
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
        //normal.y = Mathf.Round(normal.y * 100000) / 100000;
        // ���ʂ̕�������d�����߂�

        // ��̌X�Ίp�i�Ɓj���v�Z
        Vector3 up = Vector3.up;
        //Vector3 up = Vector3.up;
        // normal��up�͂ǂ�������K������Čv�Z�����̂�cos�Ƃ̒l�𓾂邱�Ƃ��ł���
        float dotProduct = Vector3.Dot(normal, up);
        //dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
        float angle = Mathf.Acos(dotProduct); // ���W�A���œ�����
        //Debug.Log("angle = " + angle);
        // tan�Ƃ��v�Z
        float tanTheta = Mathf.Tan(angle);
        //Debug.Log("tanTheta = " + tanTheta);

        float d = normal.x * capsuleBottom.x + normal.y * capsuleBottom.y + normal.z * capsuleBottom.z;
        planeY = -(normal.x * capsuleBottom.x + normal.z * capsuleBottom.z + d) / normal.y;
        
        float diff = capsuleBottom.y - planeY;
        //Debug.Log("diff = " + diff);
        //if (Mathf.Abs(prevY - planeY) <= 0.1)
        //{
        //    planeY = prevY;
        //}
        //else if(prevY > planeY)
        //{
        //    float descentSpeed = 10f;

        //    // ���Ԍo�߂ɉ��������~���x���v�Z
        //    Vector3 descentVelocity = normal * descentSpeed * Time.deltaTime;
        //    Debug.Log("descentVelocity = " + descentVelocity);
        //    // �v���C���[�̐V�����ʒu���v�Z
        //    prevY = prevY + descentVelocity.y;

        //    // �v���C���[��V�����ʒu�Ɉړ�������
        //    planeY = prevY;
        //    planeY = Mathf.Round(planeY * 10f) / 10f;

        //}
        //else
        //{
        //    planeY = Mathf.Round(planeY * 10f) / 10f;
        //}

        //Debug.Log("normal = " + normal);
        // ���ʏ�̓_�͌�₪�����ɑ��݂��邪�A
        // (-d/3a, -d/3b, -d/3c)���m���ɕ��ʏ�ɑ��݂��邽��
        // ����𕽖ʏ�̓_�Ƃ���
        Vector3 planePoint = new Vector3(-d / (3 * X), -d / (3 * Y), -d / (3 * Z));

        Vector3 VectorPlaneToCapsuleBottom = (capsuleBottom) - planePoint;
        //Debug.Log("VectorPlaneToCapsuleBottom.y = " + VectorPlaneToCapsuleBottom.y);
        float minimumDistance = Vector3.Dot(normal, VectorPlaneToCapsuleBottom);
        // minimumDistance�̒l�����̂܂܂ł͑傫������̂ŁA�����Ēl�𒲐�
        minimumDistance = minimumDistance * 0.0000001f;
        //Debug.Log("miniDistance = " + minimumDistance);



        //if (minimumDistance <= 1f)
        //    //if (minimumDistance >= -0.1f && minimumDistance <= 0.1f)
        //    //if (minimumDistance >= -0.1f && minimumDistance <= 1f)
        //{
        //    player.setPlayerPosition(planeY);
        //    prevY = planeY;
        //    Debug.Log("���ʏ�ɗ����Ă��܂�");
        //    return true;

        //}
        //else 
        //{
        //    Debug.Log("�󒆂��I�u�W�F�N�g��ɂ��܂�");
        //    return false;
        //}
        float speed = 0.5f;
        //float speed = 0.05f;
        //Debug.Log("diff = " + diff);
        //Debug.Log("speed * tanTheta = " + speed * tanTheta);

        if (0.0f < diff && diff < 0.5f)
        {
            //Debug.Log("�Ԃ�΍�");
            return true;
        }
         if (diff <= speed * tanTheta)
        {
            //Debug.Log("Collision��planeY��ݒ�@= " + planeY);
            player.setPlayerPosition(planeY);
            
            //Debug.Log("tanTheta = " + tanTheta);
            return true;
        }
        else
        {
            //gravity.SetIsGround(false);
            //gravity.ClearVelocity();
            //if (capsuleBottom.y <= planeY) 
            //{
            //    capsuleBottom.y = planeY;

            //}
            //if ()
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
    
    public void SetCapsuleBottom()
    {
        //Debug.Log("center = " + center);
        if (Mathf.Abs(prevCenter.y - center.y) <= 0.1)
        {
            center = (prevCenter + center) / 2;
        }
        center.y = Mathf.Round(center.y * 10f) / 10f;
        capsuleBottom = center - new Vector3(0.0f, (height / 2.0f), 0.0f);
        prevCenter = center;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0.2f, 0.5f, 1.0f);
    //    float halfHeight = height / 2 - radius;
    //    Vector3 upDirection = Vector3.up * halfHeight;
    //    Vector3 downDirection = -Vector3.up * halfHeight;

    //    // Draw the top sphere
    //    Gizmos.DrawWireSphere(center + upDirection, radius);

    //    // Draw the bottom sphere
    //    Gizmos.DrawWireSphere(center + downDirection, radius);

    //    // Draw the tube with multiple lines
    //    int numLines = 4; // or increase this for a smoother tube
    //    float angleStep = 360f / numLines;
    //    for (int i = 0; i < numLines; i++)
    //    {
    //        float angle = i * angleStep * Mathf.Deg2Rad;
    //        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    //        Gizmos.DrawLine(center + upDirection + offset, center + downDirection + offset);
    //    }
    //}

    // �������ɓ��e���ꂽ���������瓊�e���������Z�o
    public static float LenSegOnSeparateAxis(Vector3 Sep, Transform target, bool isCapsule)
    {
        Vector3 targetLocalScale = target.localScale;
        if(isCapsule)
        {
            targetLocalScale.x -= 0.5f;
            targetLocalScale.y += 1.0f;
            targetLocalScale.z -= 0.5f;
        }
        else
        {
            targetLocalScale = targetLocalScale / 2.0f;
        }
        float sum = 0;

        // X, Y, Z�̊e���ɑ΂��ď������s��
        for (int i = 0; i < 3; i++)
        {
            Vector3 axis = Vector3.zero;
            axis[i] = targetLocalScale[i]; // �e���̃X�P�[����K�p
            sum += Mathf.Abs(Vector3.Dot(Sep, axis)); // �e���ɑ΂��铊�e�̒������v�Z
        }

        return sum;
    }

}
