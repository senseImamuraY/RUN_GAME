using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    /// <summary>
    /// 直方体との衝突判定
    /// </summary>
    bool CheckCollisionWithCube(ICube target);

    /// <summary>
    /// 球との衝突判定
    /// </summary>
    bool CheckCollisionWithSphere(ISphere target);

    /// <summary>
    /// カプセルとの衝突判定
    /// </summary>
    bool CheckCollisionWithCapsule(ICapsule target);
    
    /// <summary>
    /// 平面との衝突判定
    /// </summary>
    bool CheckCollisionWithPlane(IPlane target);
}

