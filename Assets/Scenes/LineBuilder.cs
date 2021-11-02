using UnityEngine;
using System.Collections.Generic;

public class LineBuilder : MonoBehaviour
{
    public List<Vector2> Points;
    public float Width = 1.0f;
    public Material Material;

    private float width => Width / 100;

    public struct Corner
    {
        public Vector2 Left;
        public Vector3 Right;
    }

    public void Start()
    {
        build();
    }

    static Vector3 Vector2ToVector3(Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    void OnValidate()
    {
        if (Application.IsPlaying(this))
        {

            build();
        }
    }

    void OnDisable()
    {
        // if (go != null)
        // {
            go.Destroy();
        // }
    }

    void build()
    {
        buildDumb();
    }

    private GameObject go;
    private void buildDumb()
    {
        if (go != null)
        {
            go.Destroy();
        }

        go = new GameObject("LINE");
        var mesh = go.AddComponent<MeshFilter>().mesh;
        var meshRenderer = go.AddComponent<MeshRenderer>();

        BuildLineMesh(Points, mesh, width);

        meshRenderer.sharedMaterial = Material;
    }

    public static void BuildLineMesh(List<Vector2> points, Mesh mesh, float width) {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var indices = new List<int>();

        Debug.Assert(points.Count >= 2);

        var ic = 0;
        for (var i = 0; i < points.Count - 1; i += 1)
        {
            var ab = (points[i + 1] - points[i]).normalized;
            var segLen = (points[i + 1] - points[i]).magnitude;
            var tab = new Vector2(-ab.y, ab.x);

            var aLeft2d = points[i] - tab * width;
            var aRight2d = points[i] + tab * width;

            vertices.Add(Vector2ToVector3(aLeft2d));
            vertices.Add(Vector2ToVector3(aRight2d));
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);

            var bLeft2D = aLeft2d + segLen * ab;
            var bRight2D = aRight2d + segLen * ab;

            vertices.Add(Vector2ToVector3(bLeft2D));
            vertices.Add(Vector2ToVector3(bRight2D));
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);

            indices.AddRange(new int[] { ic ,ic+ 1,ic+ 2,ic+ 1,ic+ 3,ic+ 2 });
            ic += 4;
        }

        for (var i = 0; i < points.Count - 2; i+= 1) {
            var firstRight = vertices[4 * i + 3];
            var firstLeft = vertices[4 * i + 2];
            var secondLeft = vertices[4 * (i+1)];
            var secondRight = vertices[4 * (i+1) + 1];

            var center = (firstRight + firstLeft) / 2;

            vertices.AddRange(new Vector3[]{center, firstRight, secondRight});
            vertices.AddRange(new Vector3[]{center, secondLeft, firstLeft});

            for (var k = 0; k < 6;k++) {
                normals.Add(Vector3.up);
            }

            indices.AddRange(new int[]{ ic, ic + 1, ic +2, ic+3, ic+4, ic+5 });
            ic += 6;
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
    }


    private void GetCornerVertices(Vector2 a, Vector2 b, Vector2 c)
    {
        Corner corner = new Corner { };

        Vector2 ab = (b - a).normalized;
        Vector2 tab = new Vector2(-ab.y, ab.x);
    }
}
