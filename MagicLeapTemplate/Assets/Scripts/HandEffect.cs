using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEffect : MonoBehaviour
{
    
	public UnityEngine.XR.MagicLeap.MLHandMeshingBehavior redHand;
	public UnityEngine.XR.MagicLeap.MLHandMeshingBehavior blueHand;
    // Start is called before the first frame update
    void Start()
    {
        redHand.enabled = true;
        blueHand.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeToRed()
    {
    	redHand.enabled = true;
        blueHand.enabled = false;
    }

    public void changeToBlue()
    {
    	redHand.enabled = false;
        blueHand.enabled = true;
    }
}
