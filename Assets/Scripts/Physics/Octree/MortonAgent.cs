using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An agent will be in octree.
/// </summary>
public class MortonAgent : MonoBehaviour
{
    private LinearTreeManager<GameObject> manager;
    public LinearTreeManager<GameObject> Manager
    {
        get
        {
            return manager;
        }
        set
        {
            if (manager == value)
            {
                return;
            }

            // Remove from current manager.
            TreeData.Remove();

            // Change to new manager and register myself.
            manager = value;
            RegisterUpdate();
        }
    }

    public TreeData<GameObject> TreeData { get; private set; }

    #region MonoBehaviour

    private Collider collider;
    // Bounds like AABB of this game object.
    public Bounds Bounds
    {
        get
        {
            if (collider == null)
            {
                collider = GetComponent<Collider>();
            }
            return collider.bounds;
        }
    }
    void Awake()
    {
        TreeData = new TreeData<GameObject>(gameObject);
    }

    void OnDestroy()
    {
        TreeData.Remove();
    }

    void Update()
    {
        if (manager == null)
        {
            return;
        }

        RegisterUpdate();
    }
    #endregion MonoBehaviour

    void RegisterUpdate()
    {
        manager.Register(Bounds, TreeData);
    }


}
