using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
	public ParticleSystemRenderer psrVirus;
	public ParticleSystemRenderer psrBubble;
	public Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh.SetTriangles(mesh.triangles, 0);
        mesh.subMeshCount = 1;
        psrVirus.mesh = mesh;
        psrVirus.enabled = false;
        psrBubble.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
