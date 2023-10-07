using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider
{
    /// <summary>
    /// �����̂Ƃ̏Փ˔���
    /// </summary>
    bool CheckCollisionWithCube(ICube target);

    /// <summary>
    /// ���Ƃ̏Փ˔���
    /// </summary>
    bool CheckCollisionWithSphere(ISphere target);

    /// <summary>
    /// �J�v�Z���Ƃ̏Փ˔���
    /// </summary>
    bool CheckCollisionWithCapsule(ICapsule target);
    
    /// <summary>
    /// ���ʂƂ̏Փ˔���
    /// </summary>
    bool CheckCollisionWithPlane(IPlane target);
}

