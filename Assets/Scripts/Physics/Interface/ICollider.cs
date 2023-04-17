using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    bool CheckCollisionWithBox(IBox target);

    bool CheckCollisionWithSphere(ISphere target);

    bool CheckCollisionWithCapsule(ICapsule target);
}

