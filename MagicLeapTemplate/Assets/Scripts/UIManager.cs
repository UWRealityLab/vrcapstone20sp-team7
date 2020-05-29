using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject HeadlockedCanvas;
    public GameObject SpatialMapper;
    public GameObject HandEffectLeft, HandEffectRight;
    public bool IsIntroExplicit; // if true, meshing will be disabled until intro finishes
    public GameObject page1, page2, page3, page4;

    private float timeout = 5f;
    private float timeSinceLastCheck = 0f;
    private int pageNum = 1;
    private int TOTAL_PAGES = 2;

    // Start is called before the first frame update
    void Start()
    {
      MLInput.Start();
      if (IsIntroExplicit) {
        SpatialMapper.SetActive(false);
        HandEffectRight.SetActive(false);
        HandEffectLeft.SetActive(false);
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
      }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: get better method of starting the game (maybe touching the start button), current just timesout after x seconds
        // timeSinceLastCheck += Time.deltaTime;
        // if (timeSinceLastCheck > timeout) {
        //   StartApp();
        // }
        if (pageNum == 1)
        {
        }
        else if (pageNum == 2)
        {
            page1.SetActive(false);
            page2.SetActive(true);

        }
        else if (pageNum == 3)
        {
            // start app once user moved to the end
            page2.SetActive(false);
            page3.SetActive(true);
        } else if (pageNum == 4)
        {
            page3.SetActive(false);
            page4.SetActive(true);
        }
        else
        {
            page4.SetActive(false);
            StartApp();
        }
    }
    public void moveToNextPage()
    {
    	Debug.Log("move to next page");
    	pageNum += 1;
    }

    // code for destroying startup UI elements and letting the player interact with the game
    void StartApp() {
      HeadlockedCanvas.SetActive(false);
      SpatialMapper.SetActive(true);
      HandEffectRight.SetActive(true);
      HandEffectLeft.SetActive(true);
    }

    void OnDestroy() {
      MLInput.Stop();
    }
}
