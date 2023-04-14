using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public float Width, Height;
    Vector3 BoxPosition;
    void Start()
    {
        BoxPosition = transform.position;
        Width = 0.5f;
        Height = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
