using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICube
{
    Vector3 GetCenter { get; }
    void SetCenter(Vector3 center);
    Vector3 GetSize { get ;}

    bool IsGround { get; }

    bool GetIsColliding { get; }
    void SetColliding(bool isColliding);

    Transform GetTransform();

    bool IsColliding { get; set; }

    Vector3 GetArrange();
}
