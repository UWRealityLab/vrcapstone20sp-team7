using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/*
 * https://answers.unity.com/questions/1213025/separating-submeshes-into-unique-meshes.html
 * https://answers.unity.com/questions/326235/separating-fbx-objects.html
 */

public class ColliderScript : MonoBehaviour
{
    public GameObject hand;

    public Material material;

    private HashSet<string> processedMeshes = new HashSet<string>();
    private int index = 0;

    public void CollisionEnter(Collision collision)
    {
        OnCollisionEnter(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");

        MeshFilter meshFilter = collision.gameObject.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh != null && collision.gameObject.name.Contains("Mesh ") && !processedMeshes.Contains(collision.gameObject.name))
        {
            processedMeshes.Add(collision.gameObject.name);
            Debug.Log(mesh.subMeshCount);
            Debug.Log(mesh.triangles.Length);
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = mesh.vertices[mesh.triangles[i + 0]];
                Vector3 p2 = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 p3 = mesh.vertices[mesh.triangles[i + 2]];

                Mesh newMesh = new Mesh();
                newMesh.Clear();
                newMesh.vertices = new Vector3[] { p1, p2, p3 };
                newMesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
                newMesh.triangles = new int[] { 0, 1, 2 };

                GameObject meshFromSubmeshGameObject = new GameObject();
                meshFromSubmeshGameObject.name = "SubMesh" + index;
                meshFromSubmeshGameObject.transform.localPosition = Vector3.zero;
                meshFromSubmeshGameObject.transform.localRotation = Quaternion.identity;
                MeshFilter meshFromSubmeshFilter = meshFromSubmeshGameObject.AddComponent<MeshFilter>();
                meshFromSubmeshFilter.sharedMesh = newMesh;
                MeshCollider meshCollider = meshFromSubmeshGameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = newMesh;
                index++;
            }
        }
        else if (collision.gameObject.name.Contains("SubMesh"))
        {
            Debug.Log(collision.gameObject.name);
            MeshRenderer meshFromSubmeshMeshRendererComponent = collision.gameObject.GetComponent<MeshRenderer>();
            if (meshFromSubmeshMeshRendererComponent == null)
            {
                meshFromSubmeshMeshRendererComponent = collision.gameObject.AddComponent<MeshRenderer>();
            }
            meshFromSubmeshMeshRendererComponent.material = material;
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        
    }
}
