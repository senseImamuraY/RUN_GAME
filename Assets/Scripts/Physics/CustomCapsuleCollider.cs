using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCapsuleCollider : MonoBehaviour
{
    //target�̓��X�g�ɂ��Ȃ���΂Ȃ�Ȃ��B
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
            Debug.Log("�Ԃ���܂���");
        }
    }

    // ���̔���͌`���Ƃɕ����邩�A�e���v���[�g���g���ď������s��

}
