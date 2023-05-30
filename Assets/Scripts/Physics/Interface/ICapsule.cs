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
    //public  SetRadius { get; }
    /// <summary>
    /// 中心のローカル座標
    /// </summary>
    public Vector3 GetCenter();
    public void SetCenter(Vector3 center);

    //public void SetCenter(Vector3 value);
    /// <summary>
    /// ローカル座標においてカプセルが伸びる方向
    /// </summary>
    /// 
    //public Direction GetDirection { get; }
    //public void SetDirection(Direction value);
    /// <summary>
    /// カプセルの空間における高さ
    /// </summary>
    public float GetHeight { get; }
    //public void SetHeight(float value);
    Transform GetTransform();
}
