using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlane
{
    Vector3 GetNormal();

    float GetXSize { get; }

    float GetZSize { get; }

    Vector3 GetCenter();
}
