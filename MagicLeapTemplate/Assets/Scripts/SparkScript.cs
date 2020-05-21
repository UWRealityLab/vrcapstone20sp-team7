using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkScript : MonoBehaviour
{
    public GameObject spark;
    public GameObject menu;
    public GameObject pointers;

    private bool isRendering;
    private float currTime;

    // Start is called before the first frame update
    public void Start()
    {
        isRendering = false;
        spark.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if (isRendering)
        {
            currTime += Time.deltaTime;
            if (currTime >= 0.5f)
            {
                spark.SetActive(false);
                isRendering = false;
                menu.SetActive(false);
            }
        }
    }

    public void OnClick()
    {
        isRendering = true;
        spark.SetActive(true);
        pointers.SetActive(false);
        currTime = 0f;
    }
}
