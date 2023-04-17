using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    bool CheckCollisionWithCube(ICube target);
    /// <summary>
    /// ���Ƃ̏Փ˔���
    /// </summary>
    bool CheckCollisionWithSphere(ISphere target);

    bool CheckCollisionWithCapsule(ICapsule target);
}

