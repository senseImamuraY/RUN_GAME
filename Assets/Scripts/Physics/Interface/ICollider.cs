using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    bool CheckCollisionWithCube(ICube target);
    /// <summary>
    /// ‹…‚Æ‚ÌÕ“Ë”»’è
    /// </summary>
    bool CheckCollisionWithSphere(ISphere target);

    bool CheckCollisionWithCapsule(ICapsule target);
}

