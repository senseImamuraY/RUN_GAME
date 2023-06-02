using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlane
{
    Vector3 GetNormal();

    float GetXSize { get; }

    float GetZSize { get; }
    //Vector3 GetVectorX();
    //Vector3 GetVectorZ();

    Vector3 GetCenter();

    Quaternion GetRotation();

    Vector3 getForward();
}
