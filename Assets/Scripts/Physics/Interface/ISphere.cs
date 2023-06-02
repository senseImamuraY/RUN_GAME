using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISphere
{
    /// <summary>
    /// ���S�̃��[�J�����W
    /// </summary>
    Vector3 GetCenter { get; }
    /// <summary>
    /// ���a
    /// </summary>
    float GetRadius { get; }
    /// <summary>
    /// ���S�̃��[���h���W
    /// </summary>
    Vector3 GetWorldCenter { get; }

    bool IsColliding { get; set; }
         
}
