using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public static CollisionDetector Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckCapsuleSphereCollision(CustomCapsuleCollider player, CustomBoxCollider target)
    {
        // それぞれの中心からの距離を足した値と、ベクトルの大きさを比較する
        float dist = (target.transform.position - player.transform.position).magnitude;
        float a = target.Width + player.Radius;

        if (dist < a)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
