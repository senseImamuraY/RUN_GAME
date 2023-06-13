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
    /// 軸の方向
    /// 値はベクトルにしたときに1を代入する要素のindexを示す
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

    private Transform capsuleTransform;

    public Transform GetTransform() {  return capsuleTransform; }

    float X, Y, Z;
    // Start is called before the first frame update
    void Start()
    {
        radius = 0.5f;
        player = GetComponent<Player>();
        gravity = GetComponent<Gravity>();
        capsuleTransform = transform;
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
        // 今回は当たり判定を簡易的に作成
        // OBBとOBBの当たり判定を使用
        Vector3 Interval = box.GetCenter - center;

        Vector3 arrange = box.GetArrange();

        Transform targetTransform = box.GetTransform();
        Vector3 targetLocalScale = targetTransform.localScale;

        // 分離軸Ae1
       // float rA = transform.localScale.x;
        //loat rB = LenSegOnSeparateAxis(new Vector3(targetLocalScale.normalized.x, 0, 0), targetTransform);
        //float L = Mathf.Abs(Vector3.Dot(Interval, new Vector3(targetLocalScale.normalized.x, 0, 0)));
        //if (L > rA + rB) return false;
        float rA = transform.localScale.x - 0.5f;
        float rB = LenSegOnSeparateAxis(transform.right * transform.localScale.x, targetTransform, false, arrange);
        float L = Mathf.Abs(Vector3.Dot(Interval, transform.right));
        if (L > rA + rB) return false;

        // 分離軸Ae2
        rA = transform.localScale.y;
        //rA = transform.localScale.y + 1.0f;
        rB = LenSegOnSeparateAxis(transform.up * transform.localScale.y, targetTransform, false, arrange);
        L = Mathf.Abs(Vector3.Dot(Interval, transform.up));
        if (L > rA + rB) return false;

        // 分離軸Ae3
        rA = transform.localScale.z - 0.5f;
        rB = LenSegOnSeparateAxis(transform.forward * transform.localScale.z, targetTransform, false, arrange);
        L = Mathf.Abs(Vector3.Dot(Interval, transform.forward));
        if (L > rA + rB) return false;

        // 
        rA = LenSegOnSeparateAxis(targetTransform.right * targetTransform.localScale.x, this.transform, true, arrange);
        rB = (targetTransform.localScale.x + arrange.x) / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, targetTransform.right));
        if (L > rA + rB) return false;

        rA = LenSegOnSeparateAxis(targetTransform.up * targetTransform.localScale.y, this.transform, true, arrange);
        rB = (targetTransform.localScale.y + arrange.y) / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval, targetTransform.up));
        if (L > rA + rB) return false;

        rA = LenSegOnSeparateAxis(targetTransform.forward * targetTransform.localScale.z, this.transform, true, arrange);
        rB = (targetTransform.localScale.z + arrange.z) / 2.0f;
        L = Mathf.Abs(Vector3.Dot(Interval,targetTransform.forward));
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
                rA = LenSegOnSeparateAxis(Cross, transform, true, arrange);
                rB = LenSegOnSeparateAxis(Cross, targetTransform, false, arrange);
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
        // それぞれの中心からの距離を足した値と、ベクトルの大きさを比較する
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
        // 今回はカプセルをz軸に寝かせて考える
        // カプセルの空間との変換行列
        var forward = Vector3.zero;
        forward[(int)direction] = 1.0f;
        var casuleToLocal = Matrix4x4.TRS(arangementCenter, Quaternion.LookRotation(forward), Vector3.one);
        //var worldToCapsule = casuleToLocal * transform.worldToLocalMatrix;
        var worldToCapsule = casuleToLocal.inverse * transform.worldToLocalMatrix;

        // カプセルの空間における球の中心
        var sphereCenter = worldToCapsule.MultiplyPoint(sphere.GetWorldCenter);

        // カプセルを構成する二つの半球の中心座標
        var end = new Vector3(0.0f, 0.0f, 1.0f) * (height - radius * 2.0f) / 2.0f;
        //Debug.Log("end = " + end);
        var start = end * -1.0f;

        // 二つの球をつなぐ線分との近傍点を求める
        var startToSphere = sphereCenter - start;
        var startToEnd = end - start;
        var nearLength = Vector3.Dot(startToEnd.normalized, startToSphere);
        var nearLengthRate = nearLength / startToEnd.magnitude;
        var near = start + startToEnd * Mathf.Clamp01(nearLengthRate);

        // 球とカプセルとの最短距離を求める
        var sqrDistance = 0.0f;
        var endToSphere = sphereCenter - end;
        var nearToSphere = sphereCenter - near;
        if (nearLengthRate < 0)
        {
            // 近傍点が線分上になく、start寄りにある場合
            sqrDistance = startToSphere.sqrMagnitude;
        }
        else if (nearLengthRate > 1)
        {
            // 近傍点が線分上になく、end寄りにある場合
            sqrDistance = endToSphere.sqrMagnitude;
        }
        else
        {
            // 近傍点が線分上にある場合
            sqrDistance = nearToSphere.sqrMagnitude;
        }

        if(sqrDistance - (sphere.GetRadius + radius) * (sphere.GetRadius + radius) <= 0)
        {
            sphere.IsColliding = true;
        }
        else
        {
            sphere.IsColliding = false;
        }

        return sqrDistance - (sphere.GetRadius + radius) * (sphere.GetRadius + radius) <= 0;
    }

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        // Capsule(Player)が床の範囲内にいるかどうかを確認。いない場合returnする。
        if (IsInRange(plane) == false) return false;

        // 平面上の点からカプセルの最低点（一番下の頂点）までの距離は
        // 平面の法線ベクトルと平面からカプセルの最低点へのベクトルの内積
        // をとることで求められるという性質を利用

        Vector3 normal = plane.GetNormal();

        // 平面の方程式のdを求める
        // 坂の傾斜角（θ）を計算
        Vector3 up = Vector3.up;

        // normalとupはどちらも正規化されて計算されるのでcosθの値を得ることができる
        float dotProduct = Vector3.Dot(normal, up);
        dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);

        // ラジアンで得られる
        float angle = Mathf.Acos(dotProduct);

        // tanθを計算
        float tanTheta = Mathf.Tan(angle);
        Debug.Log("tangent = " + tanTheta);
        float d = normal.x * capsuleBottom.x + normal.y * capsuleBottom.y + normal.z * capsuleBottom.z;
        planeY = -(normal.x * capsuleBottom.x + normal.z * capsuleBottom.z + d) / normal.y;
        //Debug.Log("d = " + d);
        float diff = capsuleBottom.y - planeY;

        //float diff2 = normal.x * capsuleBottom.x + normal.y * capsuleBottom.y + normal.z * capsuleBottom.z + d;
        //Debug.Log("diff2 = " + diff2);
        // 画面の振動を防止するため
        //float speed = 1f;
        //float arrangementNum = speed * tanTheta;
        float min = 0f;
        float max = 0.5f;

        // 計算誤差による落下防止

        Debug.Log("planeY = " + planeY);
        if (min <= diff && diff < max)
        {
            player.setForward(plane.getForward());
            //player.setPlayerPosition(planeY);
            return true;
        }
        // 傾いた坂を上がった時に落ちないようにするための処理
        if (diff < min)
        {
            player.setForward(plane.getForward());
            player.setPlayerPosition(planeY);
            return true;
        }
        else
        {
            Debug.Log("空中かオブジェクト上にいます");
            return false;
        }
    }

    bool IsInRange(IPlane plane)
    {
        Vector3 planeCenter = plane.GetCenter();
        float planeXSize = plane.GetXSize;
        float planeZSize = plane.GetZSize;

        if ((this.center.x >= (planeCenter.x + planeXSize)) || (this.center.x <= (planeCenter.x - planeXSize)))
        {
            Debug.Log("範囲外にいます");
            return false;
        }
        if ((this.center.z >= (planeCenter.z + planeZSize)) || (this.center.z <= (planeCenter.z - planeZSize)))
        {
            Debug.Log("範囲外にいます");
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

    // 分離軸に投影された軸成分から投影線分長を算出
    public static float LenSegOnSeparateAxis(Vector3 Sep, Transform target, bool isCapsule, Vector3 arrangeNum)
    {
        Vector3 targetLocalScale = target.localScale;
        if(isCapsule)
        {
            targetLocalScale.x -= 0.5f;
            //targetLocalScale.y += 1.0f;
            targetLocalScale.z -= 0.5f;
        }
        else
        {
            targetLocalScale = (targetLocalScale + arrangeNum) / 2.0f;
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
