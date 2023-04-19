using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
}
