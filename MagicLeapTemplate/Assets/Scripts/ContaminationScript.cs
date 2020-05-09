using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

This class is in charge of all code related to the liveliness and contamination of the virus. Features include:

- keeping track of which meshes have been touched by the user
- decaying meshes over time
- keeping track of the number of times a mesh has been touched

*/

public class ContaminationScript : MonoBehaviour
{
    public Material startMaterial;
    public Color endColor;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // Updates the decay status of all the meshes being touched, updating their mesh and deactivating them if they
    // are older than
    void UpdateDecay() {

    }
}
