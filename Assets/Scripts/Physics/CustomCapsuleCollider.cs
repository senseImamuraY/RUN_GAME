using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCapsuleCollider : MonoBehaviour, ICollider, ICapsule
{
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

    }
    
    //public override bool CheckCollision(Shape target)
    //{

    //    if (target is CustomBoxCollider box)
    //    {
    //        return CheckCollisionWithBox(box);
    //    }
    //    return false;
    //}

    public bool CheckCollisionWithBox(IBox box)
    {
        // ���ꂼ��̒��S����̋����𑫂����l�ƁA�x�N�g���̑傫�����r����
        //float dist = (box.transform.position - this.transform.position).magnitude;
        //float wR = box.GetHalfWidth + this.Radius;

        //if (dist < wR)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        Debug.Log("Capsule�R���C�_�[��WithBox");
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithCapsule(ICapsule capsule)
    {
        // ���ꂼ��̒��S����̋����𑫂����l�ƁA�x�N�g���̑傫�����r����
        //float dist = (capsule.transform.position - this.transform.position).magnitude;
        //float wR = capsule.Radius + this.Radius;
        //if (dist < wR)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        throw new System.NotImplementedException();
    }

    public bool CheckCollisionWithSphere(ISphere target)
    {
        throw new System.NotImplementedException();
    }
}
