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
    private Color startColor; // based off the color of the Material;
    public Color endColor;

    private float decayTimer = 10f; // upper bound for when to delete a mesh, in seconds
    private Dictionary<GameObject, float> meshToTime;
    private int counter = 0; // only update every once in a while


    private HashSet<string> processedMeshes = new HashSet<string>();
    private int index = 0;

    void Start() {
      startColor = material.GetColor("_Color");
      meshToTime = new Dictionary<GameObject, float>();
    }

    void Update() {
      // Debug.Log("updating, size = " + meshToTime.Keys.Count);
      counter = 0;
      float updateTime = Time.deltaTime;

      GameObject[] gos = new GameObject[meshToTime.Keys.Count];
      meshToTime.Keys.CopyTo(gos, 0);
      List<GameObject> removals = new List<GameObject>(); // list of items to remove

      foreach(GameObject go in gos) {
        // Debug.Log("\t" + counter++ + ", " + updateTime);
        float time = meshToTime[go] + updateTime;
        if (time > decayTimer) {
          removals.Add(go);
        } else {
          meshToTime[go] = time;
          go.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.Lerp(startColor, endColor, time / decayTimer));
        }
      }
      foreach(GameObject go in removals) {
        meshToTime.Remove(go);
        //Destroy(go);
        go.GetComponent<MeshRenderer>().enabled = false;
      }
  }

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

            //for (int i = 0; i < mesh.subMeshCount; i++)
            //{
            //    int[] subMeshTriangles = mesh.GetTriangles(i);

            //    Mesh newMesh = new Mesh();
            //    newMesh.Clear();
            //    newMesh.vertices = mesh.vertices;
            //    newMesh.triangles = mesh.triangles;
            //    newMesh.uv = mesh.uv;
            //    newMesh.uv2 = mesh.uv2;
            //    newMesh.uv2 = mesh.uv2;
            //    newMesh.colors = mesh.colors;
            //    newMesh.subMeshCount = mesh.subMeshCount;
            //    newMesh.normals = mesh.normals;

            //    GameObject meshFromSubmeshGameObject = new GameObject();
            //    meshFromSubmeshGameObject.name = "SubMesh" + count;
            //    meshFromSubmeshGameObject.transform.SetParent(meshFilter.transform);
            //    meshFromSubmeshGameObject.transform.localPosition = Vector3.zero;
            //    meshFromSubmeshGameObject.transform.localRotation = Quaternion.identity;
            //    MeshFilter meshFromSubmeshFilter = meshFromSubmeshGameObject.AddComponent<MeshFilter>();
            //    meshFromSubmeshFilter.sharedMesh = CreateMesh(subMeshTriangles, i, mesh);
            //    MeshRenderer meshFromSubmeshMeshRendererComponent = meshFromSubmeshGameObject.AddComponent<MeshRenderer>();
            //    count++;
            //}
        }
        else if (collision.gameObject.name.Contains("SubMesh"))
        {
            Debug.Log(collision.gameObject.name + "submeshed");
            MeshRenderer meshFromSubmeshMeshRendererComponent = collision.gameObject.GetComponent<MeshRenderer>();
            if (meshFromSubmeshMeshRendererComponent == null)
            {
                meshFromSubmeshMeshRendererComponent = collision.gameObject.AddComponent<MeshRenderer>();
            }
            meshFromSubmeshMeshRendererComponent.enabled = true;
            meshFromSubmeshMeshRendererComponent.material = material;
            if(!meshToTime.ContainsKey(collision.gameObject)) {
              meshToTime.Add(collision.gameObject, 0f);
            }
        }

    }

    void OnCollisionExit(Collision collision)
    {

    }
}
