using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class HandTracking : MonoBehaviour
{
    public enum HandPoses { Ok, Finger, Thumb, OpenHand, Fist, NoPose };
    public HandPoses pose = HandPoses.NoPose;
    public Vector3[] pos;
    public GameObject leftThumb, leftMiddle, leftPinky, leftCenter, leftHandParticleEmission;
    public GameObject rightThumb, rightMiddle, rightPinky, rightCenter, rightHandParticleEmission;
    public GameObject menu, pointers;
    public UIManager uiManager;

    private MLHandTracking.HandKeyPose[] _gestures;
    private bool menuIsOn;
    private float _secondsToHoldPose = 2;
    private float _timer = 0;

    private void Start()
    {
        MLHandTracking.Start();
        _gestures = new MLHandTracking.HandKeyPose[5];
        _gestures[0] = MLHandTracking.HandKeyPose.Ok;
        _gestures[1] = MLHandTracking.HandKeyPose.Finger;
        _gestures[2] = MLHandTracking.HandKeyPose.OpenHand;
        _gestures[3] = MLHandTracking.HandKeyPose.Fist;
        _gestures[4] = MLHandTracking.HandKeyPose.Thumb;
        MLHandTracking.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        pos = new Vector3[3];
        menuIsOn = false;
        menu.SetActive(false);
        pointers.SetActive(false);
        _timer = _secondsToHoldPose;
    }

    private void OnDestroy()
    {
        MLHandTracking.Stop();
    }


    private void Update()
    {
        ShowPoints();
        if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Finger))
        {
            pose = HandPoses.Finger;
            _timer = _secondsToHoldPose;
        }
        else if (GetGesture(MLHandTracking.Right, MLHandTracking.HandKeyPose.Ok))
        {
            _timer -= Time.deltaTime;
            if (_timer > 0) 
            {
                pose = HandPoses.Ok;
                Debug.Log("hand ok");
            }
        }
        else
        {
            pose = HandPoses.NoPose;
            _timer = _secondsToHoldPose;
        }
        DoMenuActions();
        DoIntroActions();
    }

    private void DoIntroActions()
    {
        if (pose == HandPoses.Ok && _timer < 0)
        {
            uiManager.moveToNextPage();
            _timer = _secondsToHoldPose;
        }
    }


    private void DoMenuActions()
    {
        if (pose == HandPoses.Finger)
        {
            menu.SetActive(true);
            pointers.SetActive(true);
        }
    }

    private void ShowPoints()
    {
        /*
         * Left hand
         */
        Vector3 temp = MLHandTracking.Left.Thumb.Tip.Position;
        //temp.y -= (float)0.03;
        leftThumb.transform.position = temp;

        temp = MLHandTracking.Left.Middle.Tip.Position;
        //temp.y -= (float)0.03;
        leftMiddle.transform.position = temp;

        temp = MLHandTracking.Left.Pinky.Tip.Position;
        //temp.y -= (float)0.03;
        leftPinky.transform.position = temp;

        temp = MLHandTracking.Left.Center;
        //temp.y -= (float)0.03;
        leftCenter.transform.position = temp;
        leftHandParticleEmission.transform.position = temp;


        /*
         * Right hand
         */
        temp = MLHandTracking.Right.Thumb.Tip.Position;
        //temp.y -= (float)0.03;
        rightThumb.transform.position = temp;

        temp = MLHandTracking.Right.Middle.Tip.Position;
        //temp.y -= (float)0.03;
        rightMiddle.transform.position = temp;

        temp = MLHandTracking.Right.Pinky.Tip.Position;
        //temp.y -= (float)0.03;
        rightPinky.transform.position = temp;

        temp = MLHandTracking.Right.Center;
        //temp.y -= (float)0.03;
        rightCenter.transform.position = temp;
        rightHandParticleEmission.transform.position = temp;
    }

    private bool GetGesture(MLHandTracking.Hand hand, MLHandTracking.HandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.HandKeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
