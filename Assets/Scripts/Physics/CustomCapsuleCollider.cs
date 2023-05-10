using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86.Avx;
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
    public void SetCenter(Vector3 value) { center = value; }

    [SerializeField]
    private Direction direction = Direction.YAxis;
    
    [SerializeField]
    private float height = 2.0f;
    public float GetHeight { get { return height; } }

    private Player player;
    private float prevY;
    //private Vector3 capsulePosition;
    private Vector3 capsuleBottom;


    float X, Y, Z;
    // Start is called before the first frame update
    void Start()
    {
        radius = 0.5f;
        player = GetComponent<Player>();
        //capsulePosition= transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckCollisionWithCube(ICube box)
    {
        // それぞれの中心からの距離を足した値と、ベクトルの大きさを比較する
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
        Debug.Log("CapsuleコライダーのWithBox");
        throw new System.NotImplementedException();
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
        // カプセルの空間との変換行列
        var forward = Vector3.zero;
        forward[(int)direction] = 1.0f;
        var casuleToLocal = Matrix4x4.TRS(center, Quaternion.LookRotation(forward), Vector3.one);
        var worldToCapsule = casuleToLocal.inverse * transform.worldToLocalMatrix;

        // カプセルの空間における球の中心
        var sphereCenter = worldToCapsule.MultiplyPoint(sphere.GetWorldCenter);

        // カプセルを構成する二つの半球の中心座標
        var end = new Vector3(0.0f, 0.0f, 1.0f) * (height - radius * 2.0f) / 2.0f;
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

        return sqrDistance - (sphere.GetRadius + radius) * (sphere.GetRadius + radius) <= 0;
    }

    public bool CheckCollisionWithPlane(IPlane plane)
    {
        // 平面上の点からカプセルの最低点（一番下の頂点）までの距離は
        // 平面の法線ベクトルと平面からカプセルの最低点へのベクトルの内積
        // をとることで求められるという性質を利用
        //capsuleBottom = center - new Vector3(0.0f, -(height / 2.0f), 0.0f);
        Vector3 normal = plane.GetNormal();
        // 法線ベクトルの成分が0の場合、0で割ることになるので、
        // それを回避するために値を代入
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
        // 平面の方程式のdを求める

        // 坂の傾斜角（θ）を計算
        Vector3 up = Vector3.up;
        float dotProduct = Vector3.Dot(normal, up);
        dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);
        float angle = Mathf.Acos(dotProduct); // ラジアンで得られる
        Debug.Log("angle = " + angle);
        // tanθを計算
        float tanTheta = Mathf.Tan(angle);
        Debug.Log("tanTheta = " + tanTheta);

        float d = normal.x * capsuleBottom.x + normal.y * capsuleBottom.y + normal.z * capsuleBottom.z;
        float planeY = -(normal.x * capsuleBottom.x + normal.z * capsuleBottom.z + d) / normal.y;

        float diff = capsuleBottom.y - planeY;

        //if (Mathf.Abs(prevY - planeY) <= 0.1)
        //{
        //    planeY = prevY;
        //}
        //else if(prevY > planeY)
        //{
        //    float descentSpeed = 10f;

        //    // 時間経過に応じた下降速度を計算
        //    Vector3 descentVelocity = normal * descentSpeed * Time.deltaTime;
        //    Debug.Log("descentVelocity = " + descentVelocity);
        //    // プレイヤーの新しい位置を計算
        //    prevY = prevY + descentVelocity.y;

        //    // プレイヤーを新しい位置に移動させる
        //    planeY = prevY;
        //    planeY = Mathf.Round(planeY * 10f) / 10f;

        //}
        //else
        //{
        //    planeY = Mathf.Round(planeY * 10f) / 10f;
        //}

        //Debug.Log("normal = " + normal);
        // 平面上の点は候補が無数に存在するが、
        // (-d/3a, -d/3b, -d/3c)が確実に平面上に存在するため
        // これを平面上の点とする
        Vector3 planePoint = new Vector3(-d / (3 * X), -d / (3 * Y), -d / (3 * Z));

        Vector3 VectorPlaneToCapsuleBottom = (capsuleBottom) - planePoint;
        //Debug.Log("VectorPlaneToCapsuleBottom.y = " + VectorPlaneToCapsuleBottom.y);
        float minimumDistance = Vector3.Dot(normal, VectorPlaneToCapsuleBottom);
        // minimumDistanceの値がそのままでは大きすぎるので、割って値を調整
        minimumDistance = minimumDistance * 0.0000001f;
        Debug.Log("miniDistance = " + minimumDistance);

        // Capsule(Player)が床の範囲内にいるかどうかを確認。いない場合returnする。
        if (IsInRange(plane) == false) return false;

        //if (minimumDistance <= 1f)
        //    //if (minimumDistance >= -0.1f && minimumDistance <= 0.1f)
        //    //if (minimumDistance >= -0.1f && minimumDistance <= 1f)
        //{
        //    player.setPlayerPosition(planeY);
        //    prevY = planeY;
        //    Debug.Log("平面上に立っています");
        //    return true;

        //}
        //else 
        //{
        //    Debug.Log("空中かオブジェクト上にいます");
        //    return false;
        //}
        float speed = 0.05f;
        //Debug.Log("diff = " + diff);
        //Debug.Log("speed * tanTheta = " + speed * tanTheta);
        if (0.0f < diff && diff < radius / 1.0f)
        {
            return true;
        }
        else if (diff <= speed * tanTheta)
        {
            player.setPlayerPosition(planeY);
            //Debug.Log("tanTheta = " + tanTheta);
            return true;
        }
        else
        {
            Debug.Log("空中かオブジェクト上にいます");
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
        Debug.Log("center = " + center);
        if (Mathf.Abs(prevCenter.y - center.y) <= 0.1)
        {
            center = (prevCenter + center) / 2;
        }
        center.y = Mathf.Round(center.y * 10f) / 10f;
        capsuleBottom = center - new Vector3(0.0f, (height / 2.0f), 0.0f);
        prevCenter = center;
    }
}
