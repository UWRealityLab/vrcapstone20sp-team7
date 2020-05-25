using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColliderCleaning : MonoBehaviour
{
    public Button button;
    public SparkScript sparkScript;

    public HandEffect handEffect;
    public MeshCombiner leftMeshCombiner, rightMeshCombiner;

    public ContaminationScript contaminationScript;

    private bool isBeingHovered;
    private float currTime;

    private readonly float selectTime = 1.5f;


    public void Start()
    {
        isBeingHovered = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isBeingHovered)
        {
            currTime += Time.deltaTime;

            ColorBlock colors = button.colors;
            colors.normalColor = Color.Lerp(Color.white, Color.blue, currTime / selectTime);
            colors.highlightedColor = Color.white;
            button.colors = colors;

            if (currTime >= selectTime)
            {
                OnSelect();
                isBeingHovered = false;
            }
        }
        else
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            button.colors = colors;
        } 
    }

    public void onHoverEnter()
    {
        isBeingHovered = true;
        currTime = 0f;
        Debug.Log("entered!!");
    }

    public void onHoverExit()
    {
        isBeingHovered = false;
        Debug.Log("exited!!");
    }

    private void OnSelect()
    {
        // call a function here
        Debug.Log("selected!!");
        sparkScript.OnClick();
        leftMeshCombiner.changeToBubble();
        rightMeshCombiner.changeToBubble();
        handEffect.changeToBlue();
        contaminationScript.ChangeContaminationView(ContaminationScript.ContaminationView.Clean);
    }
}
