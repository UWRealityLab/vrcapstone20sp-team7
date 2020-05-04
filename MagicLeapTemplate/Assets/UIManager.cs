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
    public bool IsIntroExplicit; // if true, meshing will be disabled until intro finishes

    private float timeout = 10f;
    private float timeSinceLastCheck = 0f;

    // Start is called before the first frame update
    void Start()
    {
      MLInput.Start();
      if (IsIntroExplicit) {
        SpatialMapper.SetActive(false);
      }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: get better method of starting the game (maybe touching the start button), current just timesout after x seconds
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck > timeout) {
          StartApp();
        }
    }

    // code for destroying startup UI elements and letting the player interact with the game
    void StartApp() {
      HeadlockedCanvas.SetActive(false);
      SpatialMapper.SetActive(true);
    }

    void OnDestroy() {
      MLInput.Stop();
    }
}
