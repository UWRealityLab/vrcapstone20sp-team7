using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderScript : MonoBehaviour
{
    public Text text;
    public int count = 0;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        //count += 1;
        //Debug.Log("Collided, count = " + count);
        //text.text = "count: " + count;
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");
        //count -= 1;
        //Debug.Log("Exit, count = " + count);
        //text.text = "count: " + count;
    }
}
