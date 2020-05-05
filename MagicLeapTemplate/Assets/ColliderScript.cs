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
    public Text text;

    public GameObject marking, hand;

    public Material invisible, material;

    private HashSet<string> processedMeshes = new HashSet<string>();
    private int index = 0;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        //count += 1;
        //Debug.Log("Collided, count = " + count);
        //text.text = "count: " + count;
        //if (collision.gameObject.name != "Plane" && collision.gameObject.name != "Plane(Clone)" && collision.gameObject.name != "Cylinder" && collision.gameObject.name != "Cylinder(Clone)")
        //{
        //    Debug.Log(collision.gameObject.name);
        //    GameObject newPlane = Instantiate(marking);
        //    //Debug.Log(collision.transform.rotation);
        //    Vector3 pos = hand.transform.position;
        //    pos.y -= (float)0.02;
        //    newPlane.transform.position = pos;
        //    // newPlane.transform.rotation = hand.transform.rotation;
        //}

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
                //meshFromSubmeshGameObject.transform.SetParent(meshFilter.transform);
                meshFromSubmeshGameObject.transform.localPosition = Vector3.zero;
                meshFromSubmeshGameObject.transform.localRotation = Quaternion.identity;
                MeshFilter meshFromSubmeshFilter = meshFromSubmeshGameObject.AddComponent<MeshFilter>();
                meshFromSubmeshFilter.sharedMesh = newMesh;
                //MeshRenderer meshFromSubmeshMeshRendererComponent = meshFromSubmeshGameObject.AddComponent<MeshRenderer>();
                MeshCollider meshCollider = meshFromSubmeshGameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = newMesh;
                //Debug.Log(Resources.Load("Material/Red.mat", typeof(Material)) as Material);
                //meshFromSubmeshMeshRendererComponent.material = invisible; //Resources.Load("Materials\\Red.mat", typeof(Material)) as Material;
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
            Debug.Log(collision.gameObject.name);
            MeshRenderer meshFromSubmeshMeshRendererComponent = collision.gameObject.GetComponent<MeshRenderer>();
            if (meshFromSubmeshMeshRendererComponent == null)
            {
                meshFromSubmeshMeshRendererComponent = collision.gameObject.AddComponent<MeshRenderer>();
            }
            meshFromSubmeshMeshRendererComponent.material = material;
        }
        //else if (collision.gameObject.name.Contains("SubMesh"))
        //{
        //    Debug.Log("collided with submesh");
        //}
    }

    private Mesh CreateMesh(int[] triangles, int index, Mesh mesh)
    {
        Mesh newMesh = new Mesh();
        List<int> vertexIndices = new List<int>();
        List<Vector3> verts = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<Vector3> normals = new List<Vector3>();

        List<Vector2> uvs, uvs2, uvs3, uvs4;
        uvs = new List<Vector2>(); uvs2 = new List<Vector2>(); uvs3 = new List<Vector2>(); uvs4 = new List<Vector2>();
        List<int> tris = new List<int>();

        newMesh.Clear();
        int curVertIndex = 0;
        int newVertIndex;
        int curSubVertIndex = 0;
        for (int i = 0; i < triangles.Length; i++)
        {
            curVertIndex = triangles[i];

            if (!vertexIndices.Contains(curVertIndex))
            {
                newVertIndex = curSubVertIndex;
                vertexIndices.Add(curVertIndex);

                verts.Add(mesh.vertices[curVertIndex]);

                if (mesh.colors != null && mesh.colors.Length > curVertIndex)
                    colors.Add(mesh.colors[curVertIndex]);

                normals.Add(mesh.normals[curVertIndex]);

                if (mesh.uv != null && mesh.uv.Length > curVertIndex)
                    uvs.Add(mesh.uv[curVertIndex]);
                if (mesh.uv2 != null && mesh.uv2.Length > curVertIndex)
                    uvs2.Add(mesh.uv2[curVertIndex]);
                if (mesh.uv3 != null && mesh.uv3.Length > curVertIndex)
                    uvs3.Add(mesh.uv3[curVertIndex]);
                if (mesh.uv4 != null && mesh.uv4.Length > curVertIndex)
                    uvs4.Add(mesh.uv4[curVertIndex]);

                curSubVertIndex++;
            }
            else
            {
                newVertIndex = vertexIndices.IndexOf(curVertIndex);
            }

            tris.Add(newVertIndex);
        }

        newMesh.vertices = verts.ToArray();
        newMesh.triangles = tris.ToArray();
        if (uvs.Count > 0)
            newMesh.uv = uvs.ToArray();
        if (uvs2.Count > 0)
            newMesh.uv2 = uvs2.ToArray();
        if (uvs3.Count > 0)
            newMesh.uv3 = uvs3.ToArray();
        if (uvs4.Count > 0)
            newMesh.uv4 = uvs4.ToArray();
        if (colors.Count > 0)
            newMesh.colors = colors.ToArray();

        newMesh.Optimize();
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();

        return newMesh;
    }

    void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Exit");
        //count -= 1;
        //Debug.Log("Exit, count = " + count);
        //text.text = "count: " + count;
    }
}
