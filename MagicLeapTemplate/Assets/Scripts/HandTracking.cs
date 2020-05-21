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
    public GameObject leftThumb, leftMiddle, leftPinky, leftCenter, handParticleEmission;
    public GameObject menu, pointers;

    private MLHandTracking.HandKeyPose[] _gestures;
    private bool menuIsOn;

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
    }

    private void OnDestroy()
    {
        MLHandTracking.Stop();
    }


    private void Update()
    {
        if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Ok))
        {
            pose = HandPoses.Ok;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Finger))
        {
            pose = HandPoses.Finger;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.OpenHand))
        {
            pose = HandPoses.OpenHand;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Fist))
        {
            pose = HandPoses.Fist;
        }
        else if (GetGesture(MLHandTracking.Left, MLHandTracking.HandKeyPose.Thumb))
        {
            pose = HandPoses.Thumb;
        }
        else
        {
            pose = HandPoses.NoPose;
        }

        ShowPoints();
        DoMenuActions();
    }

    private void DoMenuActions()
    {
        if (pose == HandPoses.Finger)
        {
            menu.SetActive(true);
            pointers.SetActive(true);
        }
        //if (pose == HandPoses.Finger && !menuIsOn)
        //{
        //    menu.SetActive(true);
        //    pointers.SetActive(true);
        //    menuIsOn = true;
        //}
        //else if (pose == HandPoses.Thumb && menuIsOn)
        //{
        //    menu.SetActive(false);
        //    pointers.SetActive(false);
        //    menuIsOn = false;
        //}
    }

    private void ShowPoints()
    {
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
		handParticleEmission.transform.position = temp;


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
