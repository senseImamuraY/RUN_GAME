using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class CustomCubeCollider : MonoBehaviour, ICollider, ICube
{
    [SerializeField]
    private bool isGround = false;
    public bool IsGround { get { return isGround; } }

    private bool isColliding = false;
    public bool GetIsColliding { get { return isColliding; } }

    public void SetColliding(bool value) { isColliding = value; }

    [SerializeField]
    private Vector3 center; 
    public Vector3 GetCenter { get { return center; } }

    public void SetCenter(Vector3 value) { center = value; }

    [SerializeField]
    private Vector3 size = Vector3.one;
    public Vector3 GetSize { get { return size; } }

    public Vector3 Size() { return size; }

    public Vector3 Center() { return center; }

    private Transform cubeTransform;

    [SerializeField]
    private Vector3 arrange = new Vector3(0, 0, 0);

    public Vector3 GetArrange() { return arrange; }
    public Transform GetTransform() { return cubeTransform; }

    public bool IsColliding { get; set; } = false;

    // Enemyクラスへの参照
    public IEnemy Enemy { get; set; }

    private void Awake()
    {
        cubeTransform = transform;
        SetCenter(this.transform.position);
    }

    void FixedUpdate()
    {
        center = transform.position;
    }
    public bool CheckCollisionWithCube(ICube cube)
    {
        Vector3 v3SubAbs = this.GetCenter - cube.GetCenter;
        v3SubAbs = new Vector3(Mathf.Abs(v3SubAbs.x), Mathf.Abs(v3SubAbs.y), Mathf.Abs(v3SubAbs.z));
        Vector3 v3AddScale = (this.GetSize + cube.GetSize) / 2.0f;
        if ((v3SubAbs.x < v3AddScale.x) &&
            (v3SubAbs.y < v3AddScale.y) &&
            (v3SubAbs.z < v3AddScale.z))
        {
            cube.SetColliding(true);
            return true;
        }
        else
        {
            cube.SetColliding(false);
            return false;
        }
    }

    public bool CheckCollisionWithSphere(ISphere sphere)
    {
        var cubeToLocal = Matrix4x4.TRS(center, Quaternion.identity, Vector3.one);
        var worldToCube = cubeToLocal.inverse * transform.worldToLocalMatrix;

        // Cubeの空間における球の中心点
        var sphereCenter = worldToCube.MultiplyPoint(sphere.GetWorldCenter);

        // 距離の二乗を求める
        var sqLength = 0.0f;
        for (int i = 0; i < 3; i++)
        {
            var point = sphereCenter[i];
            var boxMin = size[i] * -0.5f;
            var boxMax = size[i] * 0.5f;
            if (point < boxMin)
            {
                sqLength += (point - boxMin) * (point - boxMin);
            }
            if (point > boxMax)
            {
                sqLength += (point - boxMax) * (point - boxMax);
            }
        }

        // 距離の二乗が0だったらCubeの内部にSphereの中心があるということ
        if (sqLength == 0.0f)
        {
            return true;
        }

        return sqLength <= sphere.GetRadius * sphere.GetRadius;
    }

    public bool CheckCollisionWithCapsule(ICapsule capsule)
    {
        // 今回は当たり判定を簡易的に作成
        // OBBとOBBの当たり判定を使用
        Vector3 Interval = capsule.GetCenter() - center;

        Transform targetTransform = capsule.GetTransform();
        Vector3 targetLocalScale = targetTransform.localScale;

        // 分離軸Ae1
        // float rA = transform.localScale.x;
        //loat rB = LenSegOnSeparateAxis(new Vector3(targetLocalScale.normalized.x, 0, 0), targetTransform);
        //float L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(targetLocalScale.normalized.x, 0, 0)));
        //if (L > rA + rB) return false;
        float rA = transform.localScale.x - 0.5f + arrange.x;
        float rB = LenSegOnSeparateAxis(new Vector3(transform.localScale.normalized.x, 0, 0), targetTransform, true, arrange);
        float L = Mathf.Abs(Vector3.Dot(Interval, transform.right));
        if (L > rA + rB) return false;

        // 分離軸Ae2
        rA = transform.localScale.y + 1.0f + arrange.y;
        rB = LenSegOnSeparateAxis(new Vector3(0, transform.localScale.normalized.y, 0), targetTransform, true, arrange);
        L = Mathf.Abs(Vector3.Dot(Interval, transform.up));
        if (L > rA + rB) return false;

        // 分離軸Ae3
        rA = transform.localScale.z - 0.5f + arrange.z;
        rB = LenSegOnSeparateAxis(new Vector3(0, 0, transform.localScale.normalized.z), targetTransform, true, arrange);
        L = Mathf.Abs(Vector3.Dot(Interval, transform.forward));
        if (L > rA + rB) return false;

        // 
        rA = LenSegOnSeparateAxis(new Vector3(targetLocalScale.normalized.x, 0, 0), this.transform, false, arrange);
        rB = targetTransform.localScale.x / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, targetTransform.right));
        if (L > rA + rB) return false;

        rA = LenSegOnSeparateAxis(new Vector3(0, targetLocalScale.normalized.y, 0), this.transform, false, arrange);
        rB = targetTransform.localScale.y / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, targetTransform.up));
        if (L > rA + rB) return false;

        rA = LenSegOnSeparateAxis(new Vector3(0, 0, targetLocalScale.normalized.z), this.transform, false, arrange);
        rB = targetTransform.localScale.z / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, targetTransform.forward));
        if (L > rA + rB) return false;

        Vector3[] axesA = new Vector3[3]
        {
            transform.right * transform.localScale.x, // X axis
            transform.up * transform.localScale.y, // Y axis
            transform.forward * transform.localScale.z  // Z axis
        };

        Vector3[] axesB = new Vector3[3]
        {
            targetTransform.right * targetTransform.localScale.x, // X axis
            targetTransform.up * targetTransform.localScale.y, // Y axis
            targetTransform.forward * targetTransform.localScale.z  // Z axis
        };

        for (int i = 0; i < axesA.Length; i++)
        {
            for (int j = 0; j < axesB.Length; j++)
            {
                Vector3 Cross = Vector3.Cross(axesA[i], axesB[j]);
                rA = LenSegOnSeparateAxis(Cross, transform, false, arrange);
                rB = LenSegOnSeparateAxis(Cross, targetTransform, true, arrange);
                L = Mathf.Abs(Vector3.Dot(Interval, Cross));
                if (L > rA + rB)
                {
                    return false; // No collision
                }
            }
        }

        return true;
    }

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        throw new System.NotImplementedException();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.5f, 1.0f);

        // Set the transformation matrix for the Gizmos to be the same as the object's transform.
        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw a wireframe cube at the object's position with the object's scale.
        // Since we've set the Gizmos.matrix, the position, rotation, and scale of the cube will be correct.
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
    // 分離軸に投影された軸成分から投影線分長を算出
    public static float LenSegOnSeparateAxis(Vector3 Sep, Transform target, bool isCapsule, Vector3 arrangeNum)
    {
        Vector3 targetLocalScale = target.localScale;
        if (isCapsule)
        {
            targetLocalScale.x -= 0.5f;
            //targetLocalScale.y += 1.0f;
            targetLocalScale.z -= 0.5f;
        }
        else
        {
            targetLocalScale = targetLocalScale + arrangeNum / 2.0f;
            Debug.Log("LocalScale = " + targetLocalScale);
            
        }
        float sum = 0;

        // X, Y, Zの各軸に対して処理を行う
        for (int i = 0; i < 3; i++)
        {
            Vector3 axis = Vector3.zero;
            axis[i] = targetLocalScale[i]; // 各軸のスケールを適用
            sum += Mathf.Abs(Vector3.Dot(Sep, axis)); // 各軸に対する投影の長さを計算
        }

        return sum;
    }
}

