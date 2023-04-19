using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICube
{
    //Vector3 GetAndSetCenter { get; set; }
    Vector3 GetCenter { get; }
    void SetCenter(Vector3 center);
    Vector3 GetSize { get ;}
}
