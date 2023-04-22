using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    bool CheckCollisionWithCube(ICube target);
    /// <summary>
    /// ãÖÇ∆ÇÃè’ìÀîªíË
    /// </summary>
    bool CheckCollisionWithSphere(ISphere target);

    bool CheckCollisionWithCapsule(ICapsule target);

    bool CheckCollisionWithPlane(IPlane target);
}

