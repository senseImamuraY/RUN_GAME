using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCapsuleCollider : MonoBehaviour
{
    //targetはリストにしなければならない。
    [SerializeField]
    private List<CustomBoxCollider> targetBox;
    public float Radius;
    Vector3 CapsulePosition;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        Radius = 0.5f;
        CapsulePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CustomBoxCollider target in targetBox)
        if(CollisionDetector.Instance.CheckCapsuleSphereCollision(this,target) == true)
        {
            Debug.Log("ぶつかりました");
        }
    }

    // この判定は形ごとに分けるか、テンプレートを使って処理を行う

}
