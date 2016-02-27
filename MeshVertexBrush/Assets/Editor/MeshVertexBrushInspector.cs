using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(MeshVertexBrush))]
public class MeshVertexBrushInspector : Editor
{
    Color _color = Color.red;

    bool _isPaintingMode;
    bool IsPaintingMode
    {
        get { return _isPaintingMode; }
        set
        {
            foreach (MeshVertexBrush brush in targets.Select(item => item as MeshVertexBrush))
            {
                if (brush == null) continue;

                if (value)
                    brush.gameObject.hideFlags |= HideFlags.NotEditable;
                else
                    brush.gameObject.hideFlags &= ~HideFlags.NotEditable;

                brush.hideFlags = HideFlags.None;
            }

            _isPaintingMode = value;
            SceneView.RepaintAll();
        }
    }

    // For OnSceneGUI
    // In OnSceneGUI _targets array can't be called.
    MeshVertexBrush[] _brushes;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _color = EditorGUILayout.ColorField("Color", _color);

        if (IsPaintingMode)
        {
            Color originalColor = GUI.color;
            GUI.color = Color.cyan;
            if (GUILayout.Button("Painting Mode On", GUILayout.Height(30f)))
                IsPaintingMode = false;

            GUI.color = originalColor;
        }
        else
        {
            if (GUILayout.Button("Paiting Mode Off", GUILayout.Height(30f)))
            {
                IsPaintingMode = true;
                _brushes = targets.Select(item => item as MeshVertexBrush).ToArray();
            }
        }

        if (GUILayout.Button("Clear", GUILayout.Height(30f)))
        {
            foreach (MeshVertexBrush brush in targets.Select(item => item as MeshVertexBrush))
            {
                Mesh mesh = brush.GetComponent<MeshFilter>().sharedMesh;
                mesh.colors = null;
            }

            IsPaintingMode = false;
        }

        if (GUILayout.Button("Create Painted Mesh", GUILayout.Height(30f)))
        {
            IsPaintingMode = false;

            CreateMesh();
        }
    }

    void OnSceneGUI()
    {
        if (_isPaintingMode == false) return;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        if (Event.current.alt || Event.current.control || Event.current.command || Event.current.shift ||Event.current.functionKey) return;
        if (Event.current.button != 0) return;
        if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseDrag) return;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray).Where(item => _brushes.Count(brush => brush.gameObject == item.collider.gameObject) > 0).ToArray();
        if (hits.Length == 0) return;
         
        RaycastHit hit = hits.Aggregate((x, y) => (x.point - ray.origin).sqrMagnitude <= (y.point - ray.origin).sqrMagnitude ? x : y);
        if (hit.collider == null) return;

        MeshVertexBrush selectedBrush = hit.collider.gameObject.GetComponent<MeshVertexBrush>();
        Vector3 localPoint = selectedBrush.transform.InverseTransformPoint(hit.point);

        Mesh mesh = selectedBrush.GetComponent<MeshFilter>().sharedMesh;

        int nearestIndex = 0;
        float nearestSqrMag = (mesh.vertices[nearestIndex] - localPoint).sqrMagnitude;
        for (int i = nearestIndex + 1; i < mesh.vertexCount; ++i)
        {
            float sqrMag = (mesh.vertices[i] - localPoint).sqrMagnitude;
            if (sqrMag < nearestSqrMag)
            {
                nearestIndex = i;
                nearestSqrMag = sqrMag;
            }
        }

        if (mesh.colors == null || mesh.colors.Length == 0)
            mesh.colors = Enumerable.Repeat(new Color(0f, 0f, 0f, 0f), mesh.vertexCount).ToArray();

        List<Color> colors = mesh.colors.ToList();
        colors[nearestIndex] = _color;
        mesh.SetColors(colors);
        SceneView.RepaintAll();
    }

    void OnDisable()
    {
        IsPaintingMode = false;
    }

    void CreateMesh()
    {
        foreach(MeshVertexBrush brush in targets.Select(item => item as MeshVertexBrush))
        {
            GameObject clone = Instantiate(brush.gameObject);

            Mesh mesh = Instantiate(brush.GetComponent<MeshFilter>().sharedMesh);
            List<int> removedIndices = new List<int>();
            for (int i = 0; i < mesh.colors.Length; ++i)
            {
                if (mesh.colors[i].a <= 0)
                    removedIndices.Add(i);
            }

            List<int[]> triangles = new List<int[]>();
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                triangles.Add(new int[3]);

                int triangleCount = i / 3;
                int[] triangle = triangles[triangleCount];
                triangle[0] = mesh.triangles[i];
                triangle[1] = mesh.triangles[i + 1];
                triangle[2] = mesh.triangles[i + 2];
            }

            triangles.RemoveAll(triangle => removedIndices.Any(index => triangle.Contains(index)));

            List<int> newTriangles = new List<int>();
            foreach(int[] triangle in triangles)
            {
                foreach (int index in triangle)
                    newTriangles.Add(index);
            }

            mesh.triangles = newTriangles.ToArray();
            mesh.RecalculateBounds();

            clone.GetComponent<MeshFilter>().mesh = mesh;
        }
    }
}
