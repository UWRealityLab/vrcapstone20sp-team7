using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSideScript : MonoBehaviour
{
    public ColliderScript colliderScript;

    void OnCollisionEnter(Collision collision)
    {
        colliderScript.CollisionEnter(collision);
    }
}
