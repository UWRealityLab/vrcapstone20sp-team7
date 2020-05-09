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
    // this represents the modes the contamination manager can be in.
    // Decay: shows the meshes in real time
    // heatmap: shows a heatmap of values the user has touched
    public enum ContaminationView {
        Decay,
        Heatmap
    }

    [Tooltip("Material of the contaminated mesh when touched by the user")]
    public Material startMaterial;
    private Color startColor;

    [Tooltip("Final Color that the contaminated mesh goes towards while decaying")]
    public Color endColor;

    [Tooltip("How long a mesh decays for, in seconds")]
    public float decayTimeout = 10f;

    private ContaminationView view = ContaminationView.Decay;

    private Dictionary<GameObject, float> meshToTime;     // maps meshes to the time they've been displayed
    private Dictionary<GameObject, float> meshToNumTouches; // maps meshes to the number of times the user has touched them

    private float maxHeatmapValue;
    [Tooltip("Heatmap color of the least touched areas")]
    public Color coldColor;
    [Tooltip("Heatmap color of the most touched areas")]
    public Color hotColor;
    [Tooltip("If true, turns on Heatmap mode after a certain amount of time (debugging only)")]
    public bool turnOnHeatmap;
    [Tooltip("How long before we switch to Heatmap mode, in seconds (debugging only)")]
    public float heatmapTimer = 20f;
    private float heatmapCounter = 0f; // this should be deleted once we have proper UI for switching modes


    // Start is called before the first frame update
    void Start() {
        startColor = startMaterial.GetColor("_Color");
        meshToTime = new Dictionary<GameObject, float>();
        meshToNumTouches = new Dictionary<GameObject, float>();
    }

    // Update is called once per frame
    void Update() {
        if (view == ContaminationView.Decay) {
            UpdateDecay();
        }
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

        heatmapCounter += updateTime;
        if (heatmapCounter > heatmapTimer && turnOnHeatmap) {
            ChangeContaminationView(ContaminationView.Heatmap);
        }
    }

    // call when user touches a mesh
    public void TouchMesh(GameObject mesh) {
        if (view == ContaminationView.Decay) {
            MeshRenderer mr = mesh.GetComponent<MeshRenderer>();
            if (mr == null) {
                mr = mesh.AddComponent<MeshRenderer>();
            }
            mr.enabled = true;
            mr.material = startMaterial;

            meshToTime[mesh] = 0f;
        }

        if (!meshToNumTouches.ContainsKey(mesh)) {
            meshToNumTouches[mesh] = 1f;
        } else {
            meshToNumTouches[mesh] += 1f;
        }
    }

    public void ChangeContaminationView(ContaminationView newView) {
        view = newView;

        if (view == ContaminationView.Heatmap) {
            maxHeatmapValue = 1f;
            foreach(KeyValuePair<GameObject, float> entry in meshToNumTouches) {
                if (maxHeatmapValue < entry.Value) {
                    maxHeatmapValue = entry.Value;
                }
            }
            Debug.Log("Changing to Heatmap view with max value = " + maxHeatmapValue);

            GameObject[] gos = new GameObject[meshToNumTouches.Keys.Count]; // list of meshes to check
            meshToNumTouches.Keys.CopyTo(gos, 0);
            foreach(GameObject go in gos) {
                MeshRenderer mr = go.GetComponent<MeshRenderer>();
                mr.enabled = true;
                mr.material.SetColor("_Color", Color.Lerp(coldColor, hotColor, meshToNumTouches[go] / maxHeatmapValue));
            }
        }
    }
}
