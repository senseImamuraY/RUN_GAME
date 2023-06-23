using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICapsule
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

    /// <summary>
    /// カプセルを構成する半球の半径
    /// </summary>
    public float GetRadius { get; }
 
    /// <summary>
    /// 中心のローカル座標
    /// </summary>
    public Vector3 GetCenter();

    public void SetCenter(Vector3 center);

    /// <summary>
    /// カプセルの空間における高さ
    /// </summary>
    public float GetHeight { get; }

    Transform GetTransform();
}
