using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/*

This class is in charge of all code related to the liveliness and contamination of the virus. Features include:

- keeping track of which meshes have been touched by the user
- decaying meshes over time
- keeping track of the number of times a mesh has been touched

*/

public class ContaminationScript : MonoBehaviour
{
    public Material startMaterial;
    private Color startColor;
    public Color endColor;

    public float decayTimeout = 10f; // upper bound for when to delete a mesh, in seconds

    private Dictionary<GameObject, float> meshToTime; // maps meshes to the time they've been displayed
    private Dictionary<GameObject, int> meshToNumTouches; // maps meshes to the number of times the user has touched them


    // Start is called before the first frame update
    void Start() {
        startColor = startMaterial.GetColor("_Color");
        meshToTime = new Dictionary<GameObject, float>();
        meshToNumTouches = new Dictionary<GameObject, int>();
    }

    // Update is called once per frame
    void Update() {
        UpdateDecay();
    }

    // Updates the decay status of all the meshes being touched, updating their mesh and deactivating them if they
    // are older than
    void UpdateDecay() {
        float updateTime = Time.deltaTime;

        GameObject[] gos = new GameObject[meshToTime.Keys.Count]; // list of meshes to check
        meshToTime.Keys.CopyTo(gos, 0);
        List<GameObject> removals = new List<GameObject>(); // list of items to remove

        foreach(GameObject go in gos) {
          float time = meshToTime[go] + updateTime;
          if (time > decayTimeout) {
            removals.Add(go);
          } else {
            meshToTime[go] = time;
            go.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.Lerp(startColor, endColor, time / decayTimeout));
          }
        }

        // remove renderer for all meshes that are over the time limit
        foreach(GameObject go in removals) {
          meshToTime.Remove(go);
          go.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // call when user touches a mesh
    public void TouchMesh(GameObject mesh) {
        Debug.Log("touch mesh");
        meshToTime[mesh] = 0f;

        if (!meshToNumTouches.ContainsKey(mesh)) {
            meshToNumTouches[mesh] = 0;
        } else {
            meshToNumTouches[mesh] = meshToNumTouches[mesh] + 1;
        }
    }
}
