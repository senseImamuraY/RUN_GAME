using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    /// <summary>
    /// ’¼•û‘Ì‚Æ‚ÌÕ“Ë”»’è
    /// </summary>
    bool CheckCollisionWithCube(ICube target);

    /// <summary>
    /// ‹…‚Æ‚ÌÕ“Ë”»’è
    /// </summary>
    bool CheckCollisionWithSphere(ISphere target);

    /// <summary>
    /// ƒJƒvƒZƒ‹‚Æ‚ÌÕ“Ë”»’è
    /// </summary>
    bool CheckCollisionWithCapsule(ICapsule target);
    
    /// <summary>
    /// •½–Ê‚Æ‚ÌÕ“Ë”»’è
    /// </summary>
    bool CheckCollisionWithPlane(IPlane target);
}

