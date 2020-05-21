using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEffect : MonoBehaviour
{
    
	public GameObject redHand;
	public GameObject blueHand;
    // Start is called before the first frame update
    void Start()
    {
        redHand.SetActive(true);
        blueHand.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeToRed()
    {
    	redHand.SetActive(true);
        blueHand.SetActive(false);
    }

    public void changeToBlue()
    {
    	redHand.SetActive(false);
        blueHand.SetActive(true);
    }

    public void changeToNothing()
    {
        redHand.SetActive(false);
        blueHand.SetActive(false);
    }
}
