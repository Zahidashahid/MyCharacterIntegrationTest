using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCloneObject : MonoBehaviour
{
    public float lifeTime = 3;
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
