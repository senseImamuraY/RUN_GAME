using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISphere
{
    /// <summary>
    /// 中心のローカル座標
    /// </summary>
    Vector3 GetCenter { get; }
    /// <summary>
    /// 半径
    /// </summary>
    float GetRadius { get; }
    /// <summary>
    /// 中心のワールド座標
    /// </summary>
    Vector3 GetWorldCenter { get; }

    bool IsColliding { get; set; }
         
}
